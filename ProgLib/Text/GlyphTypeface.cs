using System;
using System.Collections.Generic;

namespace ProgLib.Text
{
    /// <summary>
    /// Предоставляет информацию о глифе шрифта.
    /// </summary>
    public class GlyphTypeface
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GlyphTypeface"/>.
        /// </summary>
        /// <param name="GlyphTypeface"></param>
        public GlyphTypeface(System.Windows.Media.GlyphTypeface GlyphTypeface)
        {
            _glyphTypeface = GlyphTypeface;
        }

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="GlyphTypeface"/>, используя указанное местоположение файла шрифта.
        /// </summary>
        /// <param name="File">Местоположение файла шрифта</param>
        public GlyphTypeface(String File)
        {
            _glyphTypeface = new System.Windows.Media.GlyphTypeface(new Uri(File));
        }

        #region Variables

        private System.Windows.Media.GlyphTypeface _glyphTypeface;

        #endregion

        #region Properties

        /// <summary>
        /// Значения высоты глифов
        /// </summary>
        public IDictionary<UInt16, Double> AdvanceHeights
        {
            get
            {
                return _glyphTypeface.AdvanceHeights;
            }
        }

        /// <summary>
        /// Значения ширины глифов
        /// </summary>
        public IDictionary<UInt16, Double> AdvanceWidths
        {
            get
            {
                return _glyphTypeface.AdvanceWidths;
            }
        }

        /// <summary>
        /// Значение базового плана
        /// </summary>
        public Double Baseline
        {
            get
            {
                return _glyphTypeface.Baseline;
            }
        }

        /// <summary>
        /// Расстояние от верхней границы чёрного прямоугольника до нижнего края вектора величины глифа
        /// </summary>
        public IDictionary<UInt16, Double> BottomSideBearings
        {
            get
            {
                return _glyphTypeface.BottomSideBearings;
            }
        }

        /// <summary>
        /// Растояние от базавого плана до верха заглавной буквы английского алфавита относительно размера em
        /// </summary>
        public Double CapsHeight
        {
            get
            {
                return _glyphTypeface.CapsHeight;
            }
        }

        /// <summary>
        /// Номинальное сопоставление кодовой точки Юникода по индексу глифа, как определено в таблице "CMAP"
        /// </summary>
        public IDictionary<Int32, UInt16> CharacterToGlyphMap
        {
            get
            {
                return _glyphTypeface.CharacterToGlyphMap;
            }
        }

        /// <summary>
        /// Смещение от горизонтальной западной базового плана до низа чёрного прямоугольника глифов
        /// </summary>
        public IDictionary<UInt16, Double> DistancesFromHorizontalBaselineToBlackBoxBottom
        {
            get
            {
                return _glyphTypeface.DistancesFromHorizontalBaselineToBlackBoxBottom;
            }
        }

        /// <summary>
        /// Количество глифов
        /// </summary>
        public Int32 GlyphCount
        {
            get
            {
                return _glyphTypeface.GlyphCount;
            }
        }

        /// <summary>
        /// Высота ячейки символа относительно размера em
        /// </summary>
        public Double Height
        {
            get
            {
                return _glyphTypeface.Height;
            }
        }

        /// <summary>
        /// Расстояние от ведущего конца вектора величины глифа до левого края чёрного прямоугольника глифа
        /// </summary>
        public IDictionary<UInt16, Double> LeftSideBearings
        {
            get
            {
                return _glyphTypeface.LeftSideBearings;
            }
        }

        /// <summary>
        /// Расстояние от края чёрного прямоугольника глифа до правого конца вектора величины глифа
        /// </summary>
        public IDictionary<UInt16, Double> RightSideBearings
        {
            get
            {
                return _glyphTypeface.RightSideBearings;
            }
        }

        /// <summary>
        /// Расстояние от базового плана до зачёркивания для шрифта
        /// </summary>
        public Double StrikethroughPosition
        {
            get
            {
                return _glyphTypeface.StrikethroughPosition;
            }
        }

        /// <summary>
        /// Расстояние от верхнего конца вертикального вектора величины до верхнего края чёрного прямоугольника глифа
        /// </summary>
        public IDictionary<UInt16, Double> TopSideBearings
        {
            get
            {
                return _glyphTypeface.TopSideBearings;
            }
        }

        /// <summary>
        /// Позиция подчёркивания
        /// </summary>
        public Double UnderlinePosition
        {
            get
            {
                return _glyphTypeface.UnderlinePosition;
            }
        }

        /// <summary>
        /// Толщина подчёркивания относительно размера em
        /// </summary>
        public Double UnderlineThickness
        {
            get
            {
                return _glyphTypeface.UnderlineThickness;
            }
        }

        /// <summary>
        /// Западная x-высота относительно размера em шрифта
        /// </summary>
        public Double XHeight
        {
            get
            {
                return _glyphTypeface.XHeight;
            }
        }

        #endregion
    }
}
