using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ProgLib.Animations.Material;

namespace ProgLib.Windows.Material
{
    [ToolboxBitmap(typeof(System.Windows.Forms.CheckBox))]
    public partial class MaterialCheckBox : System.Windows.Forms.CheckBox
    {
        public MaterialCheckBox()
        {
            ForeColor = SystemColors.GrayText;
            MinimumSize = new Size((int)CreateGraphics().MeasureString(Text, new Font(Font.Name, Font.Size, Font.Style)).Width + 22, 23);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 1;
            FlatAppearance.BorderColor = SystemColors.ControlDark;
            FlatAppearance.MouseDownBackColor = SystemColors.ControlDarkDark;

            _animation = true;
            _checkedColor = SystemColors.ControlDarkDark;

            AnimationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.05
            };
            RippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };

            AnimationManager.OnAnimationProgress += sender => Invalidate();
            RippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) =>
            {
                AnimationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);
            };
        }

        private readonly AnimationManager AnimationManager;
        private readonly AnimationManager RippleAnimationManager;

        private Boolean _animation;
        private Color _checkedColor;
        private SizeF _textSize;
        
        [Category("Appearance"), Description("Отображение анимации")]
        public Boolean Animation
        {
            get { return _animation; }
            set
            {
                _animation = value;
                if (value) Margin = new Padding(0);

                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет галочки")]
        public Color CheckedColor
        {
            get
            {
                return _checkedColor;
            }
            set
            {
                _checkedColor = value;
                Invalidate();
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                _textSize = CreateGraphics().MeasureString(value, new Font(Font.Name, Font.Size, Font.Style));
                if (AutoSize) Size = GetPreferredSize(new Size(0, 0));
                Invalidate();
            }
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent.BackColor);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Отрисовка анимации
            Int32 X;

            if (Animation && RippleAnimationManager.IsAnimating())
            {
                for (var i = 0; i < RippleAnimationManager.GetAnimationCount(); i++)
                {
                    Int32 rippleSize = (RippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? (int)(21 * (0.8d + (0.2d * RippleAnimationManager.GetProgress(i)))) : 21;

                    if (CheckAlign == ContentAlignment.MiddleLeft) X = 12 - rippleSize / 2 - 2;
                    else if (CheckAlign == ContentAlignment.MiddleRight) X = (Width - (12 - rippleSize / 2 - 2) - rippleSize) - 2;
                    else if (CheckAlign == ContentAlignment.MiddleCenter)X = (Width / 2 - (rippleSize / 2)) - 1;
                    else X = 12 - rippleSize / 2 - 2;

                    using (GraphicsPath Path = DrawHelper.CreateRoundRect(X, ((Height / 2 - 6) + (18 / 2) - 1) - rippleSize / 2 - 3, rippleSize + 1, rippleSize + 1, rippleSize / 2))
                    {
                        e.Graphics.FillPath(new SolidBrush(Color.FromArgb((int)((RippleAnimationManager.GetProgress(i) * 40)), FlatAppearance.MouseDownBackColor)), Path);
                    }
                }
            }

            // Отрисовка поля "Checked"
            if (CheckAlign == ContentAlignment.MiddleLeft) X = 5;
            else if (CheckAlign == ContentAlignment.MiddleRight) X = Width - 6 - 12;
            else if (CheckAlign == ContentAlignment.MiddleCenter) X = (Width / 2 - 6);
            else X = 5;

            using (GraphicsPath checkmarkPath = DrawHelper.CreateRoundRect(X, (Height / 2 - 6), 12, 12, 1f))
            {
                if (Enabled)
                {
                    e.Graphics.FillPath(new SolidBrush(Parent.BackColor), checkmarkPath);
                    e.Graphics.DrawPath(new Pen(FlatAppearance.BorderColor), checkmarkPath);
                }
                else if (Checked)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.None;
                    e.Graphics.FillRectangle(new SolidBrush(FlatAppearance.BorderColor), (Height / 2 - 6) + 2, (Height / 2 - 6) + 2, 14, 14);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                }

                Bitmap Tick = new Bitmap(15, 13);
                using (Graphics GIMG = Graphics.FromImage(Tick))
                {
                    PointF[] Lines =
                    {
                        new PointF(2, 7),
                        new PointF(3, 5),
                        new PointF(5, 8),
                        new PointF(13, -1),
                        new PointF(15, 1),
                        new PointF(5, 11),

                        new PointF(2, 7)
                    };
                    GIMG.FillRectangle(new SolidBrush(BackColor), new Rectangle(11, 0, 2, 6));
                    GIMG.FillPolygon(new SolidBrush(_checkedColor), Lines);
                }

                e.Graphics.DrawImageUnscaledAndClipped(Tick, new Rectangle(X, (Height / 2) - 6, (int)(15.0 * AnimationManager.GetProgress()), 13));
            }

            // Отрисовка текста
            e.Graphics.DrawString(
                Text,
                Font,
                new SolidBrush(ForeColor),
                new PointF(22, Height / 2 - e.Graphics.MeasureString(Text, Font).Height / 2));
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (DesignMode) return;

            MouseDown += (sender, args) =>
            {
                if (Animation && args.Button == MouseButtons.Left)
                {
                    RippleAnimationManager.SecondaryIncrement = 0;
                    RippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Checked });
                }
            };
            MouseUp += (sender, args) =>
            {
                RippleAnimationManager.SecondaryIncrement = 0.08;
            };
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            // Обеспечивает дополнительное пространство для правильного заполнения контента
            Int32 Extra = 16;

            // 24 - размер значка
            // 4 - пробел между знаком и текстом
            Extra += 24 + 4;

            return new Size((int)Math.Ceiling(_textSize.Width) + Extra, 35);
        }
    }
}