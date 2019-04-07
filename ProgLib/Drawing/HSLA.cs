using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct HSLA
    {
        public HSLA(Int32 Hue, Double Saturation, Double Lightness, Double Alpha)
        {
            H = Hue;
            S = Saturation;
            L = Lightness;
            A = Alpha;
        }

        public Int32 H { get; set; }
        public Double S { get; set; }
        public Double L { get; set; }
        public Double A { get; set; }

        public bool Equals(HSLA Color)
        {
            return (H == Color.H) && (S == Color.S) && (L == Color.L) && (A == Color.A);
        }
        public override String ToString()
        {
            return String.Format("hsla({0}, {1}, {2}, {3})", H, S, L, A);
        }
    }
}
