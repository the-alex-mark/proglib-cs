using System;
using System.Collections.Generic;
using System.Linq;

namespace ProgLib.Audio
{
    /// <summary>
    /// Предоставляет методы для работы с аудио файлами
    /// </summary>
    public class Audio
    {
        /// <summary>
        /// Проверка типа файла
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean AudioCheck(String URL)
        {
            Boolean Check = false;

            if (!URL.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) && !URL.StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
            {
                List<String> Formats = AudioFormats.Song.Cast<String>().ToList();
                System.IO.FileInfo Information = new System.IO.FileInfo(URL);

                if (Information.Exists)
                    Check = (Formats.IndexOf(Information.Extension.ToLower()) > -1) ? true : false;
            }
                        
            return Check;
        }

        /// <summary>
        /// Проверка типа файла
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean RadioCheck(String URL)
        {
            return (URL.StartsWith("http", StringComparison.CurrentCultureIgnoreCase)) ? true : false;
        }

        /// <summary>
        /// Проверка типа файла
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static Boolean PlaylistCheck(String URL)
        {
            Boolean Check = false;

            if (!URL.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) && !URL.StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
            {
                List<String> Formats = AudioFormats.Song.Cast<String>().ToList();
                System.IO.FileInfo Information = new System.IO.FileInfo(URL);

                if (Information.Exists)
                    Check = (Information.Extension.ToLower() == AudioFormats.Playlist) ? true : false;
            }
            
            return Check;
        }

        /// <summary>
        /// Проверка типа файла
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static AudioType Check(String URL)
        {
            AudioType Check = AudioType.None;

            if (AudioCheck(URL)) { Check = AudioType.Song; }
            else
            {
                if (RadioCheck(URL)) { Check = AudioType.Radio; }
                else
                {
                    if (PlaylistCheck(URL)) { Check = AudioType.Playlist; }
                    else
                    {
                        Check = AudioType.None;
                    }
                }
            }

            return Check;
        }
    }

    public enum AudioType
    {
        None,
        Song,
        Radio,
        Playlist
    }
}
