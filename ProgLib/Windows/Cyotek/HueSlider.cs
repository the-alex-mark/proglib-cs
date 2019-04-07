using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace ProgLib.Windows.Cyotek
{
    [DefaultEvent("Scroll")]
    public class HueSlider : Control
    {
        public HueSlider()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _value = 360;
            _sliderSize = new Size(13, 8);

            Orientation = Orientation.Vertical;
            Size = new Size(32, 200);
            MinimumSize = new Size(32, 32);
            _borderColor = Color.FromArgb(38, 38, 38);
        }

        private Int32 _value;
        private Size _sliderSize;
        private Color _borderColor;
        private Orientation _orientation;
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
        public Int32 Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnScroll(ScrollEventType.ThumbPosition);

                    Invalidate();
                }
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
            get { return 360; }
        }
        [Category("Внешний вид"), Description("Ориентация элемента управления")]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                Size = new Size(Height, Width);

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
            Int32 __value = 0;

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    __value = 360 * (e.X - _sliderSize.Width / 2) / (Width - _sliderSize.Width);
                    break;
                case Orientation.Vertical:
                    __value = 360 - (360 * e.Y / (Height - _sliderSize.Height));
                    break;
            }

            Value = Math.Max(0, Math.Min(360, __value));
        }
        public virtual void OnScroll(ScrollEventType Type = ScrollEventType.ThumbPosition)
        {
            Scroll?.Invoke(this, new ScrollEventArgs(Type, _value, (ScrollOrientation)_orientation));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush Background;
            Rectangle Slider = Rectangle.Empty;

            Color[] Colors = new Color[]
            {
                Color.FromArgb(255, 0, 0),
                Color.FromArgb(255, 128, 0),
                Color.FromArgb(255, 255, 0),
                Color.FromArgb(0, 255, 0),
                Color.FromArgb(0, 191, 255),
                Color.FromArgb(0, 0, 255),
                Color.FromArgb(148, 0, 211),
                Color.FromArgb(255, 0, 0)
            };
            ColorBlend ColorBlend = new ColorBlend()
            {
                Colors = Colors,
                Positions = Enumerable.Range(0, Colors.Length).Select(i => i == 0 ? 0 : i == Colors.Length - 1 ? 1 : (float)(1.0D / Colors.Length) * i).ToArray()
            };

            Bitmap SliderIMG = new Bitmap(13, 8);
            using (Graphics GI = Graphics.FromImage(SliderIMG))
            {
                Point[] Lines =
                {
                    new Point(1, 3),
                    new Point(2, 2),
                    new Point(3, 1),
                    new Point(4, 0),

                    new Point(13, 0),
                    new Point(13, 7),
                    new Point(4, 7),
                };
                GI.FillPolygon(new SolidBrush(Color.FromArgb(183, 183, 183)), Lines);
            }

            switch (_orientation)
            {
                case Orientation.Horizontal:

                    //Background = new LinearGradientBrush(new Rectangle(0, Height - 10, Width, Height - 10), _activeColorOne, _activeColorTwo, 360, false);

                    //Background.InterpolationColors = ColorBlend;
                    //e.Graphics.DrawLine(new Pen(Background, 2), new Point(0, Height - 10), new Point(Width, Height - 10));

                    //e.Graphics.DrawLine(new Pen(Color.FromArgb(100, Color.FromArgb(100, 100, 100)), 2), new Point(0, Height - 10), new Point(Width, Height - 10));


                    Slider = new Rectangle(_value * (Width - _sliderSize.Width) / 360, Height - (_sliderSize.Height / 2 + 10), _sliderSize.Width, _sliderSize.Height);
                    e.Graphics.DrawLine(new Pen(BackColor, 2), new Point(Slider.X, Height - 10), new Point(Slider.X + Slider.Width, Height - 10));
                    break;

                case Orientation.Vertical:
                    Background = new LinearGradientBrush(new Rectangle(0, 0, Width - _sliderSize.Width, Height), Color.White, Color.Red, 270, false)
                    {
                        InterpolationColors = ColorBlend
                    };
                    e.Graphics.FillRectangle(Background, new Rectangle(0, 2, Width - (_sliderSize.Width + 2), Height - 6));
                    e.Graphics.DrawRectangle(new Pen(_borderColor, 1), new Rectangle(0, 2, Width - (_sliderSize.Width + 2), Height - 6));

                    Slider = new Rectangle(Width - _sliderSize.Width, (360 - _value) * (Height - _sliderSize.Height) / 360, _sliderSize.Width, _sliderSize.Height);
                    break;
            }

            e.Graphics.DrawImage(SliderIMG, Slider);
        }
    }
}
