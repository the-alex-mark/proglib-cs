using System;

namespace ProgLib.Audio
{
    /// <summary>
    /// Предоставляет методы для работы со звуковыми файлами.
    /// </summary>
    public class Song : Record
    {
        /// <summary>
        /// Инициализирует экземпляр типа <see cref="Song"/> для работы с аудио файлом.
        /// </summary>
        /// <param name="URL"></param>
        public Song(String URL)
        {
            if (System.IO.File.Exists(URL))
            {
                this.URL = URL;
                this.Tags = new AudioTags(URL);

                TagLib.File AudioFile = TagLib.File.Create(URL, TagLib.ReadStyle.Average);
                System.IO.FileInfo FI = new System.IO.FileInfo(URL);

                this.Format = FI.Extension.ToUpper().Remove(0, 1);
                this.Bitrate = AudioFile.Properties.AudioBitrate;
                this.Time = AudioFile.Properties.Duration;
            }
            else { throw new Exception("Неверное расположение файла!"); }
        }

        #region Properties

        /// <summary>
        /// Метаданные
        /// </summary>
        public AudioTags Tags
        {
            get;
            set;
        }

        /// <summary>
        /// Тип файла
        /// </summary>
        public String Format
        {
            get;
        }

        /// <summary>
        /// Битрейт
        /// </summary>
        public Int32 Bitrate
        {
            get;
        }

        /// <summary>
        /// Продолжительность
        /// </summary>
        public TimeSpan Time
        {
            get;
        }

        #endregion

        /// <summary>
        /// Проверяет файл на существование.
        /// </summary>
        /// <returns></returns>
        public Boolean Exists()
        {
            return System.IO.File.Exists(this.URL);
        }
    }
}
