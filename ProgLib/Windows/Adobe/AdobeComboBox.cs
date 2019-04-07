using ProgLib.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Adobe
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ComboBox))]
    public partial class AdobeComboBox : System.Windows.Forms.ComboBox
    {
        public AdobeComboBox()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // Таймер для проверки, когда выпадающий список полностью виден
            _dropDownCheck.Interval = 100;
            _dropDownCheck.Tick += new EventHandler(dropDownCheck_Tick);

            BorderColor = Color.White;
            ArrowColor = Color.White;
            ButtonColor = Color.FromArgb(26, 26, 26);
            BackColor = Color.FromArgb(50, 50, 50);

            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawVariable;
            ItemHeight = 18;

            _text = Name;
        }

        private String _text;

        public new String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет кнопки")]
        public Color ButtonColor
        {
            get;
            set;

            //get
            //{
            //    return this._ButtonColor;
            //}
            //set
            //{
            //    this._ButtonColor = value;
            //    this.DropButtonBrush = this.ButtonColor;
            //    base.Invalidate();
            //}
        }

        [Category("Appearance"), Description("Цвет стрелки компонента")]
        public Color ArrowColor
        {
            get;
            set;

            //get
            //{
            //    return this._ArrowColor;
            //}
            //set
            //{
            //    this._ArrowColor = value;
            //    this._ArrawColor_ = this.ArrowColor;
            //    base.Invalidate();
            //}
        }

        [Category("Appearance"), Description("Цвет границ компонента")]
        public Color BorderColor
        {
            get;
            set;

            //get
            //{
            //    return this._BorderColor;
            //}
            //set
            //{
            //    this._BorderColor = value;
            //    base.Invalidate();
            //}
        }

        [DllImport("user32")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        
        protected virtual GraphicsPath Ellipse(Radius Radius, Rectangle Rectangle)
        {
            GraphicsPath GP = new GraphicsPath();

            if (Radius.LeftTop != 0)
                GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, Radius.LeftTop * 2, Radius.LeftTop * 2), 180, 90);
            else GP.AddLine(new Point(Rectangle.X, Rectangle.Y), new Point(Rectangle.X, Rectangle.Y));

            if (Radius.RightTop != 0)
                GP.AddArc(new Rectangle(Rectangle.Width - Radius.RightTop * 2, Rectangle.Y, Radius.RightTop * 2, Radius.RightTop * 2), 270, 90);
            else GP.AddLine(new Point(Rectangle.Width, Rectangle.Y), new Point(Rectangle.Width, Rectangle.Y));

            if (Radius.RightBottom != 0)
                GP.AddArc(new Rectangle(Rectangle.Width - Radius.RightBottom * 2, Rectangle.Height - Radius.RightBottom * 2, Radius.RightBottom * 2, Radius.RightBottom * 2), 0, 90);
            else GP.AddLine(new Point(Rectangle.Width, Rectangle.Height), new Point(Rectangle.Width, Rectangle.Height));

            if (Radius.LeftBottom != 0)
                GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - Radius.LeftBottom * 2, Radius.LeftBottom * 2, Radius.LeftBottom * 2), 90, 90);
            else GP.AddLine(new Point(Rectangle.X, Rectangle.Height), new Point(Rectangle.X, Rectangle.Height));

            GP.CloseFigure();

            return GP;
        }
        protected virtual Image Arrow(Color Border)
        {
            Bitmap Image = new Bitmap(8, 7);
            using (Graphics G = Graphics.FromImage(Image))
            {
                G.Clear(Color.Transparent);

                Point[] Lines = new Point[]
                {
                    new Point(1, 1),
                    new Point(4, 4),
                    new Point(7, 1)
                };
                G.DrawLines(new Pen(Border, 1), Lines);
                //G.DrawRectangle(new Pen(Color.Red), new Rectangle(0, 0, 17, 17));
            }

            return Image;
        }


        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 15:
                    base.WndProc(ref m);

                    using (Graphics G = base.CreateGraphics())
                    {
                        G.Clear(Color.FromArgb(58, 58, 58));
                        G.SmoothingMode = SmoothingMode.AntiAlias;

                        G.FillPath(new SolidBrush(Color.FromArgb(50, 50, 50)), Ellipse(new Radius(5, 5, 5, 5), new Rectangle(0, 0, Width - 1, Height - 1)));
                        G.FillPath(new SolidBrush(Color.FromArgb(50, 50, 50)), Ellipse(new Radius(0, 5, 5, 0), new Rectangle(Width - 17, 0, 17, Height)));

                        G.DrawString("Цветовая модель", Font, new SolidBrush(ForeColor), new Rectangle(5, 0, Bounds.Width - 20, Bounds.Height - 1), new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                        G.DrawImage(Arrow(ForeColor), new Point(Width - 16, (Height / 2) - (Arrow(ForeColor).Height / 2)));

                        G.DrawPath(new Pen(BorderColor, 1), Ellipse(new Radius(5, 5, 5, 5), new Rectangle(0, 0, Width - 1, Height - 1)));
                    }
                    break;

                case WM_CTLCOLORLISTBOX:
                    using (Graphics G = base.CreateGraphics())
                    {
                        G.Clear(Color.FromArgb(58, 58, 58));
                        G.SmoothingMode = SmoothingMode.AntiAlias;

                        G.FillPath(new SolidBrush(Color.FromArgb(50, 50, 50)), Ellipse(new Radius(5, 5, 0, 0), new Rectangle(0, 0, Width - 1, Height - 1)));
                        G.FillPath(new SolidBrush(Color.FromArgb(50, 50, 50)), Ellipse(new Radius(0, 5, 0, 0), new Rectangle(Width - 17, 0, 17, Height)));

                        G.DrawString("Цветовая модель", Font, new SolidBrush(ForeColor), new Rectangle(5, 0, Bounds.Width - 20, Bounds.Height - 1), new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                        G.DrawImage(Arrow(ForeColor), new Point(Width - 16, (Height / 2) - (Arrow(ForeColor).Height / 2)));

                        G.DrawPath(new Pen(BorderColor, 1), Ellipse(new Radius(5, 5, 0, 0), new Rectangle(0, 0, Width - 1, Height)));
                    }

                    DrawNativeBorder(m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (e.Index < 0) return;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.FromArgb(40, 40, 40));
            }
            e.DrawBackground();

            e.Graphics.DrawString(Items[e.Index].ToString(), Font, new SolidBrush(ForeColor), new Rectangle(2, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height - 1), StringFormat.GenericDefault);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            Invalidate();
        }


        public enum PenStyles
        {
            PS_SOLID = 0,
            PS_DASH = 1,
            PS_DOT = 2,
            PS_DASHDOT = 3,
            PS_DASHDOTDOT = 4
        }

        public enum ComboBoxButtonState
        {
            STATE_SYSTEM_NONE = 0,
            STATE_SYSTEM_INVISIBLE = 0x00008000,
            STATE_SYSTEM_PRESSED = 0x00000008
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COMBOBOXINFO
        {
            public Int32 cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public ComboBoxButtonState buttonState;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left_, int top_, int right_, int bottom_)
            {
                Left = left_;
                Top = top_;
                Right = right_;
                Bottom = bottom_;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is RECT))
                {
                    return false;
                }
                return this.Equals((RECT)obj);
            }

            public bool Equals(RECT value)
            {
                return this.Left == value.Left &&
                       this.Top == value.Top &&
                       this.Right == value.Right &&
                       this.Bottom == value.Bottom;
            }

            public int Height
            {
                get
                {
                    return Bottom - Top + 1;
                }
            }

            public int Width
            {
                get
                {
                    return Right - Left + 1;
                }
            }

            public Size Size { get { return new Size(Width, Height); } }

            public Point Location { get { return new Point(Left, Top); } }

            // Handy method for converting to a System.Drawing.Rectangle
            public System.Drawing.Rectangle ToRectangle()
            {
                return System.Drawing.Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }

            public static RECT FromRectangle(Rectangle rectangle)
            {
                return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            }

            public void Inflate(int width, int height)
            {
                this.Left -= width;
                this.Top -= height;
                this.Right += width;
                this.Bottom += height;
            }

            public override int GetHashCode()
            {
                return Left ^ ((Top << 13) | (Top >> 0x13))
                    ^ ((Width << 0x1a) | (Width >> 6))
                    ^ ((Height << 7) | (Height >> 0x19));
            }

            public static implicit operator Rectangle(RECT rect)
            {
                return System.Drawing.Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }

            public static implicit operator RECT(Rectangle rect)
            {
                return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }


        public const int WM_CTLCOLORLISTBOX = 0x0134;
        private Timer _dropDownCheck = new Timer();      // Таймер, который проверяет, когда раскрывающийся список полностью виден

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hWnd, ref COMBOBOXINFO pcbi);

        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(PenStyles enPenStyle, int nWidth, int crColor);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern void Rectangle(IntPtr hdc, int X1, int Y1, int X2, int Y2);

        public static int RGB(int R, int G, int B)
        {
            return (R | (G << 8) | (B << 16));
        }

        /// <summary>
        /// На выпадающем
        /// </summary>
        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            // Начните проверку видимости раскрывающегося списка
            _dropDownCheck.Start();
        }

        /// <summary>
        /// Проверяет, когда раскрывающийся список полностью виден
        /// </summary>
        private void dropDownCheck_Tick(object sender, EventArgs e)
        {
            // Если раскрывающийся список был полностью удален
            if (DroppedDown)
            {
                // Остановить время, отправить обновление списка 
                _dropDownCheck.Stop();
                Message m = GetControlListBoxMessage(this.Handle);
                WndProc(ref m);
            }
        }

        /// <summary>
        /// Non чертеж границы зоны клиента
        /// </summary>
        /// <param name="m">Сообщение окна для обработки</param>
        /// <param name="handle">Ручка к управлению</param>
        public void DrawNativeBorder(IntPtr handle)
        {
            // Определите прямоугольник рамки windows элемента управления
            RECT controlRect;
            GetWindowRect(handle, out controlRect);
            controlRect.Right -= controlRect.Left; controlRect.Bottom -= controlRect.Top;
            controlRect.Top = controlRect.Left = 0;

            // Получить контекст устройства контроля
            IntPtr dc = GetWindowDC(handle);

            // Определите клиентскую область внутри управляющего выпрямителя
            RECT clientRect = controlRect;
            clientRect.Left += 1;
            clientRect.Top += 1;
            clientRect.Right -= 1;
            clientRect.Bottom -= 1;
            ExcludeClipRect(dc, clientRect.Left, clientRect.Top, clientRect.Right, clientRect.Bottom);

            // Создайте перо и выберите его
            Color borderColor = Color.FromArgb(90, 90, 90);
            IntPtr border = CreatePen(PenStyles.PS_SOLID, 1, RGB(borderColor.R, borderColor.G, borderColor.B));

            // Нарисуйте прямоугольник границы
            IntPtr borderPen = SelectObject(dc, border);
            Rectangle(dc, controlRect.Left, controlRect.Top, controlRect.Right, controlRect.Bottom);
            SelectObject(dc, borderPen);
            DeleteObject(border);

            // Освободить контекст устройства
            ReleaseDC(handle, dc);
            SetFocus(handle);
        }

        /// <summary>
        /// Создает сообщение WM_CTLCOLOR_LISTBOX по умолчанию
        /// </summary>
        /// <param name="handle">Ручка падения вниз</param>
        /// <returns>A WM_CTLCOLORLISTBOX message</returns>
        public Message GetControlListBoxMessage(IntPtr handle)
        {
            // Принудительная перерисовка границы фокуса без клиента
            Message m = new Message();
            m.HWnd = handle;
            m.LParam = GetListHandle(handle);
            m.WParam = IntPtr.Zero;
            m.Msg = WM_CTLCOLORLISTBOX;
            m.Result = IntPtr.Zero;
            return m;
        }

        /// <summary>
        /// Возвращает элемент управления списком
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static IntPtr GetListHandle(IntPtr handle)
        {
            COMBOBOXINFO info;
            info = new COMBOBOXINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            return GetComboBoxInfo(handle, ref info) ? info.hwndList : IntPtr.Zero;
        }
    }
}
