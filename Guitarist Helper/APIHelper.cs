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

        private string authToken = "BQAGnvOdUpiQ0DpvR7QAgzA8FPXGqvvx8hfTOrOfENYTXCW3YL03X7q_3PW4Dv5VEeE_APXv6CpZfsNtRQKldaHzEubDGiFojBniXVNzVvlpFp8wulTNhJWPx_AFxUbZ0_SoNWuDR_5CNdafqV32tXAu6J7IownV5r27OkOhGrcSRDv9";
        public HttpClient Client = new HttpClient();
        WebScraper webScraper = new WebScraper();

        public APIHelper()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public async Task<string> getPlaylistId(string genre)
        {
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/search" + "?q=" + genre.Trim() + "&type=playlist&limit=1");
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
            HttpResponseMessage response = await Client.GetAsync("https://www.chords-and-tabs.net/songs/search/" + requestedSong);
            return webScraper.GetLink(await response.Content.ReadAsStringAsync());
        }
    }
}
