using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitarist_Helper
{
    internal class JsonPlaylist
    {   //This set of classes represents json parameters that u get from spotify api
        public Playlists? playlists { get; set; }
    }

    public class Playlists
    {
        public List<PlaylistItems>? items { get; set; }
    }

    public class PlaylistItems
    {
        public ExternalUrls? external_urls { get; set; }
    }
}
