using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace ProgLib
{
    public static class ImageExpansion
    {
        /// <summary>
        /// Преобразует изображение в массив байтов.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Byte[] ToByteArray(this Image Value)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                Value.Save(MS, Value.RawFormat);
                return MS.ToArray();
            }
        }

        /// <summary>
        /// Конвертирует изображение в "Base64String".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToBase64String(this Image Value)
        {
            try
            {
                return Convert.ToBase64String(Value.ToByteArray());
            }
            catch { throw new Exception("Входной параметр \"Value\" имел неверный формат"); }
        }

        /// <summary>
        /// Преобразует строку соответствующего формата в изображение.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Image ToImage(this String Value)
        {
            try
            {
                return Image.FromStream(new MemoryStream(Convert.FromBase64String(Value)));
            }
            catch { throw new Exception("Входной параметр \"Value\" имел неверный формат"); }
        }

        public static Image ToImage(this IPicture Value)
        {
            return (Value != null) 
                ? Image.FromStream(new System.IO.MemoryStream(Value.Data.Data)) 
                : null;
        }

        public static IPicture ToIPicture(this Image Value)
        {
            TagLib.Id3v2.AttachmentFrame Picture = new TagLib.Id3v2.AttachmentFrame
            {
                Type = PictureType.Media,
                Description = "",
                MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                Data = Value.ToByteArray(),
                TextEncoding = TagLib.StringType.UTF16
            };

            return Picture;
        }
    }
}
