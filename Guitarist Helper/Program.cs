using Guitarist_Helper;

Console.Write("Insert playlist ID: ");
string playlistID = Console.ReadLine();

if (playlistID != null)
{
    SpotifyManager spotifyManager = new SpotifyManager(playlistID);
    var playlist = spotifyManager.GetList().Result;
}

Console.ReadKey();


//MAIN TODO = Implement ILogger and log functions