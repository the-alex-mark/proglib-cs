using System;
using System.Drawing;

namespace ProgLib.Drawing
{
    public static class Convert
    {
        public static (Int32, Int32, Int32) ToRgb(Color Color)
        {
            var tuple = (5, 10);
            return (5, 5, 5);
        }

        private static (int, int) GetValues()
        {
            var result = (1, 3);
            return result;
        }

        public static (Int32, Int32, Int32) ToColor(this HSL Color)
        {
            Color C = System.Drawing.Color.FromArgb(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B);
            return (C.R, C.G, C.B);
        }

        public static Color ToColor(this RGB Color)
        {
            return System.Drawing.Color.FromArgb(Color.R, Color.G, Color.B);
        }
        public static Color ToColor(this RGBA Color)
        {
            return System.Drawing.Color.FromArgb(Color.R, Color.G, Color.B);
        }
        public static Color ToColor(this HSB Color)
        {
            return System.Drawing.Color.FromArgb(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B);
        }
        //public static Color ToColor(this HSL Color)
        //{
        //    return System.Drawing.Color.FromArgb(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B);
        //}
        public static Color ToColor(this CMYK Color)
        {
            return System.Drawing.Color.FromArgb(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B);
        }
        public static Color ToColor(this YCbCr Color)
        {
            return System.Drawing.Color.FromArgb(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B);
        }
        public static Color ToColor(this String Color)
        {
            if (!Color.StartsWith("#") || Color.Length != 7) { throw new Exception("Значение не является цветом в шестнадцатеричной системе счисления!"); }
            else { return System.Drawing.Color.FromArgb(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B); }
        }

        public static RGB ToRGB(this Color Color)
        {
            return new RGB(Color.R, Color.G, Color.B);
        }
        public static RGB ToRGB(this RGBA Color)
        {
            return new RGB(Color.R, Color.G, Color.B);
        }
        public static RGB ToRGB(this HSB Color)
        {
            Double C = Color.B * Color.S;
            Double X = C * (1 - Math.Abs(((double)Color.H / 60) % 2 - 1));
            Double m = Color.B - C;

            Double R = 0, G = 0, B = 0;

            if (Color.H >= 0 && Color.H < 60)
            {
                R = C;
                G = X;
                B = 0;
            }
            else if (Color.H >= 60 && Color.H < 120)
            {
                R = X;
                G = C;
                B = 0;
            }
            else if (Color.H >= 120 && Color.H < 180)
            {
                R = 0;
                G = C;
                B = X;
            }
            else if (Color.H >= 180 && Color.H < 240)
            {
                R = 0;
                G = X;
                B = C;
            }
            else if (Color.H >= 240 && Color.H < 300)
            {
                R = X;
                G = 0;
                B = C;
            }
            else if (Color.H >= 300 && Color.H <= 360)
            {
                R = C;
                G = 0;
                B = X;
            }

            return new RGB((int)Math.Round((R + m) * 255), (int)Math.Round((G + m) * 255), (int)Math.Round((B + m) * 255));
        }
        public static RGB ToRGB(this HSL Color)
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

            if (Color.S == 0)
            {
                r = g = b = (byte)(Color.L * 255);
            }
            else
            {
                float v1, v2;
                float hue = (float)Color.H / 360;

                v2 = (Color.L < 0.5) ? ((float)Color.L * (1 + (float)Color.S)) : (((float)Color.L + (float)Color.S) - ((float)Color.L * (float)Color.S));
                v1 = 2 * (float)Color.L - v2;

                r = (byte)Math.Round((255 * HueToRGB(v1, v2, hue + (1.0f / 3))));
                g = (byte)Math.Round((255 * HueToRGB(v1, v2, hue)));
                b = (byte)Math.Round((255 * HueToRGB(v1, v2, hue - (1.0f / 3))));
            }

            return new RGB(r, g, b);
        }
        public static RGB ToRGB(this CMYK Color)
        {
            return new RGB(
                (int)(255 * (1 - Color.C) * (1 - Color.K)),
                (int)(255 * (1 - Color.M) * (1 - Color.K)),
                (int)(255 * (1 - Color.Y) * (1 - Color.K)));
        }
        public static RGB ToRGB(this YCbCr Color)
        {
            float R = Math.Max(0.0f, Math.Min(1.0f, (float)(Color.Y + 0.0000 * Color.Cb + 1.4022 * Color.Cr)));
            float G = Math.Max(0.0f, Math.Min(1.0f, (float)(Color.Y - 0.3456 * Color.Cb - 0.7145 * Color.Cr)));
            float B = Math.Max(0.0f, Math.Min(1.0f, (float)(Color.Y + 1.7710 * Color.Cb + 0.0000 * Color.Cr)));

            return new RGB((int)Math.Round((R * 255)), (int)Math.Round((G * 255)), (int)Math.Round((B * 255)));
        }
        public static RGB ToRGB(this String Color)
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
            Color = (Color.StartsWith("#") && Color.Length == 7) ? Color.Remove(0, 1) : Color;

            return new RGB(
                ToDecimal(Color.Substring(0, 2)),
                ToDecimal(Color.Substring(2, 2)),
                ToDecimal(Color.Substring(4, 2)));
        }

        public static RGBA ToRGBA(this Color Color)
        {
            return new RGBA(Color.R, Color.G, Color.B, Color.A / 255 * 100);
        }
        public static RGBA ToRGBA(this RGB Color)
        {
            return new RGBA(Color.R, Color.G, Color.B, 1);
        }
        public static RGBA ToRGBA(this HSB Color)
        {
            return new RGBA(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B, 1);
        }
        public static RGBA ToRGBA(this HSL Color)
        {
            return new RGBA(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B, 1);
        }
        public static RGBA ToRGBA(this CMYK Color)
        {
            return new RGBA(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B, 1);
        }
        public static RGBA ToRGBA(this YCbCr Color)
        {
            return new RGBA(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B, 1);
        }
        public static RGBA ToRGBA(this String Color)
        {
            if (!Color.StartsWith("#") || Color.Length != 7) { throw new Exception("Значение не является цветом в шестнадцатеричной системе счисления!"); }
            else { return new RGBA(ToRGB(Color).R, ToRGB(Color).G, ToRGB(Color).B, 1); }
        }

        public static HSB ToHSB(this Color Color)
        {
            return new HSB(ToHSB(ToRGB(Color)).H, ToHSB(ToRGB(Color)).S, ToHSB(ToRGB(Color)).B);
        }
        public static HSB ToHSB(this RGB Color)
        {
            double delta, min;
            double h = 0, s, b;

            min = Math.Min(Math.Min(Color.R, Color.G), Color.B);
            b = Math.Max(Math.Max(Color.R, Color.G), Color.B);
            delta = b - min;

            if (b == 0.0)
                s = 0;
            else
                s = delta / b;

            if (s == 0)
                h = 0.0;

            else
            {
                if (Color.R == b)
                    h = (Color.G - Color.B) / delta;
                else if (Color.G == b)
                    h = 2 + (Color.B - Color.R) / delta;
                else if (Color.B == b)
                    h = 4 + (Color.R - Color.G) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            return new HSB((int)Math.Round(h), Math.Round(s, 2), Math.Round((b / 255), 2));
        }
        public static HSB ToHSB(this RGBA Color)
        {
            return new HSB(ToHSB(ToRGB(Color)).H, ToHSB(ToRGB(Color)).S, ToHSB(ToRGB(Color)).B);
        }
        public static HSB ToHSB(this HSL Color)
        {
            return new HSB(ToHSB(ToRGB(Color)).H, ToHSB(ToRGB(Color)).S, ToHSB(ToRGB(Color)).B);
        }
        public static HSB ToHSB(this CMYK Color)
        {
            return new HSB(ToHSB(ToRGB(Color)).H, ToHSB(ToRGB(Color)).S, ToHSB(ToRGB(Color)).B);
        }
        public static HSB ToHSB(this YCbCr Color)
        {
            return new HSB(ToHSB(ToRGB(Color)).H, ToHSB(ToRGB(Color)).S, ToHSB(ToRGB(Color)).B);
        }
        public static HSB ToHSB(this String Color)
        {
            if (!Color.StartsWith("#") || Color.Length != 7) { throw new Exception("Значение не является цветом в шестнадцатеричной системе счисления!"); }
            else { return new HSB(ToHSB(ToRGB(Color)).H, ToHSB(ToRGB(Color)).S, ToHSB(ToRGB(Color)).B); }
        }

        public static HSL ToHSL(this Color Color)
        {
            return new HSL(ToHSL(ToRGB(Color)).H, ToHSL(ToRGB(Color)).S, ToHSL(ToRGB(Color)).L);
        }
        public static HSL ToHSL(this RGB Color)
        {
            //HSL HSL = new HSL();

            Int32 _h;
            Double _s, _l;

            Double R = (Color.R / 255.0f);
            Double G = (Color.G / 255.0f);
            Double B = (Color.B / 255.0f);

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

            return new HSL((int)Math.Round(H * 360), Math.Round((_s * 100) / 100, 2), Math.Round((_l * 100) / 100, 2));
        }
        public static HSL ToHSL(this RGBA Color)
        {
            return new HSL(ToHSL(ToRGB(Color)).H, ToHSL(ToRGB(Color)).S, ToHSL(ToRGB(Color)).L);
        }
        public static HSL ToHSL(this HSB Color)
        {
            return new HSL(ToHSL(ToRGB(Color)).H, ToHSL(ToRGB(Color)).S, ToHSL(ToRGB(Color)).L);
        }
        public static HSL ToHSL(this CMYK Color)
        {
            return new HSL(ToHSL(ToRGB(Color)).H, ToHSL(ToRGB(Color)).S, ToHSL(ToRGB(Color)).L);
        }
        public static HSL ToHSL(this YCbCr Color)
        {
            return new HSL(ToHSL(ToRGB(Color)).H, ToHSL(ToRGB(Color)).S, ToHSL(ToRGB(Color)).L);
        }
        public static HSL ToHSL(this String Color)
        {
            if (!Color.StartsWith("#") || Color.Length != 7) { throw new Exception("Значение не является цветом в шестнадцатеричной системе счисления!"); }
            { return new HSL(ToHSL(ToRGB(Color)).H, ToHSL(ToRGB(Color)).S, ToHSL(ToRGB(Color)).L); }
        }

        public static CMYK ToCMYK(this Color Color)
        {
            return new CMYK(ToCMYK(ToRGB(Color)).C, ToCMYK(ToRGB(Color)).M, ToCMYK(ToRGB(Color)).Y, ToCMYK(ToRGB(Color)).K);
        }
        public static CMYK ToCMYK(this RGB Color)
        {
            Double R = (double)Color.R / 255;
            Double G = (double)Color.G / 255;
            Double B = (double)Color.B / 255;

            Double K = 1 - Math.Max(Math.Max(R, G), B);
            Double C = (1 - R - K) / (1 - K);
            Double M = (1 - G - K) / (1 - K);
            Double Y = (1 - B - K) / (1 - K);

            return new CMYK((int)Math.Round(C * 100), (int)Math.Round(M * 100), (int)Math.Round(Y * 100), (int)Math.Round(K * 100));
        }
        public static CMYK ToCMYK(this RGBA Color)
        {
            return new CMYK(ToCMYK(ToRGB(Color)).C, ToCMYK(ToRGB(Color)).M, ToCMYK(ToRGB(Color)).Y, ToCMYK(ToRGB(Color)).K);
        }
        public static CMYK ToCMYK(this HSB Color)
        {
            return new CMYK(ToCMYK(ToRGB(Color)).C, ToCMYK(ToRGB(Color)).M, ToCMYK(ToRGB(Color)).Y, ToCMYK(ToRGB(Color)).K);
        }
        public static CMYK ToCMYK(this HSL Color)
        {
            return new CMYK(ToCMYK(ToRGB(Color)).C, ToCMYK(ToRGB(Color)).M, ToCMYK(ToRGB(Color)).Y, ToCMYK(ToRGB(Color)).K);
        }
        public static CMYK ToCMYK(this YCbCr Color)
        {
            return new CMYK(ToCMYK(ToRGB(Color)).C, ToCMYK(ToRGB(Color)).M, ToCMYK(ToRGB(Color)).Y, ToCMYK(ToRGB(Color)).K);
        }
        public static CMYK ToCMYK(this String Color)
        {
            if (!Color.StartsWith("#") || Color.Length != 7) { throw new Exception("Значение не является цветом в шестнадцатеричной системе счисления!"); }
            else { return new CMYK(ToCMYK(ToRGB(Color)).C, ToCMYK(ToRGB(Color)).M, ToCMYK(ToRGB(Color)).Y, ToCMYK(ToRGB(Color)).K); }
        }

        public static String ToHEX(this Color Color)
        {
            return ToHEX(ToRGB(Color));
        }
        public static String ToHEX(this RGB Color)
        {
            return String.Format("#{0:X2}{1:X2}{2:X2}", Color.R, Color.G, Color.B).ToUpper();
        }
        public static String ToHEX(this RGBA Color)
        {
            return ToHEX(ToRGB(Color));
        }
        public static String ToHEX(this HSB Color)
        {
            return ToHEX(ToRGB(Color));
        }
        public static String ToHEX(this HSL Color)
        {
            return ToHEX(ToRGB(Color));
        }
        public static String ToHEX(this YCbCr Color)
        {
            return ToHEX(ToRGB(Color));
        }
        public static String ToHEX(this CMYK Color)
        {
            return ToHEX(ToRGB(Color));
        }

        public static YCbCr ToYCbCr(this Color Color)
        {
            return new YCbCr(ToYCbCr(ToRGB(Color)).Y, ToYCbCr(ToRGB(Color)).Cb, ToYCbCr(ToRGB(Color)).Cr);
        }
        public static YCbCr ToYCbCr(this RGB Color)
        {
            float fr = (float)Color.R / 255;
            float fg = (float)Color.G / 255;
            float fb = (float)Color.B / 255;

            float Y = (float)(0.2989 * fr + 0.5866 * fg + 0.1145 * fb);
            float Cb = (float)(-0.1687 * fr - 0.3313 * fg + 0.5000 * fb);
            float Cr = (float)(0.5000 * fr - 0.4184 * fg - 0.0816 * fb);

            return new YCbCr(Y, Cb, Cr);
        }
        public static YCbCr ToYCbCr(this RGBA Color)
        {
            return new YCbCr(ToYCbCr(ToRGB(Color)).Y, ToYCbCr(ToRGB(Color)).Cb, ToYCbCr(ToRGB(Color)).Cr);
        }
        public static YCbCr ToYCbCr(this HSB Color)
        {
            return new YCbCr(ToYCbCr(ToRGB(Color)).Y, ToYCbCr(ToRGB(Color)).Cb, ToYCbCr(ToRGB(Color)).Cr);
        }
        public static YCbCr ToYCbCr(this HSL Color)
        {
            return new YCbCr(ToYCbCr(ToRGB(Color)).Y, ToYCbCr(ToRGB(Color)).Cb, ToYCbCr(ToRGB(Color)).Cr);
        }
        public static YCbCr ToYCbCr(this CMYK Color)
        {
            return new YCbCr(ToYCbCr(ToRGB(Color)).Y, ToYCbCr(ToRGB(Color)).Cb, ToYCbCr(ToRGB(Color)).Cr);
        }
        public static YCbCr ToYCbCr(this String Color)
        {
            if (!Color.StartsWith("#") || Color.Length != 7) { throw new Exception("Значение не является цветом в шестнадцатеричной системе счисления!"); }
            { return new YCbCr(ToYCbCr(ToRGB(Color)).Y, ToYCbCr(ToRGB(Color)).Cb, ToYCbCr(ToRGB(Color)).Cr); }
        }

        // -- Не готово!
        private static String XYZ(this RGB COLOR)
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
        private static String LAB(this RGB COLOR)
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
    }
}
