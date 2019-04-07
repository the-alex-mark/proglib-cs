using System;
using System.Drawing;
using System.Windows.Forms;
using ProgLib.Animations.Metro;
using ProgLib.Animations;
using ProgLib.Diagnostics;

namespace ProgLib.Windows.Metro
{
    public class MetroMessageBox
    {
        private static Int32 MessageCount = 0;

        /// <summary>
        /// Отображает окно сообщения с указанным текстом и типом сообщения
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Type"></param>
        /// <param name="Time"></param>
        public static void Show(String Text, Font Font, MessageType Type, Int32 Time)
        {
            // Форма сообщения
            Form Message = new Form()
            {
                Opacity = 0,
                BackColor = Color.FromArgb(17, 17, 17),
                Size = new Size(320, 100),
                StartPosition = FormStartPosition.Manual,
                DesktopLocation = new Point(
                    Convert.ToInt32(SystemInfo.Screen().Width),
                    10),
                FormBorderStyle = FormBorderStyle.None,
                ControlBox = false,
                ShowInTaskbar = false,
                TopMost = true
            };

            // Отрисовка левой боковой границы
            PictureBox Line = new PictureBox()
            {
                Parent = Message,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom,
                Location = new Point(0, 0),
                Size = new Size(2, 100),
                BackColor = (Type == MessageType.Information
                    ? Drawing.MetroColors.Blue
                    : (Type == MessageType.Error
                        ? Color.Red
                        : Color.Yellow))
            };

            Bitmap Close = new Bitmap(8, 8);
            using (Graphics G = Graphics.FromImage(Close))
            {
                G.DrawLine(new Pen(Color.Silver, 1), new Point(0, 0), new Point(Close.Width - 1, Close.Height - 1));
                G.DrawLine(new Pen(Color.Silver, 1), new Point(Close.Width - 1, 0), new Point(0, Close.Height - 1));
            }

            //// Кнопка закрытия сообщения
            //PictureBox Exit = new PictureBox()
            //{
            //    Parent = Message,
            //    BackColor = Color.Transparent,
            //    Size = new Size(Close.Width, Close.Height),
            //    Location = new Point(Message.Width - 6 - Close.Width, 6),
            //    SizeMode = PictureBoxSizeMode.StretchImage,
            //    Image = Close
            //};
            //Exit.MouseClick += (object O, MouseEventArgs E) => { Message.Dispose(); MessageCount--; };

            // Заголовок сообщения
            Label mCaption = new Label()
            {
                Parent = Message,
                AutoSize = true,
                Location = new Point(7, 4),
                ForeColor = (Type == MessageType.Information
                    ? Drawing.MetroColors.Blue
                    : (Type == MessageType.Error
                        ? Color.Red
                        : Color.Yellow)),
                Font = new Font(Font.FontFamily, 11, FontStyle.Bold, GraphicsUnit.Point),
                Text = (Type == MessageType.Information
                    ? "Информация"
                    : (Type == MessageType.Error
                        ? "Ошибка"
                        : "Внимание"))
            };

            // Текст сообщения
            Label mData = new Label()
            {
                Parent = Message,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom,
                Size = new Size(310, 70),
                Location = new Point(8, 25),
                ForeColor = Color.White,
                Font = new Font(Font.FontFamily, 8, FontStyle.Regular),
                Text = Text
            };

            MessageCount++;
            Opening(Message, 10, Time);
        }

        private static void Opening(Form Form, Int32 Duration, Int32 TimeWait)
        {
            Int32 _messageCount = MessageCount;

            Form.Show();
            Animation.Move(Form, new Point(Convert.ToInt32(SystemInfo.Screen().Width) - 330, 10), TransitionType.EaseInOutQuad, 15);

            Timer Location = new Timer() { Interval = 1 };
            Location.Tick += delegate (Object sender, EventArgs e)
            {
                if (_messageCount < MessageCount)
                {
                    _messageCount = MessageCount;
                    Animation.Move(Form, new Point(Convert.ToInt32(SystemInfo.Screen().Width) - 330, Form.Location.Y + 110), TransitionType.EaseInOutQuad, 15);
                }
            };

            Timer Hide = new Timer() { Interval = Duration };
            Hide.Tick += delegate (Object sender, EventArgs e)
            {
                Form.Opacity -= Form.Opacity > 0 ? 0.1 : 0;
                if (Form.Opacity < 0.1)
                {
                    Hide.Stop();
                    Form.Dispose();
                    MessageCount--;
                }

                //Animation.Animation.Move(Form, new Point(Information.Screen("Width"), Form.Location.Y), TransitionType.EaseInOutQuad, 15);
                //if (Form.Location.X == Information.Screen("Width"))
                //{
                //    Hide.Stop();
                //    Form.Dispose();
                //    MessageCount--;
                //}
            };

            Timer Wait = new Timer() { Interval = TimeWait };
            Wait.Tick += delegate (Object sender, EventArgs e)
            {
                Wait.Stop();
                Hide.Start();
            };

            Timer Show = new Timer() { Interval = Duration };
            Show.Tick += delegate (Object sender, EventArgs e)
            {
                Form.Opacity += Form.Opacity < 0.8 ? 0.1 : 0;
                if (Form.Opacity > 0.7)
                {
                    Show.Stop();
                    Wait.Start();
                }
            };

            Location.Start();
            Show.Start();
        }
    }
}
