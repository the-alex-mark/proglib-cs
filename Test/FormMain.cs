using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ProgLib;
using ProgLib.Diagnostics;
using ProgLib.Audio;
using ProgLib.Audio.Visualization;
using ProgLib.Data.Access;
using ProgLib.Network;
using ProgLib.Text.Encoding.QRCode;
using ProgLib.Text.Encoding.Barcode;
using ProgLib.Windows;
using ProgLib.Windows.Forms;
using ProgLib.Windows.Forms.Material;
using ProgLib.Windows.Forms.Minimal;
using ProgLib.Windows.Forms.VSCode;

using Test.Demonstration;
using ProgLib.Text;
using ProgLib.Data.CSharp;
using ProgLib.Data.MySql;
using MySql.Data.MySqlClient;

namespace Test
{
    public partial class FormMain : Form
    {
        #region Import

        [System.Runtime.InteropServices.DllImport("USER32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private extern static Boolean PostMessage(IntPtr hWnd, uint Msg, uint WParam, uint LParam);

        [System.Runtime.InteropServices.DllImport("USER32", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private extern static Boolean ReleaseCapture();

        #endregion

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
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message e)
        {
            switch (e.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 0,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;

                //case 70:
                //    Rectangle WorkingArea = SystemInformation.WorkingArea;
                //    Rectangle Rectangle = (Rectangle)System.Runtime.InteropServices.Marshal.PtrToStructure((IntPtr)((long)(IntPtr.Size * 2) + e.LParam.ToInt64()), typeof(Rectangle));
                //    if (Rectangle.X <= WorkingArea.Left + 10)
                //        System.Runtime.InteropServices.Marshal.WriteInt32(e.LParam, IntPtr.Size * 2, WorkingArea.Left);
                //    if (Rectangle.X + Rectangle.Width >= WorkingArea.Width - 10)
                //        System.Runtime.InteropServices.Marshal.WriteInt32(e.LParam, IntPtr.Size * 2, WorkingArea.Right - Rectangle.Width);
                //    if (Rectangle.Y <= WorkingArea.Top + 10)
                //        System.Runtime.InteropServices.Marshal.WriteInt32(e.LParam, IntPtr.Size * 2 + 4, WorkingArea.Top);
                //    if (Rectangle.Y + Rectangle.Height >= WorkingArea.Height - 10)
                //        System.Runtime.InteropServices.Marshal.WriteInt32(e.LParam, IntPtr.Size * 2 + 4, WorkingArea.Bottom - Rectangle.Height);
                //    break;

                default: break;
            }

            base.WndProc(ref e);
        }
        
        #endregion
        
        public FormMain()
        {
            InitializeComponent();
            m_aeroEnabled = true;
            
            // Оформление MainMenu
            MainMenu.MouseDown += delegate (Object _object, MouseEventArgs _mouseEventArgs)
            {
                ReleaseCapture();
                PostMessage(Handle, 0x0112, 0xF012, 0);
            };
            MainMenu.Items["mmMinimum"].Click += delegate (Object _object, EventArgs _eventArgs)
            {
                WindowState = FormWindowState.Minimized;
            };
            MainMenu.Items["mmMaximum"].Click += delegate (Object _object, EventArgs _eventArgs)
            {
                WindowState = (WindowState == FormWindowState.Maximized)
                    ? FormWindowState.Normal
                    : FormWindowState.Maximized;

                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            };
            MainMenu.Items["mmClose"].Click += delegate (Object _object, EventArgs _eventArgs)
            {
                Close();
            };
        }

        #region Variables
        
        private VSCodeTheme _vsCodeTheme;
        private VSCodeIconTheme _vsCodeIconTheme;
        private FormWindowState _windowState;

        #endregion

        #region Methods

        /// <summary>
        /// Обновляет цветовую тему.
        /// </summary>
        /// <param name="Theme"></param>
        private void UTheme(VSCodeTheme Theme, VSCodeIconTheme IconTheme)
        {
            VSCodeToolStripRenderer _renderer = new VSCodeToolStripRenderer(Theme, new VSCodeToolStripSettings(this, MainMenu, IconTheme));
            MainMenu.Renderer = _renderer;
            contextMenuStrip1.Renderer = _renderer;
            vsCodeTabSelector1.Theme = Theme;

            BackColor = _renderer.WindowBackColor;
            SideBar.BackColor = _renderer.SidebarBackColor;
        }

        #endregion

        #region Menu

        // Вид
        private void mFullScreen_Click(Object sender, EventArgs e)
        {
            // Установка максимального размера завёртывания формы
            switch (mFullScreen.Text)
            {
                case "Включить полноэкранный режим":
                    _windowState = this.WindowState;
                    this.WindowState = FormWindowState.Normal;
                    this.MaximizedBounds = Screen.FromHandle(this.Handle).Bounds;
                    this.WindowState = FormWindowState.Maximized;

                    mFullScreen.Text = "Выключить полноэкранный режим";
                    break;

                case "Выключить полноэкранный режим":
                    this.WindowState = FormWindowState.Normal;
                    this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                    this.WindowState = _windowState;

                    mFullScreen.Text = "Включить полноэкранный режим";
                    break;
            }
        }

        private void mThemeLight_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.Light;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeQuietLight_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.QuietLight;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeSolarizedLight_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.SolarizedLight;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeAbyss_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.Abyss;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeDark_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.Dark;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeKimbieDark_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.KimbieDark;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeMonokai_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.Monokai;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeMonokaiDimmed_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.MonokaiDimmed;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeRed_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.Red;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeSolarizedDark_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.SolarizedDark;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mThemeTomorrowNightBlue_Click(Object sender, EventArgs e)
        {
            _vsCodeTheme = VSCodeTheme.TomorrowNightBlue;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }

        private void mIconThemeClassic_Click(Object sender, EventArgs e)
        {
            _vsCodeIconTheme = VSCodeIconTheme.Classic;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }
        private void mIconThemeMinimal_Click(Object sender, EventArgs e)
        {
            _vsCodeIconTheme = VSCodeIconTheme.Minimal;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }

        // Окна
        private void mMaterialSkin_Click(Object sender, EventArgs e)
        {
            Form_MaterialSkin MF = new Form_MaterialSkin();
            MF.ShowDialog();
        }

        #endregion

        private void Form1_Load(Object sender, EventArgs e)
        {
            // Установка максимального размера развёртывания формы
            MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            _windowState = this.WindowState;

            // Обновление цветовой темы
            _vsCodeTheme = VSCodeTheme.Red;
            _vsCodeIconTheme = VSCodeIconTheme.Minimal;
            UTheme(_vsCodeTheme, _vsCodeIconTheme);
        }

        private void mIncreaseScale_Click(Object sender, EventArgs e)
        {
            foreach (ToolStripItem Item in MainMenu.Items)
                Item.Font = new Font(Item.Font.FontFamily.Name, Item.Font.Size + 2, Item.Font.Style);
        }

        private void mReduceScale_Click(Object sender, EventArgs e)
        {
            foreach (ToolStripItem Item in MainMenu.Items)
                Item.Font = new Font(Item.Font.FontFamily.Name, Item.Font.Size - 2, Item.Font.Style);
        }

        private void mResetScale_Click(Object sender, EventArgs e)
        {
            foreach (ToolStripItem Item in MainMenu.Items)
                Item.Font = new Font(Item.Font.FontFamily.Name, 7.5F, Item.Font.Style);
        }

        private void mTextCodeControl_Click(Object sender, EventArgs e)
        {
            Form_TextCode F_TC = new Form_TextCode();
            F_TC.ShowDialog();
        }
    }
}
