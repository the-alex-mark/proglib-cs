using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct XYZ
    {
        public XYZ(Int32 X, Int32 Y, Int32 Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public Int32 Z { get; set; }

        public Boolean Equals(XYZ Color)
        {
            return (X == Color.X) && (Y == Color.Y) && (Z == Color.Z);
        }
        public override String ToString()
        {
            return String.Format("xyz({0}, {1}, {2})", X, Y, Z);
        }
    }
}
