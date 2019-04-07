using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Animations.Metro;
using ProgLib.Animations;

namespace ProgLib.Windows
{
    public static class Window
    {
        [DllImport("USER32", CharSet = CharSet.Auto)]
        private extern static Boolean PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

        [DllImport("USER32", CharSet = CharSet.Auto)]
        private extern static Boolean ReleaseCapture();

        /// <summary>
        /// Изменяет позицию <see cref="System.Windows.Forms.Form"/> при воздействии на неё мышью. 
        /// </summary>
        /// <param name="Form"></param>
        public static void Move(this Form Form)
        {
            ReleaseCapture();
            PostMessage(Form.Handle, 0x0112, 0xF012, 0);
        }

        /// <summary>
        /// Изменяет размер <see cref="System.Windows.Forms.Form"/> при воздействии на неё мышью. 
        /// </summary>
        /// <param name="Form"></param>
        /// /// <param name="Direction"></param>
        public static void Resize(Form Form, Direction Direction)
        {
            uint Value = 0xF000;
            switch (Direction)
            {
                case Direction.Right: Value = 0xF002; break;
                case Direction.BottomRight: Value = 0xF008; break;
                case Direction.Bottom: Value = 0xF006; break;
            }

            ReleaseCapture();
            if (Value != 0xF000)
            {
                PostMessage(Form.Handle, 0x0112, Value, 0);
            }
        }

        /// <summary>
        /// Плавно загружает <see cref="System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="Form">Загружаемая форма</param>
        /// <param name="Duration">Скорость появления</param>
        public static void Show(Form Form, Int32 Duration)
        {
            Form.Opacity = 0;

            Timer Event = new Timer { Interval = Duration };
            Event.Tick += delegate (Object sender, EventArgs e)
            {
                if (Form.Opacity != 1)
                {
                    Form.Opacity += 0.05;
                }
                else { Event.Stop(); }
            };
            Event.Start();
        }

        /// <summary>
        /// Плавно загружает <see cref="System.Windows.Forms.Form"/> до определённой прозрачности.
        /// </summary>
        /// <param name="Form">Загружаемая форма</param>
        /// <param name="Opacity">Конечная прозрачность</param>
        /// <param name="Duration">Скорость появления</param>
        public static void Show(Form Form, Double Opacity, Int32 Duration)
        {
            Timer Event = new Timer { Interval = Duration };
            Event.Tick += delegate (object sender, EventArgs e)
            {
                if (Form.Opacity != Opacity)
                {
                    Form.Opacity += 0.1;
                }
                else { Event.Stop(); }
            };
            Event.Start();
        }

        /// <summary>
        /// Плавно загружает <see cref="System.Windows.Forms.Form"/> с раздвижением из центра по вертикали.
        /// </summary>
        /// <param name="Form">Загружаемая форма.</param>
        public static void Show(Form Form)
        {
            Int32[] BS = new Int32[]
            {
                    Form.Width,
                    Form.Height,
                    Form.Location.X,
                    Form.Location.Y
            };
            Form.Height = 0;
            Form.DesktopLocation = new Point(BS[2], BS[3]);

            Animation.Size(Form, new Size(BS[0], BS[1]), TransitionType.EaseInQuad, 10);
            Animation.Move(Form, new Point(BS[2], BS[3]), TransitionType.EaseOutCubic, 10);
        }

        /// <summary>
        /// Плавно деактивирует <see cref="System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="Form">Загружаемая форма.</param>
        /// /// <param name="Duration">Скорость деактивации.</param>
        public static void Inactive(Form Form, Int32 Duration)
        {
            Timer Event = new Timer { Interval = Duration };
            Event.Tick += delegate (Object sender, EventArgs e)
            {
                if (Form.Opacity > 0.75)
                {
                    Form.Opacity -= 0.05;
                }
                else { Event.Stop(); }
            };
            Event.Start();
        }

        /// <summary>
        /// Плавно скрывает <see cref="System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="Form">Скрываемая форма.</param>
        /// <param name="Duration">Скорость скрытия.</param>
        public static void Hide(Form Form, Int32 Duration)
        {
            Timer Event = new Timer { Interval = Duration };
            Event.Tick += delegate (Object sender, EventArgs e)
            {
                if (Form.Opacity != 0)
                {
                    Form.Opacity -= 0.1;
                }
                else { Event.Stop(); Form.WindowState = FormWindowState.Minimized; }
            };
            Event.Start();
        }

        /// <summary>
        /// Плавно закрывает <see cref="System.Windows.Forms.Form"/>.
        /// </summary>
        /// <param name="Form">Закрываемая форма.</param>
        /// <param name="Duration">Скорость закрытия.</param>
        public static void Close(Form Form, Int32 Duration)
        {
            Timer Event = new Timer { Interval = Duration };
            Event.Tick += delegate (Object sender, EventArgs e)
            {
                if (Form.Opacity != 1)
                {
                    Form.Opacity -= 0.1;
                }
                else { Event.Stop(); Form.Close(); }
            };
            Event.Start();
        }

        /// <summary>
        /// Плавно закрывает <see cref="System.Windows.Forms.Form"/> с выходом из приложения.
        /// </summary>
        /// <param name="Form">Закрываемая форма.</param>
        /// <param name="Duration">Скорость закрытия.</param>
        public static void Exit(Form Form, Int32 Duration)
        {
            Timer Event = new Timer { Interval = Duration };
            Event.Tick += delegate (Object sender, EventArgs e)
            {
                if (Form.Opacity != 1)
                {
                    Form.Opacity -= 0.1;
                }
                else { Environment.Exit(0); }
            };
            Event.Start();
        }

        /// <summary>
        /// Закрывает <see cref="System.Windows.Forms.Form"/> со сжатием в центр по вертикали и выходом из приложения.
        /// </summary>
        /// <param name="Form">Закрываемая форма.</param>
        public static void Exit(Form Form)
        {
            Int32[] BS = new Int32[]
            {
                    Form.Width,
                    Form.Height,
                    Form.Location.X,
                    Form.Location.Y
            };

            System.Threading.Thread TH = new System.Threading.Thread(delegate ()
            { for (; ; ) if (Form.Height < 5) Environment.Exit(0); });
            TH.Start();

            Animation.Size(Form, new Size(BS[0], 0), TransitionType.EaseInQuad, 10);
            Animation.Move(Form, new Point(BS[2], BS[3] + BS[1] / 2), TransitionType.EaseOutCubic, 10);
        }
    }
}
