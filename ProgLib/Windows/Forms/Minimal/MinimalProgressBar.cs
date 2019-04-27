using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ProgressBar))]
    public class MinimalProgressBar : System.Windows.Forms.ProgressBar
    {
        public MinimalProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            BackColor = SystemColors.ScrollBar;
            ForeColor = Color.White;
            Size = new Size(100, 23);
            _minimum = 0;
            _maximum = 100;
            _value = 0;
            _textVisible = true;
            _progressColor = Color.FromArgb(255, 128, 128);
            _textType = FsProgressTextType.AsIs;
            _orientation = OrientationType.Horizontal;
        }

        public enum FsProgressTextType
        {
            Percent,
            AsIs
        }
        public enum OrientationType
        {
            Vertical,
            Horizontal
        }

        private Int32 _value, _maximum, _minimum;
        private Color _progressColor;
        private Boolean _textVisible;
        private FsProgressTextType _textType;
        private OrientationType _orientation;
        private event EventHandler ValueChanged, ValuesIsMaximum;

        [Category("Appearance"), Description("Цвет отображения визуального значения.")]
        public Color ProgressColor
        {
            get { return _progressColor; }
            set
            {
                _progressColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Вид отображаемого текста.")]
        public FsProgressTextType TextType
        {
            get { return _textType; }
            set
            {
                _textType = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Вид контролла.")]
        public OrientationType Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Отображение рисуемого текста.")]
        public Boolean TextVisible
        {
            get { return _textVisible; }
            set
            {
                _textVisible = value;
                Invalidate();
            }
        }
        public new Int32 Maximum
        {
            get { return _maximum; }
            set
            {
                if (value <= Minimum) throw new Exception("Значение MAX должно быть больше значения MIN!");
                _maximum = value;

                Invalidate();
            }
        }
        public new Int32 Minimum
        {
            get { return _minimum; }
            set
            {
                if (value >= Maximum) throw new Exception("Значение MIN должно быть меньше значения MAX!");
                _minimum = value;

                Invalidate();
            }
        }
        public new Int32 Value
        {
            get { return _value; }
            set
            {
                if (value < Minimum || value > _maximum) throw new Exception("Значение должно быть между MIN и MAX!");
                _value = value;
                if (_value == Maximum && ValuesIsMaximum != null) ValuesIsMaximum(this, new EventArgs());
                ValueChanged?.Invoke(this, new EventArgs());

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, 0, Width, Height));

            switch (Orientation)
            {
                case OrientationType.Horizontal:
                    e.Graphics.FillRectangle(new SolidBrush(ProgressColor), new Rectangle(0, 0, Value * Width / Maximum, Height));
                    break;

                case OrientationType.Vertical:
                    e.Graphics.FillRectangle(new SolidBrush(this.ProgressColor), new Rectangle(0, Height - Value * Height / Maximum, Width, Value * Height / Maximum));
                    break;
            }

            if (TextVisible)
            {
                String TEXT = string.Empty;
                switch (TextType)
                {
                    case FsProgressTextType.AsIs:
                        TEXT = Value + " / " + Maximum;
                        break;

                    case FsProgressTextType.Percent:
                        TEXT = (Value * 100 / Maximum).ToString() + "%";
                        break;
                }

                SizeF SIZE = e.Graphics.MeasureString(TEXT, new Font("Century Gothic", 8));
                e.Graphics.DrawString(TEXT, new Font("Century Gothic", 8), new SolidBrush(ForeColor), new PointF((float)(Width / 2) - SIZE.Width / 2f, (float)(Height / 2) - SIZE.Height / 2f));
            }
        }
    }
}
