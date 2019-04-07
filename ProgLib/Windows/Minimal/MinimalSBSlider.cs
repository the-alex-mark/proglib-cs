using ProgLib.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Minimal
{
    [DefaultEvent("Scroll")]
    public partial class MinimalSBSlider : Control
    {
        public MinimalSBSlider()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            _hue = 360;
            _saturation = 100;
            _brightness = 100;
            _sliderSize = new Size(8, 8);

            Size = new Size(150, 150);
            _borderColor = Color.FromArgb(38, 38, 38);
        }
        
        private Int32 _hue, _saturation, _brightness;
        private Size _sliderSize;
        private Color _borderColor;
        public event ScrollEventHandler Scroll;

        [Category("Внешний вид"), Description("Цвет границы элемента управления")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        public Color Color
        {
            get { return Drawing.Convert.ToColor(new HSB(_hue, _saturation / 100, _brightness / 100)); }
        }

        [Category("Поведение"), Description("Насыщенность цвета")]
        public Int32 Hue
        {
            get { return _hue; }
            set
            {
                _hue = Math.Max(0, Math.Min(360, value));
                Invalidate();
            }
        }

        [Category("Поведение"), Description("Насыщенность цвета")]
        public Int32 Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = Math.Max(0, Math.Min(100, value));

                OnScroll(ScrollEventType.ThumbPosition);
                Invalidate();
            }
        }

        [Category("Поведение"), Description("Яркость цвета")]
        public Int32 Brightness
        {
            get { return _brightness; }
            set
            {
                _brightness = Math.Max(0, Math.Min(100, value));

                OnScroll(ScrollEventType.ThumbPosition);
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) MouseScroll(e);

            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) MouseScroll(e);

            base.OnMouseMove(e);
        }
        private void MouseScroll(MouseEventArgs e)
        {
            _saturation = Math.Max(0, Math.Min(100, 100 * e.X / (Width - 1)));
            _brightness = Math.Max(0, Math.Min(100, 100 - (100 * e.Y / (Height - 1))));
        }

        public virtual void OnScroll(ScrollEventType Type = ScrollEventType.ThumbPosition)
        {
            Scroll?.Invoke(this, new ScrollEventArgs(Type, 0, ScrollOrientation.HorizontalScroll));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush _gradientSaturation = new LinearGradientBrush(new Point(0, 10), new Point(Width, 10), Color.White, Drawing.Convert.ToColor(new HSB(_hue, 1, 1)));
            e.Graphics.FillRectangle(_gradientSaturation, new Rectangle(0, 0, Width - 1, Height - 1));

            LinearGradientBrush _gradientBrightness = new LinearGradientBrush(new Rectangle(-20, -20, Width + 20, Height + 20), Color.Transparent, Color.Black, 90);
            e.Graphics.FillRectangle(_gradientBrightness, new Rectangle(0, 0, Width - 1, Height - 1));

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawEllipse(new Pen(Color.FromArgb(183, 183, 183), 1), new Rectangle(
                new Point((_saturation * (Width - 3) / 100) - 3, ((100 - _brightness) * (Height - 3) / 100) - 3),
                new Size(_sliderSize.Width, _sliderSize.Height)));

            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.DrawRectangle(new Pen(_borderColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));
        }
    }
}
