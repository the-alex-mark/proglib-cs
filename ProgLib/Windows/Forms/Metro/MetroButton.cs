using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Metro
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    public partial class MetroButton : System.Windows.Forms.Button
    {
        public MetroButton()
        {
            InitializeComponent();

            Font = new Font(Font.FontFamily, (float)8.25, FontStyle.Bold);
            Size = new Size(127, 37);
            _theme = Theme.Light;
            _styleColor = Drawing.MetroColors.Blue;
            _highlighted = false;

            if (_theme == Theme.Dark)
            {
                _backColor = Color.FromArgb(34, 34, 34);
                _foreColor = (_mouseState == MouseState.Hover || _mouseState == MouseState.Down) ? Color.FromArgb(34, 34, 34) : Color.FromArgb(238, 238, 238);
            }
            else
            {
                _backColor = Color.FromArgb(238, 238, 238);
                _foreColor = (_mouseState == MouseState.Hover || _mouseState == MouseState.Down) ? Color.White : Color.Black;
            }
        }

        private Theme _theme;
        private MouseState _mouseState;
        private Color _styleColor, _backColor, _foreColor;
        private Boolean _highlighted;

        [Category("Внешний вид")]
        public new Color BackColor
        {
            get { return _backColor; }
        }

        [Category("Metro Appearance"), Description("Цветовая тема элемента управления")]
        public Theme Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                Invalidate();
            }
        }

        [Category("Metro Appearance"), Description("Цвет оформления")]
        public Color StyleColor
        {
            get { return _styleColor; }
            set
            {
                _styleColor = value;
                Invalidate();
            }
        }

        [Category("Metro Appearance"), Description("Выделенная кнопка")]
        public Boolean Highlighted
        {
            get { return _highlighted; }
            set
            {
                _highlighted = value;
                Invalidate();
            }
        }
        
        [Category("Внешний вид")]
        public new Color ForeColor
        {
            get { return _foreColor; }
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

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _mouseState = MouseState.Down;

            OnClick(e);
            base.OnMouseHover(e);
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            _mouseState = MouseState.Hover;

            base.OnMouseHover(e);
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _mouseState = MouseState.None;

            base.OnMouseLeave(e);
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _mouseState = MouseState.Hover;

            base.OnMouseLeave(e);
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            // Настройка цветов
            _backColor = (Enabled) ? MetroPaint.BackColor.Button.Normal(_theme) : MetroPaint.BackColor.Button.Disabled(_theme);
            _foreColor = (Enabled) 
                ? (_mouseState == MouseState.Hover) ? MetroPaint.ForeColor.Button.Hover(_theme) : (_mouseState == MouseState.Down) ? MetroPaint.ForeColor.Button.Press(_theme) : MetroPaint.ForeColor.Button.Normal(_theme) 
                : MetroPaint.ForeColor.Button.Disabled(_theme);

            // Отрисовка
            e.Graphics.Clear(_backColor);

            if (Enabled)
            {
                e.Graphics.DrawRectangle(
                    new Pen((_highlighted) ? _styleColor : MetroPaint.BorderColor.Button.Normal(_theme), (_highlighted) ? 2 : 1),
                    (_highlighted) ? new Rectangle(1, 1, Width - 2, Height - 2) : new Rectangle(0, 0, Width - 1, Height - 1));
            }
            else
                e.Graphics.DrawRectangle(new Pen(MetroPaint.BorderColor.Button.Disabled(_theme), 1), new Rectangle(0, 0, Width - 1, Height - 1));

            if (_mouseState == MouseState.Hover)
                e.Graphics.FillRectangle(new SolidBrush(MetroPaint.BackColor.Button.Hover(_theme)), new Rectangle(0, 0, Width, Height));

            if (_mouseState == MouseState.Down)
                e.Graphics.FillRectangle(new SolidBrush(MetroPaint.BackColor.Button.Press(_theme)), new Rectangle(0, 0, Width, Height));

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(0, 0, Width - 1, Height - 1),
                _foreColor,
                (_mouseState == MouseState.Hover) ? MetroPaint.BackColor.Button.Hover(_theme) : (_mouseState == MouseState.Down) ? MetroPaint.BackColor.Button.Press(_theme) : _backColor,
                AsTextFormatFlags(TextAlign) | TextFormatFlags.EndEllipsis);
        }
    }
}