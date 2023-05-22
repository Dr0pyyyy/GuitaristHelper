using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Guitarist_Helper
{
    internal class SpotifyManager
    {
        private string PLaylistID { get; set; }
        private APIHelper ApiHelper { get; set; }

        public SpotifyManager(string playlistID)
        {
            //Getting pure ID without url
            this.PLaylistID = playlistID.Split('/').Last();
            this.PLaylistID = this.PLaylistID.Split('?').First();
            this.ApiHelper = new APIHelper();
        }

        //TODO - GetList a GroupLists se musí přejmenovat
        public async Task<List<string>> GetList()
        {
            List<Tuple<string, string>> songs = await GetSongNames();
            return GroupLists(songs, await GetSongLinks(songs));
        }

        private List<string> GroupLists(List<Tuple<string, string>> songs, List<string> links)
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

        private async Task<List<Tuple<string, string>>> GetSongNames()
        {
            var jsontracks = await ApiHelper.GetSongNames(PLaylistID);
            List<Tuple<string, string>> songs = new List<Tuple<string, string>>();
            foreach (var track in jsontracks.tracks.items)
            {
                songs.Add(new Tuple<string, string>(track.track.name, track.track.artists.First().name));
            }
            return songs;
        }
    }
}
