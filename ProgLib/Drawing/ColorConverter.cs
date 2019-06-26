using System;
using System.Drawing;
using System.Linq;

// TODO: Доработать методы переводов цветовых моделей взятых из класса "ColorTranslator".
namespace ProgLib.Drawing
{
    /// <summary>
    /// Осуществляет перевод цветов из различных цветовых моделей в <see cref="Color"/> и обратно.
    /// </summary>
    public static class ColorConverter
    {
        #region Methods

        #region From

        /// <summary>
        /// Преобразует цветовую модель HSB в ARGB.
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 1.</param>
        /// <param name="Brightness">Яркость. Допустимые значения - от 0 до 1.</param>
        /// <returns></returns>
        private static (Int32 Red, Int32 Green, Int32 Blue) HsbToArgb(Int32 Hue, Double Saturation, Double Brightness)
        {
            Double C = Brightness * Saturation;
            Double X = C * (1 - Math.Abs(((double)Hue / 60) % 2 - 1));
            Double m = Brightness - C;

            Double R = 0, G = 0, B = 0;

            if (Hue >= 0 && Hue < 60)
            {
                R = C;
                G = X;
                B = 0;
            }
            else if (Hue >= 60 && Hue < 120)
            {
                R = X;
                G = C;
                B = 0;
            }
            else if (Hue >= 120 && Hue < 180)
            {
                R = 0;
                G = C;
                B = X;
            }
            else if (Hue >= 180 && Hue < 240)
            {
                R = 0;
                G = X;
                B = C;
            }
            else if (Hue >= 240 && Hue < 300)
            {
                R = X;
                G = 0;
                B = C;
            }
            else if (Hue >= 300 && Hue <= 360)
            {
                R = C;
                G = 0;
                B = X;
            }

            return ((int)Math.Round((R + m) * 255), (int)Math.Round((G + m) * 255), (int)Math.Round((B + m) * 255));
        }

        /// <summary>
        /// Преобразует цветовую модель HSL в ARGB.
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 1.</param>
        /// <param name="Lightness">Светлота. Допустимые значения - от 0 до 1.</param>
        /// <returns></returns>
        private static (Int32 Red, Int32 Green, Int32 Blue) HslToArgb(Int32 Hue, Double Saturation, Double Lightness)
        {
            float HueToRGB(float v1, float v2, float vH)
            {
                if (vH < 0)
                    vH += 1;

                if (vH > 1)
                    vH -= 1;

                if ((6 * vH) < 1)
                    return (v1 + (v2 - v1) * 6 * vH);

                if ((2 * vH) < 1)
                    return v2;

                if ((3 * vH) < 2)
                    return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

                return v1;
            }

            byte r = 0;
            byte g = 0;
            byte b = 0;

            if (Saturation == 0)
            {
                r = g = b = (byte)(Lightness * 255);
            }
            else
            {
                float v1, v2;
                float hue = (float)Hue / 360;

                v2 = (Lightness < 0.5) ? ((float)Lightness * (1 + (float)Saturation)) : (((float)Lightness + (float)Saturation) - ((float)Lightness * (float)Saturation));
                v1 = 2 * (float)Lightness - v2;

                r = (byte)Math.Round((255 * HueToRGB(v1, v2, hue + (1.0f / 3))));
                g = (byte)Math.Round((255 * HueToRGB(v1, v2, hue)));
                b = (byte)Math.Round((255 * HueToRGB(v1, v2, hue - (1.0f / 3))));
            }

            return (r, g, b);
        }

        /// <summary>
        /// Преобразует цветовую модель CMYK в ARGB.
        /// </summary>
        /// <param name="Cyan">Голубой. Допустимые значения - от 0 до 100.</param>
        /// <param name="Magenta">Пурпурный. Допустимые значения - от 0 до 100.</param>
        /// <param name="Yellow">Жёлтый. Допустимые значения - от 0 до 100.</param>
        /// <param name="Key">Ключ. Допустимые значения - от 0 до 100.</param>
        /// <returns></returns>
        private static (Int32 Red, Int32 Green, Int32 Blue) CmykToArgb(Int32 Cyan, Int32 Magenta, Int32 Yellow, Int32 Key)
        {
            return ((255 * (1 - Cyan) * (1 - Key)), (255 * (1 - Magenta) * (1 - Key)), (255 * (1 - Yellow) * (1 - Key)));
        }

        /// <summary>
        /// Преобразует цветовую модель YCbCr в ARGB.
        /// </summary>
        /// <param name="Y">Luma.</param>
        /// <param name="Cb">Blue-Luma.</param>
        /// <param name="Cr">Red-Luma.</param>
        /// <returns></returns>
        private static (Int32 Red, Int32 Green, Int32 Blue) YCbCrToArgb(Single Y, Single Cb, Single Cr)
        {
            float R = Math.Max(0.0f, Math.Min(1.0f, (float)(Y + 0.0000 * Cb + 1.4022 * Cr)));
            float G = Math.Max(0.0f, Math.Min(1.0f, (float)(Y - 0.3456 * Cb - 0.7145 * Cr)));
            float B = Math.Max(0.0f, Math.Min(1.0f, (float)(Y + 1.7710 * Cb + 0.0000 * Cr)));

            return ((int)Math.Round((R * 255)), (int)Math.Round((G * 255)), (int)Math.Round((B * 255)));
        }

        /// <summary>
        /// Преобразует цветовую модель Hex в ARGB.
        /// </summary>
        /// <param name="Value">Строка, содержащая цветовое значение в формате Hex.</param>
        /// <returns></returns>
        private static (Int32 Red, Int32 Green, Int32 Blue) HexToArgb(String Value)
        {
            Int32 ToDecimal(String Line)
            {
                Line = Line.ToUpper();

                Int32 Count = Line.Length;
                Double Decimal = 0;

                for (int i = 0; i < Count; ++i)
                {
                    Byte A = (byte)Line[i];

                    if (A >= 48 && A <= 57) { A -= 48; }
                    else if (A >= 65 && A <= 70) { A -= 55; }

                    Decimal += A * Math.Pow(16, ((Count - i) - 1));
                }

                return (int)Decimal;
            }
            Value = (Value.StartsWith("#") && Value.Length == 7) ? Value.Remove(0, 1) : Value;

            return (ToDecimal(Value.Substring(0, 2)), ToDecimal(Value.Substring(2, 2)), ToDecimal(Value.Substring(4, 2)));
        }

        #endregion

        #region To

        private static HSB ArgbToHSB(Color Value)
        {
            double delta, min;
            double h = 0, s, b;

            min = Math.Min(Math.Min(Value.R, Value.G), Value.B);
            b = Math.Max(Math.Max(Value.R, Value.G), Value.B);
            delta = b - min;

            if (b == 0.0)
                s = 0;
            else
                s = delta / b;

            if (s == 0)
                h = 0.0;

            else
            {
                if (Value.R == b)
                    h = (Value.G - Value.B) / delta;
                else if (Value.G == b)
                    h = 2 + (Value.B - Value.R) / delta;
                else if (Value.B == b)
                    h = 4 + (Value.R - Value.G) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            return new HSB((int)Math.Round(h), Math.Round(s, 2), Math.Round((b / 255), 2));
        }

        #endregion

        #endregion

        /// <summary>
        /// Создает структуру <see cref="Color"/> из указанного, предварительно определенного цвета.
        /// </summary>
        /// <param name="KnownColor">Элемент перечисления <see cref="KnownColor"/>.</param>
        /// <returns></returns>
        public static Color FromKnownColor(KnownColor KnownColor)
        {
            return Color.FromKnownColor(KnownColor);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из указанного имени предопределенного цвета.
        /// </summary>
		/// <param name="Name">Строка, которая является именем предопределенного цвета. Допустимые имена те же, что и у элементов перечня <see cref="KnownColor"/>.</param>
		/// <returns></returns>
		public static Color FromName(String Name)
        {
            return Color.FromName(Name);
        }
        
        /// <summary>
        /// Переводит значение цвета в технологии OLE в структуру <see cref="Color"/>.
        /// </summary>
        /// <param name="OleColor">Переводимый цвет в формате OLE.</param>
        /// <returns></returns>
        public static Color FromOle(Int32 OleColor)
        {
            return ColorTranslator.FromOle(OleColor);
        }

        /// <summary>
        /// Переводимый значения цвета Windows в структуру <see cref="Color"/>.
        /// </summary>
        /// <param name="Win32Color">Переводимый цвет Windows.</param>
        /// <returns></returns>
        public static Color FromWin32(Int32 Win32Color)
        {
            return ColorTranslator.FromWin32(Win32Color);
        }

        /// <summary>
        /// Переводит представление цвета HTML в структуру <see cref="Color"/>.
        /// </summary>
        /// <param name="HtmlColor">Строковое представление переводимого цвета HTML.</param>
        /// <returns></returns>
        public static Color FromHtml(String HtmlColor)
        {
            return ColorTranslator.FromHtml(HtmlColor);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов Hex.
        /// </summary>
        /// <param name="HexColor">Строка, содержащая цветовое значение в формате Hex.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHex(String HexColor)
        {
            if (!HexColor.StartsWith("#") || HexColor.Length != 7)
                throw new ArgumentException("Значение строки \"Hex\" имело неверный формат!");

            var _color = HexToArgb(HexColor);
            return Color.FromArgb(_color.Red, _color.Green, _color.Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из указанной структуры <see cref="Color"/>, но с новым определенным значением Alpha.
        /// </summary>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 255.</param>
        /// <param name="BaseColor">Цвет <see cref="Color"/>, на основе которого будет создан новый цвет <see cref="FullColor"/>.</param>
        /// <exception cref="ArgumentException">Alpha меньше 0 или больше 255.</exception>
        /// <returns></returns>
        public static Color FromArgb(Int32 Alpha, Color BaseColor)
        {
            return Color.FromArgb(Alpha, BaseColor);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из указанных 8-разрядных значений цветов (красный, зеленый, синий).
        /// Значение альфа неявно определено как 255 (полностью непрозрачно).
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException">Параметр Red, Green или Blue меньше 0 или больше 255.</exception>
        /// <returns></returns>
        public static Color FromArgb(Int32 Red, Int32 Green, Int32 Blue)
        {
            return Color.FromArgb(255, Red, Green, Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из четырех значений компонентов ARGB (Прозрачность, Красный, Зеленый и Синий).
        /// </summary>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 255.</param>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromArgb(Int32 Alpha, Int32 Red, Int32 Green, Int32 Blue)
        {
            return Color.FromArgb(Alpha, Red, Green, Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов HSB / HSV (Оттенок, Насыщенность и Яркость).
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 1.</param>
        /// <param name="Brightness">Яркость. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHsb(Int32 Hue, Double Saturation, Double Brightness)
        {
            if (!Hue.IsEntryRange(0, 360))
                throw new ArgumentException("Значения аргумента \"Hue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 360.");

            if (!Saturation.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Saturation\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            if (!Brightness.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Brightness\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            var _color = HsbToArgb(Hue, Saturation, Brightness);
            return Color.FromArgb(_color.Red, _color.Green, _color.Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов HSB / HSV (Оттенок, Насыщенность и Яркость).
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 100.</param>
        /// <param name="Brightness">Яркость. Допустимые значения - от 0 до 100.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHsb(Int32 Hue, Int32 Saturation, Int32 Brightness)
        {
            return ColorConverter.FromHsb(Hue, ((double)Saturation / 100), (double)Brightness / 100);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов HSL (Оттенок, Насыщенность и Светлота).
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 1.</param>
        /// <param name="Lightness">Светлота. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHsl(Int32 Hue, Double Saturation, Double Lightness)
        {
            if (!Hue.IsEntryRange(0, 360))
                throw new ArgumentException("Значения аргумента \"Hue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 360.");

            if (!Saturation.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Saturation\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            if (!Lightness.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Brightness\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            var _color = HslToArgb(Hue, Saturation, Lightness);
            return Color.FromArgb(_color.Red, _color.Green, _color.Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов HSL (Оттенок, Насыщенность и Светлота).
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 100.</param>
        /// <param name="Lightness">Светлота. Допустимые значения - от 0 до 100.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHsl(Int32 Hue, Int32 Saturation, Int32 Lightness)
        {
            return ColorConverter.FromHsl(Hue, ((double)Saturation / 100), (double)Lightness / 100);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов CMYK (Голубой, Пурпурный, Жёлтый и Ключ).
        /// </summary>
        /// <param name="Cyan">Голубой. Допустимые значения - от 0 до 100.</param>
        /// <param name="Magenta">Пурпурный. Допустимые значения - от 0 до 100.</param>
        /// <param name="Yellow">Жёлтый. Допустимые значения - от 0 до 100.</param>
        /// <param name="Key">Ключ. Допустимые значения - от 0 до 100.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromCmyk(Int32 Cyan, Int32 Magenta, Int32 Yellow, Int32 Key)
        {
            if (!Cyan.IsEntryRange(0, 100))
                throw new ArgumentException("Значения аргумента \"Cyan\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100.");

            if (!Magenta.IsEntryRange(0, 100))
                throw new ArgumentException("Значения аргумента \"Magenta\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100.");

            if (!Yellow.IsEntryRange(0, 100))
                throw new ArgumentException("Значения аргумента \"Yellow\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100.");

            if (!Key.IsEntryRange(0, 100))
                throw new ArgumentException("Значения аргумента \"Key\" выходило за допустимый диапазон! Допустимые значения - от 0 до 100.");

            var _color = CmykToArgb(Cyan, Magenta, Yellow, Key);
            return Color.FromArgb(_color.Red, _color.Green, _color.Blue);
        }
    }
}
