using ProgLib.Data;
using ProgLib.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib
{
    public static class StringExpansion
    {
        /// <summary>
        /// Возвращает копию этой строки, с переверённой первой буквой в верхний регистр.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static String ToFirstUpper(this String Value)
        {
            if (Value.Length > 0)
            {
                String Temp = Value.ToLower();
                return Temp[0].ToString().ToUpper() + Temp.Substring(1);
            }

            else return Value;
        }

        /// <summary>
        /// Возвращает копию этой строки, с переведённой каждой первой буквой слова в верхний регистр.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static String ToFirstsUpper(this String Value)
        {
            String Result = Value;

            if (Value.Length > 0)
            {
                Result = "";

                String[] Words = Value.ToLower().Split(' ');
                foreach (String Word in Words)
                    Result += Word[0].ToString().ToUpper() + Word.Substring(1) + " ";

                return Result;
            }

            else return Result;
        }

        /// <summary>
        /// Возвращает копию этой строки, изменяя регистр каждого символа на обратный.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private static String TransformRegister(this String Value)
        {
            String Result = Value;

            if (Value.Length > 0)
            {
                Result = "";
                foreach (Char Symbol in Value)
                    Result += (Char.IsLower(Symbol)) ? Symbol.ToString().ToUpper() : Symbol.ToString().ToLower();

                return Result;
            }

            else return Result;
        }

        /// <summary>
        /// Возвращает таблицу типа <see cref="DataTable"/> в табличном представлении типа <see cref="StringDataTable"/>.
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public static StringDataTable ToStringTable(this DataTable Table)
        {
            return new StringDataTable(Table);
        }

        /// <summary>
        /// Возвращает таблицу типа <see cref="DataTable"/> в табличном представлении типа <see cref="String"/>.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Format"></param>
        /// <returns></returns>
        public static String ToStringTable(this DataTable Table, StringDataTableFormat Format = StringDataTableFormat.Default)
        {
            return new StringDataTable(Table).Result(Format);
        }
    }
}
