using System;

namespace ProgLib.Text
{
    /// <summary>
    /// Предоставляет информацию о шрифте.
    /// </summary>
    public class Typeface
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Typeface"/>.
        /// </summary>
        /// <param name="Typeface"></param>
        public Typeface(System.Windows.Media.Typeface Typeface)
        {
            _typeface = Typeface;
        }

        #region Variables

        System.Windows.Media.Typeface _typeface;

        #endregion

        #region Properties

        /// <summary>
        /// Растояние от базавого плана до верха заглавной буквы английского алфавита для шрифта
        /// </summary>
        public Double CapsHeight
        {
            get
            {
                return _typeface.CapsHeight;
            }
        }

        /// <summary>
        /// Имя семейства шрифтов, из которого создавался данный шрифт
        /// </summary>
        public String FontFamily
        {
            get
            {
                String[] Names = _typeface.FontFamily.Source.Split('#');
                return Names[Names.Length - 1];
            }
        }

        /// <summary>
        /// Значение, указывающее следует ли эмулировать полужирную плотность для глифов
        /// </summary>
        public Boolean IsBoldSimulated
        {
            get
            {
                return _typeface.IsBoldSimulated;
            }
        }

        /// <summary>
        /// Значение, указывающееследует ли эмулировать курсивный стиль для глифов
        /// </summary>
        public Boolean IsObliqueSimulated
        {
            get
            {
                return _typeface.IsObliqueSimulated;
            }
        }

        /// <summary>
        /// Расстояние от базового плана до зачёркивания для шрифта
        /// </summary>
        public Double StrikethroughPosition
        {
            get
            {
                return _typeface.StrikethroughPosition;
            }
        }

        /// <summary>
        /// Толщина зачёркивания относительно em размера шрифта
        /// </summary>
        public Double StrikethroughThickness
        {
            get
            {
                return _typeface.StrikethroughThickness;
            }
        }

        /// <summary>
        /// Стиль шрифта
        /// </summary>
        public String Style
        {
            get
            {
                return _typeface.Style.ToString();
            }
        }

        /// <summary>
        /// Расстояние от базового плана до подчёркивания для шрифта
        /// </summary>
        public Double UnderlinePosition
        {
            get
            {
                return _typeface.UnderlinePosition;
            }
        }

        /// <summary>
        /// Толщина подчёркивания относительно em размера шрифта
        /// </summary>
        public Double UnderlineThickness
        {
            get
            {
                return _typeface.UnderlineThickness;
            }
        }

        /// <summary>
        /// Относительная плотность шрифта
        /// </summary>
        public String Weight
        {
            get
            {
                return _typeface.Weight.ToString();
            }
        }

        /// <summary>
        /// Расстояние от базового плана до верха прописной буквы английского алфавита для шрифта (это расстояние включает верхние выносные элементы)
        /// </summary>
        public Double XHeight
        {
            get
            {
                return _typeface.XHeight;
            }
        }

        #endregion
    }
}
