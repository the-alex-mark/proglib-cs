using ProgLib.Diagnostics;
using ProgLib.Windows.Material;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Minimal
{
    public class MinimalMessageBox
    {
        public static void Show(String Text, String Header, MinimalMessageBoxRenderer Renderer, MessageType Type)
        {
            // Форма сообщения
            MinimalForm Message = new MinimalForm()
            {
                Adhesion = false,
                Animation = Renderer.Animation,
                BackColor = Renderer.BackColor,
                StatusBarColor = Renderer.StatusBarColor,
                StyleColor = Renderer.StyleColor,
                Border = Renderer.Border,
                Font = Renderer.Font,
                ForeColor = Renderer.HeaderColor,

                Size = new Size(400, 150),
                MinimumSize = new Size(400, 150),
                MaximumSize = new Size(610, SystemInfo.Screen().Height),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.None,
                MinimizeBox = false,
                MaximizeBox = false,
                //Sizable = false,
                ShowInTaskbar = false,
                TopMost = true,

                Text = Header
            };
            
            // Текст сообщения
            Label _text = new Label()
            {
                Parent = Message,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                Size = new Size(Message.Width - 9, Message.Height - 77),
                //BackColor = Color.Red,
                Location = new Point(5, 32),
                ForeColor = Renderer.TextColor,
                Font = new Font(Renderer.Font.FontFamily, 8, FontStyle.Regular),
                Text = Text
            };

            // Набор кнопок
            FlowLayoutPanel _buttons = new FlowLayoutPanel()
            {
                Parent = Message,
                Padding = new Padding(2, 5, 2, 5),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                FlowDirection = FlowDirection.RightToLeft,
                Size = new Size(Message.Width - 2, 42),
                Location = new Point(1, Message.Height - 43),
            };
            Buttons(_buttons, MessageBoxButtons.YesNoCancel);

            Resize(_text.Text, Message);
            Message.FormClosed += delegate (Object sender, FormClosedEventArgs e)
            {
                Message.Dispose();
            };
            Message.ShowDialog();
        }

        private static void Buttons(FlowLayoutPanel Panel, MessageBoxButtons Buttons)
        {
            MinimalButton _ok = new MinimalButton()
            {
                Size = new Size(88, 27),
                StyleColor = Panel.ForeColor,
                Text = "ОК"
            };
            MinimalButton _yes = new MinimalButton()
            {
                Size = new Size(88, 27),
                StyleColor = Panel.ForeColor,
                Text = "Да"
            };
            MinimalButton _no = new MinimalButton()
            {
                Size = new Size(88, 27),
                StyleColor = Panel.ForeColor,
                Text = "Нет"
            };
            MinimalButton _cancel = new MinimalButton()
            {
                Size = new Size(88, 27),
                StyleColor = Panel.ForeColor,
                Text = "Отмена"
            };

            switch (Buttons)
            {
                case MessageBoxButtons.OK:
                    _ok.Parent = Panel;
                    break;

                case MessageBoxButtons.YesNo:
                    _no.Parent = Panel;
                    _yes.Parent = Panel;
                    break;

                case MessageBoxButtons.YesNoCancel:
                    _cancel.Parent = Panel;
                    _no.Parent = Panel;
                    _yes.Parent = Panel;
                    break;

                default:
                    _ok.Parent = Panel;
                    break;
            }
        }
        private static void Resize(String Text, Form Form)
        {
            Form.Size = new Size(
                TextRenderer.MeasureText(Text, Form.Font).Width + 10,
                TextRenderer.MeasureText(Text, Form.Font).Height + 82);
        }
    }
}