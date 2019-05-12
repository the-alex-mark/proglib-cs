using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Material
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public class MaterialTabControl : TabControl, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }
    }
}
