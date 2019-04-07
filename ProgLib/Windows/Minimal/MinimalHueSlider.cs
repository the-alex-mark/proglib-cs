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
    public partial class MinimalHueSlider : Control
    {
        public MinimalHueSlider()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _value = 360;
            _sliderSize = new Size(13, 8);
            
            Size = new Size(32, 155);
            MinimumSize = new Size(32, 32);
            _borderColor = Color.FromArgb(38, 38, 38);
            _sliderColor = SystemColors.ControlDark;
        }

        private Int32 _value;
        private Size _sliderSize;
        private Color _borderColor, _sliderColor;
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

        [Category("Внешний вид"), Description("Цвет границы элемента управления")]
        public Color SliderColor
        {
            get { return _sliderColor; }
            set
            {
                _sliderColor = value;
                Invalidate();
            }
        }

        [Category("Поведение")]
        public Int32 Value
        {
            get { return _value; }
            set
            {
                _value = Math.Max(0, Math.Min(360, value));
                OnScroll(ScrollEventType.ThumbPosition);

                Invalidate();
            }
        }

        [Category("Поведение")]
        public Int32 Minimum
        {
            get { return 0; }
        }

        [Category("Поведение")]
        public Int32 Maximum
        {
            get { return 360; }
        }

        protected virtual Color[] Rainbow
        {
            get
            {
                return new Color[]
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
            Int32 __value = 360 - (360 * e.Y / (Height - _sliderSize.Height));
            Value = Math.Max(0, Math.Min(360, __value));
        }
        public virtual void OnScroll(ScrollEventType Type = ScrollEventType.ThumbPosition)
        {
            Scroll?.Invoke(this, new ScrollEventArgs(Type, _value, ScrollOrientation.VerticalScroll));
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            ColorBlend ColorBlend = new ColorBlend()
            {
                Colors = Rainbow,
                Positions = Enumerable.Range(0, Rainbow.Length).Select(i => i == 0 ? 0 : i == Rainbow.Length - 1 ? 1 : (float)(1.0D / Rainbow.Length) * i).ToArray()
            };
            LinearGradientBrush Background = new LinearGradientBrush(new Rectangle(0, 0, Width - _sliderSize.Width, Height), Color.White, Color.Red, 270, false)
            {
                InterpolationColors = ColorBlend
            };
            e.Graphics.FillRectangle(Background, new Rectangle(0, 2, Width - (_sliderSize.Width + 2), Height - 6));
            e.Graphics.DrawRectangle(new Pen(_borderColor, 1), new Rectangle(0, 2, Width - (_sliderSize.Width + 2), Height - 6));

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
                GI.FillPolygon(new SolidBrush(_sliderColor), Lines);
            }
            e.Graphics.DrawImage(
                SliderIMG,
                new Rectangle(Width - _sliderSize.Width, (360 - _value) * (Height - _sliderSize.Height) / 360, _sliderSize.Width, _sliderSize.Height));
        }
    }
}