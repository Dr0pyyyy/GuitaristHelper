using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitarist_Helper
{
    public interface IMusicManager
    {

        APIHelper ApiHelper { get; set; }

        Task<List<string>> GetPlaylist(string playlistID);
    }
}
