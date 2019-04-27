using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialForm : Form
    {
        [DllImport("user32.dll")]
        public static extern Int32 SendMessage(IntPtr hWnd, Int32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll")]
        public static extern Boolean ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern Int32 TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, Int32 x, Int32 y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, Boolean bRevert);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX info);
        
        private readonly Dictionary<Int32, Int32> _resizingLocationsToCmd = new Dictionary<Int32, Int32>
        {
            {12, 3}, {13, 4}, {14, 5}, {10, 1}, {11, 2}, {15, 6}, {16, 7}, {17, 8}
        };
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFOEX
        {
            public Int32 cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public Int32 dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szDevice = new char[32];
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public Int32 left;
            public Int32 top;
            public Int32 right;
            public Int32 bottom;

            public Int32 Width()
            {
                return right - left;
            }

            public Int32 Height()
            {
                return bottom - top;
            }
        }
        
        private enum ResizeDirection
        {
            BottomLeft,
            Left,
            Right,
            BottomRight,
            Bottom,
            None
        }

        private enum ButtonState
        {
            ButtonHide_Down,
            ButtonHide_Over,
            
            ButtonShow_Down,
            ButtonShow_Over,

            ButtonClose_Down,
            ButtonClose_Over,
            
            None
        }

        private readonly Cursor[] _resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        private const Int32 StatusBar_Height = 28;
        
        private ResizeDirection _resizeDir;
        private ButtonState _buttonState = ButtonState.None;

        private Rectangle
            ButtonHide_Bounds, ButtonShow_Bounds, ButtonClose_Bounds, StatusBar_Bounds;

        private Color _StatusBarColor, _ControlBoxColor, _BorderColor;
        private Boolean _maximized, _headerMouseDown, _Border;
        private Size _previousSize;
        private Point _previousLocation;

        [Browsable(true), Category("Appearance"), Description("Цвет StatusBar.")]
        public Color StatusBarColor
        {
            get
            {
                return _StatusBarColor;
            }
            set
            {
                _StatusBarColor = value;
                Invalidate();
            }
        }

        [Browsable(true), Category("Appearance"), Description("Цвет кнопок управления формой.")]
        public Color ControlBoxColor
        {
            get
            {
                return _ControlBoxColor;
            }
            set
            {
                _ControlBoxColor = value;
                Invalidate();
            }
        }

        [Browsable(true), Category("Appearance"), Description("Цвет кнопок управления формой.")]
        public Color BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
                Invalidate();
            }
        }

        [Browsable(true), Category("Appearance"), Description("Цвет кнопок управления формой.")]
        public Boolean Border
        {
            get
            {
                return _Border;
            }
            set
            {
                _Border = value;
                Invalidate();
            }
        }
        
        [Browsable(true), Category("Appearance"), Description("Ручное изменение размера формы.")]
        public Boolean Sizable
        {
            get;
            set;
        }
        
        public MaterialForm()
        {
            InitializeComponent();

            Sizable = true;            
            MinimumSize = new Size(300, 300);
            Padding = new Padding(5, StatusBar_Height + 5, 5, 5);
            Size = new Size(300, 300);
            BackColor = SystemColors.Control;            
            StatusBarColor = SystemColors.ControlDarkDark;
            ControlBoxColor = SystemColors.Control;
            Border = true;
            BorderColor = SystemColors.ControlDarkDark;
            ShowIcon = false;
        }

        protected override void WndProc(ref Message e)
        {
            if (e.Msg == 70)
            {
                Rectangle WorkingArea = SystemInformation.WorkingArea;
                Rectangle Rectangle = (Rectangle)System.Runtime.InteropServices.Marshal.PtrToStructure((IntPtr)((long)(IntPtr.Size * 2) + e.LParam.ToInt64()), typeof(Rectangle));
                if (Rectangle.X <= WorkingArea.Left + 10)
                    Marshal.WriteInt32(e.LParam, IntPtr.Size * 2, WorkingArea.Left);
                if (Rectangle.X + Rectangle.Width >= WorkingArea.Width - 10)
                    Marshal.WriteInt32(e.LParam, IntPtr.Size * 2, WorkingArea.Right - Rectangle.Width);
                if (Rectangle.Y <= WorkingArea.Top + 10)
                    Marshal.WriteInt32(e.LParam, IntPtr.Size * 2 + 4, WorkingArea.Top);
                if (Rectangle.Y + Rectangle.Height >= WorkingArea.Height - 10)
                    Marshal.WriteInt32(e.LParam, IntPtr.Size * 2 + 4, WorkingArea.Bottom - Rectangle.Height);
            }

            base.WndProc(ref e);

            if (DesignMode || IsDisposed) return;

            if (e.Msg == 0x0203) { MaximizeWindow(!_maximized); } else
            if (e.Msg == 0x0200 && _maximized &&
                !(ButtonHide_Bounds.Contains(PointToClient(Cursor.Position)) || ButtonShow_Bounds.Contains(PointToClient(Cursor.Position)) || ButtonClose_Bounds.Contains(PointToClient(Cursor.Position))))
            {
                if (_headerMouseDown)
                {
                    _maximized = false;
                    _headerMouseDown = false;

                    Point mousePoint = PointToClient(Cursor.Position);
                    if (mousePoint.X < Width / 2)
                        Location = mousePoint.X < _previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - _previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);
                    else
                        Location = Width - mousePoint.X < _previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - _previousSize.Width + Width - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - _previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);

                    Size = _previousSize;
                    ReleaseCapture();
                    SendMessage(Handle, 0xA1, 0x2, 0);
                }
            }
            else
            if (e.Msg == 0x0201 &&
                !(ButtonHide_Bounds.Contains(PointToClient(Cursor.Position)) || ButtonShow_Bounds.Contains(PointToClient(Cursor.Position)) || ButtonClose_Bounds.Contains(PointToClient(Cursor.Position))))
            {
                if (!_maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, 0xA1, 0x2, 0);
                }
                else
                {
                    _headerMouseDown = true;
                }
            }
            else
            if (e.Msg == 0x0204)
            {
                Point cursorPos = PointToClient(Cursor.Position);

                if (StatusBar_Bounds.Contains(cursorPos) && !ButtonHide_Bounds.Contains(cursorPos) &&
                    !ButtonShow_Bounds.Contains(cursorPos) && !ButtonClose_Bounds.Contains(cursorPos))
                {
                    // Show default system menu when right clicking titlebar
                    var id = TrackPopupMenuEx(GetSystemMenu(Handle, false), 0x0000 | 0x0100, Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                    // Pass the command as a WM_SYSCOMMAND message
                    SendMessage(Handle, 0x0112, id, 0);
                }
            }
            else
            if (e.Msg == 0xA1)
            {
                // This re-enables resizing by letting the application know when the
                // user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
                if (!Sizable) return;

                byte bFlag = 0;

                // Get which side to resize from
                if (_resizingLocationsToCmd.ContainsKey((Int32)e.WParam))
                    bFlag = (byte)_resizingLocationsToCmd[(Int32)e.WParam];

                if (bFlag != 0)
                    SendMessage(Handle, 0x0112, 0xF000 | bFlag, (Int32)e.LParam);
            }
            else
            if (e.Msg == 0x0202) { _headerMouseDown = false; }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var par = base.CreateParams;
                // 0x20000: Trigger the creation of the system menu
                // 0x00080000: Allow minimizing from taskbar
                par.Style = par.Style | 0x20000 | 0x00080000; // Turn on the WS_MINIMIZEBOX style flag
                return par;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e);

            if (e.Button == MouseButtons.Left && !_maximized)
                ResizeForm(_resizeDir);
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DesignMode) return;
            _buttonState = ButtonState.None;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DesignMode) return;

            if (Sizable)
            {
                //True if the mouse is hovering over a child control
                var isChildUnderMouse = GetChildAtPoint(e.Location) != null;

                if (e.Location.X < 7 && e.Location.Y > Height - 7 && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Location.X < 7 && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.Left;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.X > Width - 7 && e.Location.Y > Height - 7 && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.Location.X > Width - 7 && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.Y > Height - 7 && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else
                {
                    _resizeDir = ResizeDirection.None;

                    // Только сбросить курсор при необходимости, это предотвращает его от мерцания, когда дочерний элемент управления изменяет курсор на свои собственные потребности.
                    if (_resizeCursors.Contains(Cursor))
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            UpdateButtons(e);
        }

        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDisposed) return;
            // Преобразовать в позицию клиента и перейти к Form.MouseMove
            Point clientCursorPos = PointToClient(e.Location);
            MouseEventArgs newE = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseMove(newE);
        }

        private void UpdateButtons(MouseEventArgs e, Boolean up = false)
        {
            if (DesignMode) return;
            var oldState = _buttonState;
            Boolean showMin = MinimizeBox && ControlBox;
            Boolean showMax = MaximizeBox && ControlBox;

            if (e.Button == MouseButtons.Left && !up)
            {
                if (showMin && !showMax && ButtonShow_Bounds.Contains(e.Location))
                    _buttonState = ButtonState.ButtonHide_Down;
                else if (showMin && showMax && ButtonHide_Bounds.Contains(e.Location))
                    _buttonState = ButtonState.ButtonHide_Down;
                else if (showMax && ButtonShow_Bounds.Contains(e.Location))
                    _buttonState = ButtonState.ButtonShow_Down;
                else if (ControlBox && ButtonClose_Bounds.Contains(e.Location))
                    _buttonState = ButtonState.ButtonClose_Down;
                else
                    _buttonState = ButtonState.None;
            }
            else
            {
                if (showMin && !showMax && ButtonShow_Bounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.ButtonHide_Over;

                    if (oldState == ButtonState.ButtonHide_Down && up)
                        Animation("HIDE", 5);
                    //WindowState = FormWindowState.Minimized;

                }
                else if (showMin && showMax && ButtonHide_Bounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.ButtonHide_Over;

                    if (oldState == ButtonState.ButtonHide_Down && up)
                        Animation("HIDE", 5);
                    //WindowState = FormWindowState.Minimized;
                }
                else if (MaximizeBox && ControlBox && ButtonShow_Bounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.ButtonShow_Over;

                    if (oldState == ButtonState.ButtonShow_Down && up)
                        MaximizeWindow(!_maximized);

                }
                else if (ControlBox && ButtonClose_Bounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.ButtonClose_Over;

                    if (oldState == ButtonState.ButtonClose_Down && up)
                        Animation("EXIT", 5);
                    //Close();
                }
                else _buttonState = ButtonState.None;
            }

            if (oldState != _buttonState) Invalidate();
        }

        private void MaximizeWindow(Boolean maximize)
        {
            if (!MaximizeBox || !ControlBox) return;

            _maximized = maximize;

            if (maximize)
            {
                IntPtr monitorHandle = MonitorFromWindow(Handle, 2);
                MONITORINFOEX monitorInfo = new MONITORINFOEX();
                GetMonitorInfo(new HandleRef(null, monitorHandle), monitorInfo);
                _previousSize = Size;
                _previousLocation = Location;
                Size = new Size(monitorInfo.rcWork.Width(), monitorInfo.rcWork.Height());
                Location = new Point(monitorInfo.rcWork.left, monitorInfo.rcWork.top);
            }
            else
            {
                Size = _previousSize;
                Location = _previousLocation;
            }

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e, true);

            base.OnMouseUp(e);
            ReleaseCapture();
        }

        private void ResizeForm(ResizeDirection direction)
        {
            if (DesignMode) return;
            var dir = -1;
            switch (direction)
            {
                case ResizeDirection.BottomLeft:
                    dir = 16;
                    break;
                case ResizeDirection.Left:
                    dir = 10;
                    break;
                case ResizeDirection.Right:
                    dir = 11;
                    break;
                case ResizeDirection.BottomRight:
                    dir = 17;
                    break;
                case ResizeDirection.Bottom:
                    dir = 15;
                    break;
            }

            ReleaseCapture();
            if (dir != -1)
            {
                SendMessage(Handle, 0xA1, dir, 0);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ButtonHide_Bounds   = new Rectangle(Width - 57, 0, 16, StatusBar_Height);
            ButtonShow_Bounds   = new Rectangle(Width - 39, 0, 16, StatusBar_Height);
            ButtonClose_Bounds  = new Rectangle(Width - 21, 0, 16, StatusBar_Height);
            StatusBar_Bounds    = new Rectangle(0, 0, Width, StatusBar_Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            e.Graphics.FillRectangle(new SolidBrush(_StatusBarColor), StatusBar_Bounds);
            if (_Border) e.Graphics.DrawRectangle(new Pen(_BorderColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));
                        
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle ButtonHide_Icon = new Rectangle(ButtonHide_Bounds.X + 2, 8, 10, 10);
            Rectangle ButtonShow_Icon = new Rectangle(ButtonShow_Bounds.X + 2, 8, 10, 10);
            Rectangle ButtonClose_Icon = new Rectangle(ButtonClose_Bounds.X + 2, 8, 10, 10);

            // Отрисовка выделения кнопок ControlBox при наведении и нажатии мышью.
            Brush A = new SolidBrush(BlendColor(_StatusBarColor, ControlBoxColor, 40));
            Brush B = new SolidBrush(BlendColor(_StatusBarColor, ControlBoxColor, 50));
            
            if (_buttonState == ButtonState.ButtonHide_Over && MinimizeBox && ControlBox)
                e.Graphics.FillEllipse(A, MaximizeBox && ControlBox ? ButtonHide_Icon : ButtonShow_Icon);

            if (_buttonState == ButtonState.ButtonHide_Down && MinimizeBox && ControlBox)
                e.Graphics.FillEllipse(B, MaximizeBox && ControlBox ? ButtonHide_Icon : ButtonShow_Icon);

            if (_buttonState == ButtonState.ButtonShow_Over && MaximizeBox && ControlBox)
                e.Graphics.FillEllipse(A, ButtonShow_Icon);

            if (_buttonState == ButtonState.ButtonShow_Down && MaximizeBox && ControlBox)
                e.Graphics.FillEllipse(B, ButtonShow_Icon);

            if (_buttonState == ButtonState.ButtonClose_Over && ControlBox)
                e.Graphics.FillEllipse(A, ButtonClose_Icon);

            if (_buttonState == ButtonState.ButtonClose_Down && ControlBox)
                e.Graphics.FillEllipse(B, ButtonClose_Icon);

            // Отрисовка иконок ControlBox.
            using (Pen ButtonColor = new Pen(_ControlBoxColor, 1))
            {
                if (MinimizeBox && ControlBox)
                    e.Graphics.DrawEllipse(ButtonColor, ButtonHide_Icon);

                if (MaximizeBox && ControlBox)
                    e.Graphics.DrawEllipse(ButtonColor, ButtonShow_Icon);

                if (ControlBox)
                    e.Graphics.DrawEllipse(ButtonColor, ButtonClose_Icon);
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            // Отрисовка текста заголовка и иконки Form.
            if (!ShowIcon)
            {
                e.Graphics.DrawString(
                    Text,
                    new Font(Font.Name, (float)8.25, Font.Style),
                    new SolidBrush(ForeColor),
                    new Rectangle(5, 0, Width, StatusBar_Height - 2),
                    new StringFormat { LineAlignment = StringAlignment.Center });
            }
            else
            {
                if (Icon != null)
                    e.Graphics.DrawIcon(Icon, new Rectangle(6, 6, 15, 15));
                else
                    e.Graphics.DrawRectangle(new Pen(SystemColors.Control, 1), new Rectangle(6, 6, 15, 15));

                e.Graphics.DrawString(
                    Text,
                    new Font(Font.Name, (float)8.25, Font.Style),
                    new SolidBrush(ForeColor),
                    new Rectangle(25, 0, Width, StatusBar_Height - 2),
                    new StringFormat { LineAlignment = StringAlignment.Center });
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Opacity = 0;
            Animation("SHOW", 300);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            Animation("SHOW", 5);
        }

        public static Color BlendColor(Color BackgroundColor, Color FrontColor, Double Blend)
        {
            Double Ratio = Blend / 255D;

            return Color.FromArgb(
                (Int32)((BackgroundColor.R * (1D - Ratio)) + (FrontColor.R * Ratio)),
                (Int32)((BackgroundColor.G * (1D - Ratio)) + (FrontColor.G * Ratio)),
                (Int32)((BackgroundColor.B * (1D - Ratio)) + (FrontColor.B * Ratio)));
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            DoubleBuffered = true;

            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.SupportsTransparentBackColor, true);

            FormBorderStyle = FormBorderStyle.None;
            AutoScaleMode = AutoScaleMode.None;
            StartPosition = FormStartPosition.CenterScreen;

            AutoScaleDimensions = new Size(300, 300);
            ClientSize = new Size(298, 273);
            
            // Это позволяет форме вызвать событие MouseMove, даже если мышь находится над другим элементом управления.
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;

            ResumeLayout(false);
        }

        private void Animation(String Mode, Int32 Duration)
        {
            Timer T = new Timer { Interval = Duration };
            T.Tick += delegate (Object sender, EventArgs e)
            {
                switch (Mode)
                {
                    case "SHOW":
                        if (Opacity != 1) { Opacity += 0.1; } else { T.Stop(); }
                        break;

                    case "HIDE":
                        if (Opacity > 0) { Opacity -= 0.1; } else { T.Stop(); WindowState = FormWindowState.Minimized; }
                        break;

                    case "EXIT":
                        if (Opacity > 0) { Opacity -= 0.1; } else { Close(); }
                        break;

                    default: break;
                }
            };

            T.Start();
        }
    }

    public class MouseMessageFilter : IMessageFilter
    {
        private const Int32 WM_MOUSEMOVE = 0x0200;

        public static event MouseEventHandler MouseMove;

        public Boolean PreFilterMessage(ref Message m)
        {

            if (m.Msg == WM_MOUSEMOVE)
            {
                if (MouseMove != null)
                {
                    Int32 x = Control.MousePosition.X, y = Control.MousePosition.Y;

                    MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            return false;
        }
    }
}
