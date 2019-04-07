using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct HSL
    {
        public HSL(Int32 Hue, Double Saturation, Double Lightness)
        {
            H = Hue;
            S = Saturation;
            L = Lightness;
        }

        public Int32 H { get; set; }
        public Double S { get; set; }
        public Double L { get; set; }

        public bool Equals(HSL Color)
        {
            return (H == Color.H) && (S == Color.S) && (L == Color.L);
        }
        public override String ToString()
        {
            return String.Format("hsl({0}, {1}, {2})", H, S, L);
        }
    }
}
