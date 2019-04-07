using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Text;

namespace ProgLib.Windows.Metro
{
    public class MetroForm : System.Windows.Forms.Form
    {
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public new FormBorderStyle FormBorderStyle { get { return base.FormBorderStyle; } set { base.FormBorderStyle = value; } }
        public Boolean Sizable { get; set; }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX info);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTBOTTOM = 15;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int BORDER_WIDTH = 7;
        private ResizeDirection _resizeDir;
        private ButtonState _buttonState = ButtonState.None;

        private const int WMSZ_TOP = 3;
        private const int WMSZ_TOPLEFT = 4;
        private const int WMSZ_TOPRIGHT = 5;
        private const int WMSZ_LEFT = 1;
        private const int WMSZ_RIGHT = 2;
        private const int WMSZ_BOTTOM = 6;
        private const int WMSZ_BOTTOMLEFT = 7;
        private const int WMSZ_BOTTOMRIGHT = 8;

        private readonly Dictionary<int, int> _resizingLocationsToCmd = new Dictionary<int, int>
        {
            {HTTOP,         WMSZ_TOP},
            {HTTOPLEFT,     WMSZ_TOPLEFT},
            {HTTOPRIGHT,    WMSZ_TOPRIGHT},
            {HTLEFT,        WMSZ_LEFT},
            {HTRIGHT,       WMSZ_RIGHT},
            {HTBOTTOM,      WMSZ_BOTTOM},
            {HTBOTTOMLEFT,  WMSZ_BOTTOMLEFT},
            {HTBOTTOMRIGHT, WMSZ_BOTTOMRIGHT}
        };

        private const int STATUS_BAR_BUTTON_WIDTH = STATUS_BAR_HEIGHT;
        private const int STATUS_BAR_HEIGHT = 24;
        private const int ACTION_BAR_HEIGHT = 40;

        private const uint TPM_LEFTALIGN = 0x0000;
        private const uint TPM_RETURNCMD = 0x0100;

        private const int WM_SYSCOMMAND = 0x0112;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x00080000;

        private const int MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFOEX
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szDevice = new char[32];
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Width()
            {
                return right - left;
            }

            public int Height()
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
            XOver,
            MaxOver,
            MinOver,
            XDown,
            MaxDown,
            MinDown,
            None
        }

        private readonly Cursor[] _resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        private Rectangle _minButtonBounds;
        private Rectangle _maxButtonBounds;
        private Rectangle _xButtonBounds;
        private Rectangle _actionBarBounds;
        private Rectangle _statusBarBounds;

        private bool _maximized;
        private Size _previousSize;
        private Point _previousLocation;
        private bool _headerMouseDown;

        public MetroForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            Sizable = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            // This enables the form to trigger the MouseMove event even when mouse is over another control
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;

            Font = new Font(Font.FontFamily, 12, Font.Style);
            BackColor = Color.White;
            ForeColor = Color.White;

            _theme = Theme.Light;
            _styleColor = Drawing.MetroColors.Blue;
            _textAlign = ContentAlignment.TopLeft;
        }
        
        private Theme _theme;
        private Color _styleColor;
        private ContentAlignment _textAlign;

        [Category("Metro Appearance"), Description("Цветовая тема элемента управления")]
        public Theme Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                Invalidate();
            }
        }

        [Category("Metro Appearance"), Description("Цвет оформления при Checked равном \"true\"")]
        public Color StyleColor
        {
            get { return _styleColor; }
            set
            {
                _styleColor = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Выравнивание текста, который будет отображаться в данном элементе управления."), DefaultValue(ContentAlignment.BottomLeft)]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set
            {
                _textAlign = value;
                Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode || IsDisposed) return;

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                MaximizeWindow(!_maximized);
            }
            else if (m.Msg == WM_MOUSEMOVE && _maximized &&
                (_statusBarBounds.Contains(PointToClient(Cursor.Position)) || _actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                !(_minButtonBounds.Contains(PointToClient(Cursor.Position)) || _maxButtonBounds.Contains(PointToClient(Cursor.Position)) || _xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (_headerMouseDown)
                {
                    _maximized = false;
                    _headerMouseDown = false;

                    var mousePoint = PointToClient(Cursor.Position);
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
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            else if (m.Msg == WM_LBUTTONDOWN &&
                (_statusBarBounds.Contains(PointToClient(Cursor.Position)) || _actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                !(_minButtonBounds.Contains(PointToClient(Cursor.Position)) || _maxButtonBounds.Contains(PointToClient(Cursor.Position)) || _xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (!_maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
                else
                {
                    _headerMouseDown = true;
                }
            }
            else if (m.Msg == WM_RBUTTONDOWN)
            {
                Point cursorPos = PointToClient(Cursor.Position);

                if (_statusBarBounds.Contains(cursorPos) && !_minButtonBounds.Contains(cursorPos) &&
                    !_maxButtonBounds.Contains(cursorPos) && !_xButtonBounds.Contains(cursorPos))
                {
                    // Show default system menu when right clicking titlebar
                    var id = TrackPopupMenuEx(GetSystemMenu(Handle, false), TPM_LEFTALIGN | TPM_RETURNCMD, Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                    // Pass the command as a WM_SYSCOMMAND message
                    SendMessage(Handle, WM_SYSCOMMAND, id, 0);
                }
            }
            else if (m.Msg == WM_NCLBUTTONDOWN)
            {
                // This re-enables resizing by letting the application know when the
                // user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
                if (!Sizable) return;

                byte bFlag = 0;

                // Get which side to resize from
                if (_resizingLocationsToCmd.ContainsKey((int)m.WParam))
                    bFlag = (byte)_resizingLocationsToCmd[(int)m.WParam];

                if (bFlag != 0)
                    SendMessage(Handle, WM_SYSCOMMAND, 0xF000 | bFlag, (int)m.LParam);
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _headerMouseDown = false;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var par = base.CreateParams;
                // WS_SYSMENU: Trigger the creation of the system menu
                // WS_MINIMIZEBOX: Allow minimizing from taskbar
                par.Style = par.Style | WS_MINIMIZEBOX | WS_SYSMENU; // Turn on the WS_MINIMIZEBOX style flag
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
                
                if (e.Location.X > Width - 14 && e.Location.Y > Height - 14 && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else
                {
                    _resizeDir = ResizeDirection.None;

                    //Only reset the cursor when needed, this prevents it from flickering when a child control changes the cursor to its own needs
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
            // Convert to client position and pass to Form.MouseMove
            var clientCursorPos = PointToClient(e.Location);
            var newE = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseMove(newE);
        }

        private void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            if (DesignMode) return;
            var oldState = _buttonState;
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;

            if (e.Button == MouseButtons.Left && !up)
            {
                if (showMin && !showMax && _maxButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.MinDown;
                else if (showMin && showMax && _minButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.MinDown;
                else if (showMax && _maxButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.MaxDown;
                else if (ControlBox && _xButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.XDown;
                else
                    _buttonState = ButtonState.None;
            }
            else
            {
                if (showMin && !showMax && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown && up)
                        WindowState = FormWindowState.Minimized;
                }
                else if (showMin && showMax && _minButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown && up)
                        WindowState = FormWindowState.Minimized;
                }
                else if (MaximizeBox && ControlBox && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MaxOver;

                    if (oldState == ButtonState.MaxDown && up)
                        MaximizeWindow(!_maximized);

                }
                else if (ControlBox && _xButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.XOver;

                    if (oldState == ButtonState.XDown && up)
                        Close();
                }
                else _buttonState = ButtonState.None;
            }

            if (oldState != _buttonState) Invalidate();
        }

        private void MaximizeWindow(bool maximize)
        {
            if (!MaximizeBox || !ControlBox) return;

            _maximized = maximize;

            if (maximize)
            {
                var monitorHandle = MonitorFromWindow(Handle, MONITOR_DEFAULTTONEAREST);
                var monitorInfo = new MONITORINFOEX();
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
                    dir = HTBOTTOMLEFT;
                    break;
                case ResizeDirection.Left:
                    dir = HTLEFT;
                    break;
                case ResizeDirection.Right:
                    dir = HTRIGHT;
                    break;
                case ResizeDirection.BottomRight:
                    dir = HTBOTTOMRIGHT;
                    break;
                case ResizeDirection.Bottom:
                    dir = HTBOTTOM;
                    break;
            }

            ReleaseCapture();
            if (dir != -1)
            {
                SendMessage(Handle, WM_NCLBUTTONDOWN, dir, 0);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _minButtonBounds = new Rectangle(Width - 25 - 25 - 25 - 5, 5, 25, 20);
            _maxButtonBounds = new Rectangle(Width - 25 - 25 - 5, 5, 25, 20);
            _xButtonBounds = new Rectangle(Width - 25 - 5, 5, 25, 20);
            _statusBarBounds = new Rectangle(0, 0, Width, STATUS_BAR_HEIGHT);
            _actionBarBounds = new Rectangle(0, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT);
        }

        protected virtual TextFormatFlags AsTextFormatFlags(ContentAlignment Alignment)
        {
            switch (Alignment)
            {
                case ContentAlignment.BottomLeft: return TextFormatFlags.Bottom | TextFormatFlags.Left;
                case ContentAlignment.BottomCenter: return TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.BottomRight: return TextFormatFlags.Bottom | TextFormatFlags.Right;
                case ContentAlignment.MiddleLeft: return TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                case ContentAlignment.MiddleCenter: return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.MiddleRight: return TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                case ContentAlignment.TopLeft: return TextFormatFlags.Top | TextFormatFlags.Left;
                case ContentAlignment.TopCenter: return TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.TopRight: return TextFormatFlags.Top | TextFormatFlags.Right;
            }
            throw new InvalidEnumArgumentException();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            g.Clear(MetroPaint.BackColor.Form(_theme));
            g.FillRectangle(new SolidBrush(_styleColor), new Rectangle(0, 0, Width, 5));
                        
            // Определите, должны ли мы вообще рисовать кнопки.
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;
            
            g.DrawImage(MetroPaint.ControlBox.Hide.Normal(_theme), new Point(Width - 25 - 25 - 25 - 5, 5));
            g.DrawImage(MetroPaint.ControlBox.Minimum.Normal(_theme), new Point(Width - 25 - 25 - 5, 5));
            g.DrawImage(MetroPaint.ControlBox.Close.Normal(_theme), new Point(Width - 25 - 5, 5));

            // Когда MaximizeButton == false, кнопка сворачивания будет закрашена на свое место
            if (_buttonState == ButtonState.MinOver && showMin)
                g.DrawImage(MetroPaint.ControlBox.Hide.Hover(_theme), new Point(Width - 25 - 25 - 25 - 5, 5));

            if (_buttonState == ButtonState.MinDown && showMin)
                g.DrawImage(MetroPaint.ControlBox.Hide.Press(_theme, _styleColor), new Point(Width - 25 - 25 - 25 - 5, 5));

            if (_buttonState == ButtonState.MaxOver && showMax)
                g.DrawImage(MetroPaint.ControlBox.Minimum.Hover(_theme), new Point(Width - 25 - 25 - 5, 5));

            if (_buttonState == ButtonState.MaxDown && showMax)
                g.DrawImage(MetroPaint.ControlBox.Minimum.Press(_theme, _styleColor), new Point(Width - 25 - 25 - 5, 5));

            if (_buttonState == ButtonState.XOver && ControlBox)
                g.DrawImage(MetroPaint.ControlBox.Close.Hover(_theme), new Point(Width - 25 - 5, 5));

            if (_buttonState == ButtonState.XDown && ControlBox)
                g.DrawImage(MetroPaint.ControlBox.Close.Press(_theme, _styleColor), new Point(Width - 25 - 5, 5));
            
            g.DrawImage(MetroPaint.ControlBox.Resize.Normal(_theme), new Point(Width - 10 - 4, Height - 10 - 4));
            
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(6, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT),
                MetroPaint.ForeColor.Form(_theme),
                MetroPaint.BackColor.Form(_theme),
                AsTextFormatFlags(_textAlign) | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);
        }
    }

    public class MouseMessageFilter : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;

        public static event MouseEventHandler MouseMove;

        public bool PreFilterMessage(ref Message m)
        {

            if (m.Msg == WM_MOUSEMOVE)
            {
                if (MouseMove != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;

                    MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            return false;
        }
    }
}
