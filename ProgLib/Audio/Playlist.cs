using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio
{
    /// <summary>
    /// Класс для работы с плейлистами.
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Инициализирует экземпляр для работы с плейлистом.
        /// </summary>
        public Playlist()
        {
            this.Name = "";
            this.Records = new List<Record>();
        }

        /// <summary>
        /// Инициализирует экземпляр для работы с плейлистом.
        /// </summary>
        /// <param name="Name"></param>
        public Playlist(String Name)
        {
            this.Name = Name;
            this.Records = new List<Record>();
        }

        /// <summary>
        /// Инициализирует экземпляр для работы с плейлистом.
        /// </summary>
        /// <param name="Name">Назвние плейлиста</param>
        /// <param name="Addresses">Список записей</param>
        public Playlist(String Name, String[] Addresses)
        {
            this.Name = Name;
            this.Records = new List<Record>();

            foreach (String Address in Addresses)
            {
                this.Records.Add(
                    (Address.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) || Address.StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
                        ? (Record)new Radio(Address)
                        : (Record)new Song(Address));
            }
        }

        /// <summary>
        /// Инициализирует экземпляр для работы с плейлистом.
        /// </summary>
        /// <param name="Name">Назвние плейлиста</param>
        /// <param name="Records">Список записей</param>
        public Playlist(String Name, Record[] Records)
        {
            this.Name = Name;
            this.Records = Records.ToList();
        }

        /// <summary>
        /// Название плейлиста
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Список записей
        /// </summary>
        public List<Record> Records { get; set; }

        /// <summary>
        /// Получает данные из файла плейлиста.
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public static Playlist Load(String File)
        {
            String[] Content = System.IO.File.ReadAllLines(File);
            Playlist _playlist = new Playlist
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(File)
            };

            if (Content[0].Trim().ToUpper() == "#EXTM3U")
            {
                for (int i = 0; i < Content.Length; i++)
                {
                    if (Content[i].StartsWith("#EXTINF", StringComparison.CurrentCultureIgnoreCase))
                    {
                        String[] Information = Content[i].Split(new String[] { ":", "," }, StringSplitOptions.None);
                        if (Information.Length > 2)
                        {
                            _playlist.Records.Add(
                                (Content[i + 1].StartsWith("http", StringComparison.CurrentCultureIgnoreCase) || Content[i + 1].StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
                                    ? (Record)new Radio(Information[2], Content[i + 1])
                                    : (Record)new Song(Content[i + 1]));
                        }
                    }
                    else if (Content[i].StartsWith("# ", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _playlist.Records.Add(
                                (Content[i + 1].StartsWith("http", StringComparison.CurrentCultureIgnoreCase) || Content[i + 1].StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
                                    ? (Record)new Radio(Content[i + 1], Content[i + 1])
                                    : (Record)new Song(Content[i + 1]));
                    }
                }
            }

            return _playlist;
        }

        /// <summary>
        /// Сохраняет плейлист по указанному расположению.
        /// </summary>
        /// <param name="File"></param>
        public void Save(String File)
        {
            String Content = "#EXTM3U";

            if (Records.Count > 0)
            {
                foreach (Record Record in Records)
                {
                    if (Record is Song)
                    {
                        Content += String.Format(
                            "\n" + "#EXTINF:{0},{1}" + "\n" + "{2}",
                            "-1", System.IO.Path.GetFileNameWithoutExtension(Record.URL), Record.URL);
                    }

                    if (Record is Radio)
                    {
                        Content += String.Format(
                            "\n" + "#EXTINF:{0},{1}" + "\n" + "{2}",
                            "-1", ((Record as Radio).Name != "") ? (Record as Radio).Name : Record.URL, Record.URL);
                    }
                }
            }
            else { throw new Exception("Список файлов был пуст"); }

            System.IO.File.WriteAllText(File, Content);
        }
    }
}