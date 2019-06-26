using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ProgLib.Data;
using ProgLib.Text;

namespace ProgLib
{
    public static class Expansion
    {
        /// <summary>
        /// Задаёт случайный порядок списка
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            Int32 n = list.Count;
            while (n > 1)
            {
                n--;
                Int32 k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private static Random rng = new Random();
        
        /// <summary>
        /// Проверяет значение на вхождение в указанный диапазон.
        /// </summary>
        /// <param name="Value">Проверяемое значение</param>
        /// <param name="FirstValue">Значение первого элемента диапазона</param>
        /// <param name="LastValue">Значение последнего элемента диапазона</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Boolean IsEntryRange(this Int32 Value, Int32 FirstValue, Int32 LastValue)
        {
            if (FirstValue >= LastValue)
                throw new ArgumentException("Значение последнего элемента диапазона должно быть больше первого.");

            if (Value < FirstValue || Value > LastValue)
                return false;

            return true;
        }

        /// <summary>
        /// Проверяет значение на вхождение в указанный диапазон.
        /// </summary>
        /// <param name="Value">Проверяемое значение</param>
        /// <param name="FirstValue">Значение первого элемента диапазона</param>
        /// <param name="LastValue">Значение последнего элемента диапазона</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Boolean IsEntryRange(this Double Value, Double FirstValue, Double LastValue)
        {
            if (FirstValue >= LastValue)
                throw new ArgumentException("Значение последнего элемента диапазона должно быть больше первого.");

            if (Value < FirstValue || Value > LastValue)
                return false;

            return true;
        }
    }
}
