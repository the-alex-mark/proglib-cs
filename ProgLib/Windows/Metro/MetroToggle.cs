using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ProgLib.Windows.Metro
{
    public partial class MetroToggle : System.Windows.Forms.CheckBox
    {
        public MetroToggle()
        {
            InitializeComponent();

            _theme = Theme.Light;
            _styleColor = Drawing.MetroColors.Blue;
            
            AutoSize = false;
            Size = new Size(50, 17);
        }
        
        private Theme _theme;
        private MouseState _mouseState;
        private Color _styleColor;

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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _mouseState = MouseState.Hover;
            base.OnMouseHover(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _mouseState = MouseState.None;
            base.OnMouseLeave(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //// -- Настройка цветов
            //Color BorderColor, ActiveColor, SlideColor;

            //ActiveColor = (Checked) ? _styleColor : Color.FromArgb(153, 153, 153);
            //if (_theme == Theme.Dark)
            //{
            //    BorderColor = (_mouseState == MouseState.Hover) ? Color.FromArgb(204, 204, 204) : Color.FromArgb(153, 153, 153);
            //    SlideColor = Color.FromArgb(204, 204, 204);
            //}
            //else
            //{
            //    BorderColor = (_mouseState == MouseState.Hover) ? Color.FromArgb(51, 51, 51) : Color.FromArgb(153, 153, 153);
            //    SlideColor = Color.FromArgb(51, 51, 51);
            //}
            
            //// -- Отрисовка
            //e.Graphics.Clear(BackColor);

            //e.Graphics.DrawRectangle(
            //    new Pen(BorderColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));

            //e.Graphics.FillRectangle(
            //    new SolidBrush(ActiveColor), new Rectangle(2, 2, Width - 4, Height - 4));

            //if (Checked)
            //{
            //    e.Graphics.FillRectangle(
            //        new SolidBrush(BackColor), new Rectangle(Width - 11, 0, 12, Height));

            //    e.Graphics.FillRectangle(
            //        new SolidBrush(SlideColor), new Rectangle(Width - 10, 0, 10, Height));
            //}
            //else
            //{
            //    e.Graphics.FillRectangle(
            //        new SolidBrush(BackColor), new Rectangle(-1, 0, 12, Height));

            //    e.Graphics.FillRectangle(
            //        new SolidBrush(SlideColor), new Rectangle(0, 0, 10, Height));
            //}

            
            // Настройка цветов
            Color _borderColor, _activeColor;
            _activeColor = (Checked) ? _styleColor : Color.FromArgb(153, 153, 153);
            if (Enabled)
            {
                _borderColor = (_mouseState == MouseState.Hover)
                    ? MetroPaint.BorderColor.Toggle.Hover(_theme)
                    : MetroPaint.BorderColor.Toggle.Normal(_theme);
            }
            else
                _borderColor = MetroPaint.BorderColor.Toggle.Disabled(_theme);
            
            // Отрисовка
            e.Graphics.Clear(BackColor);

            e.Graphics.DrawRectangle(new Pen(_borderColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));
            e.Graphics.FillRectangle(new SolidBrush(_activeColor), new Rectangle(2, 2, Width - 4, Height - 4));

            if (Checked)
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(BackColor), new Rectangle(Width - 11, 0, 12, Height));

                e.Graphics.FillRectangle(
                    new SolidBrush((_theme == Theme.Dark) ? Color.FromArgb(204, 204, 204) : Color.FromArgb(51, 51, 51)), new Rectangle(Width - 10, 0, 10, Height));
            }
            else
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(BackColor), new Rectangle(-1, 0, 12, Height));

                e.Graphics.FillRectangle(
                    new SolidBrush((_theme == Theme.Dark) ? Color.FromArgb(204, 204, 204) : Color.FromArgb(51, 51, 51)), new Rectangle(0, 0, 10, Height));
            }
        }
    }
}
