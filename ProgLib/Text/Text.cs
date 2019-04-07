using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Text
{
    public class Text
    {
        /// <summary>
        /// Возвращает минимальное количество операций вставки одного символа, удаления одного символа и замены одного символа на другой, необходимых для превращения одной строки в другую.
        /// </summary>
        /// <param name="Search">Искомый текст</param>
        /// <param name="Value">Текст, с которым сравнивается искомый текст</param>
        /// <returns></returns>
        public static Int32 EditorialDistance(String Search, String Value)
        {
            if (Search == null) throw new ArgumentNullException("Search");
            if (Value == null) throw new ArgumentNullException("Value");

            Int32 Diff;
            Int32[,] m = new Int32[Search.Length + 1, Value.Length + 1];

            for (int i = 0; i <= Search.Length; i++) { m[i, 0] = i; }
            for (int j = 0; j <= Value.Length; j++) { m[0, j] = j; }

            for (int i = 1; i <= Search.Length; i++)
            {
                for (int j = 1; j <= Value.Length; j++)
                {
                    Diff = (Search[i - 1] == Value[j - 1]) ? 0 : 1;
                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1), m[i - 1, j - 1] + Diff);
                }
            }
            return m[Search.Length, Value.Length];
        }

        /// <summary>
        /// Осуществляет неточный поиск по расстоянию Левенштейна и возвращает список найденных слов
        /// </summary>
        /// <param name="Search">Искомый текст</param>
        /// <param name="Values">Список текста, с которым сравнивается искомый текст</param>
        /// <returns></returns>
        public static String[] SearchByEditorialDistance(String Search, String[] Values)
        {
            if (Search == null) throw new ArgumentNullException("Search");
            if (Values == null) throw new ArgumentNullException("Values");

            Int32 Minimum = 999999;
            List<String> Result = new List<String>();

            foreach (String Text in Values)
            {
                Minimum = (Minimum <= EditorialDistance(Search, Text))
                    ? EditorialDistance(Search, Text)
                    : Minimum;
            }

            foreach (String Text in Values)
                if (Minimum == EditorialDistance(Search, Text)) Result.Add(Text);

            return Result.ToArray();
        }
    }
}