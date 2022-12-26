using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitarist_Helper
{
    internal class SpotifyManager
    {
        private string Genre { get; set; }
        private APIHelper apiHelper { get; set; }

        public SpotifyManager(string genre)
        {
            this.Genre = genre;
            apiHelper = new APIHelper();
        }

        public async Task<string> GetLinks()
        {
            List<string> tracks = await GetTracks();

            return ""; //Final result
        }

        private async Task<List<String>> GetTracks()
        {
            string playlistID = await apiHelper.getPlaylistId(Genre);
            return await apiHelper.getTracks(playlistID);
        }
    }
}
