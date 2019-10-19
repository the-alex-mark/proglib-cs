using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Windows.Forms.Cyotek
{
    public struct HSB
    {
        public HSB(Int32 H, Double S, Double B)
        {
            this.H = H;
            this.S = S;
            this.B = B;
        }

        public Int32 H
        {
            get;
            set;
        }

        public Double S
        {
            get;
            set;
        }

        public Double B
        {
            get;
            set;
        }
    }
}
