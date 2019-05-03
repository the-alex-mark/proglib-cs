using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ProgressBar))]
    public class MinimalProgressBar : System.Windows.Forms.ProgressBar
    {
        public MinimalProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            Orientation = Orientation.Horizontal;
            Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            BackColor = SystemColors.ScrollBar;
            ForeColor = Color.White;
            ProgressColor = ProgLib.Drawing.MetroColors.Blue;
            TextVisible = true;
            TextType = FsProgressTextType.AsIs;

            Size = new Size(180, 25);

            Minimum = 0;
            Maximum = 100;
            Value = 0;
        }
        
        #region Variables

        private Int32 _value = 0, _maximum = 100, _minimum = 0;
        private Font _font;
        private Color _progressColor;
        private Boolean _textVisible;
        private FsProgressTextType _textType;
        private Orientation _orientation;
        private event EventHandler ValueChanged, ValuesIsMaximum;

        #endregion
        
        #region Properties

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
        public Orientation Orientation
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

        [Category("Appearance")]
        public new Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
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

        #endregion

        #region Methods

        public enum FsProgressTextType
        {
            Percent,
            AsIs
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            // Отрисовка состояния в зависимости от ориентации
            switch (_orientation)
            {
                case Orientation.Horizontal:
                    e.Graphics.FillRectangle(new SolidBrush(ProgressColor), new Rectangle(0, 0, Value * Width / Maximum, Height));
                    break;

                case Orientation.Vertical:
                    e.Graphics.FillRectangle(new SolidBrush(this.ProgressColor), new Rectangle(0, Height - Value * Height / Maximum, Width, Value * Height / Maximum));
                    break;
            }
            
            if (_textVisible)
            {
                String _text = String.Empty;
                switch (TextType)
                {
                    case FsProgressTextType.AsIs:
                        _text = Value + " / " + Maximum;
                        break;

                    case FsProgressTextType.Percent:
                        _text = (Value * 100 / Maximum).ToString() + "%";
                        break;
                }

                // Отрисовка текста
                TextRenderer.DrawText(
                    e.Graphics,
                    _text,
                    Font,
                    new Rectangle(0, 0, Width - 1, Height - 1),
                    ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);
            }
        }
    }
}
