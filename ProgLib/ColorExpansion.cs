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
        /// <returns></returns>
        public static String ToString(this Color Value, ColorModel Model)
        {
            switch (Model)
            {
                case ColorModel.Rgb:
                    return $"rgb({Value.R}, {Value.G}, {Value.B})";

                case ColorModel.Rgba:
                    return $"rgba({Value.R}, {Value.G}, {Value.B}, {Math.Round(((double)(Value.A * 100) / 255) / 100, 2)})";

                //case ColorModel.Hsb:
                //    return $"hsb({this.Value.R}, {this.Value.G}, {this.Value.B})";

                default:
                    return Value.ToString();
            }
        }
    }
}
