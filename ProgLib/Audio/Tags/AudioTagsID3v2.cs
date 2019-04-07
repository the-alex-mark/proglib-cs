using System;
using System.Drawing;

namespace ProgLib.Audio.Tags
{
    public struct AudioTagsID3v2
    {
        /// <summary>
        /// Объявляет новый экземпляр для работы с Tags версии ID3v2
        /// </summary>
        /// <param name="File"></param>
        public AudioTagsID3v2(String File)
        {
            this.File = File;
            ID3v2 = TagLib.File.Create(File).GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag;
        }

        private readonly String File;
        private readonly TagLib.Id3v2.Tag ID3v2;

        /// <summary>
        /// Обложка
        /// </summary>
        public Image Picture
        {
            get
            {
                return (ID3v2.Pictures.Length != 0)
                    ? Image.FromStream(new System.IO.MemoryStream(ID3v2.Pictures[0].Data.Data))
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

                ID3v2.Pictures = new TagLib.IPicture[] { Media };
            }
        }

        /// <summary>
        /// Название альбома
        /// </summary>
        public String Album
        {
            get
            {
                return ID3v2.Album;
            }
            set
            {
                ID3v2.Album = value;
            }
        }

        /// <summary>
        /// Название песни
        /// </summary>
        public String Title
        {
            get
            {
                return ID3v2.Title;
            }
            set
            {
                ID3v2.Title = value;
            }
        }

        /// <summary>
        /// Исполнители
        /// </summary>
        public String[] Performers
        {
            get
            {
                return ID3v2.Performers;
            }
            set
            {
                ID3v2.Performers = value;
            }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public String Comment
        {
            get
            {
                return ID3v2.Comment;
            }
            set
            {
                ID3v2.Comment = value;
            }
        }

        /// <summary>
        /// Авторские права
        /// </summary>
        public String Copyright
        {
            get
            {
                return ID3v2.Copyright;
            }
            set
            {
                ID3v2.Copyright = value;
            }
        }

        /// <summary>
        /// Жанры
        /// </summary>
        public String[] Genres
        {
            get
            {
                return ID3v2.Genres;
            }
            set
            {
                ID3v2.Genres = value;
            }
        }

        /// <summary>
        /// Год
        /// </summary>
        public Int32 Year
        {
            get
            {
                return Convert.ToInt32(ID3v2.Year);
            }
            set
            {
                ID3v2.Year = Convert.ToUInt32(value);
            }
        }

        /// <summary>
        /// Лирика
        /// </summary>
        public String Lyrics
        {
            get
            {
                return ID3v2.Copyright;
            }
            set
            {
                ID3v2.Copyright = value;
            }
        }
    }
}
