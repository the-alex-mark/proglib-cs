using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    public partial class MinimalButton : System.Windows.Forms.Button
    {
        public MinimalButton()
        {
            Size = new Size(88, 27);
            _border = true;
            _styleColor = Drawing.MetroColors.Blue;
        }

        private Color _styleColor;
        private Boolean _border;

        [Category("Minimal Appearance"), Description("Цвет оформления.")]
        public Color StyleColor
        {
            get { return _styleColor; }
            set
            {
                _styleColor = value;
                Invalidate();
            }
        }

        [Category("Minimal Appearance"), Description("Отображение границ.")]
        public Boolean Border
        {
            get { return _border; }
            set
            {
                _border = value;
                Invalidate();
            }
        }

        protected virtual TextFormatFlags AsTextFormatFlags(ContentAlignment Alignment)
        {
            switch (Alignment)
            {
                case ContentAlignment.BottomLeft: return TextFormatFlags.Bottom | TextFormatFlags.Left;
                case ContentAlignment.BottomCenter: return TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.BottomRight: return TextFormatFlags.Bottom | TextFormatFlags.Right;
                case ContentAlignment.MiddleLeft: return TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                case ContentAlignment.MiddleCenter: return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.MiddleRight: return TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                case ContentAlignment.TopLeft: return TextFormatFlags.Top | TextFormatFlags.Left;
                case ContentAlignment.TopCenter: return TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.TopRight: return TextFormatFlags.Top | TextFormatFlags.Right;
            }
            throw new InvalidEnumArgumentException();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(0, 0, Width - 1, Height - 1),
                ForeColor,
                BackColor,
                AsTextFormatFlags(TextAlign) | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);

            e.Graphics.DrawRectangle(new Pen(_styleColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));
        }
    }
}
