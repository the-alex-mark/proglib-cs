using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public class MinimalTabControl : System.Windows.Forms.TabControl
    {
        public MinimalTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // Double buffering
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            bUpDown = false;

            this.ControlAdded += new ControlEventHandler(FlatTabControl_ControlAdded);
            this.ControlRemoved += new ControlEventHandler(FlatTabControl_ControlRemoved);
            this.SelectedIndexChanged += new EventHandler(FlatTabControl_SelectedIndexChanged);

            BackgroundColor = SystemColors.Control;
            BorderColor = SystemColors.ControlDark;
            RoundTabPage = true;
        }

        #region Variables
        
        private Container components = null;
        private SubClass scUpDown = null;
        private Boolean bUpDown; // правда, когда кнопка счетчик не требуется
        private const Int32 nMargin = 5;

        private Color _backgroundColor;
        private Color _borderColor;
        private Boolean _roundTabPage;

        #endregion

        #region Properties

        [Editor(typeof(TabpageExCollectionEditor), typeof(UITypeEditor))]
        public new TabPageCollection TabPages
        {
            get { return base.TabPages; }
        }

        [Browsable(true), Category("Appearance"), Description("Положение вкладок.")]
        new public TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                if ((value != TabAlignment.Top) && (value != TabAlignment.Bottom))
                    value = TabAlignment.Top;

                base.Alignment = value;
            }
        }

        [Browsable(false), Category("Appearance"), Description("")]
        new public Boolean Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = false; }
        }

        [Browsable(true), Category("Appearance"), Description("Фоновый цвет.")]
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; this.Invalidate(); }
        }

        [Browsable(true), Category("Appearance"), Description("Цвет границ.")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Appearance"), Description("Вид отображения вкладок.")]
        public Boolean RoundTabPage
        {
            get { return _roundTabPage; }
            set { _roundTabPage = value; Invalidate(); }
        }

        #endregion

        #region Methods

        #region Обработчик событий "WndProc"

        private Int32 scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_PAINT:
                    {
                        // Перерисовывка
                        IntPtr hDC = Win32.GetWindowDC(scUpDown.Handle);
                        Graphics g = Graphics.FromHdc(hDC);

                        DrawIcons(g);

                        g.Dispose();
                        Win32.ReleaseDC(scUpDown.Handle, hDC);

                        // return 0 (processed)
                        m.Result = IntPtr.Zero;

                        // Проверка currentrect
                        Rectangle rect = new Rectangle();
                        Win32.GetClientRect(scUpDown.Handle, ref rect);
                        Win32.ValidateRect(scUpDown.Handle, ref rect);
                    }
                    return 1;
            }

            return 0;
        }

        #endregion

        #region Сгенерированный код конструктора компонентов

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }

        #endregion

        #region Класс "TabpageExCollectionEditor"

        internal class TabpageExCollectionEditor : CollectionEditor
        {
            public TabpageExCollectionEditor(System.Type type) : base(type) { }

            protected override Type CreateCollectionItemType()
            {
                return typeof(TabPage);
            }
        }

        #endregion

        internal void DrawTab(Graphics G, TabPage TabPage, Int32 nIndex)
        {
            Rectangle recBounds = GetTabRect(nIndex);
            RectangleF tabTextArea = (RectangleF)this.GetTabRect(nIndex);

            Point[] Lines = new Point[7];
            if (Alignment == TabAlignment.Top)
            {
                Lines[0] = new Point(recBounds.Left, recBounds.Bottom);
                Lines[1] = new Point(recBounds.Left, recBounds.Top + 3);
                Lines[2] = new Point(recBounds.Left + 3, recBounds.Top);
                Lines[3] = new Point(recBounds.Right - 3, recBounds.Top);
                Lines[4] = new Point(recBounds.Right, recBounds.Top + 3);
                Lines[5] = new Point(recBounds.Right, recBounds.Bottom);
                Lines[6] = new Point(recBounds.Left, recBounds.Bottom);
            }
            else
            {
                Lines[0] = new Point(recBounds.Left, recBounds.Top);
                Lines[1] = new Point(recBounds.Right, recBounds.Top);
                Lines[2] = new Point(recBounds.Right, recBounds.Bottom - 3);
                Lines[3] = new Point(recBounds.Right - 3, recBounds.Bottom);
                Lines[4] = new Point(recBounds.Left + 3, recBounds.Bottom);
                Lines[5] = new Point(recBounds.Left, recBounds.Bottom - 3);
                Lines[6] = new Point(recBounds.Left, recBounds.Top);
            }

            // Заполнение вкладки цветом фона
            G.FillPolygon(new SolidBrush(TabPage.BackColor), Lines);

            // Отрисовка границы
            if (RoundTabPage) G.DrawPolygon(new Pen(BorderColor), Lines);
            else G.DrawRectangle(new Pen(BorderColor), recBounds);

            if ((SelectedIndex == nIndex))
            {
                // Очистка нижних строк
                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        G.DrawLine(new Pen(TabPage.BackColor), recBounds.Left + 1, recBounds.Bottom, recBounds.Right - 1, recBounds.Bottom);
                        G.DrawLine(new Pen(TabPage.BackColor), recBounds.Left + 1, recBounds.Bottom + 1, recBounds.Right - 1, recBounds.Bottom + 1);
                        break;

                    case TabAlignment.Bottom:
                        G.DrawLine(new Pen(TabPage.BackColor), recBounds.Left + 1, recBounds.Top, recBounds.Right - 1, recBounds.Top);
                        G.DrawLine(new Pen(TabPage.BackColor), recBounds.Left + 1, recBounds.Top - 1, recBounds.Right - 1, recBounds.Top - 1);
                        G.DrawLine(new Pen(TabPage.BackColor), recBounds.Left + 1, recBounds.Top - 2, recBounds.Right - 1, recBounds.Top - 2);
                        break;
                }
            }

            // Отрисовка иконки вкладки
            if ((TabPage.ImageIndex >= 0) && (ImageList != null) && (ImageList.Images[TabPage.ImageIndex] != null))
            {
                Int32 nLeftMargin = 8;
                Int32 nRightMargin = 2;

                Image img = ImageList.Images[TabPage.ImageIndex];

                Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 1, img.Width, img.Height);

                // Настройка прямоугольников
                float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

                rimage.Y += (recBounds.Height - img.Height) / 2;
                tabTextArea.X += nAdj;
                tabTextArea.Width -= nAdj;

                // Отрисовка иконки
                G.DrawImage(img, rimage);
            }

            // Отрисовка названия вкладки
            //G.DrawString(TabPage.Text, Font, new SolidBrush(TabPage.ForeColor), tabTextArea, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            // Отрисовка названия вкладки
            TextRenderer.DrawText(
                G,
                TabPage.Text,
                Font,
                new Rectangle((int)tabTextArea.X, (int)tabTextArea.Y, (int)tabTextArea.Width, (int)tabTextArea.Height),
                TabPage.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);
        }

        private Image DrawIcon(String Type, Boolean Enabled)
        {
            Bitmap Icon = new Bitmap(18, 16);

            using (Graphics G = Graphics.FromImage(Icon))
            {
                switch (Type)
                {
                    case "Left":

                        G.Clear(_backgroundColor);

                        if (!Enabled)
                        {
                            Point[] Lines =
                            {
                                new Point(11, 4),
                                new Point(7, 8),
                                new Point(11, 12)
                            };
                            G.DrawPolygon(new Pen(_borderColor), Lines);
                        }
                        else
                        {
                            Point[] Lines =
                            {
                                new Point(11, 3),
                                new Point(6, 8),
                                new Point(11, 13)
                            };
                            G.FillPolygon(new SolidBrush(_borderColor), Lines);
                        }

                        break;

                    case "Right":

                        G.Clear(_backgroundColor);

                        if (!Enabled)
                        {
                            Point[] Lines =
                            {
                                new Point(7, 4),
                                new Point(11, 8),
                                new Point(7, 12)
                            };
                            G.DrawPolygon(new Pen(_borderColor), Lines);
                        }
                        else
                        {
                            Point[] Lines =
                            {
                                new Point(7, 3),
                                new Point(12, 8),
                                new Point(7, 13)
                            };
                            G.FillPolygon(new SolidBrush(_borderColor), Lines);
                        }

                        break;
                }
            }

            return Icon;
        }

        internal void DrawIcons(Graphics g)
        {
            // позиции calc
            Rectangle TabControlArea = this.ClientRectangle;

            Rectangle r0 = new Rectangle();
            Win32.GetClientRect(scUpDown.Handle, ref r0);

            Brush br = new SolidBrush(_backgroundColor);
            g.FillRectangle(br, r0);
            br.Dispose();

            Rectangle rborder = new Rectangle(r0.X, r0.Y, r0.Width - 1, r0.Height);
            rborder.Inflate(-1, -1);
            g.DrawRectangle(new Pen(BorderColor), rborder);

            int nMiddle = (r0.Width / 2);
            int nTop = (r0.Height - 16) / 2;
            int nLeft = (nMiddle - 16) / 2;

            Rectangle r1 = new Rectangle(nLeft, nTop, 16, 16);
            Rectangle r2 = new Rectangle(nMiddle + nLeft, nTop, 16, 16);
            //----------------------------

            //----------------------------
            // draw buttons
            g.SmoothingMode = SmoothingMode.None;
            Image img = DrawIcon("Left", true);
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(0);
                    if (r3.Left < TabControlArea.Left)
                        g.DrawImage(img, r1);
                    else
                    {
                        img = DrawIcon("Left", false);
                        if (img != null)
                            g.DrawImage(img, r1);
                    }
                }
            }

            img = DrawIcon("Right", true);
            if (img != null)
            {
                if (this.TabCount > 0)
                {
                    Rectangle r3 = this.GetTabRect(this.TabCount - 1);
                    if (r3.Right > (TabControlArea.Width - r0.Width))
                        g.DrawImage(img, r2);
                    else
                    {
                        img = DrawIcon("Right", false);
                        if (img != null)
                            g.DrawImage(img, r2);
                    }
                }
            }
            //----------------------------
        }

        private void FindUpDown()
        {
            bool bFound = false;

            // Поиск элемена управления UpDown
            IntPtr pWnd = Win32.GetWindow(this.Handle, Win32.GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                // Получение имени класса окна 
                Char[] className = new Char[33];
                Int32 length = Win32.GetClassName(pWnd, className, 32);
                String s = new String(className, 0, length);

                if (s == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        this.scUpDown = new SubClass(pWnd, true);
                        this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);

                        bUpDown = true;
                    }
                    break;
                }

                pWnd = Win32.GetWindow(pWnd, Win32.GW_HWNDNEXT);
            }

            if ((!bFound) && (bUpDown))
                bUpDown = false;
        }

        private void UpdateUpDown()
        {
            if (bUpDown)
            {
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rect = new Rectangle();

                    Win32.GetClientRect(scUpDown.Handle, ref rect);
                    Win32.InvalidateRect(scUpDown.Handle, ref rect, true);
                }
            }
        }

        #endregion

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            FindUpDown();
        }

        private void FlatTabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            FindUpDown();
            UpdateUpDown();
        }

        private void FlatTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUpDown();
            Invalidate();   // we need to update border and background colors
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!Visible) return;

            Rectangle _tabControlArea = this.ClientRectangle;
            Rectangle _tabArea = this.DisplayRectangle;
            
            // Заполнение клиентской области
            e.Graphics.FillRectangle(new SolidBrush(_backgroundColor), _tabControlArea);
            
            // Отрисовка границ
            _tabArea.Inflate(SystemInformation.Border3DSize.Width, SystemInformation.Border3DSize.Width);
            e.Graphics.DrawRectangle(new Pen(BorderColor), _tabArea);
            
            // Область зажима для отрисовки вкладок
            Int32 nWidth = _tabArea.Width + nMargin;
            if (bUpDown)
            {
                // Исключение элемента управления обновлениями для рисования
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    Win32.GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);

                    nWidth = rupdown2.X;
                }
            }
            e.Graphics.SetClip(new Rectangle(_tabArea.Left, _tabControlArea.Top, nWidth - nMargin, _tabControlArea.Height));

            // Отрисовка вкладок
            for (int i = 0; i < this.TabCount; i++) DrawTab(e.Graphics, this.TabPages[i], i);
            
            // Отрисовка фона для покрытия плоских пограничных областей
            if (this.SelectedTab != null)
            {
                TabPage _tabPage = this.SelectedTab;

                _tabArea.Offset(1, 1);
                _tabArea.Width -= 2;
                _tabArea.Height -= 2;

                e.Graphics.DrawRectangle(new Pen(_tabPage.BackColor), _tabArea);
                _tabArea.Width -= 1;
                _tabArea.Height -= 1;
                e.Graphics.DrawRectangle(new Pen(_tabPage.BackColor), _tabArea);
            }
        }

        /// <summary> 
        /// Освобождает все ресурсы, используемые текущим экземпляром класса <see cref="MinimalTabControl"/>.
        /// </summary>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                    components.Dispose();
            }

            base.Dispose(Disposing);
        }
    }
}
