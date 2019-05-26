using System;

namespace ProgLib.Audio
{
    /// <summary>
    /// Предоставляет метаданные звукового файла.
    /// </summary>
    public class AudioTags
    {
        /// <summary>
        /// Инициализирует пустой экземпляр класса <see cref="AudioTags"/> для работы с метаданными звукового файла.
        /// </summary>
        public AudioTags()
        {
            this.ID3v1 = null;
            this.ID3v2 = null;
        }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="AudioTags"/> для работы с метаданными звукового файла.
        /// </summary>
        /// <param name="File"></param>
        public AudioTags(String File)
        {
            this.ID3v1 = TagLib.File.Create(File).GetTag(TagLib.TagTypes.Id3v1) as TagLib.Id3v1.Tag;
            this.ID3v2 = TagLib.File.Create(File).GetTag(TagLib.TagTypes.Id3v2) as TagLib.Id3v2.Tag;
        }

        #region Properties

        /// <summary>
        /// Экземпляр для работы с метаданными версии ID3v1
        /// </summary>
        public TagLib.Id3v1.Tag ID3v1
        {
            get;
            set;
        }

        /// <summary>
        /// Экземпляр для работы с метаданными версии ID3v2
        /// </summary>
        public TagLib.Id3v2.Tag ID3v2
        {
            get;
            set;
        }

        #endregion
    }
}
