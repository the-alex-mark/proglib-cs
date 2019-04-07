using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio.Tags
{
    public struct AudioTagsAll
    {
        /// <summary>
        /// Объявляет новый экземпляр для работы с Tags
        /// </summary>
        /// <param name="File"></param>
        public AudioTagsAll(String File)
        {
            this.Song = TagLib.File.Create(File);
        }

        private readonly TagLib.File Song;

        /// <summary>
        /// Обложка
        /// </summary>
        public Image Picture
        {
            get
            {
                return (Song.Tag.Pictures.Length != 0)
                            ? Image.FromStream(new System.IO.MemoryStream(Song.Tag.Pictures[0].Data.Data))
                            : null;
            }
            set
            {
                TagLib.Id3v2.AttachedPictureFrame Media = new TagLib.Id3v2.AttachedPictureFrame
                {
                    Type = TagLib.PictureType.Media,
                    Description = "",
                    MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                    Data = value.ToByteArray(),
                    TextEncoding = TagLib.StringType.UTF16
                };

                Song.Tag.Pictures = new TagLib.IPicture[] { Media };
            }
        }

        /// <summary>
        /// Название альбома
        /// </summary>
        public String Album
        {
            get
            {
                return Song.Tag.Album;
            }
            set
            {
                Song.Tag.Album = value;
            }
        }

        /// <summary>
        /// Название песни
        /// </summary>
        public String Title
        {
            get
            {
                return Song.Tag.Title;
            }
            set
            {
                Song.Tag.Title = value;
            }
        }

        /// <summary>
        /// Исполнители
        /// </summary>
        public String[] Performers
        {
            get
            {
                return Song.Tag.Performers;
            }
            set
            {
                Song.Tag.Performers = value;
            }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public String Comment
        {
            get
            {
                return Song.Tag.Comment;
            }
            set
            {
                Song.Tag.Comment = value;
            }
        }

        /// <summary>
        /// Авторские права
        /// </summary>
        public String Copyright
        {
            get
            {
                return Song.Tag.Copyright;
            }
            set
            {
                Song.Tag.Copyright = value;
            }
        }

        /// <summary>
        /// Жанры
        /// </summary>
        public String[] Genres
        {
            get
            {
                return Song.Tag.Genres;
            }
            set
            {
                Song.Tag.Genres = value;
            }
        }

        /// <summary>
        /// Год
        /// </summary>
        public Int32 Year
        {
            get
            {
                return Convert.ToInt32(Song.Tag.Year);
            }
            set
            {
                Song.Tag.Year = Convert.ToUInt32(value);
            }
        }

        /// <summary>
        /// Лирика
        /// </summary>
        public String Lyrics
        {
            get
            {
                return Song.Tag.Copyright;
            }
            set
            {
                Song.Tag.Copyright = value;
            }
        }
    }
}
