using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="Text"></param>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static String ToLatin(String Text, TranslitType Type = TranslitType.ISO)
        {
            return Translit.ToLatin(Text, Type);
        }

        /// <summary>
        /// Преобразует латинский текст в кириллистический, в указанном формате.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static String ToCyrillic(String Text, TranslitType Type = TranslitType.ISO)
        {
            return Translit.ToCyrillic(Text, Type);
        }

        /// <summary>
        /// Возвращает новую строку, в которой все вхождения заданных знаков Юникода в текущем экземпляре заменены другим заданным знаком Юникода.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="OldChars">Заменяемый знак Юникода</param>
        /// <param name="NewChar">Знак Юникода для замены всех обнаруженных вхождений OldChars</param>
        /// <returns></returns>
        public static String Replace(this String Text, Char[] OldChars, Char NewChar)
        {
            foreach (Char Symbol in OldChars)
                Text = Text.Replace(Symbol, NewChar);

            return Text;
        }

        /// <summary>
        /// Возвращает новую строку, в которой все вхождения заданных строк в текущем экземпляре заменены другой заданной строкой.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="OldValues">Строка, которую требуется заменить</param>
        /// <param name="NewValue">Строка для замены всех вхождений OldValues</param>
        /// <returns></returns>
        public static String Replace(this String Text, String[] OldValues, String NewValue)
        {
            foreach (String Value in OldValues)
                Text = Text.Replace(Value, NewValue);

            return Text;
        }

        /// <summary>
        /// Возвращает копию этой строки, с переверённой первой буквой в верхний регистр.
        /// Пример: "Как в предложениях".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToFirstUpper(this String Value)
        {
            return TextRegister.ToFirstUpper(Value);
        }

        /// <summary>
        /// Возвращает копию этой строки, с переведённой каждой первой буквой слова в верхний регистр.
        /// Пример: "Начинать С Прописных".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToFirstsUpper(this String Value)
        {
            return TextRegister.ToFirstsUpper(Value);
        }

        /// <summary>
        /// Возвращает копию этой строки, изменяя регистр каждого символа на обратный.
        /// Пример: "иЗМЕНИТЬ РЕГИСТР".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String TransformRegister(this String Value)
        {
            return TextRegister.TransformRegister(Value);
        }

        /// <summary>
        /// Возвращает таблицу типа <see cref="DataTable"/> в табличном представлении типа <see cref="StringDataTable"/>.
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public static StringTable ToStringTable(this DataTable Table)
        {
            return new StringTable(Table);
        }

        /// <summary>
        /// Возвращает таблицу типа <see cref="DataTable"/> в табличном представлении типа <see cref="String"/>.
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Format"></param>
        /// <returns></returns>
        public static String ToStringTable(this DataTable Table, StringTableFormat Format = StringTableFormat.Default)
        {
            return new StringTable(Table).Result(Format);
        }
    }
}
