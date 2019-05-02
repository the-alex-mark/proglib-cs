using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
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
        /// Преобразует изображение в массив байтов
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Byte[] ToByteArray(this Image Value)
        {
            using (MemoryStream MS = new MemoryStream())
            {
                Value.Save(MS, Value.RawFormat);
                return MS.ToArray();
            }
        }

        /// <summary>
        /// Конвертирует изображение в строку
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static String ToBase64String(this Image Value)
        {
            try
            {
                return Convert.ToBase64String(Value.ToByteArray());
            }
            catch { throw new Exception("Входной параметр \"Value\" имел неверный формат"); }
        }

        /// <summary>
        /// Преобразует строку соответствующего формата в изображение
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Image ToImage(this String Value)
        {
            try
            {
                return Image.FromStream(new MemoryStream(Convert.FromBase64String(Value)));
            }
            catch { throw new Exception("Входной параметр \"Value\" имел неверный формат"); }
        }
        
        /// <summary>
        /// Возвращает таблицу типа <see cref="DataTable"/> в табличном представлении типа <see cref="StringTable"/>
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        public static StringTable ToStringTable(this DataTable Table)
        {
            return new StringTable(Table);
        }

        /// <summary>
        /// Возвращает таблицу типа <see cref="DataTable"/> в табличном представлении типа <see cref="String"/>
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="Format"></param>
        /// <returns></returns>
        public static String ToStringTable(this DataTable Table, TableFormat Format = TableFormat.Default)
        {
            return new StringTable(Table).Result(Format);
        }
        
        /// <summary>
        /// Складывает два значения типа <see cref="Point"/>
        /// </summary>
        /// <param name="Point1"></param>
        /// <param name="Point2"></param>
        /// <returns></returns>
        public static Point Add(this Point Point1, Point Point2)
        {
            return new Point(Point1.X + Point2.X, Point1.Y + Point2.Y);
        }
    }
}
