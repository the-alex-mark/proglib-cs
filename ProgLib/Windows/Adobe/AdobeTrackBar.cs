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

namespace ProgLib.Windows.Adobe
{
    [DefaultEvent("Scroll")]
    public partial class AdobeTrackBar : Control
    {
        public AdobeTrackBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _value = 0;
            _minimum = 0;
            _maximum = 100;

            _sliderSize = new Size(18, 18);
            _gradient = Gradient.None;
            _activeColorOne = Color.Green;
            _activeColorTwo = Color.FromArgb(100, 100, 100);
            Orientation = Orientation.Horizontal;
            Size = new Size(200, 32);
            MinimumSize = new Size(32, 32);
            ForeColor = Color.FromArgb(183, 183, 183);
            Font = new Font(Font.Name, 7.5F, FontStyle.Regular);

            _lineColor = Color.FromArgb(100, 100, 100);
            _sliderColor = Color.FromArgb(183, 183, 183);
        }

        private Int32 _value, _maximum, _minimum;
        private Size _sliderSize;
        private Color _activeColorOne, _activeColorTwo, _sliderColor, _lineColor;
        private Orientation _orientation;
        private Gradient _gradient;
        public event ScrollEventHandler Scroll;

        [Category("Поведение")]
        public Int32 Value
        {
            get { return _value; }
            set
            {
                _value = Math.Max(_minimum, Math.Min(_maximum, value)); ;
                OnScroll(ScrollEventType.ThumbPosition);

                Invalidate();
            }
        }

        [Category("Поведение")]
        public Int32 Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                Invalidate();
            }
        }

        [Category("Поведение")]
        public Int32 Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Цвет фоновой линии")]
        public Gradient Gradient
        {
            get { return _gradient; }
            set
            {
                _gradient = value;

                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Первый цвет градиента")]
        public Color ActiveColorOne
        {
            get { return _activeColorOne; }
            set
            {
                _activeColorOne = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Второй цвет градиента")]
        public Color ActiveColorTwo
        {
            get { return _activeColorTwo; }
            set
            {
                _activeColorTwo = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Цвет ползунка")]
        public Color SliderColor
        {
            get { return _sliderColor; }
            set
            {
                _sliderColor = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Цвет линии")]
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                Invalidate();
            }
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
                    __value = _maximum * (e.X - _sliderSize.Width / 2) / (Width - _sliderSize.Width);
                    break;
                case Orientation.Vertical:
                    __value = _maximum - (_maximum * (e.Y - _sliderSize.Height / 2) / (Height - _sliderSize.Height));
                    break;
            }

            Value = Math.Max(_minimum, Math.Min(_maximum, __value));
        }
        public virtual void OnScroll(ScrollEventType Type = ScrollEventType.ThumbPosition)
        {
            Scroll?.Invoke(this, new ScrollEventArgs(Type, _value, (ScrollOrientation)_orientation));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_maximum > _minimum)
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
                Int32 Count = Colors.Length;
                ColorBlend ColorBlend = new ColorBlend()
                {
                    Colors = Colors,
                    Positions = Enumerable.Range(0, Count).Select(i => i == 0 ? 0 : i == Count - 1 ? 1 : (float)(1.0D / Count) * i).ToArray()
                };

                switch (_orientation)
                {
                    case Orientation.Horizontal:
                        Background = new LinearGradientBrush(new Rectangle(0, Height - 10, Width, Height - 10), _activeColorOne, _activeColorTwo, 360, false);
                        switch (_gradient)
                        {
                            case Gradient.None:
                                e.Graphics.DrawLine(new Pen(_lineColor, 2), new Point(0, Height - 10), new Point(Width, Height - 10));
                                break;

                            case Gradient.Rainbow:
                                Background.InterpolationColors = ColorBlend;
                                e.Graphics.DrawLine(new Pen(Background, 2), new Point(0, Height - 10), new Point(Width, Height - 10));
                                break;

                            case Gradient.Custom:
                                e.Graphics.DrawLine(new Pen(Background, 2), new Point(0, Height - 10), new Point(Width, Height - 10));
                                break;
                        }

                        e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(0, 0, Width - 30, 16), new StringFormat { Alignment = StringAlignment.Near });
                        e.Graphics.DrawString(Value.ToString(), Font, new SolidBrush(ForeColor), new Rectangle(Width - 30, 0, 30, 16), new StringFormat { Alignment = StringAlignment.Far });

                        Slider = new Rectangle(_value * (Width - _sliderSize.Width) / _maximum, Height - (_sliderSize.Height / 2 + 10), _sliderSize.Width, _sliderSize.Height);
                        e.Graphics.DrawLine(new Pen(BackColor, 2), new Point(Slider.X, Height - 10), new Point(Slider.X + Slider.Width, Height - 10));
                        break;

                    case Orientation.Vertical:
                        Background = new LinearGradientBrush(new Rectangle(Width - 10, 0, Width - 10, Height), _activeColorTwo, _activeColorOne, 270, false);
                        switch (_gradient)
                        {
                            case Gradient.None:
                                e.Graphics.DrawLine(new Pen(_lineColor, 2), new Point(Width - 10, 0), new Point(Width - 10, Height));
                                break;

                            case Gradient.Rainbow:
                                Background.InterpolationColors = ColorBlend;
                                e.Graphics.DrawLine(new Pen(Background, 2), new Point(Width - 10, 0), new Point(Width - 10, Height));
                                break;

                            case Gradient.Custom:
                                e.Graphics.DrawLine(new Pen(Background, 2), new Point(Width - 10, 0), new Point(Width - 10, Height));
                                break;
                        }

                        Slider = new Rectangle(Width - (_sliderSize.Width / 2 + 10), (_maximum - _value) * (Height - _sliderSize.Width) / _maximum, _sliderSize.Width, _sliderSize.Height);
                        e.Graphics.DrawLine(new Pen(BackColor, 2), new Point(Width - 10, Slider.Y), new Point(Width - 10, Slider.Y + Slider.Height + 1));
                        break;
                }

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(new Pen(_sliderColor, 2), new Rectangle(Slider.X + 3, Slider.Y + 3, _sliderSize.Width - 7, _sliderSize.Height - 7));
            }
        }
    }
}
