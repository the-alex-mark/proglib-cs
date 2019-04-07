using System;
using System.Drawing;

namespace ProgLib.Audio.Tags
{
    public struct AudioTagsID3v1
    {
        /// <summary>
        /// Объявляет новый экземпляр для работы с Tags версии ID3v1
        /// </summary>
        /// <param name="File"></param>
        public AudioTagsID3v1(String File)
        {
            this.File = File;
            ID3v1 = TagLib.File.Create(File).GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag;
        }

        private readonly String File;
        private readonly TagLib.Id3v1.Tag ID3v1;

        /// <summary>
        /// Обложка
        /// </summary>
        public Image Picture
        {
            get
            {
                return (ID3v1.Pictures.Length != 0)
                    ? Image.FromStream(new System.IO.MemoryStream(ID3v1.Pictures[0].Data.Data))
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

                ID3v1.Pictures = new TagLib.IPicture[] { Media };
            }
        }

        /// <summary>
        /// Название альбома
        /// </summary>
        public String Album
        {
            get
            {
                return ID3v1.Album;
            }
            set
            {
                ID3v1.Album = value;
            }
        }

        /// <summary>
        /// Название песни
        /// </summary>
        public String Title
        {
            get
            {
                return ID3v1.Title;
            }
            set
            {
                ID3v1.Title = value;
            }
        }

        /// <summary>
        /// Исполнители
        /// </summary>
        public String[] Performers
        {
            get
            {
                return ID3v1.Performers;
            }
            set
            {
                ID3v1.Performers = value;
            }
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        public String Comment
        {
            get
            {
                return ID3v1.Comment;
            }
            set
            {
                ID3v1.Comment = value;
            }
        }

        /// <summary>
        /// Авторские права
        /// </summary>
        public String Copyright
        {
            get
            {
                return ID3v1.Copyright;
            }
            set
            {
                ID3v1.Copyright = value;
            }
        }

        /// <summary>
        /// Жанры
        /// </summary>
        public String[] Genres
        {
            get
            {
                return ID3v1.Genres;
            }
            set
            {
                ID3v1.Genres = value;
            }
        }

        /// <summary>
        /// Год
        /// </summary>
        public Int32 Year
        {
            get
            {
                return Convert.ToInt32(ID3v1.Year);
            }
            set
            {
                ID3v1.Year = Convert.ToUInt32(value);
            }
        }

        /// <summary>
        /// Лирика
        /// </summary>
        public String Lyrics
        {
            get
            {
                return ID3v1.Copyright;
            }
            set
            {
                ID3v1.Copyright = value;
            }
        }
    }
}
