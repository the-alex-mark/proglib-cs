using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.VSCode
{
    public class VSCodeToolStripRenderer : ToolStripRenderer
    {
        public VSCodeToolStripRenderer()
        {
            SetTheme(VSCodeTheme.Light);
            _vsCodeSettings = null;
        }

        public VSCodeToolStripRenderer(VSCodeTheme Theme)
        {
            SetTheme(Theme);
            _vsCodeSettings = null;
        }

        public VSCodeToolStripRenderer(VSCodeTheme Theme, VSCodeToolStripSettings Settings)
        {
            SetTheme(Theme);
            SetMenu(Settings.Menu);
            _vsCodeSettings = Settings;
        }
        
        #region Variables
        
        private VSCodeTheme _vsCodeTheme;
        private VSCodeToolStripSettings _vsCodeSettings;

        #endregion

        #region Properties

        public Color BackColor
        {
            get;
            private set;
        }
        public Color ForeColor
        {
            get;
            private set;
        }
        public Color DisabledForeColor
        {
            get;
            private set;
        }
        public Color SelectColor
        {
            get;
            private set;
        }

        public Color DropDownMenuBackColor
        {
            get;
            private set;
        }
        public Color DropDownMenuForeColor
        {
            get;
            private set;
        }
        public Color DropDownMenuSelectForeColor
        {
            get;
            private set;
        }
        public Color DropDownMenuSelectColor
        {
            get;
            private set;
        }

        public Color SeparatorColor
        {
            get;
            private set;
        }
        public Color CheckColor
        {
            get;
            private set;
        }
        public Color SelectCheckColor
        {
            get;
            private set;
        }
        public Color ArrowColor
        {
            get;
            private set;
        }
        public Color SelectArrowColor
        {
            get;
            private set;
        }

        public Color CloseColor
        {
            get;
            private set;
        }
        public Color WindowBackColor
        {
            get;
            private set;
        }
        public Color SidebarBackColor
        {
            get;
            private set;
        }

        #endregion

        #region Method
        
        /// <summary>
        /// Конвертирует список клавиш в <see cref="String"/>.
        /// </summary>
        /// <param name="Keys"></param>
        /// <returns></returns>
        private String KeysToString(Keys Keys)
        {
            KeysConverter KC = new KeysConverter();
            String ShortcutKeys = KC.ConvertToString(Keys);

            ShortcutKeys = ShortcutKeys.Replace("Ctrl", "CTRL");
            ShortcutKeys = ShortcutKeys.Replace("Shift", "SHIFT");
            ShortcutKeys = ShortcutKeys.Replace("Alt", "ALT");

            ShortcutKeys = ShortcutKeys.Replace("Oemplus", "=");
            ShortcutKeys = ShortcutKeys.Replace("OemMinus", "_");

            return (ShortcutKeys != "None") ? ShortcutKeys : "";
        }
        
        /// <summary>
        /// Задаёт цветовую палитру, в зависимости от выбранной темы.
        /// </summary>
        /// <param name="Theme"></param>
        private void SetTheme(VSCodeTheme Theme)
        {
            switch (Theme)
            {
                case VSCodeTheme.Light:
                    this.BackColor = Color.FromArgb(221, 221, 221);
                    this.ForeColor = Color.Black;
                    this.DisabledForeColor = Color.FromArgb(90, 90, 90);
                    this.SelectColor = Color.FromArgb(198, 198, 198);

                    this.DropDownMenuBackColor = Color.FromArgb(243, 243, 243);
                    this.DropDownMenuForeColor = Color.Black;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(36, 119, 206);

                    this.SeparatorColor = Color.FromArgb(207, 207, 207);
                    this.CheckColor = this.DropDownMenuForeColor;
                    this.SelectCheckColor = this.DropDownMenuSelectForeColor;
                    this.ArrowColor = this.DropDownMenuForeColor;
                    this.SelectArrowColor = this.DropDownMenuSelectForeColor;

                    this.CloseColor = Color.FromArgb(232, 38, 55);
                    this.WindowBackColor = Color.FromArgb(255, 255, 255);
                    this.SidebarBackColor = Color.FromArgb(240, 240, 240);
                    break;

                case VSCodeTheme.QuietLight:
                    this.BackColor = Color.FromArgb(196, 183, 215);
                    this.ForeColor = Color.FromArgb(41, 41, 41);
                    this.DisabledForeColor = Color.FromArgb(80, 80, 80);
                    this.SelectColor = Color.FromArgb(176, 164, 193);

                    this.DropDownMenuBackColor = Color.FromArgb(245, 245, 245);
                    this.DropDownMenuForeColor = Color.FromArgb(70, 70, 70);
                    this.DropDownMenuSelectForeColor = Color.Black;
                    this.DropDownMenuSelectColor = Color.FromArgb(196, 217, 177);

                    this.SeparatorColor = Color.FromArgb(201, 201, 201);
                    this.CheckColor = Color.Black;
                    this.SelectCheckColor = Color.Black;
                    this.ArrowColor = Color.Black;
                    this.SelectArrowColor = Color.Black;

                    this.CloseColor = Color.FromArgb(228, 33, 52);
                    this.WindowBackColor = Color.FromArgb(245, 245, 245);
                    this.SidebarBackColor = Color.FromArgb(237, 237, 245);
                    break;

                case VSCodeTheme.SolarizedLight:
                    this.BackColor = Color.FromArgb(238, 232, 213);
                    this.ForeColor = Color.FromArgb(41, 41, 41);
                    this.DisabledForeColor = Color.FromArgb(80, 80, 80);
                    this.SelectColor = Color.FromArgb(214, 208, 191);

                    this.DropDownMenuBackColor = Color.FromArgb(238, 232, 213);
                    this.DropDownMenuForeColor = Color.FromArgb(55, 55, 55);
                    this.DropDownMenuSelectForeColor = Color.Black;
                    this.DropDownMenuSelectColor = Color.FromArgb(223, 202, 136);

                    this.SeparatorColor = Color.FromArgb(197, 194, 182);
                    this.CheckColor = Color.Black;
                    this.SelectCheckColor = Color.Black;
                    this.ArrowColor = Color.Black;
                    this.SelectArrowColor = Color.Black;

                    this.CloseColor = Color.FromArgb(232, 38, 53);
                    this.WindowBackColor = Color.FromArgb(253, 246, 227);
                    this.SidebarBackColor = Color.FromArgb(221, 214, 193);
                    break;

                case VSCodeTheme.Abyss:
                    this.BackColor = Color.FromArgb(16, 25, 44);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(110, 115, 124);
                    this.SelectColor = Color.FromArgb(40, 48, 66);

                    this.DropDownMenuBackColor = Color.FromArgb(24, 31, 47);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(8, 40, 107);

                    this.SeparatorColor = Color.FromArgb(89, 93, 103);
                    this.CheckColor = Color.White;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.White;
                    this.SelectArrowColor = Color.White;

                    this.CloseColor = Color.FromArgb(221, 17, 36);
                    this.WindowBackColor = Color.FromArgb(0, 12, 24);
                    this.SidebarBackColor = Color.FromArgb(5, 19, 54);
                    break;

                case VSCodeTheme.Dark:
                    this.BackColor = Color.FromArgb(60, 60, 60);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(104, 104, 104);
                    this.SelectColor = Color.FromArgb(80, 80, 80);

                    this.DropDownMenuBackColor = Color.FromArgb(37, 37, 38);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(9, 71, 113);

                    this.SeparatorColor = Color.FromArgb(97, 97, 98);
                    this.CheckColor = Color.White;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.White;
                    this.SelectArrowColor = Color.White;

                    this.CloseColor = Color.FromArgb(215, 21, 38);
                    this.WindowBackColor = Color.FromArgb(30, 30, 30);
                    this.SidebarBackColor = Color.FromArgb(51, 51, 51);
                    break;

                case VSCodeTheme.KimbieDark:
                    this.BackColor = Color.FromArgb(66, 53, 35);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(114, 105, 92);
                    this.SelectColor = Color.FromArgb(85, 74, 57);

                    this.DropDownMenuBackColor = Color.FromArgb(54, 39, 18);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(124, 80, 33);

                    this.SeparatorColor = Color.FromArgb(107, 98, 86);
                    this.CheckColor = Color.White;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.White;
                    this.SelectArrowColor = Color.White;

                    this.CloseColor = Color.FromArgb(215, 20, 35);
                    this.WindowBackColor = Color.FromArgb(34, 26, 15);
                    this.SidebarBackColor = Color.FromArgb(34, 26, 15);
                    break;

                case VSCodeTheme.Monokai:
                    this.BackColor = Color.FromArgb(30, 31, 28);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(100, 100, 98);
                    this.SelectColor = Color.FromArgb(53, 54, 51);

                    this.DropDownMenuBackColor = Color.FromArgb(30, 31, 28);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(117, 113, 94);

                    this.SeparatorColor = Color.FromArgb(93, 93, 92);
                    this.CheckColor = Color.White;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.White;
                    this.SelectArrowColor = Color.White;

                    this.CloseColor = Color.FromArgb(212, 18, 35);
                    this.WindowBackColor = Color.FromArgb(39, 40, 34);
                    this.SidebarBackColor = Color.FromArgb(39, 40, 34);
                    break;

                case VSCodeTheme.MonokaiDimmed:
                    this.BackColor = Color.FromArgb(80, 80, 80);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(105, 105, 105);
                    this.SelectColor = Color.FromArgb(98, 98, 98);

                    this.DropDownMenuBackColor = Color.FromArgb(39, 39, 39);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(112, 112, 112);

                    this.SeparatorColor = Color.FromArgb(98, 98, 98);
                    this.CheckColor = Color.White;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.White;
                    this.SelectArrowColor = Color.White;

                    this.CloseColor = Color.FromArgb(217, 23, 40);
                    this.WindowBackColor = Color.FromArgb(53, 53, 53);
                    this.SidebarBackColor = Color.FromArgb(30, 30, 30);
                    break;

                case VSCodeTheme.Red:
                    this.BackColor = Color.FromArgb(119, 0, 0);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(149, 96, 96);
                    this.SelectColor = Color.FromArgb(133, 26, 26);

                    this.DropDownMenuBackColor = Color.FromArgb(88, 0, 0);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(136, 0, 0);

                    this.SeparatorColor = Color.FromArgb(128, 75, 75);
                    this.CheckColor = this.DropDownMenuForeColor;
                    this.SelectCheckColor = this.DropDownMenuSelectForeColor;
                    this.ArrowColor = this.DropDownMenuForeColor;
                    this.SelectArrowColor = this.DropDownMenuSelectForeColor;

                    this.CloseColor = Color.FromArgb(221, 18, 35);
                    this.WindowBackColor = Color.FromArgb(57, 0, 0);
                    this.SidebarBackColor = Color.FromArgb(88, 0, 0);
                    break;

                case VSCodeTheme.SolarizedDark:
                    this.BackColor = Color.FromArgb(0, 44, 57);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(96, 116, 122);
                    this.SelectColor = Color.FromArgb(26, 66, 77);

                    this.DropDownMenuBackColor = Color.FromArgb(0, 33, 43);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(0, 90, 111);

                    this.SeparatorColor = Color.FromArgb(75, 95, 101);
                    this.CheckColor = Color.White;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.White;
                    this.SelectArrowColor = Color.White;

                    this.CloseColor = Color.FromArgb(209, 19, 38);
                    this.WindowBackColor = Color.FromArgb(0, 43, 54);
                    this.SidebarBackColor = Color.FromArgb(0, 56, 71);
                    break;

                case VSCodeTheme.TomorrowNightBlue:
                    this.BackColor = Color.FromArgb(0, 17, 38);
                    this.ForeColor = Color.FromArgb(204, 204, 204);
                    this.DisabledForeColor = Color.FromArgb(96, 110, 127);
                    this.SelectColor = Color.FromArgb(26, 41, 60);

                    this.DropDownMenuBackColor = Color.FromArgb(0, 23, 51);
                    this.DropDownMenuForeColor = Color.White;
                    this.DropDownMenuSelectForeColor = Color.White;
                    this.DropDownMenuSelectColor = Color.FromArgb(97, 111, 129);

                    this.SeparatorColor = Color.FromArgb(75, 89, 105);
                    this.CheckColor = Color.White;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.White;
                    this.SelectArrowColor = Color.White;

                    this.CloseColor = Color.FromArgb(209, 17, 36);
                    this.WindowBackColor = Color.FromArgb(0, 23, 51);
                    this.SidebarBackColor = Color.FromArgb(0, 36, 81);
                    break;
            }

            _vsCodeTheme = Theme;
        }

        private void SetMenu(ToolStrip Menu)
        {
            if (Menu is MenuStrip)
            {
                MenuStrip _menuStrip = Menu as MenuStrip;
                _menuStrip.Padding = new Padding(0);

                foreach (ToolStripItem _item in GetItems(_menuStrip.Items))
                    SetMargin(GetItems(_item));
            }

            if (Menu is ContextMenuStrip)
            {
                ContextMenuStrip _contextMenuStrip = Menu as ContextMenuStrip;
                SetMargin(GetItems(_contextMenuStrip.Items));
            }
        }

        /// <summary>
        /// Задаёт Margin первому и последнему элементам списка.
        /// </summary>
        /// <param name="Items"></param>
        private void SetMargin(ToolStripItem[] Items)
        {
            switch (Items.Length)
            {
                case 0:
                    break;

                case 1:
                    Items[0].Margin = new Padding(0, 2, 0, 3);
                    break;

                default:
                    Items[0].Margin = new Padding(0, 2, 0, 0);
                    Items[Items.Length - 1].Margin = new Padding(0, 0, 0, 3);
                    break;
            }

            if (Items.Length > 0)
            {
                foreach (ToolStripItem Item in Items)
                    SetMargin(GetItems(Item));
            }
        }

        /// <summary>
        /// Получает список <see cref="ToolStripItem"/> родительского <see cref="ToolStripItem"/>.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        private ToolStripItem[] GetItems(ToolStripItem Item)
        {
            List<ToolStripItem> Items = new List<ToolStripItem>();
            
            if (Item is ToolStripMenuItem)
            {
                foreach (ToolStripItem i in ((ToolStripMenuItem)Item).DropDownItems)
                    Items.Add(i);
            }

            else if (Item is ToolStripSplitButton)
            {
                foreach (ToolStripItem i in ((ToolStripSplitButton)Item).DropDownItems)
                    Items.Add(i);
            }

            else if (Item is ToolStripDropDownButton)
            {
                foreach (ToolStripItem i in ((ToolStripDropDownButton)Item).DropDownItems)
                    Items.Add(i);
            }

            return Items.ToArray();
        }

        /// <summary>
        /// Получает список <see cref="ToolStripItem"/> коллекции <see cref="ToolStripItemCollection"/>.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        private ToolStripItem[] GetItems(ToolStripItemCollection Collection)
        {
            List<ToolStripItem> Items = new List<ToolStripItem>();
            foreach (ToolStripItem Item in Collection)
                Items.Add(Item);

            return Items.ToArray();
        }

        private Int32 GetCount(ToolStripItem[] Items)
        {
            Int32 Count = 0;
            foreach (ToolStripItem Item in Items)
            {
                Count += 1;
                Count += GetCount(GetItems(Item));
            }

            return Count;
        }
        
        #endregion

        // Отрисовка фонового цвета главного меню
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(this.BackColor),
                new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));
        }

        // Отрисовка фонового цвета выпадающего меню
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(this.DropDownMenuBackColor),
                new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));
        }

        // Отрисовка выделенного Item в главном меню
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (_vsCodeSettings != null)
            {
                switch (e.Item.Name)
                {
                    case "mmMinimum":
                        e.Item.Image = _vsCodeSettings.ControlBox.Minimum(_vsCodeTheme);
                        e.Item.Padding = new Padding(12, 4, 12, 4);
                        break;

                    case "mmMaximum":
                        e.Item.Image = (_vsCodeSettings.Form != null) ? _vsCodeSettings.ControlBox.Maximum(_vsCodeTheme, _vsCodeSettings.Form.WindowState) : _vsCodeSettings.ControlBox.Maximum(_vsCodeTheme, FormWindowState.Normal);
                        e.Item.Padding = new Padding(12, 4, 12, 4);
                        break;

                    case "mmClose":
                        e.Item.Image = _vsCodeSettings.ControlBox.Close(_vsCodeTheme);
                        e.Item.Padding = new Padding(12, 4, 12, 4);
                        break;

                    default:
                        e.Item.Padding = (e.Item.IsOnDropDown) 
                            ? new Padding((e.Item.Font.Size <= 7.5F) ? 0 : 2) 
                            : new Padding(4);
                        break;
                }
            }
            
            if (e.Item.Enabled)
            {
                // Выделение Item в главном меню
                if (e.Item.IsOnDropDown == false)
                {
                    if (e.Item.Selected)
                    {
                        switch (e.Item.Name)
                        {
                            case "mmTitle":
                            case "mmIcon":
                                e.Graphics.FillRectangle(
                                    new SolidBrush(Color.Transparent),
                                    new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                                break;

                            case "mmClose":
                                e.Graphics.FillRectangle(
                                    new SolidBrush(this.CloseColor),
                                    new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                                break;

                            default:
                                e.Graphics.FillRectangle(
                                    new SolidBrush(this.SelectColor),
                                    new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                                break;
                        }
                    }

                    if ((e.Item as ToolStripMenuItem).DropDown.Visible)
                    {
                        e.Graphics.FillRectangle(
                            new SolidBrush(this.SelectColor),
                            new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                    }

                    (e.Item as ToolStripMenuItem).DropDown.Padding = new Padding(10);
                }

                // Выделение Item в выпадающем меню
                if (e.Item.IsOnDropDown && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(
                        new SolidBrush(this.DropDownMenuSelectColor),
                        new Rectangle(1, 1, e.Item.Width, e.Item.Height));
                }
            }
        }
        
        // Отрисовка текста Item
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            String ShortcutKeys = KeysToString((e.Item as ToolStripMenuItem).ShortcutKeys);
            Int32 W = TextRenderer.MeasureText(ShortcutKeys, e.Item.Font).Width;
            Int32 H = TextRenderer.MeasureText("Text", e.Item.Font).Height;

            //Font _font = e.Item.Font;
            Font _font = new Font(e.Item.Font.Name, e.Item.Font.Size - 0.5F, e.Item.Font.Style);
            
            Int32 _leftAndRightPadding = H + 5;

            Color _foreColor = this.DropDownMenuForeColor;
            Color _selectForeColor = this.DropDownMenuSelectForeColor;

            Color _selectColor = this.DropDownMenuSelectColor;
            Color _backColor = this.DropDownMenuBackColor;
            Color _disabledFontColor = this.DisabledForeColor;

            if (e.Item.IsOnDropDown && e.Item.Selected)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    e.Item.Text,
                    _font,
                    new Rectangle(_leftAndRightPadding, 0, e.Item.Width, e.Item.Height),
                    (e.Item.Enabled) ? _selectForeColor : _disabledFontColor,
                    (e.Item.Enabled) ? _selectColor : _backColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

                if ((e.Item as ToolStripMenuItem).ShowShortcutKeys)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        ShortcutKeys,
                        _font,
                        new Rectangle(e.Item.Width - W - _leftAndRightPadding, 0, W, e.Item.Height),
                        (e.Item.Enabled) ? _selectForeColor : _disabledFontColor,
                        (e.Item.Enabled) ? _selectColor : _backColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right | TextFormatFlags.EndEllipsis);
                }
            }

            else if (e.Item.IsOnDropDown && !e.Item.Selected)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    e.Item.Text,
                    _font,
                    new Rectangle(_leftAndRightPadding, 0, e.Item.Width, e.Item.Height),
                    (e.Item.Enabled) ? _foreColor : _disabledFontColor,
                    _backColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

                if ((e.Item as ToolStripMenuItem).ShowShortcutKeys)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        ShortcutKeys,
                        _font,
                        new Rectangle(e.Item.Width - W - _leftAndRightPadding, 0, W, e.Item.Height),
                        (e.Item.Enabled) ? _foreColor : _disabledFontColor,
                        _backColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right | TextFormatFlags.EndEllipsis);
                }
            }

            // Отрисовка текста главного меню
            else
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    e.Item.Text,
                    _font,
                    new Rectangle(0, 0, e.Item.Width, e.Item.Height),
                    (e.Item.Enabled) ? this.ForeColor : _disabledFontColor,
                    Color.Transparent,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        // Отрисовка разделителя
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);

            Int32 H = TextRenderer.MeasureText("Text", e.Item.Font).Height;
            Int32 _leftAndRightPadding = H - 5;

            e.Graphics.FillRectangle(
                new SolidBrush(this.SeparatorColor),
                new Rectangle(_leftAndRightPadding, 3, e.Item.Width - (_leftAndRightPadding * 2), 1));
        }

        // Отрисовка стрелки
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            Rectangle _arrowRectangle = new Rectangle(e.ArrowRectangle.Location, e.ArrowRectangle.Size);
            _arrowRectangle.Inflate(-3, -6);

            Int32 H = TextRenderer.MeasureText("Text", e.Item.Font).Height;
            Int32 _leftAndRightPadding = e.Item.Width - H + 2;

            Single _centerHeight = _arrowRectangle.Top + _arrowRectangle.Height / 2;

            PointF[] _lines = new PointF[]
            {
                new PointF(_leftAndRightPadding - 4, _centerHeight - 4),
                new PointF(_leftAndRightPadding, _centerHeight),
                new PointF(_leftAndRightPadding - 4, _centerHeight + 4)
            };

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawLines(new Pen((e.Item.IsOnDropDown && e.Item.Selected) ? this.SelectArrowColor : this.ArrowColor), _lines);

            e.Graphics.SmoothingMode = SmoothingMode.Default;
        }

        // Отрисовка флажка
        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            Rectangle _imageRectangle = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
            _imageRectangle.Inflate(-4, -6);

            Int32 H = TextRenderer.MeasureText("Text", e.Item.Font).Height;
            Int32 _leftAndRightPadding = H - 3;

            Point[] _lines = new Point[]
            {
                new Point(_leftAndRightPadding, (_imageRectangle.Bottom - _imageRectangle.Height / 2)),
                new Point((_leftAndRightPadding + _imageRectangle.Width / 3),  _imageRectangle.Bottom),
                new Point(_leftAndRightPadding + 6, _imageRectangle.Top - 1)
            };

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawLines(new Pen((e.Item.IsOnDropDown && e.Item.Selected) ? this.SelectCheckColor : this.CheckColor), _lines);

            e.Graphics.SmoothingMode = SmoothingMode.Default;
        }
    }
}
