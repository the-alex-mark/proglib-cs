using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox))]
    public partial class MinimalComboBox : System.Windows.Forms.ComboBox
    {
        public MinimalComboBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawVariable;
            ItemHeight = 18;

            ForeColor = SystemColors.GrayText;
            _selectForeColor = ForeColor;
            BackColor = SystemColors.Control;
            _borderColor = SystemColors.ControlDark;
            _arrowColor = SystemColors.ControlDark;
            _buttonColor = BackColor;

            try
            {
                _selectColor = Color.FromArgb(BackColor.R - 20, BackColor.G - 20, BackColor.B - 20);
            }
            catch { }
        }

        [DllImport("user32")]
        private static extern IntPtr GetDC(IntPtr hWnd);
        private Color _selectForeColor, _borderColor, _arrowColor, _buttonColor, _selectColor;
        
        [Category("Appearance"), Description("Цвет выделенного \"Item\"")]
        public Color SelectColor
        {
            get { return _selectColor; }
            set
            {
                _selectColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет шрифта выделенного \"Item\"")]
        public Color SelectForeColor
        {
            get { return _selectForeColor; }
            set
            {
                _selectForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет кнопки")]
        public Color ButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет стрелки")]
        public Color ArrowColor
        {
            get { return _arrowColor; }
            set
            {
                _arrowColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет границ")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        protected virtual Image Arrow(Color Border)
        {
            Bitmap Image = new Bitmap(8, 7);
            using (Graphics G = Graphics.FromImage(Image))
            {
                G.Clear(Color.Transparent);

                Point[] Lines = new Point[]
                {
                    new Point(1, 1),
                    new Point(4, 4),
                    new Point(7, 1)
                };
                G.DrawLines(new Pen(Border, 1), Lines);
            }

            return Image;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 15)
            {
                using (Graphics G = CreateGraphics())
                {
                    G.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
                    G.FillRectangle(new SolidBrush(_buttonColor), new Rectangle(Width - 17, 0, 17, Height));
                    using (GraphicsPath PTH = new GraphicsPath())
                    {
                        G.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(2, 5, Bounds.Width - 20, Bounds.Height - 1), StringFormat.GenericDefault);
                        G.DrawImage(Arrow(_arrowColor), new Point(Width - 13, ((Height - 3) / 2)));
                    }
                }
                
                ControlPaint.DrawBorder(Graphics.FromHdc(GetDC(Handle)), new Rectangle(0, 0, Width, Height), _borderColor, ButtonBorderStyle.Solid);
            }
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            Color _foreColorSelect = ForeColor;

            if (e.Index < 0) return;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          _selectColor);

                _foreColorSelect = SelectForeColor;
            }
            e.DrawBackground();

            e.Graphics.DrawString(Items[e.Index].ToString(), Font, new SolidBrush(_foreColorSelect), new Rectangle(2, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 1), StringFormat.GenericDefault);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            Invalidate();
        }
    }
}