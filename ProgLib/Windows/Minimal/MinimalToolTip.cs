using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ProgLib.Windows.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ToolTip))]
    public partial class MinimalToolTip : System.Windows.Forms.ToolTip, IComponent, IDisposable
    {
        public MinimalToolTip()
        {
            OwnerDraw = true;

            Popup += new PopupEventHandler(OnPopup);
            Draw += new DrawToolTipEventHandler(OnDraw);
        }

        private Size Size;
        private Color _borderColor;

        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        private void OnPopup(object sender, PopupEventArgs e)
        {
            Size = TextRenderer.MeasureText(GetToolTip(e.AssociatedControl), new Font("Century Gothic", 9f));
            e.ToolTipSize = new Size(Size.Width, Size.Height + 5);

            Size = e.ToolTipSize;
        }
        private void OnDraw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(_borderColor), 1f), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));

            e.Graphics.DrawString(
                e.ToolTipText,
                new Font("Century Gothic", 9f),
                new SolidBrush(ForeColor),
                e.Bounds,
                new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, HotkeyPrefix = HotkeyPrefix.None, FormatFlags = StringFormatFlags.NoWrap });
        }
    }
}