using Guitarist_Helper;

string? playlistID = "https://open.spotify.com/playlist/37i9dQZF1EQncLwOalG3K7?si=00d272231653457c";
if (playlistID != null)
{
    SpotifyManager spotifyManager = new SpotifyManager(playlistID);

    var playlist = spotifyManager.GetList();
    var a = playlist.Result;
}