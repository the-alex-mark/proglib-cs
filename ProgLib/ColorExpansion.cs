using ProgLib.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib
{
    public static class ColorExpansion
    {
        /// <summary>
        /// Преобразует структуру <see cref="Color"/> в удобную для восприятия строку.
        /// </summary>
        /// <param name="Value">Преобразуемый цвет</param>
        /// <param name="Model">Цветовая модель</param>
        /// <param name="CSS">Значение, указывающее будет ли отображаться CSS код</param>
        /// <returns></returns>
        public static String ToString(this Color Value, ColorModel Model, Boolean CSS = true)
        {
            if (CSS)
            {
                switch (Model)
                {
                    case ColorModel.Html:
                        return ColorConvert.ToHtml(Value);

                    case ColorModel.Hex:
                        return ColorConvert.ToHex(Value);

                    case ColorModel.Rgb:
                        return $"rgb({Value.R}, {Value.G}, {Value.B})";

                    case ColorModel.Rgba:
                        return $"rgba({Value.R}, {Value.G}, {Value.B}, {Math.Round((double)Value.A / 255, 2)})";

                    case ColorModel.Hsb:
                        return $"{ColorConvert.ToHsb(Value).Hue}°, {ColorConvert.ToHsb(Value).Saturation * 100}%, {ColorConvert.ToHsb(Value).Brightness * 100}%";

                    case ColorModel.Hsl:
                        return $"hsl({ColorConvert.ToHsla(Value).Hue}, {ColorConvert.ToHsla(Value).Saturation * 100}%, {ColorConvert.ToHsla(Value).Lightness * 100}%)";

                    case ColorModel.Hsla:
                        return $"hsla({ColorConvert.ToHsla(Value).Hue}, {ColorConvert.ToHsla(Value).Saturation * 100}%, {ColorConvert.ToHsla(Value).Lightness * 100}%, {ColorConvert.ToHsla(Value).Alpha})";

                    case ColorModel.Cmyk:
                        return $"{ColorConvert.ToCmyk(Value).Cyan}%, {ColorConvert.ToCmyk(Value).Magenta}%, {ColorConvert.ToCmyk(Value).Yellow}%, {ColorConvert.ToCmyk(Value).Key}%";

                    default:
                        return Value.ToString();
                }
            }
            else
            {
                switch (Model)
                {
                    case ColorModel.Html:
                        return ColorConvert.ToHtml(Value);

                    case ColorModel.Hex:
                        return ColorConvert.ToHex(Value);

                    case ColorModel.Rgb:
                        return $"{Value.R}, {Value.G}, {Value.B}";

                    case ColorModel.Rgba:
                        return $"{Value.R}, {Value.G}, {Value.B}, {Math.Round((double)Value.A / 255, 2)}";

                    case ColorModel.Hsb:
                        return $"{ColorConvert.ToHsb(Value).Hue}°, {ColorConvert.ToHsb(Value).Saturation * 100}%, {ColorConvert.ToHsb(Value).Brightness * 100}%";

                    case ColorModel.Hsl:
                        return $"{ColorConvert.ToHsla(Value).Hue}°, {ColorConvert.ToHsla(Value).Saturation * 100}%, {ColorConvert.ToHsla(Value).Lightness * 100}%";

                    case ColorModel.Hsla:
                        return $"{ColorConvert.ToHsla(Value).Hue}°, {ColorConvert.ToHsla(Value).Saturation * 100}%, {ColorConvert.ToHsla(Value).Lightness * 100}%, {ColorConvert.ToHsla(Value).Alpha}";

                    case ColorModel.Cmyk:
                        return $"{ColorConvert.ToCmyk(Value).Cyan}%, {ColorConvert.ToCmyk(Value).Magenta}%, {ColorConvert.ToCmyk(Value).Yellow}%, {ColorConvert.ToCmyk(Value).Key}%";

                    default:
                        return Value.ToString();
                }
            }
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Ole.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Int32 ToOle(this Color Value)
        {
            return ColorConvert.ToOle(Value);
        }

        /// <summary>
        /// Переводит значения значения Windows цвета в RGBA.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Int32 ToWin32(this Color Value)
        {
            return ColorConvert.ToWin32(Value);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Html.
        /// </summary>
        /// <param name="Value"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHtml(this Color Value)
        {
            return ColorConvert.ToHtml(Value);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в Hex.
        /// </summary>
        /// <param name="Value"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        public static String ToHex(this Color Value)
        {
            return ColorConvert.ToHex(Value);
        }

        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSB.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Brightness) ToHsb(this Color Value)
        {
            return ColorConvert.ToHsb(Value);
        }
        
        /// <summary>
        /// Переводит значения цветовой модели RGBA в HSLA.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static (Int32 Hue, Double Saturation, Double Lightness, Double Alpha) ToHsla(this Color Value)
        {
            return ColorConvert.ToHsla(Value);
        }
                
        /// <summary>
        /// Переводит значения цветовой модели RGBA в CMYK.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static (Double Cyan, Double Magenta, Double Yellow, Double Key) ToCmyk(this Color Value)
        {
            return ColorConvert.ToCmyk(Value);
        }
    }
}