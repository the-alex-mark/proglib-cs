using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgLib.Audio.Tags;

namespace ProgLib.Audio
{
    /// <summary>
    /// Класс для работы с аудио файлами
    /// </summary>
    public class Song : Record
    {
        /// <summary>
        /// Объявляет экземпляр для работы с аудио файлом
        /// </summary>
        /// <param name="File"></param>
        public Song(String File)
        {
            if (System.IO.File.Exists(File))
            {
                this.URL = File;
                this.Tags = new AudioTags(File);

                TagLib.File AudioFile = TagLib.File.Create(File, TagLib.ReadStyle.Average);
                System.IO.FileInfo FI = new System.IO.FileInfo(File);

                this.Format = FI.Extension.ToUpper().Remove(0, 1);
                this.Bitrate = AudioFile.Properties.AudioBitrate;
                this.Time = AudioFile.Properties.Duration.TotalSeconds;
            }
            else
            {
                this.URL = File;
                this.Tags = new AudioTags();

                this.Format = "";
                this.Bitrate = 0;
                this.Time = 0;

                //throw new Exception("Неудалось найти указанный файл.");
            }
        }

        /// <summary>
        /// Метаданные
        /// </summary>
        public AudioTags Tags { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        public String Format { get; }

        /// <summary>
        /// Битрейт
        /// </summary>
        public Int32 Bitrate { get; }

        /// <summary>
        /// Продолжительность
        /// </summary>
        public Double Time { get; }

        /// <summary>
        /// Проверяет файл на существование
        /// </summary>
        /// <returns></returns>
        public Boolean Exists()
        {
            return System.IO.File.Exists(URL);
        }
    }
}
