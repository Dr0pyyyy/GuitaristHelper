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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

        private string authToken = "BQD7XIYyLaVgKmNLlOhR_LL1B8Lhu_rxPji8TSOw4ySqrgHsFEGw1Z9i_YGYGZfX6EQkpXKLXxhUL9umm_Lij4eM5fo5tK0HJMQE15uxw9JqwPVjHwOUmf0aizku0j74YONt1Des-bQNry1yqL-fdlD3cl3PdVJUjGZ0SsrFJ-VNZiZ2";
        public HttpClient Client = new HttpClient();

        public APIHelper()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public async Task<string> getPlaylistId(string playlistID)
        {
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/search" + "?q=" + playlistID.Trim() + "&type=playlist&limit=1");
            JsonPlaylist playlist = JsonSerializer.Deserialize<JsonPlaylist>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return playlist.playlists.items.First().external_urls.spotify.Split("/").Last();
        }

        public async Task<List<String>> getTracks(string playlistID)
        {
            List<string> result = new List<string>();
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/playlists/" + playlistID);
            JsonTracks tracks = JsonSerializer.Deserialize<JsonTracks>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            foreach (Item item in tracks.tracks.items)
            {
                string line = item.track.artists.First().name + " - " + item.track.name;
                result.Add(line);
            }
            return result;
        }



        /*
        private void getToken() Tahle metoda bude možná potřeba při vytváření auth tokenu
        { 
            
        }
        */
    }
}
