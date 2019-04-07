using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct CMYK
    {
        public CMYK(Int32 Cyan, Int32 Magenta, Int32 Yellow, Int32 Key)
        {
            C = Cyan;
            M = Magenta;
            Y = Yellow;
            K = Key;
        }

        public Int32 C { get; set; }
        public Int32 M { get; set; }
        public Int32 Y { get; set; }
        public Int32 K { get; set; }

        public bool Equals(CMYK Color)
        {
            return (C == Color.C) && (M == Color.M) && (Y == Color.Y) && (K == Color.K);
        }
        public override String ToString()
        {
            return String.Format("cmyk({0}, {1}, {2}, {3})", C, M, Y, K);
        }
    }
}
