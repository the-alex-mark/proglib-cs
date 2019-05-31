using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio
{
    public class AudioFormats
    {
        /// <summary>
        /// Возврящает список форматов звуковых файлов.
        /// </summary>
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
        
        /// <summary>
        /// Возвращает список форматов файлов плейлистов.
        /// </summary>
        public static String[] Playlist
        {
            get
            {
                return new String[]
                {
                    ".asx",
                    ".aimppl",
                    ".aimppl4",
                    ".cue",
                    ".m3u",
                    ".m3u8",
                    ".pls",
                    ".wax",
                    ".xspf"
                };
            }
        }
    }
}
