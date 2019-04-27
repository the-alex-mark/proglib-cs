using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Numerics
{
    /// <summary>
    /// Представляет шестнадцатиричное число
    /// </summary>
    public struct Hexadecimal
    {
        public Hexadecimal(String Value)
        {
            this.Value = Value;
        }

        public static implicit operator Hexadecimal(String Values)
        {
            return new Hexadecimal(Values);
        }

        public static implicit operator String(Hexadecimal Hexadecimal)
        {
            return Hexadecimal.Value.ToUpper();
        }

        public String Value { get; set; }

        /// <summary>
        /// Преобразует значение в строковое представление
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value;
        }
    }
}
