using System;
using System.Collections.Generic;

namespace ProgLib.Text
{
    /// <summary>
    /// Представляет методы для преобразования текста в различные алфавитные.
    /// </summary>
    public class Translit
    {
        #region Variables

        /// <summary>
        /// Словарь (ГОСТ 16876-71)
        /// </summary>
        private static Dictionary<String, String> GOST = new Dictionary<String, String>
        {
            //{ "Є", "EH" },
            //{ "І", "I" },
            //{ "і", "i" },
            { "№", "#" },
            //{ "є", "eh" },
            { "А", "A" },
            { "Б", "B" },
            { "В", "V" },
            { "Г", "G" },
            { "Д", "D" },
            { "Е", "E" },
            { "Ё", "JO" },
            { "Ж", "ZH" },
            { "З", "Z" },
            { "И", "I" },
            { "Й", "JJ" },
            { "К", "K" },
            { "Л", "L" },
            { "М", "M" },
            { "Н", "N" },
            { "О", "O" },
            { "П", "P" },
            { "Р", "R" },
            { "С", "S" },
            { "Т", "T" },
            { "У", "U" },
            { "Ф", "F" },
            { "Х", "KH" },
            { "Ц", "C" },
            { "Ч", "CH" },
            { "Ш", "SH" },
            { "Щ", "SHH" },
            { "Ъ", "'" },
            { "Ы", "Y" },
            { "Ь", "" },
            { "Э", "EH" },
            { "Ю", "YU" },
            { "Я", "YA" },
            { "а", "a" },
            { "б", "b" },
            { "в", "v" },
            { "г", "g" },
            { "д", "d" },
            { "е", "e" },
            { "ё", "jo" },
            { "ж", "zh" },
            { "з", "z" },
            { "и", "i" },
            { "й", "jj" },
            { "к", "k" },
            { "л", "l" },
            { "м", "m" },
            { "н", "n" },
            { "о", "o" },
            { "п", "p" },
            { "р", "r" },
            { "с", "s" },
            { "т", "t" },
            { "у", "u" },
            { "ф", "f" },
            { "х", "kh" },
            { "ц", "c" },
            { "ч", "ch" },
            { "ш", "sh" },
            { "щ", "shh" },
            { "ъ", "" },
            { "ы", "y" },
            { "ь", "" },
            { "э", "eh" },
            { "ю", "yu" },
            { "я", "ya" },
            { "«", "" },
            { "»", "" },
            { "—", "-" }
        };

        /// <summary>
        /// Словарь (ISO 9-95)
        /// </summary>
        private static Dictionary<String, String> ISO = new Dictionary<String, String>
        {
            //{ "Є", "YE" },
            //{ "І", "I" },
            //{ "Ѓ", "G" },
            //{ "і", "i" },
            { "№", "#" },
            //{ "є", "ye" },
            //{ "ѓ", "g" },
            { "А", "A" },
            { "Б", "B" },
            { "В", "V" },
            { "Г", "G" },
            { "Д", "D" },
            { "Е", "E" },
            { "Ё", "YO" },
            { "Ж", "ZH" },
            { "З", "Z" },
            { "И", "I" },
            { "Й", "J" },
            { "К", "K" },
            { "Л", "L" },
            { "М", "M" },
            { "Н", "N" },
            { "О", "O" },
            { "П", "P" },
            { "Р", "R" },
            { "С", "S" },
            { "Т", "T" },
            { "У", "U" },
            { "Ф", "F" },
            { "Х", "X" },
            { "Ц", "C" },
            { "Ч", "CH" },
            { "Ш", "SH" },
            { "Щ", "SHH" },
            { "Ъ", "'" },
            { "Ы", "Y" },
            { "Ь", "" },
            { "Э", "E" },
            { "Ю", "YU" },
            { "Я", "YA" },
            { "а", "a" },
            { "б", "b" },
            { "в", "v" },
            { "г", "g" },
            { "д", "d" },
            { "е", "e" },
            { "ё", "yo" },
            { "ж", "zh" },
            { "з", "z" },
            { "и", "i" },
            { "й", "j" },
            { "к", "k" },
            { "л", "l" },
            { "м", "m" },
            { "н", "n" },
            { "о", "o" },
            { "п", "p" },
            { "р", "r" },
            { "с", "s" },
            { "т", "t" },
            { "у", "u" },
            { "ф", "f" },
            { "х", "x" },
            { "ц", "c" },
            { "ч", "ch" },
            { "ш", "sh" },
            { "щ", "shh" },
            { "ъ", "" },
            { "ы", "y" },
            { "ь", "" },
            { "э", "e" },
            { "ю", "yu" },
            { "я", "ya" },
            { "«", "" },
            { "»", "" },
            { "—", "-" }
        };

        /// <summary>
        /// Пользовательский словарь
        /// </summary>
        /// TODO: Заполнить пользовательский алфавит
        private static Dictionary<String, String> Custom = new Dictionary<String, String>
        {
            { "а", "a" },
            { "б", "b" },
            { "в", "v" },
            { "г", "g" },
            { "д", "d" },
            { "е", "e" },
            { "ё", "yo" },
            { "ж", "zh" },
            { "з", "z" },
            { "и", "i" },
            { "й", "j" },
            { "к", "k" },
            { "л", "l" },
            { "м", "m" },
            { "н", "n" },
            { "о", "o" },
            { "п", "p" },
            { "р", "r" },
            { "с", "s" },
            { "т", "t" },
            { "у", "u" },
            { "ф", "f" },
            { "х", "h" },
            { "ц", "c" },
            { "ч", "ch" },
            { "ш", "sh" },
            { "щ", "sch" },
            { "ъ", "j" },
            { "ы", "i" },
            { "ь", "j" },
            { "э", "e" },
            { "ю", "yu" },
            { "я", "ya" },
            { "А", "A" },
            { "Б", "B" },
            { "В", "V" },
            { "Г", "G" },
            { "Д", "D" },
            { "Е", "E" },
            { "Ё", "Yo" },
            { "Ж", "Zh" },
            { "З", "Z" },
            { "И", "I" },
            { "Й", "J" },
            { "К", "K" },
            { "Л", "L" },
            { "М", "M" },
            { "Н", "N" },
            { "О", "O" },
            { "П", "P" },
            { "Р", "R" },
            { "С", "S" },
            { "Т", "T" },
            { "У", "U" },
            { "Ф", "F" },
            { "Х", "H" },
            { "Ц", "C" },
            { "Ч", "Ch" },
            { "Ш", "Sh" },
            { "Щ", "Sch" },
            { "Ъ", "J" },
            { "Ы", "I" },
            { "Ь", "J" },
            { "Э", "E" },
            { "Ю", "Yu" },
            { "Я", "Ya" }
        };

        #endregion

        #region Methods

        /// <summary>
        /// Получает алфавитный словарь в указанном формате.
        /// </summary>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static Dictionary<String, String> GetDictionary(TranslitType Type)
        {
            switch (Type)
            {
                case TranslitType.GOST:
                    return GOST;

                case TranslitType.ISO:
                    return ISO;
                    
                case TranslitType.Custom:
                    return Custom;

                default:
                    return ISO;
            }
        }

        #endregion

        /// <summary>
        /// Преобразует текст в пользовательский алфавит.
        /// </summary>
        /// <param name="Text">Преобразуемый текст</param>
        /// <param name="Dictionary">Пользовательский алфавит</param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public static String Convert(String Text, Dictionary<String, String> Dictionary, TranslitMode Mode = TranslitMode.Front)
        {
            switch (Mode)
            {
                case TranslitMode.Front:
                    foreach (KeyValuePair<String, String> Pair in Dictionary)
                        Text = Text.Replace(Pair.Key, Pair.Value);

                    return Text;

                case TranslitMode.Back:
                    foreach (KeyValuePair<String, String> Pair in Dictionary)
                    {
                        if (Pair.Value != "")
                            Text = Text.Replace(Pair.Value, Pair.Key);
                    }

                    return Text;

                default:
                    return Text;
            }
        }

        /// <summary>
        /// Преобразует текст в указанный алфавит и формат преобразования.
        /// </summary>
        /// <param name="Text">Преобразуемый текст</param>
        /// <param name="Alphabet">Алфавит</param>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static String Convert(String Text, TranslitAlphabet Alphabet, TranslitType Type = TranslitType.ISO)
        {
            switch (Alphabet)
            {
                case TranslitAlphabet.Latin:
                    return ToLatin(Text, Type);

                case TranslitAlphabet.Cyrillic:
                    return ToCyrillic(Text, Type);

                default:
                    return Text;
            }
        }

        /// <summary>
        /// Преобразует кириллистический текст в латинский, в указанном формате.
        /// </summary>
        /// <param name="Text">Преобразуемый текст</param>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static String ToLatin(String Text, TranslitType Type = TranslitType.ISO)
        {
            foreach (KeyValuePair<String, String> Pair in GetDictionary(Type))
                Text = Text.Replace(Pair.Key, Pair.Value);

            return Text;
        }

        /// <summary>
        /// Преобразует латинский текст в кириллистический, в указанном формате.
        /// </summary>
        /// <param name="Text">Преобразуемый текст</param>
        /// <param name="Type">Формат преобразования</param>
        /// <returns></returns>
        public static String ToCyrillic(String Text, TranslitType Type = TranslitType.ISO)
        {
            foreach (KeyValuePair<String, String> Pair in GetDictionary(Type))
            {
                if (Pair.Value != "")
                    Text = Text.Replace(Pair.Value, Pair.Key);
            }

            return Text;
        }
    }
}
