using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ProgLib.Animations.Material;

namespace ProgLib.Windows.Material
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    public partial class MaterialButton : System.Windows.Forms.Button
    {
        public MaterialButton()
        {
            ForeColor = SystemColors.GrayText;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 1;
            FlatAppearance.BorderColor = SystemColors.ControlDark;
            FlatAppearance.MouseDownBackColor = SystemColors.ControlDarkDark;
            Size = new Size(110, 25);

            _animation = true;
            
            AnimationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            HoverAnimationManager = new AnimationManager
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };

            HoverAnimationManager.OnAnimationProgress += sender => Invalidate();
            AnimationManager.OnAnimationProgress += sender => Invalidate();
        }

        private readonly AnimationManager AnimationManager;
        private readonly AnimationManager HoverAnimationManager;

        private Boolean _animation;

        [Category("Appearance"), Description("Отображение анимации элемента управления")]
        public Boolean Animation
        {
            get { return _animation; }
            set
            {
                _animation = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            // Плавное изменение цвета при наведении курсора
            using (Brush Select = new SolidBrush(Color.FromArgb((int)(HoverAnimationManager.GetProgress() * FlatAppearance.MouseOverBackColor.A), FlatAppearance.MouseOverBackColor.RemoveAlpha())))
            {
                e.Graphics.FillRectangle(Select, ClientRectangle);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Анимация при нажати ПК мыши
            if (_animation && AnimationManager.IsAnimating())
            {
                for (var i = 0; i < AnimationManager.GetAnimationCount(); i++)
                {
                    using (Brush RippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (AnimationManager.GetProgress(i) * 100)), FlatAppearance.MouseDownBackColor)))
                    {
                        Int32 RippleSize = (int)(AnimationManager.GetProgress(i) * Width * 2);
                        e.Graphics.FillEllipse(RippleBrush, new Rectangle(AnimationManager.GetSource(i).X - RippleSize / 2, AnimationManager.GetSource(i).Y - RippleSize / 2, RippleSize, RippleSize));
                    }
                }

                e.Graphics.SmoothingMode = SmoothingMode.None;
            }

            // Отрисовка текста кнопки
            e.Graphics.DrawString(
                Text,
                Font,
                new SolidBrush(ForeColor),
                new Rectangle(0, 0, Width - 1, Height - 1),
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                );

            // Отрисовка границ кнопки
            e.Graphics.DrawRectangle(new Pen(FlatAppearance.BorderColor, FlatAppearance.BorderSize), new Rectangle(0, 0, Width - 1, Height - 1));
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (DesignMode) return;

            MouseEnter += (sender, args) =>
            {
                HoverAnimationManager.StartNewAnimation(AnimationDirection.In);
                Invalidate();
            };
            MouseLeave += (sender, args) =>
            {
                HoverAnimationManager.StartNewAnimation(AnimationDirection.Out);
                Invalidate();
            };
            MouseDown += (sender, args) =>
            {
                if (Animation && args.Button == MouseButtons.Left)
                {
                    AnimationManager.StartNewAnimation(AnimationDirection.In, args.Location);
                    Invalidate();
                }
            };
            MouseUp += (sender, args) =>
            {
                Invalidate();
            };
        }
    }
}
