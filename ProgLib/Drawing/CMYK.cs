using System;
using System.Drawing;
using System.Linq;

namespace ProgLib.Drawing
{
    /// <summary>
    /// Представляет цвета в терминах каналов Cyan, Magenta, Yellow и Key (CMYK)
    /// </summary>
    public struct CMYK
    {
        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="CMYK"/>, исходя из базового цвета структуры <see cref="Color"/>.
        /// </summary>
        /// <param name="BaseColor"></param>
        public CMYK(Color BaseColor)
        {
            CMYK _color = Convert.ToCMYK(BaseColor);

            this.C = _color.C;
            this.M = _color.M;
            this.Y = _color.Y;
            this.K = _color.K;
        }

        /// <summary>
        /// Инициализирует новый экземпляр типа <see cref="CMYK"/>, исходя из указанных данных. 
        /// </summary>
        /// <param name="Cyan">Канал Cyan. Допустимые значения - от 0 до 100</param>
        /// <param name="Magenta">Канал Magenta. Допустимые значения - от 0 до 100</param>
        /// <param name="Yellow">Канал Yellow. Допустимые значения - от 0 до 100</param>
        /// <param name="Key">Канал Key. Допустимые значения - от 0 до 100</param>
        /// <exception cref="ArgumentException"></exception>
        public CMYK(Int32 Cyan, Int32 Magenta, Int32 Yellow, Int32 Key)
        {
            if (!Enumerable.Range(0, 100).Contains(Cyan))
                throw new ArgumentException("Значения аргумента \"Cyan\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100");

            if (!Enumerable.Range(0, 100).Contains(Magenta))
                throw new ArgumentException("Значения аргумента \"Magenta\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100");

            if (!Enumerable.Range(0, 100).Contains(Yellow))
                throw new ArgumentException("Значения аргумента \"Yellow\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100");

            if (!Enumerable.Range(0, 100).Contains(Key))
                throw new ArgumentException("Значения аргумента \"Key\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100");

            this.C = Cyan;
            this.M = Magenta;
            this.Y = Yellow;
            this.K = Key;
        }

        #region Properties

        /// <summary>
        /// Получает значение Cyan этой структуры <see cref="CMYK"/>.
        /// </summary>
        public Int32 C
        {
            get;
        }

        /// <summary>
        /// Получает значение Magenta этой структуры <see cref="CMYK"/>.
        /// </summary>
        public Int32 M
        {
            get;
        }

        /// <summary>
        /// Получает значение Yellow этой структуры <see cref="CMYK"/>.
        /// </summary>
        public Int32 Y
        {
            get;
        }

        /// <summary>
        /// Получает значение Key этой структуры <see cref="CMYK"/>.
        /// </summary>
        public Int32 K
        {
            get;
        }

        #endregion
        
        /// <summary>
        /// Преобразует структуру <see cref="CMYK"/> в удобную для восприятия строку.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return $"cmyk({C}, {M}, {Y}, {K})";
        }
    }
}
