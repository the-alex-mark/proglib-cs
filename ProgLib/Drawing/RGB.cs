using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct RGB
    {
        public RGB(Int32 Red, Int32 Green, Int32 Blue)
        {
            R = Red;
            G = Green;
            B = Blue;
        }

        public Int32 R { get; set; }
        public Int32 G { get; set; }
        public Int32 B { get; set; }

        public Boolean Equals(RGB Color)
        {
            return (R == Color.R) && (G == Color.G) && (B == Color.B);
        }
        public override String ToString()
        {
            return String.Format("rgb({0}, {1}, {2})", R, G, B);
        }
    }
}
