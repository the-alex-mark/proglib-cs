using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct RGBA
    {
        public RGBA(Int32 Red, Int32 Green, Int32 Blue, Double Alpha)
        {
            R = Red;
            G = Green;
            B = Blue;
            A = Alpha;
        }

        public Int32 R { get; set; }
        public Int32 G { get; set; }
        public Int32 B { get; set; }
        public Double A { get; set; }

        public Boolean Equals(RGBA Color)
        {
            return (R == Color.R) && (G == Color.G) && (B == Color.B) && (A == Color.A);
        }
        public override String ToString()
        {
            return String.Format("rgba({0}, {1}, {2}, {3})", R, G, B, A);
        }
    }
}
