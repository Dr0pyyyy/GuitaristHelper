using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guitarist_Helper
{
    internal class APIHelper
    {
        private string authToken = "BQDHiBnAokNiTK8r7pzl-5ETo42haV0kwI_mNXcRixZrjvwJJSDc3WqMe_4x6rWHY0gZwHp4USIlPGM7ELu6ivRfep9K8hbp7vMlRTesEXfHRlrT1nIvxB39sHkEKgM8cDVcCCB5wgwhIXZz2h3pul5BY1IL6CRdwaQigQfWA_7cE_YV";
        public HttpClient Client = new HttpClient();

        public APIHelper()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public async Task<string> getPlaylistId(string playlist)
        {
            var response = await Client.GetAsync("https://api.spotify.com/v1/search" + "?q=" + playlist.Trim() + "&type=playlist&limit=1");
            response.EnsureSuccessStatusCode();
            var json = JsonSerializer.Deserialize<JsonPlaylist>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return json.playlists.items.First().external_urls.spotify.Split("/").Last();
        }

        public async Task<List<String>> getTracks(string playlistID)
        {
            var response = await Client.GetAsync("https://api.spotify.com/v1/playlists/" + playlistID);
            response.EnsureSuccessStatusCode();
            var json = JsonSerializer.Deserialize<JsonTracks>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return null;
        }



        /*
        private void getToken() Tahle metoda bude možná potřeba při vytváření auth tokenu
        { 
            
        }
        */
    }
}
