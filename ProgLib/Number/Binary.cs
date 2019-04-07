using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Number
{
    /// <summary>
    /// Представляет двоичное число
    /// </summary>
    public struct Binary
    {
        public Binary(Int32[] Values)
        {
            this.Values = (from Value in Values select (Value) == 0 ? 0 : 1).ToArray();
        }

        public Binary(Boolean[] Values)
        {
            this.Values = (from Value in Values select (Value) == false ? 0 : 1).ToArray();
        }

        public static implicit operator Binary(Int32[] Values)
        {
            return new Binary(Values);
        }
        public static implicit operator Binary(Boolean[] Values)
        {
            return new Binary(Values);
        }

        public static implicit operator Int32[] (Binary Binary)
        {
            return Binary.Values;
        }
        public static implicit operator Boolean[] (Binary Binary)
        {
            return (from Value in Binary.Values select (Value == 0) ? false : true).ToArray();
        }

        /// <summary>
        /// Данные
        /// </summary>
        public Int32[] Values { get; set; }

        /// <summary>
        /// Количество двоичных значений
        /// </summary>
        public Int32 Length { get { return Values.Length; } }

        /// <summary>
        /// Преобразует значение в строковое представление
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Values.Aggregate("", (_array, _value) => _array += _value.ToString());
        }
    }
}
