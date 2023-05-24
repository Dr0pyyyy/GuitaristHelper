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

        public SpotifyManager(ILogger<SpotifyManager> logger)
        {
            //Getting ID without url
            this.ApiHelper = new APIHelper();
            this._logger = logger;
        }

        public async Task<List<string>> GetPlaylist(string playlistID)
        {
            playlistID = playlistID.Split('/').Last().Split('?').First();
            List<Tuple<string, string>> songs = await GetSongNames(playlistID);
            return MergeLists(songs, await GetSongLinks(songs));
        }

        private List<string> MergeLists(List<Tuple<string, string>> songs, List<string> links)
        {
            List<string> result = new List<string>();
            int i = 0;
            foreach (var item in songs)
            {
                result.Add(item.Item1 + " - " + item.Item2);
                result.Add(links[i]);
                i++;
            }
            return result;
        }

        private async Task<List<String>> GetSongLinks(List<Tuple<string, string>> songs)
        {
            List<string> links = new List<string>();
            foreach (var song in songs)
            {
                string link = await ApiHelper.GetSongLink(song.Item1, song.Item2);
                if (link != null)
                    links.Add("https://www.chords-and-tabs.net" + link);
                else
                    links.Add("Sorry we couldnt find chords/tabs for this song!");
            }
            return links;
        }

        private async Task<List<Tuple<string, string>>> GetSongNames(string playlistID)
        {
            var jsontracks = await ApiHelper.GetSongNames(playlistID);
            List<Tuple<string, string>> songs = new List<Tuple<string, string>>();
            jsontracks.tracks.items.ForEach(track => songs.Add(new Tuple<string, string>(track.track.name, track.track.artists.First().name)));
            return songs;
        }
    }
}
