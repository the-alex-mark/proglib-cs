using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProgLib.Windows.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.GroupBox))]
    public partial class MinimalGroupBox : System.Windows.Forms.GroupBox
    {
        public MinimalGroupBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            ForeColor = SystemColors.GrayText;
            _borderColor = SystemColors.ControlDark;
        }

        private Color _borderColor;

        [Category("Appearance"), Description("Цвет границ элемента управления")]
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            SizeF _textSize = e.Graphics.MeasureString(Text, Font);
            Rectangle _rectangle = new Rectangle(
                ClientRectangle.X,
                ClientRectangle.Y + (int)(_textSize.Height / 2f),
                ClientRectangle.Width - 1,
                ClientRectangle.Height - (int)(_textSize.Height / 2f) - 1);

            // Отрисовка текста
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), (float)(Padding.Left + 4), 0f);

            // Отрисовка границ
            e.Graphics.DrawLine(new Pen(_borderColor), _rectangle.Location, new Point(_rectangle.X, _rectangle.Y + _rectangle.Height));
            e.Graphics.DrawLine(new Pen(_borderColor), new Point(_rectangle.X + _rectangle.Width, _rectangle.Y), new Point(_rectangle.X + _rectangle.Width, _rectangle.Y + _rectangle.Height));
            e.Graphics.DrawLine(new Pen(_borderColor), new Point(_rectangle.X, _rectangle.Y + _rectangle.Height), new Point(_rectangle.X + _rectangle.Width, _rectangle.Y + _rectangle.Height));
            e.Graphics.DrawLine(new Pen(_borderColor), new Point(_rectangle.X, _rectangle.Y), new Point(_rectangle.X + Padding.Left, _rectangle.Y));
            e.Graphics.DrawLine(new Pen(_borderColor), new Point(_rectangle.X + Padding.Left + 8 + (int)_textSize.Width, _rectangle.Y), new Point(_rectangle.X + _rectangle.Width, _rectangle.Y));
        }
    }
}