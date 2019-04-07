using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio
{
    public class AudioFormats
    {
        public static String[] Song
        {
            get
            {
                return new String[]
                {
                    ".aac",
                    ".ac3",
                    ".ape",
                    ".aiff",
                    ".cda",
                    ".dff",
                    ".dsf",
                    ".fla",
                    ".flac",
                    ".it",
                    ".kar",
                    ".m4a",
                    ".m4b",
                    ".mac",
                    ".mid",
                    ".midi",
                    ".mo3",
                    ".mod",
                    ".mp+",
                    ".mp1",
                    ".mp2",
                    ".mp3",
                    ".mpga",
                    ".mpc",
                    ".mtm",
                    ".ofr",
                    ".ofs",
                    ".oga",
                    ".ogg",
                    ".opis",
                    ".rmi",
                    ".s3m",
                    ".spx",
                    ".tak",
                    ".tta",
                    ".umx",
                    ".wav",
                    ".w64",
                    ".wma",
                    ".wv",
                    ".xm"
                };
            }
        }
        
        public static String Playlist
        {
            get { return ".m3u"; }
        }
    }
}
