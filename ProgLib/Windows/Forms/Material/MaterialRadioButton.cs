using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Animations.Material;

namespace ProgLib.Windows.Material
{
    [ToolboxBitmap(typeof(System.Windows.Forms.RadioButton))]
    public partial class MaterialRadioButton : System.Windows.Forms.RadioButton
    {
        public MaterialRadioButton()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            _animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.06
            };
            _rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();
            _rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) => _animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);

            Animation = true;
            _checkedColor = Color.FromArgb(255, 128, 128);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderColor = SystemColors.ControlDark;
            FlatAppearance.MouseDownBackColor = Color.Red;
        }

        private readonly AnimationManager _animationManager;
        private readonly AnimationManager _rippleAnimationManager;

        private Boolean _animation;
        private Color _checkedColor;

        [Category("Appearance"), Description("Отображение анимации")]
        public bool Animation
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
        
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // clear the control
            g.Clear(Parent.BackColor);

            Double animationProgress = _animationManager.GetProgress();
            
            Single animationSize = (float)(animationProgress * 8f) - 1;
            Single animationSizeHalf = animationSize / 2;
            animationSize = (float)(animationProgress * 9f);

            SolidBrush brush = new SolidBrush(Color.FromArgb((int)(animationProgress * 255.0), _checkedColor));

            // Отрисовка анимации
            if (Animation && _rippleAnimationManager.IsAnimating())
            {
                for (var i = 0; i < _rippleAnimationManager.GetAnimationCount(); i++)
                {
                    Int32 rippleSize = (_rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? (int)(22 * (0.8d + (0.2d * _rippleAnimationManager.GetProgress(i)))) : 22;
                    
                    g.FillPath(
                        new SolidBrush(Color.FromArgb((int)((_rippleAnimationManager.GetProgress(i) * 40)), FlatAppearance.MouseDownBackColor)),
                        DrawHelper.CreateRoundRect(4 - 4, (Height / 2) + 1 - (rippleSize / 2), rippleSize, rippleSize, rippleSize / 2));
                }
            }

            g.FillEllipse(
                new SolidBrush(Parent.BackColor), new Rectangle(4, (Height / 2) - 6, 14, 14));

            g.DrawEllipse(
                new Pen(FlatAppearance.BorderColor), new Rectangle(4, (Height / 2) - 6, 14, 14));

            if (Checked)
            {
                g.FillPath(
                    new SolidBrush(Color.FromArgb((int)(animationProgress * 255.0), _checkedColor)),
                    DrawHelper.CreateRoundRect(4 + 6 - animationSizeHalf, (Height / 2) - animationSizeHalf, animationSize, animationSize, 4f));
            }

            //SizeF stringSize = g.MeasureString(Text, Font);
            g.DrawString(
                Text,
                Font,
                new SolidBrush(ForeColor),
                new PointF(22, Height / 2 - e.Graphics.MeasureString(Text, Font).Height / 2));

            brush.Dispose();
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (DesignMode) return;

            MouseDown += (sender, args) =>
            {
                if (Animation && args.Button == MouseButtons.Left)
                {
                    _rippleAnimationManager.SecondaryIncrement = 0;
                    _rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Checked });
                }
            };
            MouseUp += (sender, args) =>
            {
                _rippleAnimationManager.SecondaryIncrement = 0.08;
            };
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            // Обеспечивает дополнительное пространство для правильного заполнения контента
            Int32 Extra = 16;

            // 24 - размер значка
            // 4 - пробел между знаком и текстом
            Extra += 24 + 4;

            return new Size((int)Math.Ceiling(CreateGraphics().MeasureString(Text, Font).Width) + Extra, 35);
        }
    }
}
