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
    [ToolboxBitmap(typeof(System.Windows.Forms.CheckBox))]
    public partial class MetroCheckBox : System.Windows.Forms.CheckBox
    {
        public MetroCheckBox()
        {
            InitializeComponent();

            _theme = Theme.Light;
            _useStyleColor = false;
            _styleColor = Drawing.MetroColors.Blue;
        }

        private Theme _theme;
        private MouseState _mouseState;
        private Color _styleColor;
        private Boolean _useStyleColor;

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

        [Category("Metro Appearance"), Description("Цвет оформления при Checked равном \"true\"")]
        public Color StyleColor
        {
            get { return _styleColor; }
            set
            {
                _styleColor = value;
                Invalidate();
            }
        }

        [Category("Metro Appearance"), Description("Цвет оформления при Checked равном \"true\"")]
        public Boolean UseStyleColor
        {
            get { return _useStyleColor; }
            set
            {
                _useStyleColor = value;
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
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
        protected override void OnPaint(PaintEventArgs e)
        {
            // Настройка цветов
            Color _foreColor = MetroPaint.ForeColor.CheckBox.Normal(_theme);

            // Отрисовка
            e.Graphics.Clear(BackColor);

            Rectangle _borderRectangle = new Rectangle(5, (Height / 2) - 8, 13, 13);
            e.Graphics.DrawRectangle(new Pen((Enabled) ? MetroPaint.BorderColor.CheckBox.Normal(_theme) : MetroPaint.BorderColor.CheckBox.Disabled(_theme), 1), _borderRectangle);

            if (_mouseState == MouseState.Hover)
            {
                e.Graphics.DrawRectangle(new Pen(MetroPaint.BorderColor.CheckBox.Hover(_theme), 1), _borderRectangle);
                _foreColor = MetroPaint.ForeColor.CheckBox.Hover(_theme);
            }

            if (_mouseState == MouseState.None)
                _foreColor = (_useStyleColor) ? _styleColor : MetroPaint.ForeColor.CheckBox.Normal(_theme);
            
            if (Checked)
                e.Graphics.FillRectangle(new SolidBrush(_styleColor), new Rectangle(_borderRectangle.X + 2, _borderRectangle.Y + 2, 10, 10));
            
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(_borderRectangle.X + _borderRectangle.Width + 4, 0, Width - 1, Height - 1),
                (Enabled) ? _foreColor : MetroPaint.ForeColor.CheckBox.Disabled(_theme),
                BackColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
    }
}