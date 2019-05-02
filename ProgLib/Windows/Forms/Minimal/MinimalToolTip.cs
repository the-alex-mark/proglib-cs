using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ToolTip))]
    public partial class MinimalToolTip : System.Windows.Forms.ToolTip, IComponent, IDisposable
    {
        public MinimalToolTip()
        {
            OwnerDraw = true;
            ForeColor = Color.Black;
            BackColor = Color.Gainsboro;
            BorderColor = SystemColors.ControlDark;
            Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);

            Popup += new PopupEventHandler(OnPopup);
            Draw += new DrawToolTipEventHandler(OnDraw);
        }
        
        #region Properties

        public Color BorderColor { get; set; }

        public Font Font { get; set; }

        #endregion
        
        private void OnPopup(object sender, PopupEventArgs e)
        {
            Size _size = TextRenderer.MeasureText(GetToolTip(e.AssociatedControl), Font);
            e.ToolTipSize = new Size(_size.Width + 35, _size.Height + 8);
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            // Отрисовка границ
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(BorderColor), 1f), new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));

            // Отрисовка текста
            TextRenderer.DrawText(
                e.Graphics,
                e.ToolTipText,
                Font,
                e.Bounds,
                ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);
        }
    }
}