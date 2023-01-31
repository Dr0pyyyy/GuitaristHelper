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
using System.Net;
using System.Net.Http;

namespace Guitarist_Helper
{
    internal class APIHelper
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null refence return.
#pragma warning disable CS8604 // Possible null reference argument.

        private string authToken;
        private HttpClient Client = new HttpClient();
        private WebScraper webScraper = new WebScraper();

        public APIHelper()
        {
            authToken = GetAccessToken().Result;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            Task.Run(() => this.GetAccessToken()).Wait();
        }

        public async Task<string> GetAccessToken()
        {
            //Created instance of HttpClient, because it needs special settings to get access token
            HttpClient spotifyClient = new HttpClient();
            byte[] clientByte = Encoding.UTF8.GetBytes("543981021bd348419b8abcb150449f5a:e9910fe8f8fb451db7ac823c343932ad");
            var clientIdAndSecret = Convert.ToBase64String(clientByte);
            spotifyClient.BaseAddress = new Uri("https://accounts.spotify.com/api/token");
            spotifyClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientIdAndSecret);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new StringContent("grant_type=client_credentials", Encoding.Default, "application/x-www-form-urlencoded");

            var response = await spotifyClient.PostAsync(spotifyClient.BaseAddress, request.Content);
            response.EnsureSuccessStatusCode();

            var token = JsonSerializer.Deserialize<AccessToken>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions());
            return token.access_token;
        }

        public async Task<string> GetPlaylistId(string nameOfPlaylist)
        {
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/search" + "?q=" + nameOfPlaylist.Trim() + "&type=playlist&limit=1");
            response.EnsureSuccessStatusCode();
            JsonPlaylist playlist = JsonSerializer.Deserialize<JsonPlaylist>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return playlist.playlists.items.First().external_urls.spotify.Split("/").Last();
        }

        public async Task<JsonTracks> GetSongNames(string playlistID)
        {
            List<string> result = new List<string>();
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/playlists/" + playlistID);
            response.EnsureSuccessStatusCode();
            JsonTracks tracks = JsonSerializer.Deserialize<JsonTracks>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return tracks;
        }

        public async Task<string> GetSongLink(string songName, string artistName)
        {
            string requestedSong = artistName.Replace(" ", "%20") + "%20" + songName.Replace(" ", "%20");
            HttpResponseMessage response = await Client.GetAsync("https://www.chords-and-tabs.net/songs/search/" + requestedSong);
            response.EnsureSuccessStatusCode();
            //Response contains whole html data of requested song and need to be scraped
            return webScraper.GetLink(await response.Content.ReadAsStringAsync());
        }
    }
}
