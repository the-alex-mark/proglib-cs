using System;
using System.Drawing;
using System.Linq;

namespace ProgLib.Drawing
{
    /// <summary>
    /// Представляет цвета в терминах каналов Hue, Saturation, и Lightness (HSL).
    /// </summary>
    public struct HSL
    {
        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="HSL"/>, исходя из базового цвета структуры <see cref="Color"/>.
        /// </summary>
        /// <param name="BaseColor"></param>
        public HSL(Color BaseColor)
        {
            HSL _color = Convert.ToHSL(BaseColor);

            this.H = _color.H;
            this.S = _color.S;
            this.L = _color.L;
        }

        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="HSL"/>, исходя из указанных данных.
        /// </summary>
        /// <param name="Hue">Канал Hue. Допустимые значения - от 0 до 360</param>
        /// <param name="Saturation">Канал Saturation. Допустимые значения - от 0 до 1</param>
        /// <param name="Lightness">Канал Lightness. Допустимые значения - от 0 до 1</param>
        /// <exception cref="ArgumentException"></exception>
        public HSL(Int32 Hue, Double Saturation, Double Lightness)
        {
            if (!Enumerable.Range(0, 360).Contains(Hue))
                throw new ArgumentException("Значения аргумента \"Hue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 360");

            if (Saturation < 0 || Saturation > 1)
                throw new ArgumentException("Значения аргумента \"Saturation\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1");

            if (Lightness < 0 || Lightness > 1)
                throw new ArgumentException("Значения аргумента \"Lightness\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1");

            this.H = Hue;
            this.S = Saturation;
            this.L = Lightness;
        }
        
        #region Properties

        /// <summary>
        /// Получает значение Hue этой структуры <see cref="HSL"/>.
        /// </summary>
        public Int32 H
        {
            get;
        }

        /// <summary>
        /// Получает значение Saturation этой структуры <see cref="HSL"/>.
        /// </summary>
        public Double S
        {
            get;
        }

        /// <summary>
        /// Получает значение Lightness этой структуры <see cref="HSL"/>.
        /// </summary>
        public Double L
        {
            get;
        }

        #endregion

        /// <summary>
        /// Преобразует структуру <see cref="HSL"/> в удобную для восприятия строку.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return $"hsl({H}, {S}, {L})";
        }
    }
}
