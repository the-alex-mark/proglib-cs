using System;
using System.Drawing;
using System.Linq;

namespace ProgLib.Drawing
{
    /// <summary>
    /// Представляет цвета в терминах каналов Hue, Saturation, и Brightness (HSB).
    /// </summary>
    public struct HSB
    {
        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="HSB"/>, исходя из базового цвета структуры <see cref="Color"/>.
        /// </summary>
        /// <param name="BaseColor"></param>
        public HSB(Color BaseColor)
        {
            HSB _color = Convert.ToHSB(BaseColor);

            this.H = _color.H;
            this.S = _color.S;
            this.B = _color.B;
        }

        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="HSB"/>, исходя из указанных данных.
        /// </summary>
        /// <param name="Hue">Канал Hue. Допустимые значения - от 0 до 360</param>
        /// <param name="Saturation">Канал Saturation. Допустимые значения - от 0 до 1</param>
        /// <param name="Brightness">Канал Brightness. Допустимые значения - от 0 до 1</param>
        /// <exception cref="ArgumentException"></exception>
        public HSB(Int32 Hue, Double Saturation, Double Brightness)
        {
            if (!Enumerable.Range(0, 360).Contains(Hue))
                throw new ArgumentException("Значения аргумента \"Hue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 360");

            if (Saturation < 0 || Saturation > 1)
                throw new ArgumentException("Значения аргумента \"Saturation\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1");

            if (Brightness < 0 || Brightness > 1)
                throw new ArgumentException("Значения аргумента \"Brightness\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1");

            this.H = Hue;
            this.S = Saturation;
            this.B = Brightness;
        }

        #region Properties

        /// <summary>
        /// Получает значение Hue этой структуры <see cref="HSB"/>.
        /// </summary>
        public Int32 H
        {
            get;
        }

        /// <summary>
        /// Получает значение Saturation этой структуры <see cref="HSB"/>.
        /// </summary>
        public Double S
        {
            get;
        }

        /// <summary>
        /// Получает значение Brightness этой структуры <see cref="HSB"/>.
        /// </summary>
        public Double B
        {
            get;
        }

        #endregion

        /// <summary>
        /// Преобразует структуру <see cref="HSB"/> в удобную для восприятия строку.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return $"hsb({H}, {S}, {B})";
        }
    }
}
