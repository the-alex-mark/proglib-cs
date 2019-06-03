using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct HSLA
    {
        ///// <summary>
        ///// Инициализирует новый экземпляр типа <see cref="HSLA"/>, исходя из базового цвета структуры <see cref="Color"/>.
        ///// </summary>
        ///// <param name="BaseColor"></param>
        //public HSLA(Color BaseColor)
        //{
        //    HSLA _color = Convert.ToHSLA(new RGBA(BaseColor.R, BaseColor.G, BaseColor.G, BaseColor.A));

        //    this.H = _color.H;
        //    this.S = _color.S;
        //    this.L = _color.L;
        //    this.A = _color.A;
        //}

        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="HSLA"/>, исходя из указанных данных.
        /// </summary>
        /// <param name="Hue">Канал Hue. Допустимые значения - от 0 до 360</param>
        /// <param name="Saturation">Канал Saturation. Допустимые значения - от 0 до 1</param>
        /// <param name="Lightness">Канал Lightness. Допустимые значения - от 0 до 1</param>
        /// <exception cref="ArgumentException"></exception>
        public HSLA(Int32 Hue, Double Saturation, Double Lightness)
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
            this.A = 1;
        }

        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="HSLA"/>, исходя из указанных данных.
        /// </summary>
        /// <param name="Hue">Канал Hue. Допустимые значения - от 0 до 360</param>
        /// <param name="Saturation">Канал Saturation. Допустимые значения - от 0 до 1</param>
        /// <param name="Lightness">Канал Lightness. Допустимые значения - от 0 до 1</param>
        /// <param name="Alpha">Канал Alpha. Допустимые значения - от 0 до 1</param>
        /// <exception cref="ArgumentException"></exception>
        public HSLA(Int32 Hue, Double Saturation, Double Lightness, Double Alpha)
        {
            if (!Enumerable.Range(0, 360).Contains(Hue))
                throw new ArgumentException("Значения аргумента \"Hue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 360");

            if (Saturation < 0 || Saturation > 1)
                throw new ArgumentException("Значения аргумента \"Saturation\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1");

            if (Lightness < 0 || Lightness > 1)
                throw new ArgumentException("Значения аргумента \"Lightness\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1");

            if (Alpha < 0 || Alpha > 1)
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1");

            this.H = Hue;
            this.S = Saturation;
            this.L = Lightness;
            this.A = Alpha;
        }

        public Int32 H { get; set; }
        public Double S { get; set; }
        public Double L { get; set; }
        public Double A { get; set; }

        public bool Equals(HSLA Color)
        {
            return (H == Color.H) && (S == Color.S) && (L == Color.L) && (A == Color.A);
        }
        public override String ToString()
        {
            return String.Format("hsla({0}, {1}, {2}, {3})", H, S, L, A);
        }
    }
}
