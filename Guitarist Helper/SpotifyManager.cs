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
        private APIHelper apiHelper { get; set; }

        public SpotifyManager(string genre)
        {
            this.Genre = genre;
            apiHelper = new APIHelper();
        }

        public async Task<string> GetList()
        {
            JsonTracks tracks = await GetTracks(); //Tracks and artists

            var links = await GetLinks(tracks); //Links for chords

            return ""; //Final result
        }

        private async Task<List<String>> GetLinks(JsonTracks songs)
        {
            foreach (var song in songs.tracks.items)
            {
                string html = await apiHelper.getSongTabs(song.track.name, song.track.artists.First().name); //whole html
                //Parse value here create frontend first
            }

            return null;
        }

        private async Task<JsonTracks> GetTracks()
        {
            string playlistID = await apiHelper.getPlaylistId(Genre);
            return await apiHelper.getTracks(playlistID);
        }
    }
}
