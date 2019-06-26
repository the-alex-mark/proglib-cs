using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Text.Encoding
{
    /// <summary>
    /// Предоставляет методы простейших шифрований данных.
    /// </summary>
    public class Encryption
    {
        #region Variables

        private static Char[] RU = { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 'Э', 'Ю', 'Я' };
        private static Char[] EN = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        #endregion
        
        /// <summary>
        /// Шифр Цезаря.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Key"></param>
        /// <param name="Language"></param>
        /// <returns></returns>
        public static String Caesar(String Text, Int32 Key, Language Language)
        {
            String Letter = "";

            switch (Language)
            {
                case Language.Russian:
                    for (int i = 0; i < Text.Length; i++)
                    {
                        if (Text[i].ToString() != " ")
                        {
                            for (int j = 0; j < RU.Length; j++)
                            {
                                if (Text[i].ToString().ToUpper() == RU[j].ToString())
                                {
                                    if ((j + Key) >= RU.Length)
                                    {
                                        Letter = RU[(j + Key) - RU.Length].ToString();
                                    }
                                    else
                                    {
                                        Letter = RU[j + Key].ToString();
                                    }
                                }
                            }
                        }
                        else { Letter = " "; }
                        Text += Letter;
                    }
                    break;

                case Language.English:
                    for (int i = 0; i < Text.Length; i++)
                    {
                        if (Text[i].ToString() != " ")
                        {
                            for (int j = 0; j < EN.Length; j++)
                            {
                                if (Text[i].ToString().ToUpper() == EN[j].ToString())
                                {
                                    if ((j + Key) >= EN.Length)
                                    {
                                        Letter = EN[(j + Key) - EN.Length].ToString();
                                    }
                                    else
                                    {
                                        Letter = EN[j + Key].ToString();
                                    }
                                }
                            }
                        }
                        else { Letter = " "; }
                        Text += Letter;
                    }
                    break;
            }

            return Text;
        }

        /// <summary>
        /// Перестановочный шифр.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static String Transposition(String Text, Size Key)
        {
            Text = Text.Replace(" ", String.Empty);
            String[,] Table = new String[Key.Width, Key.Height];

            for (int i = 0; i < Key.Width; i++)
            {
                Int32 G = i;
                for (int j = Key.Height - 1; j > -1; j--)
                {
                    Table[i, j] = (G >= Text.Length) ? "+" : Text[G].ToString(); G += Key.Width;
                    Text += (Table[i, j] != "+") ? Table[i, j].ToUpper() : "";
                }
            }

            return Text;
        }
    }
}
