﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Text
{
    /// <summary>
    /// Предоставляет методы для работы над переводом регистра строки.
    /// </summary>
    public class FontRegister
    {
        /// <summary>
        /// Возвращает копию этой строки, переведённую в верхний регистр.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToUpper(String Value)
        {
            return Value.ToUpper();
        }

        /// <summary>
        /// Возвращает копию этой строки, переведённую в нижний регистр.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToLower(String Value)
        {
            return Value.ToLower();
        }

        /// <summary>
        /// Возвращает копию этой строки, с переверённой первой буквой в верхний регистр.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToFirstUpper(String Value)
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
        public static String ToFirstsUpper(String Value)
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
