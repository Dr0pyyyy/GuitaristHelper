using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection.Metadata.Ecma335;

namespace Guitarist_Helper
{
    internal class APIHelper
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null refence return.
#pragma warning disable CS8604 // Possible null reference argument.

        private string authToken = "BQAbHUidghMwvQ_f-2rXXmmkWJRWgW3aCFJkKNU9w8pdRuwCzA-Vl8XmqYSnwZ7T_1VSpYQxCHBe3f4stexi2bZ8n-lGPJluk3OU-c0Mcu7kjHNFP21hoXvqX7YOWSPAF48mXcGLba0sYFIBYm4YQtFvdECOP-JxtQLn1wuqAlWjH8or";
        public HttpClient Client = new HttpClient();

        public APIHelper()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public async Task<string> getPlaylistId(string playlistID)
        {
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/search" + "?q=" + playlistID.Trim() + "&type=playlist&limit=1");
            response.EnsureSuccessStatusCode();
            JsonPlaylist playlist = JsonSerializer.Deserialize<JsonPlaylist>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return playlist.playlists.items.First().external_urls.spotify.Split("/").Last();
        }

        public async Task<JsonTracks> getTracks(string playlistID)
        {
            List<string> result = new List<string>();
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/playlists/" + playlistID);
            response.EnsureSuccessStatusCode();
            JsonTracks tracks = JsonSerializer.Deserialize<JsonTracks>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return tracks;
        }

        public async Task<string> getSongTabs(string songName, string artistName)
        {
            string requestedSong = artistName.Replace(" ", "%20") + "%20" + songName.Replace(" ", "%20");
            HttpResponseMessage response = await Client.GetAsync("https://www.chords-and-tabs.net/songs/search/Ed%20sheeran%20Perfect");
            return await response.Content.ReadAsStringAsync();
        }

        /*
        private async Task<string> getOauthToken()
        {
            //Update when main function of the app will be complete
            //Most likely in v2.0. that will be with frontend
            return "";
        }*/
    }
}
