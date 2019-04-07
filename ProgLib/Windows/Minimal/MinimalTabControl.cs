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

namespace ProgLib.Windows.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public class MinimalTabControl : System.Windows.Forms.TabControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private SubClass scUpDown = null;
        private Boolean bUpDown; // правда, когда кнопка счетчик не требуется
        private const Int32 nMargin = 5;

        private Color _BackgroundColor;
        private Color _BorderColor;
        private Boolean _RoundTabPage;

        public MinimalTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // double buffering
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
            BorderColor = Color.FromArgb(64, 64, 64);
            RoundTabPage = true;
        }

        /// <summary> 
        /// Очистите все используемые ресурсы.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawControl(e.Graphics);
        }

        internal void DrawControl(Graphics g)
        {
            if (!Visible)
                return;



            Rectangle TabControlArea = this.ClientRectangle;
            Rectangle TabArea = this.DisplayRectangle;

            //----------------------------
            // заполнить клиентскую область
            g.FillRectangle(new SolidBrush(_BackgroundColor), TabControlArea);
            //----------------------------

            //----------------------------
            // рисовать границу
            int nDelta = SystemInformation.Border3DSize.Width;

            Pen border = new Pen(BorderColor);
            TabArea.Inflate(nDelta, nDelta);
            g.DrawRectangle(border, TabArea);
            border.Dispose();
            //----------------------------


            //----------------------------
            // область зажима для рисования вкладок
            Region rsaved = g.Clip;
            Rectangle rreg;

            int nWidth = TabArea.Width + nMargin;
            if (bUpDown)
            {
                // исключить элемент управления обновлениями для рисования
                if (Win32.IsWindowVisible(scUpDown.Handle))
                {
                    Rectangle rupdown = new Rectangle();
                    Win32.GetWindowRect(scUpDown.Handle, ref rupdown);
                    Rectangle rupdown2 = this.RectangleToClient(rupdown);

                    nWidth = rupdown2.X;
                }
            }

            rreg = new Rectangle(TabArea.Left, TabControlArea.Top, nWidth - nMargin, TabControlArea.Height);

            g.SetClip(rreg);

            // рисовать вкладки
            for (int i = 0; i < this.TabCount; i++)
                DrawTab(g, this.TabPages[i], i);

            g.Clip = rsaved;
            //----------------------------


            //----------------------------
            // рисовать фон для покрытия плоских пограничных областей
            if (this.SelectedTab != null)
            {
                TabPage tabPage = this.SelectedTab;
                Color color = tabPage.BackColor;
                border = new Pen(color);

                TabArea.Offset(1, 1);
                TabArea.Width -= 2;
                TabArea.Height -= 2;

                g.DrawRectangle(border, TabArea);
                TabArea.Width -= 1;
                TabArea.Height -= 1;
                g.DrawRectangle(border, TabArea);

                border.Dispose();
            }
            //----------------------------
        }

        internal void DrawTab(Graphics G, TabPage tabPage, int nIndex)
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

            //----------------------------
            // заполните эту вкладку цветом фона
            G.FillPolygon(new SolidBrush(tabPage.BackColor), Lines);
            //----------------------------

            //----------------------------
            // рисовать границу
            if (RoundTabPage) G.DrawPolygon(new Pen(BorderColor), Lines);
            else G.DrawRectangle(new Pen(BorderColor), recBounds);

            if ((SelectedIndex == nIndex))
            {
                //----------------------------
                // clear bottom lines
                Pen pen = new Pen(tabPage.BackColor);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        G.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom, recBounds.Right - 1, recBounds.Bottom);
                        G.DrawLine(pen, recBounds.Left + 1, recBounds.Bottom + 1, recBounds.Right - 1, recBounds.Bottom + 1);
                        break;

                    case TabAlignment.Bottom:
                        G.DrawLine(pen, recBounds.Left + 1, recBounds.Top, recBounds.Right - 1, recBounds.Top);
                        G.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 1, recBounds.Right - 1, recBounds.Top - 1);
                        G.DrawLine(pen, recBounds.Left + 1, recBounds.Top - 2, recBounds.Right - 1, recBounds.Top - 2);
                        break;
                }

                pen.Dispose();
                //----------------------------
            }
            //----------------------------

            //----------------------------
            // draw tab's icon
            if ((tabPage.ImageIndex >= 0) && (ImageList != null) && (ImageList.Images[tabPage.ImageIndex] != null))
            {
                int nLeftMargin = 8;
                int nRightMargin = 2;

                Image img = ImageList.Images[tabPage.ImageIndex];

                Rectangle rimage = new Rectangle(recBounds.X + nLeftMargin, recBounds.Y + 1, img.Width, img.Height);

                // adjust rectangles
                float nAdj = (float)(nLeftMargin + img.Width + nRightMargin);

                rimage.Y += (recBounds.Height - img.Height) / 2;
                tabTextArea.X += nAdj;
                tabTextArea.Width -= nAdj;

                // draw icon
                G.DrawImage(img, rimage);
            }
            //----------------------------

            //----------------------------
            // draw string
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            G.DrawString(tabPage.Text, Font, new SolidBrush(tabPage.ForeColor), tabTextArea, stringFormat);
            //----------------------------
        }

        private Image DrawIcon(String Type, Boolean Enabled)
        {
            Bitmap Icon = new Bitmap(18, 16);

            using (Graphics G = Graphics.FromImage(Icon))
            {
                switch (Type)
                {
                    case "Left":

                        G.Clear(_BackgroundColor);

                        if (!Enabled)
                        {
                            Point[] Lines =
                            {
                                new Point(11, 4),
                                new Point(7, 8),
                                new Point(11, 12)
                            };
                            G.DrawPolygon(new Pen(_BorderColor), Lines);
                        }
                        else
                        {
                            Point[] Lines =
                            {
                                new Point(11, 3),
                                new Point(6, 8),
                                new Point(11, 13)
                            };
                            G.FillPolygon(new SolidBrush(_BorderColor), Lines);
                        }

                        break;

                    case "Right":

                        G.Clear(_BackgroundColor);

                        if (!Enabled)
                        {
                            Point[] Lines =
                            {
                                new Point(7, 4),
                                new Point(11, 8),
                                new Point(7, 12)
                            };
                            G.DrawPolygon(new Pen(_BorderColor), Lines);
                        }
                        else
                        {
                            Point[] Lines =
                            {
                                new Point(7, 3),
                                new Point(12, 8),
                                new Point(7, 13)
                            };
                            G.FillPolygon(new SolidBrush(_BorderColor), Lines);
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

            Brush br = new SolidBrush(_BackgroundColor);
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

        private void FindUpDown()
        {
            bool bFound = false;

            // find the UpDown control
            IntPtr pWnd = Win32.GetWindow(this.Handle, Win32.GW_CHILD);

            while (pWnd != IntPtr.Zero)
            {
                //----------------------------
                // Get the window class name
                char[] className = new char[33];

                int length = Win32.GetClassName(pWnd, className, 32);

                string s = new string(className, 0, length);
                //----------------------------

                if (s == "msctls_updown32")
                {
                    bFound = true;

                    if (!bUpDown)
                    {
                        //----------------------------
                        // Subclass it
                        this.scUpDown = new SubClass(pWnd, true);
                        this.scUpDown.SubClassedWndProc += new SubClass.SubClassWndProcEventHandler(scUpDown_SubClassedWndProc);
                        //----------------------------

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

        #region scUpDown_SubClassedWndProc Event Handler

        private Int32 scUpDown_SubClassedWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32.WM_PAINT:
                    {
                        //------------------------
                        // redraw
                        IntPtr hDC = Win32.GetWindowDC(scUpDown.Handle);
                        Graphics g = Graphics.FromHdc(hDC);

                        DrawIcons(g);

                        g.Dispose();
                        Win32.ReleaseDC(scUpDown.Handle, hDC);
                        //------------------------

                        // return 0 (processed)
                        m.Result = IntPtr.Zero;

                        //------------------------
                        // validate current rect
                        Rectangle rect = new Rectangle();

                        Win32.GetClientRect(scUpDown.Handle, ref rect);
                        Win32.ValidateRect(scUpDown.Handle, ref rect);
                        //------------------------
                    }
                    return 1;
            }

            return 0;
        }
        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }

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

        [Browsable(true), Category("Appearance"), Description("Фоновый цвет компонента.")]
        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; this.Invalidate(); }
        }

        [Browsable(true), Category("Appearance"), Description("Цвет границ компонента.")]
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; Invalidate(); }
        }

        [Browsable(true), Category("Appearance"), Description("Вид отображения вкладок.")]
        public Boolean RoundTabPage
        {
            get { return _RoundTabPage; }
            set { _RoundTabPage = value; Invalidate(); }
        }

        #endregion

        #region TabpageExCollectionEditor

        internal class TabpageExCollectionEditor : CollectionEditor
        {
            public TabpageExCollectionEditor(System.Type type) : base(type)
            {
            }

            protected override Type CreateCollectionItemType()
            {
                return typeof(TabPage);
            }
        }

        #endregion
    }

}
