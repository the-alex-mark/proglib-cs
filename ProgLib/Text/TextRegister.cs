using System;

namespace ProgLib.Text
{
    /// <summary>
    /// Предоставляет методы для работы над переводом регистра строки.
    /// </summary>
    public class TextRegister
    {
        /// <summary>
        /// Возвращает копию этой строки, с переверённой первой буквой в верхний регистр.
        /// Пример: "Как в предложениях".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToFirstUpper(String Value)
        {
            if (Value.Length > 0)
            {
                String Temp = Value.ToLower();
                return Temp[0].ToString().ToUpper() + ((Temp.Length > 1) ? Temp.Substring(1) : "");
            }

            else return Value;
        }

        /// <summary>
        /// Возвращает копию этой строки, переведённую в нижний регистр.
        /// Пример: "все строчные".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToLower(String Value)
        {
            return Value.ToLower();
        }

        /// <summary>
        /// Возвращает копию этой строки, переведённую в верхний регистр.
        /// Пример: "ВСЕ ПРОПИСНЫЕ".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToUpper(String Value)
        {
            return Value.ToUpper();
        }
        
        /// <summary>
        /// Возвращает копию этой строки, с переведённой каждой первой буквой слова в верхний регистр.
        /// Пример: "Начинать С Прописных".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToFirstsUpper(String Value)
        {
            String Result = Value;

            if (Value.Length > 0)
            {
                Result = "";

                String[] Words = Value.ToLower().Split(' ');
                foreach (String Word in Words)
                {
                    if (Word.Length > 0)
                        Result += Word[0].ToString().ToUpper() + Word.Substring(1) + " ";
                }

                return Result;
            }

            else return Result;
        }

        /// <summary>
        /// Возвращает копию этой строки, изменяя регистр каждого символа на обратный.
        /// Пример: "иЗМЕНИТЬ РЕГИСТР".
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String TransformRegister(String Value)
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
    }
}
