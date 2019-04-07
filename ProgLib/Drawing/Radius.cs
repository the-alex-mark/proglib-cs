using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Drawing
{
    public struct Radius
    {
        public Radius(Int32 All)
        {
            LeftTop = All;
            RightTop = All;
            LeftBottom = All;
            RightBottom = All;
        }

        public Radius(Int32 LeftTop, Int32 RightTop, Int32 RightBottom, Int32 LeftBottom)
        {
            this.LeftTop = LeftTop;
            this.RightTop = RightTop;
            this.RightBottom = RightBottom;
            this.LeftBottom = LeftBottom;
        }

        public Int32 LeftTop { get; }
        public Int32 RightTop { get; }
        public Int32 RightBottom { get; }
        public Int32 LeftBottom { get; }
    }
}
