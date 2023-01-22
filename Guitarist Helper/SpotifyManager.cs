using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitarist_Helper
{
    internal class SpotifyManager
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null refence return.
#pragma warning disable CS8604 // Possible null reference argument.

        private string Genre { get; set; }
        private APIHelper ApiHelper { get; set; }

        public SpotifyManager(string genre)
        {
            this.Genre = genre;
            this.ApiHelper = new APIHelper();

        }

        public async Task<string> GetList()
        {
            JsonTracks tracks = await GetTracks(); //Tracks and artists

            var links = await GetLinks(tracks); //Links for chords

            //Tady udělat list kterej bude obsahovat jak nazvy songu a artisty, tak linky na dany songy

            return ""; //Final result
        }

        private async Task<List<String>> GetLinks(JsonTracks songs)
        {
            List<string> links = new List<string>();
            foreach (var song in songs.tracks.items)
            {
                var link = await ApiHelper.getSongTabs(song.track.name, song.track.artists.First().name);
                if (link != null)
                {
                    links.Add("https://www.chords-and-tabs.net" + link);
                }
                else
                {
                    links.Add("Sorry we couldnt find chords or tabs for this song!");
                }
            }
            return links;
        }

        private async Task<JsonTracks> GetTracks()
        {
            string playlistID = await ApiHelper.getPlaylistId(Genre);
            return await ApiHelper.getTracks(playlistID);
        }
    }
}
