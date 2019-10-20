using System;

namespace ProgLib
{
    /// <summary>
    /// Представляет восьмиричное число
    /// </summary>
    public struct Octal
    {
        public Octal(Int32 Value)
        {
            this.Value = Value;
        }

        public static implicit operator Octal(Int32 Values)
        {
            return new Octal(Values);
        }

        public static implicit operator Int32(Octal Octal)
        {
            return Octal.Value;
        }

        public Int32 Value { get; set; }

        /// <summary>
        /// Преобразует значение в строковое представление
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
