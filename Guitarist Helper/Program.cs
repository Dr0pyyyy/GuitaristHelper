using Guitarist_Helper;
string? nameOfPlaylist = "pop mix";
if (nameOfPlaylist != null)
{   
    SpotifyManager spotifyManager = new SpotifyManager(nameOfPlaylist);

    var result = await spotifyManager.GetList();
}

