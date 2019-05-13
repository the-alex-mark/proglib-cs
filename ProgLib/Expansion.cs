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
    }
}
