using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ProgLib.Windows
{
    public class CustomForm : Form
    {
        public CustomForm()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            Adhesion = true;
            ControlBox = MinimizeBox = MaximizeBox = true;
            Sizable = true;
            _buttonState = ControlBoxButtonState.None;
            
            _animation = false;
            _statusBarColor = Color.Silver;
            _styleColor = Drawing.MetroColors.Blue;
            _border = true;
        }

        private readonly Int32 _statusBarHeight = 24;
        private Rectangle _statusBarBounds, _minimizeBounds, _maximizeBounds, _closeBounds;
        private ControlBoxButton _button;
        private ControlBoxButtonState _buttonState;
        private readonly Cursor[] _cursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        private Color _statusBarColor, _styleColor;
        private Boolean _animation, _border;

        [Category("Minimal Appearance"), Description("Цвет StatusBar формы.")]
        public Color StatusBarColor
        {
            get { return _statusBarColor; }
            set
            {
                _statusBarColor = value;
                Invalidate();
            }
        }

        [Category("Minimal Appearance"), Description("Цвет оформления.")]
        public Color StyleColor
        {
            get { return _styleColor; }
            set
            {
                _styleColor = value;
                Invalidate();
            }
        }

        [Category("Minimal Appearance"), Description("Отображение границ формы.")]
        public Boolean Border
        {
            get { return _border; }
            set
            {
                _border = value;
                Invalidate();
            }
        }

        [Category("Minimal Appearance"), Description("Отображение анимации.")]
        public Boolean Animation
        {
            get { return _animation; }
            set
            {
                _animation = value;
                Invalidate();
            }
        }
        
        [Category("Minimal Appearance"), Description("Возможность растяжения формы.")]
        public Boolean Sizable
        {
            get;
            set;
        }

        [Category("Minimal Appearance"), Description("Прилипание формы к границам экрана.")]
        public Boolean Adhesion
        {
            get;
            set;
        }
        
        public Image ResizeIcon(Color StyleColor)
        {
            Bitmap Resize = new Bitmap(10, 10);
            using (Graphics G = Graphics.FromImage(Resize))
            {
                G.Clear(BackColor);

                G.FillRectangle(new SolidBrush(StyleColor), new Rectangle(8, 0, 2, 2));
                G.FillRectangle(new SolidBrush(StyleColor), new Rectangle(4, 4, 2, 2));
                G.FillRectangle(new SolidBrush(StyleColor), new Rectangle(8, 4, 2, 2));
                G.FillRectangle(new SolidBrush(StyleColor), new Rectangle(0, 8, 2, 2));
                G.FillRectangle(new SolidBrush(StyleColor), new Rectangle(4, 8, 2, 2));
                G.FillRectangle(new SolidBrush(StyleColor), new Rectangle(8, 8, 2, 2));
            }

            return Resize;
        }
        private void UpdateButtons(ControlBoxButtonState State, MouseEventArgs e)
            {
                // Определение положения управляющих кнопок
                Rectangle
                    _minimizeBoundsBase = new Rectangle(Width - 57, 4, 16, 16),
                    _maximizeBoundsBase = new Rectangle(Width - 39, 4, 16, 16),
                    _closeBoundsBase = new Rectangle(Width - 21, 4, 16, 16);

                    _minimizeBounds = (MaximizeBox) ? _minimizeBoundsBase : _maximizeBoundsBase;
                    _maximizeBounds = _maximizeBoundsBase;
                    _closeBounds = _closeBoundsBase;

            _statusBarBounds = new Rectangle(0, 0, Width - 1, _statusBarHeight);

            // Определение активной управляющей кнопки при перемещении мыши
            if (_minimizeBounds.Contains(e.Location)) _button = ControlBoxButton.Minimize;
            if (_maximizeBounds.Contains(e.Location)) _button = ControlBoxButton.Maximize;
            if (_closeBounds.Contains(e.Location)) _button = ControlBoxButton.Close;

            // Определение действий управляющих кнопок
            _buttonState = (State == ControlBoxButtonState.Down) ? ControlBoxButtonState.Down : ControlBoxButtonState.None;

            // Установка событий управляющих кнопок
            if (ControlBox)
            {
                if (MinimizeBox && State == ControlBoxButtonState.Down && _button == ControlBoxButton.Minimize)
                {
                    if (Animation)
                        Window.Hide(this, 5);
                    else WindowState = FormWindowState.Minimized;
                }
                if (MaximizeBox && State == ControlBoxButtonState.Down && _button == ControlBoxButton.Maximize)
                {
                    WindowState = FormWindowState.Maximized;
                }
                if (State == ControlBoxButtonState.Down && _button == ControlBoxButton.Close)
                {
                    if (Animation)
                        Window.Close(this, 5);
                    else Close();
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Window.Show(this, 5);
            base.OnLoad(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Определение положения управляющих кнопок
            Rectangle
                _minimizeBoundsBase = new Rectangle(Width - 57, 4, 16, 16),
                _maximizeBoundsBase = new Rectangle(Width - 39, 4, 16, 16),
                _closeBoundsBase = new Rectangle(Width - 21, 4, 16, 16);

                _minimizeBounds = (MaximizeBox) ? _minimizeBoundsBase : _maximizeBoundsBase;
                _maximizeBounds = _maximizeBoundsBase;
                _closeBounds = _closeBoundsBase;

            _statusBarBounds = new Rectangle(0, 0, Width - 1, _statusBarHeight);
            Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            UpdateButtons(ControlBoxButtonState.None, e);

            if (Sizable)
            {
                if (e.Location.X > Width - 14 && e.Location.Y > Height - 14)
                {
                    Cursor = Cursors.SizeNWSE;
                }
                else { if (_cursors.Contains(Cursor)) Cursor = Cursors.Default; }
            }

            if (e.Button == MouseButtons.Left)
            {
                if (_statusBarBounds.Contains(e.Location))
                    Window.Move(this);

                if (Sizable)
                {
                    if (e.Location.X > Width - 14 && e.Location.Y > Height - 14)
                        Window.Resize(this, Direction.BottomRight);
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
                UpdateButtons(ControlBoxButtonState.Down, e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            
            // StatusBar
            e.Graphics.FillRectangle(new SolidBrush(_statusBarColor), new Rectangle(0, 0, Width - 1, _statusBarHeight));
            
            // Buttons
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (ControlBox)
            {
                if (MinimizeBox)
                    e.Graphics.DrawEllipse(new Pen(_styleColor, 1), new Rectangle(_minimizeBounds.X + 3, _minimizeBounds.Y + 3, 10, 10));
                if (MaximizeBox)
                    e.Graphics.DrawEllipse(new Pen(_styleColor, 1), new Rectangle(_maximizeBounds.X + 3, _maximizeBounds.Y + 3, 10, 10));

                e.Graphics.DrawEllipse(new Pen(_styleColor, 1), new Rectangle(_closeBounds.X + 3, _closeBounds.Y + 3, 10, 10));
            }

            // Header
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(2, 0, Width - 70, _statusBarHeight),
                ForeColor,
                _statusBarColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);

            // Resize
            if (Sizable)
                e.Graphics.DrawImage(ResizeIcon(_styleColor), new Point(Width - 10 - 4, Height - 10 - 4));

            // Border
            if (Border)
                e.Graphics.DrawRectangle(new Pen(_styleColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle |= 0x02000000 | 0x20000 | 0x00080000;
                return CP;
            }
        }
        protected override void WndProc(ref Message e)
        {
            if (Adhesion)
            {
                if (e.Msg == 70)
                {
                    Rectangle WorkingArea = SystemInformation.WorkingArea;
                    Rectangle Rectangle = (Rectangle)Marshal.PtrToStructure((IntPtr)((long)(IntPtr.Size * 2) + e.LParam.ToInt64()), typeof(Rectangle));
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
            }
        }
    }
}