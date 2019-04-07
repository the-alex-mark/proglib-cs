using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct Lab
    {
        public Lab(Int32 L, Int32 A, Int32 B)
        {
            this.L = L;
            this.A = A;
            this.B = B;
        }

        public Int32 L { get; set; }
        public Int32 A { get; set; }
        public Int32 B { get; set; }

        public Boolean Equals(Lab Color)
        {
            return (L == Color.L) && (A == Color.A) && (B == Color.B);
        }
        public override String ToString()
        {
            return String.Format("lab({0}, {1}, {2})", L, A, B);
        }
    }
}
