using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Guitarist_Helper
{
    internal class JsonTracks
    {
        public ExternalUrls? external_urls { get; set; } //Link to playlist
        public string? name { get; set; } //Name of playlist
        public Tracks? tracks { get; set; }
    }

    public class Tracks
    {
        public List<Item>? items { get; set; } //Jednotlivé tracky
    }

    public class Item
    {
        public Track? track { get; set; }
    }

    public class Track
    {
        public List<Artist>? artists { get; set; }
        public ExternalUrls? external_urls { get; set; }
        public string? name { get; set; }
        public string? preview_url { get; set; }
    }

    public class Artist
    {
        public string? name { get; set; }
    }
}
