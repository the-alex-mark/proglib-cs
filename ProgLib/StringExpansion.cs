using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProgLib.Data;
using ProgLib.Text;

namespace ProgLib
{
    public static class StringExpansion
    {
        // TODO: Завершить работу над функцией поиска подстроки
        public static Boolean Like(this String Value1, String Value2)
        {



            return false;
        }

        /// <summary>
        /// Преобразует кириллистический текст в латинский, в указанном формате.
        /// </summary>
        /// <param name="Text">Преобразуемый текст</param>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static String ToLatin(String Text, TranslitType Type = TranslitType.ISO)
        {
            return Translit.ToLatin(Text, Type);
        }

        /// <summary>
        /// Преобразует латинский текст в кириллистический, в указанном формате.
        /// </summary>
        /// <param name="Text">Преобразуемый текст</param>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static String ToCyrillic(String Text, TranslitType Type = TranslitType.ISO)
        {
            return Translit.ToCyrillic(Text, Type);
        }

        /// <summary>
        /// Возвращает копию этой строки, с переверённой первой буквой в верхний регистр.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToFirstUpper(this String Value)
        {
            return FontRegister.ToFirstsUpper(Value);
        }

        /// <summary>
        /// Возвращает копию этой строки, с переведённой каждой первой буквой слова в верхний регистр.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToFirstsUpper(this String Value)
        {
            return FontRegister.ToFirstsUpper(Value);
        }

        /// <summary>
        /// Возвращает копию этой строки, изменяя регистр каждого символа на обратный.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String TransformRegister(this String Value)
        {
            return FontRegister.TransformRegister(Value);
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
