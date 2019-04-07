using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct YCbCr
    {
        public YCbCr(float y, float cb, float cr)
        {
            Y = y;
            Cb = cb;
            Cr = cr;
        }

        public float Y { get; set; }
        public float Cb { get; set; }
        public float Cr { get; set; }

        public bool Equals(YCbCr ycbcr)
        {
            return (Y == ycbcr.Y) && (Cb == ycbcr.Cb) && (Cr == ycbcr.Cr);
        }
        public override String ToString()
        {
            return String.Format("YCbCr({0}, {1}, {2})", Y, Cb, Cr);
        }
    }
}
