using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    /// <summary>
    /// Представляет цвета в терминах каналов Hue, Saturation, и Brightness (HSB).
    /// </summary>
    public struct HSB
    {
        public HSB(Int32 Hue, Double Saturation, Double Brightness)
        {
            H = Hue;
            S = Saturation;
            B = Brightness;
        }

        public Int32 H { get; set; }
        public Double S { get; set; }
        public Double B { get; set; }

        public Boolean Equals(HSB Color)
        {
            return (H == Color.H) && (S == Color.S) && (B == Color.B);
        }
        public override String ToString()
        {
            return String.Format("hsb({0}, {1}, {2})", H, S, B);
        }
    }
}
