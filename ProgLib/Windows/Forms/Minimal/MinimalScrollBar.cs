using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.VScrollBar)), DefaultEvent("Scroll")]
    public class MinimalScrollBar : System.Windows.Forms.Control
    {
        public MinimalScrollBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
            Orientation = ScrollOrientation.VerticalScroll;
            ThumbHeight = 35;
            BackColor = SystemColors.Control;
            BorderColor = SystemColors.Control;
            ThumbColor = SystemColors.ControlDarkDark;
            LineColor = Color.Silver;

            Size = new Size(9, 237);
        }

        #region Global Variables

        private Int32 _value, _maximum = 100, _thumbHeight;
        private Color _thumbColor, _borderColor, _lineColor;
        private ScrollOrientation _Orientation;

        // Событие изменения положения ползунка
        public event ScrollEventHandler Scroll;

        #endregion

        #region Properties

        public Int32 Value
        {
            get { return _value; }
            set
            {
                bool flag = _value == value;
                if (!flag)
                {
                    _value = value;
                    OnScroll(ScrollEventType.ThumbPosition);

                    Invalidate();
                }
            }
        }
        public Int32 Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет ползунка.")]
        public Color ThumbColor
        {
            get { return _thumbColor; }
            set { _thumbColor = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Высота ползунка.")]
        public Int32 ThumbHeight
        {
            get { return _thumbHeight; }
            set { _thumbHeight = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Цвет обводки.")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Цвет обводки.")]
        public Color LineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; Invalidate(); }
        }

        [Category("Appearance"), Description("Ориентация контролла.")]
        public ScrollOrientation Orientation
        {
            get { return _Orientation; }
            set
            {
                _Orientation = value;

                Invalidate();
            }
        }

        #endregion

        #region Methods

        private void MouseScroll(MouseEventArgs e)
        {
            Int32 V = 0;

            switch (Orientation)
            {
                case ScrollOrientation.HorizontalScroll:
                    V = Maximum * (e.X - _thumbHeight / 2) / (Width - _thumbHeight);
                    break;

                case ScrollOrientation.VerticalScroll:
                    V = Maximum * (e.Y - _thumbHeight / 2) / (Height - _thumbHeight);
                    break;
            }

            Value = Math.Max(0, Math.Min(Maximum, V));
        }

        #endregion

        public virtual void OnScroll(ScrollEventType Type = ScrollEventType.ThumbPosition)
        {
            Scroll?.Invoke(this, new ScrollEventArgs(Type, Value, Orientation));
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
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Maximum > 0)
            {
                Rectangle _thumbRectangle = Rectangle.Empty;

                switch (Orientation)
                {
                    case ScrollOrientation.HorizontalScroll:
                        _thumbRectangle = new Rectangle(_value * (Width - ThumbHeight) / Maximum, 2, ThumbHeight, Height - 4);
                        break;

                    case ScrollOrientation.VerticalScroll:
                        _thumbRectangle = new Rectangle(2, _value * (Height - ThumbHeight) / Maximum, Width - 4, ThumbHeight);
                        break;
                }

                // Отрисовка ползунка
                e.Graphics.FillRectangle(new SolidBrush(_thumbColor), _thumbRectangle);

                // Отрисовка границ
                e.Graphics.DrawRectangle(new Pen(_borderColor), new Rectangle(0, 0, Width - 1, Height - 1));
            }
        }
    }
}
