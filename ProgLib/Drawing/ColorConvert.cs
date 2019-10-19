using System;
using System.Drawing;

// TODO: Доработать методы переводов цветовых моделей взятых из класса "ColorTranslator".
// TODO: Доработать методы переводов обратно из RGBA.
namespace ProgLib.Drawing
{
    /// <summary>
    /// Осуществляет перевод цветов из различных цветовых моделей в <see cref="Color"/> и обратно.
    /// </summary>
    public static class ColorConvert
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
        private static (Int32 Red, Int32 Green, Int32 Blue) CmykToArgb(Double Cyan, Double Magenta, Double Yellow, Double Key)
        {
            Cyan    = Math.Round(Cyan, 2);
            Magenta = Math.Round(Magenta, 2);
            Yellow  = Math.Round(Yellow, 2);
            Key     = Math.Round(Key, 2);

            return (
                (int)Math.Round((255 * (1 - Cyan)    * (1 - Key))),
                (int)Math.Round((255 * (1 - Magenta) * (1 - Key))),
                (int)Math.Round((255 * (1 - Yellow)  * (1 - Key))));
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

        /// <summary>
        /// Преобразует цветовую модель XYZ в ARGB.
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <returns></returns>
        private static (Int32 Red, Int32 Green, Int32 Blue) XyzToARgb(Int32 X, Int32 Y, Int32 Z)
        {
            Int32 _red = Math.Min(255, Math.Max(0, (int)Math.Round( (3.2404542 * X) - (1.5371385 * Y) - (0.4985314 * Z) ) ) );
            Int32 _green = Math.Min(255, Math.Max(0, (int)Math.Round( (-0.9692660 * X) + (1.8760108 * Y) + (0.0415560 * Z) ) ) );
            Int32 _blue = Math.Min(255, Math.Max(0, (int)Math.Round( (0.0556434 * X) - (0.2040259 * Y) + (1.0572252 * Z) ) ) );

            return (_red, _green, _blue);
        }

        #endregion

        #region To
        
        // -- Не готово!
        private static String XYZ(Color COLOR)
        {
            Double RLinear = COLOR.R / 255.0;
            Double GLinear = COLOR.G / 255.0;
            Double BLinear = COLOR.B / 255.0;

            Double R = (RLinear > 0.04045) ? Math.Pow((RLinear + 0.055) / (1 + 0.055), 2.2) : (RLinear / 12.92);
            Double G = (GLinear > 0.04045) ? Math.Pow((GLinear + 0.055) / (1 + 0.055), 2.2) : (GLinear / 12.92);
            Double B = (BLinear > 0.04045) ? Math.Pow((BLinear + 0.055) / (1 + 0.055), 2.2) : (BLinear / 12.92);

            Double X = (R * 0.4124 + G * 0.3576 + B * 0.1805) * 100;
            Double Y = (R * 0.2126 + G * 0.7152 + B * 0.0722) * 100;
            Double Z = (R * 0.0193 + G * 0.1192 + B * 0.9505) * 100;

            return String.Format("{0:0}, {1:0}, {2:0}", X, Y, Z);
        }
        private static String LAB(Color COLOR)
        {
            Double FXYZ(Double T)
            {
                return ((T > 0.008856) ? Math.Pow(T, (1.0 / 3.0)) : (7.787 * T + 16.0 / 116.0));
            }

            Double RLinear = COLOR.R / 255.0;
            Double GLinear = COLOR.G / 255.0;
            Double BLinear = COLOR.B / 255.0;

            Double R = (RLinear > 0.04045) ? Math.Pow((RLinear + 0.055) / (1 + 0.055), 2.2) : (RLinear / 12.92);
            Double G = (GLinear > 0.04045) ? Math.Pow((GLinear + 0.055) / (1 + 0.055), 2.2) : (GLinear / 12.92);
            Double B = (BLinear > 0.04045) ? Math.Pow((BLinear + 0.055) / (1 + 0.055), 2.2) : (BLinear / 12.92);

            Double X = (R * 0.4124 + G * 0.3576 + B * 0.1805) * 100;
            Double Y = (R * 0.2126 + G * 0.7152 + B * 0.0722) * 100;
            Double Z = (R * 0.0193 + G * 0.1192 + B * 0.9505) * 100;

            // ----------------------------------------------------------------------------

            Double L = 116.0 * FXYZ(Y / 100.000) - 16;
            Double A = 500.0 * (FXYZ(X / 95.047) - FXYZ(Y / 100.000));
            B = 200.0 * (FXYZ(Y / 100.000) - FXYZ(Z / 108.883));

            return String.Format("lab ({0:0}, {1:0}, {2:0})", L, A, B);
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
        /// <param name="Value">Переводимый цвет в формате OLE.</param>
        /// <returns></returns>
        public static Color FromOle(Int32 Value)
        {
            return ColorTranslator.FromOle(Value);
        }

        /// <summary>
        /// Переводит значения Windows цвета в структуру <see cref="Color"/>.
        /// </summary>
        /// <param name="Value">Переводимый цвет Windows.</param>
        /// <returns></returns>
        public static Color FromWin32(Int32 Value)
        {
            return ColorTranslator.FromWin32(Value);
        }

        /// <summary>
        /// Переводит представление цвета HTML в структуру <see cref="Color"/>.
        /// </summary>
        /// <param name="Value">Строковое представление переводимого цвета HTML.</param>
        /// <returns></returns>
        public static Color FromHtml(String Value)
        {
            return ColorTranslator.FromHtml(Value);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов Hex.
        /// </summary>
        /// <param name="Value">Строка, содержащая цветовое значение в формате Hex.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHex(String Value)
        {
            #region Проверка правильности входных данных

            if (!Value.StartsWith("#") || Value.Length != 7)
                throw new ArgumentException("Значение строки \"Hex\" имело неверный формат!");

            #endregion

            var _color = HexToArgb(Value);
            return Color.FromArgb(_color.Red, _color.Green, _color.Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из указанной структуры <see cref="Color"/>, но с новым определенным значением Alpha.
        /// </summary>
        /// <param name="Value">Цвет <see cref="Color"/>, на основе которого будет создан новый цвет <see cref="Color"/>.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException">Alpha меньше 0 или больше 255.</exception>
        /// <returns></returns>
        public static Color FromRgba(Color Value, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            Int32 _alpha = Math.Min(255, Math.Max(0, (int)Math.Round(255 * Alpha)));
            return Color.FromArgb(_alpha, Value);
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
        public static Color FromRgba(Int32 Red, Int32 Green, Int32 Blue)
        {
            return Color.FromArgb(Red, Green, Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из четырех значений компонентов RGBA (Красный, Зеленый, Синий и Прозрачность).
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromRgba(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"RBlueed\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            Int32 _alpha = Math.Min(255, Math.Max(0, (int)Math.Round(255 * Alpha)));
            return Color.FromArgb(_alpha, Red, Green, Blue);
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
            #region Проверка правильности входных данных

            if (!Hue.IsEntryRange(0, 360))
                throw new ArgumentException("Значения аргумента \"Hue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 360.");

            if (!Saturation.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Saturation\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            if (!Brightness.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Brightness\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

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

            return Color.FromArgb((int)Math.Round((R + m) * 255), (int)Math.Round((G + m) * 255), (int)Math.Round((B + m) * 255));
        }
        
        /// <summary>
        /// Создает структуру <see cref="Color"/> из трёх значений компонентов HSL (Оттенок, Насыщенность и Светлота).
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 1.</param>
        /// <param name="Lightness">Светлота. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHsla(Int32 Hue, Double Saturation, Double Lightness)
        {
            #region Проверка правильности входных данных
            
            if (!Hue.IsEntryRange(0, 360))
                throw new ArgumentException("Значения аргумента \"Hue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 360.");

            if (!Saturation.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Saturation\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            if (!Lightness.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Brightness\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            Single HueToRgba(Single v1, Single v2, Single vH)
            {
                if (vH < 0) vH += 1;
                if (vH > 1) vH -= 1;
                if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
                if ((2 * vH) < 1) return v2;
                if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2.0f / 3) - vH) * 6);

                return v1;
            }

            Byte Red = 0, Green = 0, Blue = 0;
            if (Saturation != 0)
            {
                Single v1, v2;
                Single hue = (float)Hue / 360;

                v2 = (Lightness < 0.5) ? ((float)Lightness * (1 + (float)Saturation)) : (((float)Lightness + (float)Saturation) - ((float)Lightness * (float)Saturation));
                v1 = 2 * (float)Lightness - v2;

                Red = (byte)Math.Round((255 * HueToRgba(v1, v2, hue + (1.0f / 3))));
                Green = (byte)Math.Round((255 * HueToRgba(v1, v2, hue)));
                Blue = (byte)Math.Round((255 * HueToRgba(v1, v2, hue - (1.0f / 3))));
            }
            else { Red = Green = Blue = (byte)(Lightness * 255); }

            return Color.FromArgb(Red, Green, Blue);
        }

        /// <summary>
        /// Создает структуру <see cref="Color"/> из четырёх значений компонентов HSLA (Оттенок, Насыщенность, Светлота и Прозрачность).
        /// </summary>
        /// <param name="Hue">Оттенок. Допустимые значения - от 0 до 360.</param>
        /// <param name="Saturation">Насыщенность. Допустимые значения - от 0 до 1.</param>
        /// <param name="Lightness">Светлота. Допустимые значения - от 0 до 1.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromHsla(Int32 Hue, Double Saturation, Double Lightness, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");
            
            #endregion

            Int32 _alpha = Math.Min(255, Math.Max(0, (int)Math.Round(255 * Alpha)));
            return Color.FromArgb(_alpha, FromHsla(Hue, Saturation, Lightness));
        }
        
        /// <summary>
        /// Создает структуру <see cref="Color"/> из четырёх значений компонентов CMYK (Голубой, Пурпурный, Жёлтый и Ключ).
        /// </summary>
        /// <param name="Cyan">Голубой. Допустимые значения - от 0 до 1.</param>
        /// <param name="Magenta">Пурпурный. Допустимые значения - от 0 до 1.</param>
        /// <param name="Yellow">Жёлтый. Допустимые значения - от 0 до 1.</param>
        /// <param name="Key">Ключ. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Color FromCmyk(Double Cyan, Double Magenta, Double Yellow, Double Key)
        {
            #region Проверка правильности входных данных

            if (!Cyan.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Cyan\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            if (!Magenta.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Magenta\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            if (!Yellow.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Yellow\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            if (!Key.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Key\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            Cyan = Math.Round(Cyan, 2);
            Magenta = Math.Round(Magenta, 2);
            Yellow = Math.Round(Yellow, 2);
            Key = Math.Round(Key, 2);

            return Color.FromArgb((int)Math.Round((255 * (1 - Cyan) * (1 - Key))), (int)Math.Round((255 * (1 - Magenta) * (1 - Key))), (int)Math.Round((255 * (1 - Yellow) * (1 - Key))));
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Ole.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Int32 ToOle(Int32 Red, Int32 Green, Int32 Blue)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"RBlueed\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            #endregion

            return ColorTranslator.ToOle(Color.FromArgb(Red, Green, Blue));
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Ole.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Int32 ToOle(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            Int32 _alpha = Math.Min(255, Math.Max(0, (int)Math.Round(255 * Alpha)));
            return ColorTranslator.ToOle(Color.FromArgb(_alpha, Red, Green, Blue));
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Ole.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Int32 ToOle(Color Value)
        {
            return ColorTranslator.ToOle(Value);
        }
        
        /// <summary>
        /// Переводит значения значения Windows цвета в RGBA.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Int32 ToWin32(Int32 Red, Int32 Green, Int32 Blue)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"RBlueed\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            #endregion

            return ColorTranslator.ToWin32(Color.FromArgb(Red, Green, Blue));
        }

        /// <summary>
        /// Переводит значения значения Windows цвета в RGBA.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static Int32 ToWin32(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            Int32 _alpha = Math.Min(255, Math.Max(0, (int)Math.Round(255 * Alpha)));
            return ColorTranslator.ToWin32(Color.FromArgb(_alpha, Red, Green, Blue));
        }

        /// <summary>
        /// Переводит значения значения Windows цвета в RGBA.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Int32 ToWin32(Color Value)
        {
            return ColorTranslator.ToWin32(Value);
        }
        
        /// <summary>
        /// Переводит значения цветовой модели RGBA в Html.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHtml(Int32 Red, Int32 Green, Int32 Blue)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            #endregion
            
            return ColorTranslator.ToHtml(Color.FromArgb(Red, Green, Blue));
        }
        
        /// <summary>
        /// Переводит значения цветовой модели RGBA в Html.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHtml(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion
            
            return ColorTranslator.ToHtml(Color.FromArgb(Red, Green, Blue));
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Html.
        /// </summary>
        /// <param name="Value"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHtml(Color Value)
        {
            return ColorTranslator.ToHtml(Value);
        }
        
        /// <summary>
        /// Переводит значения цветовой модели RGBA в Hex.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHex(Int32 Red, Int32 Green, Int32 Blue)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            #endregion

            return ColorTranslator.ToHtml(Color.FromArgb(Red, Green, Blue)).Replace("#", "0x");
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Hex.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHex(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            return ColorTranslator.ToHtml(Color.FromArgb(Red, Green, Blue)).Replace("#", "0x");
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Hex.
        /// </summary>
        /// <param name="Value"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHex(Color Value)
        {
            return ColorTranslator.ToHtml(Value).Replace("#", "0x");
        }
        
        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSB.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Brightness) ToHsb(Int32 Red, Int32 Green, Int32 Blue)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            #endregion

            Double delta, min;
            Double h = 0, s, b;

            min = Math.Min(Math.Min(Red, Green), Blue);
            b = Math.Max(Math.Max(Red, Green), Blue);
            delta = b - min;

            if (b == 0.0)
                s = 0;
            else
                s = delta / b;

            if (s == 0)
                h = 0.0;

            else
            {
                if (Red == b)
                    h = (Green - Blue) / delta;
                else if (Green == b)
                    h = 2 + (Blue - Red) / delta;
                else if (Blue == b)
                    h = 4 + (Red - Green) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            return ((int)Math.Round(h), Math.Round(s, 2), Math.Round((b / 255), 2));
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSB.
        /// </summary>
        /// <param name="Red">Красный. Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Brightness) ToHsb(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion

            return ToHsb(Red, Green, Blue);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSB.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Brightness) ToHsb(Color Value)
        {
            return ToHsb(Value.R, Value.G, Value.B);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSLA.
        /// </summary>
        /// <param name="Red"> Красный.Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Lightness, Double Alpha) ToHsla(Int32 Red, Int32 Green, Int32 Blue)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            #endregion

            Double _s, _l;
            Double R = (Red / 255.0f), G = (Green / 255.0f), B = (Blue / 255.0f);

            Double Minimum = Math.Min(Math.Min(R, G), B);
            Double Maximum = Math.Max(Math.Max(R, G), B);
            Double Delta = Maximum - Minimum;

            _l = (Maximum + Minimum) / 2;

            Double H = 0;

            if (Delta == 0) { _s = 0.0f; }
            else
            {
                _s = (_l <= 0.5) ? (Delta / (Maximum + Minimum)) : (Delta / (2 - Maximum - Minimum));

                if (R == Maximum)
                    H = ((G - B) / 6) / Delta;
                else if (G == Maximum)
                    H = (1.0f / 3) + ((B - R) / 6) / Delta;
                else
                    H = (2.0f / 3) + ((R - G) / 6) / Delta;

                if (H < 0) H += 1;
                if (H > 1) H -= 1;
            }

            return ((int)Math.Round(H * 360), Math.Round((_s * 100) / 100, 2), Math.Round((_l * 100) / 100, 2), 1);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSLA.
        /// </summary>
        /// <param name="Red"> Красный.Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Lightness, Double Alpha) ToHsla(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            var _hsla = ToHsla(Red, Green, Blue);
            return (_hsla.Hue, _hsla.Saturation, _hsla.Lightness, Alpha);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSLA.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Lightness, Double Alpha) ToHsla(Color Value)
        {
            return ToHsla(Value.R, Value.G, Value.B, Math.Round((double)Value.A / 255, 2));
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в CMYK.
        /// </summary>
        /// <param name="Red"> Красный.Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static (Double Cyan, Double Magenta, Double Yellow, Double Key) ToCmyk(Int32 Red, Int32 Green, Int32 Blue)
        {
            #region Проверка правильности входных данных

            if (!Red.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Red\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Green.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Green\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            if (!Blue.IsEntryRange(0, 255))
                throw new ArgumentException("Значения аргумента \"Blue\" выходило за допустимый диапазон! Допустимые значения - от 0 до 255.");

            #endregion

            Double R = (double)Red / 255, G = (double)Green / 255, B = (double)Blue / 255;

            Double K = 1 - Math.Max(Math.Max(R, G), B);
            Double C = (1 - R - K) / (1 - K);
            Double M = (1 - G - K) / (1 - K);
            Double Y = (1 - B - K) / (1 - K);

            return ((int)Math.Round(C * 100), (int)Math.Round(M * 100), (int)Math.Round(Y * 100), (int)Math.Round(K * 100));
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в CMYK.
        /// </summary>
        /// <param name="Red"> Красный.Допустимые значения - от 0 до 255.</param>
        /// <param name="Green">Зеленый. Допустимые значения - от 0 до 255.</param>
        /// <param name="Blue">Синий. Допустимые значения - от 0 до 255.</param>
        /// <param name="Alpha">Прозрачность. Допустимые значения - от 0 до 1.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static (Double Cyan, Double Magenta, Double Yellow, Double Key) ToCmyk(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            #region Проверка правильности входных данных

            if (!Alpha.IsEntryRange(0, 1))
                throw new ArgumentException("Значения аргумента \"Alpha\" выходило за допустимый диапазон! Допустимые значения - от 0 до 1.");

            #endregion
            
            return ToCmyk(Red, Green, Blue);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в CMYK.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static (Double Cyan, Double Magenta, Double Yellow, Double Key) ToCmyk(Color Value)
        {
            return ToCmyk(Value.R, Value.G, Value.B);
        }
    }
}