using ProgLib.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgLib
{
    public static class AudioExpansion
    {
        /// <summary>
        /// Проверяет, является ли строка адресом звукового файла.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean IsSong(this String URL)
        {
            if (!URL.IsRadio())
            {
                List<String> Formats = AudioFormats.Song.ToList();
                System.IO.FileInfo Information = new System.IO.FileInfo(URL);

                if (Information.Exists)
                    return (Formats.IndexOf(Information.Extension.ToLower()) > -1) ? true : false;
            }

            return false;
        }

        /// <summary>
        /// Проверяет, является ли строка адресом интернет радиостанции.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean IsRadio(this String URL)
        {
            return (!URL.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) && !URL.StartsWith("www", StringComparison.CurrentCultureIgnoreCase)) ? true : false;
        }

        /// <summary>
        /// Проверяет, является ли строка адресом файла плейлиста.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean IsPlaylist(this String URL)
        {
            if (!URL.IsRadio())
            {
                List<String> Formats = AudioFormats.Playlist.ToList();
                System.IO.FileInfo Information = new System.IO.FileInfo(URL);

                if (Information.Exists)
                    return (Formats.IndexOf(Information.Extension.ToLower()) > -1) ? true : false;
            }

            return false;
        }

        /// <summary>
        /// Возвращает значение типа <see cref="AudioType"/>, на которое ссылается адрес.
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static AudioType GetAudioType(this String URL)
        {
            if (URL.IsSong())
                return AudioType.Song;
            else

            if (URL.IsRadio())
                return AudioType.Radio;
            else

            if (URL.IsPlaylist())
                return AudioType.Playlist;
            else

                return AudioType.None;
        }
    }
}
