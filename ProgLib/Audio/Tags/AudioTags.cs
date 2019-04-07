using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio.Tags
{
    public struct AudioTags
    {
        /// <summary>
        /// Объявляет новый экземпляр для работы с Tags
        /// </summary>
        /// <param name="File"></param>
        public AudioTags(String File)
        {
            this.All = new AudioTagsAll(File);
            this.ID3v1 = new AudioTagsID3v1(File);
            this.ID3v2 = new AudioTagsID3v1(File);
        }

        /// <summary>
        /// Экземпляр для работы с Tags
        /// </summary>
        public AudioTagsAll All { get; set; }

        /// <summary>
        /// Экземпляр для работы с Tags версии ID3v1
        /// </summary>
        public AudioTagsID3v1 ID3v1 { get; set; }

        /// <summary>
        /// Экземпляр для работы с Tags версии ID3v2
        /// </summary>
        public AudioTagsID3v1 ID3v2 { get; set; }
    }
}
