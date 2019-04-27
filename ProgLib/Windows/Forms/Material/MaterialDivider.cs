using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Material
{
    public partial class MaterialDivider : Control
    {
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        public MaterialDivider()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Height = 1;
            //BackColor = SkinManager.GetDividersColor();
        }
    }
}
