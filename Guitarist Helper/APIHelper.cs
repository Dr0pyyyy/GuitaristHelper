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

        private string authToken;
        public HttpClient Client = new HttpClient();
        WebScraper webScraper = new WebScraper();

        public APIHelper()
        {
            authToken = "asdasd"; //až bude fungovat metoda GetAccessToken(), tak ji dosadit do authToken

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public async Task<string> GetAccessToken()
        {//FIX
            StringContent queryString = new StringContent("543981021bd348419b8abcb150449f5a:e9910fe8f8fb451db7ac823c343932ad");

            HttpResponseMessage response = await Client.PostAsync("https://accounts.spotify.com/api/token", queryString);

            var token = JsonSerializer.Deserialize<string>(response.Content.ReadAsStream());

            return token;
        }

        public async Task<string> GetPlaylistId(string nameOfPlaylist)
        {
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/search" + "?q=" + nameOfPlaylist.Trim() + "&type=playlist&limit=1");
            response.EnsureSuccessStatusCode();
            JsonPlaylist playlist = JsonSerializer.Deserialize<JsonPlaylist>(response.Content.ReadAsStream(), new JsonSerializerOptions());
            return playlist.playlists.items.First().external_urls.spotify.Split("/").Last();
        }

        public async Task<JsonTracks> GetSongs(string playlistID)
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
            //Response contains whole html data of requested song and need to be scraped
            return webScraper.GetLink(await response.Content.ReadAsStringAsync());
        }
    }
}
