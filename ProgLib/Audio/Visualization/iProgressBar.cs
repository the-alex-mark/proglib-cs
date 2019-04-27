using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Audio.Visualization
{
    public class iProgressBar : System.Windows.Forms.ProgressBar
    {
        public iProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            MIN = 0;
            MAX = 100;
            VALUE = 0;
            BackColor = SystemColors.ControlDark;
            ForeColor = Color.White;
            ProgressColor = Color.Green;
            VisibleText = false;
            Size = new Size(100, 23);
            Style = ProgressBarStyle.Continuous;


            TextType = FsProgressTextType.AsIs;
            Orientation = OrientationType.Horizontal;
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

        private Int32 VALUE;
        private Int32 MAX;
        private Int32 MIN;
        public event EventHandler ValueChanged;
        public event EventHandler ValuesIsMaximum;

        [Category("Appearance"), Description("Цвет отображения визуального значения")]
        public Color ProgressColor
        {
            get;
            set;
        }

        [Category("Appearance"), Description("Вид отображаемого текста")]
        public FsProgressTextType TextType
        {
            get;
            set;
        }

        [Category("Appearance"), Description("Вид контролла")]
        public OrientationType Orientation
        {
            get;
            set;
        }

        [Category("Appearance"), Description("ВКЛ / ВЫКЛ отображения рисуемого текста")]
        public Boolean VisibleText
        {
            get;
            set;
        }
        public new Int32 Maximum
        {
            get { return MAX; }
            set
            {
                if (value <= Minimum) throw new Exception("Значение MAX должно быть больше значения MIN!");
                MAX = value;

                Invalidate();
            }
        }
        public new Int32 Minimum
        {
            get { return MIN; }
            set
            {
                if (value >= Maximum) throw new Exception("Значение MIN должно быть меньше значения MAX!");
                MIN = value;

                Invalidate();
            }
        }
        public new Int32 Value
        {
            get { return VALUE; }
            set
            {
                if (value < Minimum || value > MAX) throw new Exception("Значение должно быть между MIN и MAX!");
                VALUE = value;
                if (VALUE == Maximum && ValuesIsMaximum != null) ValuesIsMaximum(this, new EventArgs());
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

            if (VisibleText)
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
