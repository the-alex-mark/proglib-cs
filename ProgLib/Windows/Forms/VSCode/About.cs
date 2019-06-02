using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

using ProgLib.Diagnostics;

namespace ProgLib.Windows.Forms.VSCode
{
    /// <summary>
    /// Предоставляет методы для работы с окном, отображающим свойства сборки.
    /// </summary>
    public class About
    {
        /// <summary>
        /// Отображает окно со свойствами указанной сборки.
        /// </summary>
        /// <param name="Information">Свойства сборки</param>
        public static void Show(AssemblyInfo Information)
        {
            FormAbout _about = new FormAbout();
            _about.Show(Information, "", VSCodeTheme.Light, VSCodeIconTheme.Minimal);
        }

        /// <summary>
        /// Отображает окно со свойствами указанной сборки.
        /// </summary>
        /// <param name="Information">Свойства сборки</param>
        /// <param name="Link">Ссылка на сайт компании или страницу разработчика</param>
        public static void Show(AssemblyInfo Information, String Link)
        {
            FormAbout _about = new FormAbout();
            _about.Show(Information, Link, VSCodeTheme.Light, VSCodeIconTheme.Minimal);
        }

        /// <summary>
        /// Отображает окно со свойствами указанной сборки.
        /// </summary>
        /// <param name="Information">Свойства сборки</param>
        /// <param name="Link">Ссылка на сайт компании или страницу разработчика</param>
        /// <param name="Theme"></param>
        public static void Show(AssemblyInfo Information, String Link, VSCodeTheme Theme)
        {
            FormAbout _about = new FormAbout();
            _about.Show(Information, Link, Theme, VSCodeIconTheme.Minimal);
        }

        /// <summary>
        /// Отображает окно со свойствами указанной сборки.
        /// </summary>
        /// <param name="Information">Свойства сборки</param>
        /// <param name="Link">Ссылка на сайт компании или страницу разработчика</param>
        /// <param name="Theme"></param>
        /// <param name="IconTheme"></param>
        public static void Show(AssemblyInfo Information, String Link, VSCodeTheme Theme, VSCodeIconTheme IconTheme)
        {
            FormAbout _about = new FormAbout();
            _about.Show(Information, Link, Theme, IconTheme);
        }
    }

    /// <summary>
    /// Форма, показывающая указанные метаданные сборки.
    /// </summary>
    partial class FormAbout : Form
    {
        #region Designer

        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.mmClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mmMaximum = new System.Windows.Forms.ToolStripMenuItem();
            this.mmMinimum = new System.Windows.Forms.ToolStripMenuItem();
            this.mmTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.Title = new System.Windows.Forms.Label();
            this.Version = new System.Windows.Forms.Label();
            this.Copyright = new System.Windows.Forms.Label();
            this.Link = new System.Windows.Forms.LinkLabel();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mmClose,
            this.mmMaximum,
            this.mmMinimum,
            this.mmTitle});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Margin = new System.Windows.Forms.Padding(3);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(0);
            this.MainMenu.Size = new System.Drawing.Size(427, 24);
            this.MainMenu.TabIndex = 6;
            this.MainMenu.Text = "menuStrip1";
            // 
            // mmClose
            // 
            this.mmClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mmClose.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mmClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mmClose.Name = "mmClose";
            this.mmClose.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.mmClose.Size = new System.Drawing.Size(28, 24);
            // 
            // mmMaximum
            // 
            this.mmMaximum.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mmMaximum.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mmMaximum.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mmMaximum.Name = "mmMaximum";
            this.mmMaximum.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.mmMaximum.Size = new System.Drawing.Size(28, 24);
            this.mmMaximum.Visible = false;
            // 
            // mmMinimum
            // 
            this.mmMinimum.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mmMinimum.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mmMinimum.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mmMinimum.Name = "mmMinimum";
            this.mmMinimum.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.mmMinimum.Size = new System.Drawing.Size(28, 24);
            this.mmMinimum.Visible = false;
            // 
            // mmTitle
            // 
            this.mmTitle.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.mmTitle.Name = "mmTitle";
            this.mmTitle.Size = new System.Drawing.Size(80, 24);
            this.mmTitle.Text = "О программе";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Century Gothic", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Title.Location = new System.Drawing.Point(11, 45);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(274, 27);
            this.Title.TabIndex = 7;
            this.Title.Text = "Название приложения";
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Version.Location = new System.Drawing.Point(15, 71);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(97, 12);
            this.Version.TabIndex = 8;
            this.Version.Text = "Версия 2.0 сборка 2";
            // 
            // Copyright
            // 
            this.Copyright.AutoSize = true;
            this.Copyright.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Copyright.Location = new System.Drawing.Point(15, 124);
            this.Copyright.Name = "Copyright";
            this.Copyright.Size = new System.Drawing.Size(83, 12);
            this.Copyright.TabIndex = 9;
            this.Copyright.Text = "Авторские права";
            // 
            // Link
            // 
            this.Link.AutoSize = true;
            this.Link.Location = new System.Drawing.Point(15, 136);
            this.Link.Name = "Link";
            this.Link.Size = new System.Drawing.Size(58, 12);
            this.Link.TabIndex = 10;
            this.Link.TabStop = true;
            this.Link.Text = "google.com";
            this.Link.VisitedLinkColor = System.Drawing.Color.Blue;
            this.Link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            // 
            // FormAbout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(427, 169);
            this.Controls.Add(this.Link);
            this.Controls.Add(this.Copyright);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(646, 366);
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "О программе";
            this.Load += new System.EventHandler(this.FormAbout_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormAbout_KeyDown);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem mmClose;
        private System.Windows.Forms.ToolStripMenuItem mmMaximum;
        private System.Windows.Forms.ToolStripMenuItem mmMinimum;
        private System.Windows.Forms.ToolStripMenuItem mmTitle;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.Label Copyright;
        private System.Windows.Forms.LinkLabel Link;

        #endregion

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
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
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
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
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

        public FormAbout()
        {
            InitializeComponent();
            m_aeroEnabled = false;

            // Оформление MainMenu
            MainMenu.Renderer = new VSCodeToolStripRenderer(VSCodeTheme.Light);
            MainMenu.MouseDown += delegate (Object _object, MouseEventArgs _mouseEventArgs)
            {
                ReleaseCapture();
                PostMessage(Handle, 0x0112, 0xF012, 0);
            };
            MainMenu.Items["mmTitle"].MouseDown += delegate (Object _object, MouseEventArgs _mouseEventArgs)
            {
                ReleaseCapture();
                PostMessage(Handle, 0x0112, 0xF012, 0);
            };
            MainMenu.Items["mmClose"].Click += delegate (Object _object, EventArgs _eventArgs)
            {
                Close();
            };
        }

        #region Properties

        private String AssemblyLink
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обновляет цветовую тему.
        /// </summary>
        /// <param name="Theme"></param>
        /// <param name="IconTheme"></param>
        private void UTheme(VSCodeTheme Theme, VSCodeIconTheme IconTheme = VSCodeIconTheme.Minimal)
        {
            VSCodeToolStripRenderer _renderer = new VSCodeToolStripRenderer(Theme, new VSCodeToolStripSettings(this, MainMenu, IconTheme));
            MainMenu.Renderer = _renderer;

            this.BackColor = _renderer.WindowBackColor;
            this.Title.ForeColor = _renderer.DropDownMenuForeColor;
            this.Version.ForeColor = _renderer.DropDownMenuForeColor;
            this.Copyright.ForeColor = _renderer.DropDownMenuForeColor;
        }

        #endregion

        #region Shown

        /// <summary>
        /// Отображает окно с метаданными текущей сборки.
        /// </summary>
        public new void Show()
        {
            Show(AssemblyInfo.Load(Assembly.GetExecutingAssembly().Location), "", VSCodeTheme.Light);
        }

        /// <summary>
        /// Отображает окно с указанными метаданными.
        /// </summary>
        /// <param name="Information"></param>
        public void Show(AssemblyInfo Information)
        {
            Show(Information, "", VSCodeTheme.Light);
        }

        /// <summary>
        /// Отображает окно с указанными метаданными.
        /// </summary>
        /// <param name="Information"></param>
        /// <param name="Link"></param>
        public void Show(AssemblyInfo Information, String Link)
        {
            Show(Information, Link, VSCodeTheme.Light);
        }

        /// <summary>
        /// Отображает окно с указанными метаданными.
        /// </summary>
        /// <param name="Information"></param>
        /// <param name="Theme"></param>
        public void Show(AssemblyInfo Information, VSCodeTheme Theme)
        {
            Show(Information, "", Theme);
        }

        /// <summary>
        /// Отображает окно с указанными метаданными.
        /// </summary>
        /// <param name="Information"></param>
        /// <param name="Link"></param>
        /// <param name="Theme"></param>
        public void Show(AssemblyInfo Information, String Link, VSCodeTheme Theme)
        {
            UTheme(Theme);
            this.AssemblyLink = Link;

            this.Title.Text = Information.Title;
            this.Version.Text = $"Версия {Information.Version.Major}.{Information.Version.Minor} сборка {Information.FileVersion.Major}";
            this.Copyright.Text = Information.Copyright;
            this.Link.Text = Link;

            if (Link == "")
            {
                this.Copyright.Location = this.Link.Location;
                this.Link.Visible = false;
            }

            ShowDialog();
        }

        /// <summary>
        /// Отображает окно с указанными метаданными.
        /// </summary>
        /// <param name="Information"></param>
        /// <param name="Link"></param>
        /// <param name="Theme"></param>
        /// <param name="IconTheme"></param>
        public void Show(AssemblyInfo Information, String Link, VSCodeTheme Theme, VSCodeIconTheme IconTheme)
        {
            UTheme(Theme, IconTheme);
            this.AssemblyLink = Link;

            this.Title.Text = Information.Title;
            this.Version.Text = $"Версия {Information.Version.Major}.{Information.Version.Minor} сборка {Information.FileVersion.Major}";
            this.Copyright.Text = Information.Copyright;
            this.Link.Text = Link;

            if (Link == "")
            {
                this.Copyright.Location = this.Link.Location;
                this.Link.Visible = false;
            }

            ShowDialog();
        }

        #endregion
        
        private void FormAbout_Load(Object sender, EventArgs e)
        {

        }
        private void FormAbout_KeyDown(Object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;

                default: break;
            }
        }

        /// <summary>
        /// Обработчик события нажатия на ссылку.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Link_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (AssemblyLink != "")
                    Process.Start(AssemblyLink);
            }
            catch { MessageBox.Show("Отсутствует подключение к интернету.", Title.Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
