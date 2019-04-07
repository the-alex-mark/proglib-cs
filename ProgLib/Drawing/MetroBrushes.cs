using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProgLib.Drawing
{
    public class MetroBrushes
    {
        public static List<Brush> Collection
        {
            get
            {
                List<Brush> Collection = new List<Brush>
                {
                    Black, White, Silver, Blue, Green, Lime, Teal, Orange, Brown, Pink, Magenta, Purple, Red, Yellow
                };

                return Collection;
            }
        }

        public static Brush Black
        {
            get
            {
                return new SolidBrush(MetroColors.Black);
            }
        }
        public static Brush White
        {
            get
            {
                return new SolidBrush(MetroColors.White);
            }
        }
        public static Brush Silver
        {
            get
            {
                return new SolidBrush(MetroColors.Silver);
            }
        }
        public static Brush Blue
        {
            get
            {
                return new SolidBrush(MetroColors.Blue);
            }
        }
        public static Brush Green
        {
            get
            {
                return new SolidBrush(MetroColors.Green);
            }
        }
        public static Brush Lime
        {
            get
            {
                return new SolidBrush(MetroColors.Lime);
            }
        }
        public static Brush Teal
        {
            get
            {
                return new SolidBrush(MetroColors.Teal);
            }
        }
        public static Brush Orange
        {
            get
            {
                return new SolidBrush(MetroColors.Orange);
            }
        }
        public static Brush Brown
        {
            get
            {
                return new SolidBrush(MetroColors.Brown);
            }
        }
        public static Brush Pink
        {
            get
            {
                return new SolidBrush(MetroColors.Pink);
            }
        }
        public static Brush Magenta
        {
            get
            {
                return new SolidBrush(MetroColors.Magenta);
            }
        }
        public static Brush Purple
        {
            get
            {
                return new SolidBrush(MetroColors.Purple);
            }
        }
        public static Brush Red
        {
            get
            {
                return new SolidBrush(MetroColors.Red);
            }
        }
        public static Brush Yellow
        {
            get
            {
                return new SolidBrush(MetroColors.Yellow);
            }
        }
    }
}