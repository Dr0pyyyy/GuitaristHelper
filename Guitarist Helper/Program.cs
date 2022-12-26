using Guitarist_Helper;

string? genre = "pop mix";
if (genre != null)
{
    SpotifyManager spotifyManager = new SpotifyManager(genre);

    var result = await spotifyManager.GetLinks();
}

