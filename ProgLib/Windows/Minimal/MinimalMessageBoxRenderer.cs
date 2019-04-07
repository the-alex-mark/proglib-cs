using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Windows.Minimal
{
    public class MinimalMessageBoxRenderer
    {
        public Boolean Animation
        {
            get;
            set;
        }

        public Color BackColor
        {
            get;
            set;
        }

        public Color StatusBarColor
        {
            get;
            set;
        }

        public Color StyleColor
        {
            get;
            set;
        }

        public Boolean Border
        {
            get;
            set;
        }
        
        public Font Font
        {
            get;
            set;
        }

        public Color HeaderColor
        {
            get;
            set;
        }

        public Color TextColor
        {
            get;
            set;
        }
    }
}
