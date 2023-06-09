﻿using System;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Guitarist_Helper
{
    public class APIHelper
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null refence return.

        private readonly string AccessToken;
        private HttpClient Client = new HttpClient();
        private WebScraper WebScraper = new WebScraper();

        private readonly IConfiguration _config;

        public APIHelper(IConfiguration config)
        {
            AccessToken = GetAccessToken().Result;
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            Task.Run(() => this.GetAccessToken()).Wait();

            _config = config;
        }

        public async Task<Root> GetSongNames(string playlistID)
        {
            HttpResponseMessage response = await Client.GetAsync("https://api.spotify.com/v1/playlists/" + playlistID);
            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<Root>(response.Content.ReadAsStream(), new JsonSerializerOptions());
        }

        public async Task<string> GetSongLink(string songName, string artistName)
        {
            string requestedSong = artistName.Replace(" ", "%20") + "%20" + songName.Replace(" ", "%20");
            HttpResponseMessage response = await Client.GetAsync("https://www.chords-and-tabs.net/songs/search/" + requestedSong);
            response.EnsureSuccessStatusCode();
            //Response contains whole html data of requested song and need to be scraped
            return WebScraper.GetLink(await response.Content.ReadAsStringAsync());
        }

        private async Task<string> GetAccessToken()
        {
            HttpClient spotifyClient = GetSpotifyClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Content = new StringContent("grant_type=client_credentials", Encoding.Default, "application/x-www-form-urlencoded");
            var response = await spotifyClient.PostAsync(spotifyClient.BaseAddress, request.Content);
            response.EnsureSuccessStatusCode();

            var token = JsonSerializer.Deserialize<AccessToken>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions());
            return token.access_token;
        }

        private HttpClient GetSpotifyClient()
        {
            //Created instance of HttpClient, because it needs special settings to get access token
            HttpClient spotifyClient = new HttpClient();
            byte[] clientDataByte = Encoding.UTF8.GetBytes("e73883597a9346cab465adad28569272:13a30406eac44974b05b00547daa0149");
            var clientData = Convert.ToBase64String(clientDataByte);

            spotifyClient.BaseAddress = new Uri("https://accounts.spotify.com/api/token");
            spotifyClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientData);
            return spotifyClient;
        }
    }
}
