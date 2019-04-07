using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Drawing;
using ProgLib.Properties;
using ProgLib.Windows.Minimal;

namespace ProgLib.Windows.Cyotek
{
    public partial class ColorDialog : Form
    {
        #region Shadow

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            Int32 nLeftRect, Int32 nTopRect, Int32 nRightRect, Int32 nBottomRect, Int32 nWidthEllipse, Int32 nHeightEllipse);

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
        }


        #endregion

        public ColorDialog()
        {
            InitializeComponent();
            
            MovingForm(panel1, materialTabSelector1);
        }

        private struct ColorInfo
        {
            public ColorInfo(Color Color, String Name, String Hex)
            {
                this.Color = Color;
                this.Name = Name;
                this.Hex = Hex;
            }

            public Color Color { get; private set; }
            public String Name { get; private set; }
            public String Hex { get; private set; }
        }
        private Brush Transparent()
        {
            return new TextureBrush(new Bitmap(Resources.CellTransparentBackground), WrapMode.Tile);
        }
        private void RedrawingListBox(ListBox _control, Type _colorType)
        {
            List<ColorInfo> _listColors = new List<ColorInfo>();
            PropertyInfo[] _listProperty = _colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);

            foreach (PropertyInfo _property in _listProperty)
            {
                _control.Items.Add(_property.Name);
                _listColors.Add(new ColorInfo(Color.FromName(_property.Name), _property.Name, Color.FromName(_property.Name).ToHEX()));
            }
            
            _control.BackColor = _control.Parent.BackColor;
            _control.BorderStyle = BorderStyle.None;
            _control.ItemHeight = 32;
            _control.DrawMode = DrawMode.OwnerDrawVariable;
            _control.DrawItem += delegate (Object _object, DrawItemEventArgs _drawItemEventArgs)
            {
                if (_drawItemEventArgs.Index < 0) return;

                // Отрисовка выделения Item
                if ((_drawItemEventArgs.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    _drawItemEventArgs = new DrawItemEventArgs(
                        _drawItemEventArgs.Graphics,
                        _drawItemEventArgs.Font,
                        _drawItemEventArgs.Bounds,
                        _drawItemEventArgs.Index,
                        _drawItemEventArgs.State ^ DrawItemState.Selected,
                        _drawItemEventArgs.ForeColor,
                        BackColor);
                }
                _drawItemEventArgs.DrawBackground();
                _drawItemEventArgs.DrawFocusRectangle();
                
                // Отрисовка цвета и границы
                _drawItemEventArgs.Graphics.FillRectangle(
                    (_listColors[_drawItemEventArgs.Index].Name == "Transparent") ? Transparent() : new SolidBrush(_listColors[_drawItemEventArgs.Index].Color), 
                    new Rectangle(_drawItemEventArgs.Bounds.X + 3, _drawItemEventArgs.Bounds.Y + 3, 43 - 6, _drawItemEventArgs.Bounds.Height - 6));
                
                _drawItemEventArgs.Graphics.DrawRectangle(new Pen(Color.Black), new Rectangle(_drawItemEventArgs.Bounds.X + 3, _drawItemEventArgs.Bounds.Y + 3, 43 - 6, _drawItemEventArgs.Bounds.Height - 7));

                // Отрисовка названия цвета
                TextRenderer.DrawText(
                    _drawItemEventArgs.Graphics,
                    _listColors[_drawItemEventArgs.Index].Name.ToString(),
                    Font,
                    new Rectangle(42, _drawItemEventArgs.Bounds.Y + 3, _drawItemEventArgs.Bounds.Width - 1, _drawItemEventArgs.Bounds.Height - 1),
                    /*((_drawItemEventArgs.State & DrawItemState.Focus) == DrawItemState.Focus) ? Color.White : */Color.Black,
                    Color.Transparent,
                    TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);

                // Отрисовка значения цветовой модели HEX
                TextRenderer.DrawText(
                    _drawItemEventArgs.Graphics,
                    _listColors[_drawItemEventArgs.Index].Hex.ToLower(),
                    new Font(Font.FontFamily, 6.5F, FontStyle.Regular),
                    new Rectangle(42, _drawItemEventArgs.Bounds.Y, _drawItemEventArgs.Bounds.Width - 1, _drawItemEventArgs.Bounds.Height - 4),
                    /*((_drawItemEventArgs.State & DrawItemState.Focus) == DrawItemState.Focus) ? Color.White : */Color.Gray,
                    Color.Transparent,
                    TextFormatFlags.Left | TextFormatFlags.Bottom | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);
            };

            PictureBox Line = new PictureBox()
            {
                Parent = _control.Parent,
                Size = new Size(1, _control.Height + 25),
                Location = new Point(_control.Width - 14, _control.Location.Y),
                BackColor = _control.BackColor
            };
            Line.BringToFront();
        }
        private void MovingForm(Panel _control, params Control[] _exceptions)
        {
            _control.MouseDown += delegate (Object _object, MouseEventArgs _mouseEventArgs)
            {
                Window.Move(this);
            };

            foreach (Control _child in _control.Controls)
            {
                foreach (Control _exception in _exceptions)
                {
                    if (_child != _exception)
                    {
                        _child.MouseDown += delegate (Object _object, MouseEventArgs _mouseEventArgs)
                        {
                            Window.Move(this);
                        };
                    }
                }
            }
        }

        private void ColorDialog_Load(Object sender, EventArgs e)
        {
            RedrawingListBox(ListSystemColors, typeof(SystemColors));
            RedrawingListBox(ListWebColors, typeof(Color));

            materialTabControl1.SelectedTab = PageCustomColors;
            //ListWebColors.SelectedIndex = 0;
            //ListSystemColors.SelectedIndex = 0;

            //Font _font = ProgLib.Text.FontFromResources.UseFont(Resources.MontserratRegular);
            //label1.Font = new Font(_font.FontFamily, 18F);

            //button1.Font = new Font(_font.FontFamily, 7.5F);
            //materialButton1.Font = new Font(_font.FontFamily, 7.5F);
            //materialButton2.Font = new Font(_font.FontFamily, 7.5F);
        }
    }
}