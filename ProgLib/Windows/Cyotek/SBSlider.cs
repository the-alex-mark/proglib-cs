using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using ProgLib.Drawing;

namespace ProgLib.Windows.Cyotek
{
    [DefaultEvent("Scroll")]
    public class SBSlider : Control
    {
        public SBSlider()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _value = new HSB(360, 100, 100);
            _hue = 360;
            _saturation = 100;
            _brightness = 100;
            _activeColor = Color.FromArgb(255, 0, 0);
            _sliderSize = new Size(8, 8);

            Size = new Size(194, 194);
            _borderColor = Color.FromArgb(38, 38, 38);
        }

        private HSB _value;
        private Int32 _hue, _saturation, _brightness;
        private Size _sliderSize;
        private Color _borderColor, _activeColor;
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
        [Category("Поведение"), Description("")]
        public Color ActiveColor
        {
            get { return _activeColor; }
            set
            {
                _activeColor = value;
                Invalidate();
            }
        }
        [Category("Поведение"), Description("Насыщенность цвета")]
        public Int32 Hue
        {
            get { return _hue; }
            set
            {
                _hue = value;
                ActiveColor = new HSB(Hue, 1, 1).ToColor();
                Invalidate();
            }
        }
        [Category("Поведение"), Description("Насыщенность цвета")]
        public Int32 Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;
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
                _brightness = value;
                OnScroll(ScrollEventType.ThumbPosition);

                Invalidate();
            }
        }

        [Category("Поведение"), Description(""), Browsable(false)]
        public HSB Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Hue = (int)_value.H;
                Saturation = (int)_value.S;
                Brightness = (int)_value.B;

                Invalidate();
            }
        }
        [Category("Поведение"), Description("")]
        public Int32 Minimum
        {
            get { return 0; }
        }
        [Category("Поведение"), Description("")]
        public Int32 Maximum
        {
            get { return 100; }
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
            Saturation = Math.Max(Minimum, Math.Min(Maximum, Maximum * e.X / (Width - 1)));
            Brightness = Math.Max(Minimum, Math.Min(Maximum, Maximum - (Maximum * e.Y / (Height - 1))));
        }

        public virtual void OnScroll(ScrollEventType Type = ScrollEventType.ThumbPosition)
        {
            Scroll?.Invoke(this, new ScrollEventArgs(Type, 0, ScrollOrientation.HorizontalScroll));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush _gradientSaturation = new LinearGradientBrush(new Point(0, 10), new Point(Width, 10), Color.White, _activeColor);
            e.Graphics.FillRectangle(_gradientSaturation, new Rectangle(0, 0, Width - 1, Height - 1));

            LinearGradientBrush _gradientBrightness = new LinearGradientBrush(new Rectangle(-20, -20, Width + 20, Height + 20), Color.Transparent, Color.Black, 90);
            e.Graphics.FillRectangle(_gradientBrightness, new Rectangle(0, 0, Width - 1, Height - 1));

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawEllipse(new Pen(Color.FromArgb(183, 183, 183), 1), new Rectangle(
                new Point((_saturation * (Width - 3) / 100) - 3, ((Maximum - _brightness) * (Height - 3) / 100) - 3),
                new Size(_sliderSize.Width, _sliderSize.Height)));

            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.DrawRectangle(new Pen(_borderColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));
        }
    }
}
