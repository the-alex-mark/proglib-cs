using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace ProgLib.Text
{
    /// <summary>
    /// Предоставляет информацию о семестве шрифтов.
    /// </summary>
    public class FontInfo
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FontInfo"/>, используя указанное местоположение файла шрифта.
        /// </summary>
        /// <param name="File">Местоположение файла шрифта</param>
        public FontInfo(String File)
        {
            _glyphTypeface = new System.Windows.Media.GlyphTypeface(new Uri(File));
            _typefaces = Fonts.GetTypefaces(File);
        }

        #region Variables

        private System.Windows.Media.GlyphTypeface _glyphTypeface;
        ICollection<System.Windows.Media.Typeface> _typefaces;

        #endregion

        #region Properties

        /// <summary>
        /// Список шрифтов данного файла
        /// </summary>
        public List<Typeface> Typefaces
        {
            get
            {
                List<Typeface> Typefaces = new List<Typeface>();
                foreach (System.Windows.Media.Typeface Typeface in _typefaces)
                    Typefaces.Add(new Typeface(Typeface));

                return Typefaces;
            }
        }

        /// <summary>
        /// Информация о глифе шрифта
        /// </summary>
        public GlyphTypeface Glyph
        {
            get
            {
                return new GlyphTypeface(_glyphTypeface);
            }
        }

        /// <summary>
        /// Авторские права
        /// </summary>
        public String Copyright
        {
            get
            {
                return _glyphTypeface.Copyrights[new System.Globalization.CultureInfo("en-US")];
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public String Description
        {
            get
            {
                return _glyphTypeface.Descriptions[new System.Globalization.CultureInfo("en-US")];
            }
        }

        /// <summary>
        /// Дизайнер
        /// </summary>
        public String Designer
        {
            get
            {
                return _glyphTypeface.DesignerNames[new System.Globalization.CultureInfo("en-US")];
            }
        }

        /// <summary>
        /// Имя семейства шрифта
        /// </summary>
        public String FamilyName
        {
            get
            {
                return _glyphTypeface.FamilyNames[new System.Globalization.CultureInfo("en-US")];
            }
        }

        /// <summary>
        /// Лицензия
        /// </summary>
        public String License
        {
            get
            {
                return _glyphTypeface.LicenseDescriptions[new System.Globalization.CultureInfo("en-US")];
            }
        }

        /// <summary>
        /// Производитель
        /// </summary>
        public String Manufacturer
        {
            get
            {
                return _glyphTypeface.ManufacturerNames[new System.Globalization.CultureInfo("en-US")];
            }
        }

        /// <summary>
        /// Стиль шрифта
        /// </summary>
        public String Style
        {
            get
            {
                return _glyphTypeface.Style.ToString();
            }
        }

        /// <summary>
        /// Значение, указывающее, соответствует ли шрифт кодировке Юникод
        /// </summary>
        public Boolean Symbol
        {
            get
            {
                return _glyphTypeface.Symbol;
            }
        }

        /// <summary>
        /// Товарный знак
        /// </summary>
        public String Trademark
        {
            get
            {
                return _glyphTypeface.Trademarks[new System.Globalization.CultureInfo("en-US")];
            }
        }

        /// <summary>
        /// Версия шрифта
        /// </summary>
        public Double Version
        {
            get
            {
                return _glyphTypeface.Version;
            }
        }

        /// <summary>
        /// Проектная ширина шрифта
        /// </summary>
        public String Weight
        {
            get
            {
                return _glyphTypeface.Weight.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает поток файла шрифта.
        /// </summary>
        /// <returns></returns>
        public Stream GetFontStream()
        {
            return _glyphTypeface.GetFontStream();
        }

        #endregion
    }
}
