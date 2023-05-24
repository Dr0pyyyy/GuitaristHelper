using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Guitarist_Helper
{
    public class SpotifyManager:IMusicManager
    {
        public APIHelper ApiHelper { get; set; }

        private readonly ILogger<SpotifyManager> _logger;

        private readonly IConfiguration _configuration;

        public SpotifyManager(ILogger<SpotifyManager> logger, IConfiguration config)
        {
            //Getting ID without url
            this.ApiHelper = new APIHelper(config);
            this._logger = logger;
            this._configuration = config;
        }

        public async Task<List<string>> GetPlaylist(string playlistID)
        {
            playlistID = playlistID.Split('/').Last().Split('?').First();
            List<Tuple<string, string>> songs = await GetSongNames(playlistID);
            return MergeLists(songs, await GetSongLinks(songs));
        }

        private List<string> MergeLists(List<Tuple<string, string>> songs, List<string> links)
        {
            _logger.LogInformation("Merging songs with links");
            List<string> result = new List<string>();
            int linkIndex = 0;
            foreach (var song in songs)
            {
                result.Add(song.Item1 + " - " + song.Item2);
                result.Add(links[linkIndex]);
                linkIndex++;
            }
            _logger.LogInformation("Lists merged successfully");
            return result;
        }

        private async Task<List<String>> GetSongLinks(List<Tuple<string, string>> songs)
        {
            _logger.LogInformation("Getting links of wanted songs");
            List<string> links = new List<string>();
            foreach (var song in songs)
            {
                string link = await ApiHelper.GetSongLink(song.Item1, song.Item2);
                if (link != null)
                    links.Add("https://www.chords-and-tabs.net" + link);
                else
                    links.Add("Sorry we couldnt find chords/tabs for this song!");
            }
            _logger.LogInformation("List of links created successfully");
            return links;
        }

        private async Task<List<Tuple<string, string>>> GetSongNames(string playlistID)
        {
            _logger.LogInformation("Getting list of song names from selected playlist");
            var json = await ApiHelper.GetSongNames(playlistID);
            List<Tuple<string, string>> songs = new List<Tuple<string, string>>();
            json.tracks.items.ForEach(song => songs.Add(new Tuple<string, string>(song.track.name, song.track.artists.First().name)));
            _logger.LogInformation("List of song names created successfully");
            return songs;
        }
    }
}
