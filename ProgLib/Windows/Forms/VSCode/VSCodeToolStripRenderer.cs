﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.VSCode
{
    public class VSCodeToolStripRenderer : ToolStripRenderer
    {
        public VSCodeToolStripRenderer(VSCodeTheme Theme)
        {
            this.Theme = Theme;
            this.GetTheme(Theme);
            this.ControlBox = new VSCodeControlBox(VSCodeIconTheme.Classic);
            this.Settings = true;

            Index = 0;
            Count = 0;
            Counter = true;
        }

        public VSCodeToolStripRenderer(VSCodeTheme Theme, VSCodeIconTheme IconTheme)
        {
            this.Theme = Theme;
            this.GetTheme(Theme);
            this.ControlBox = new VSCodeControlBox(IconTheme);
            this.Settings = true;

            Index = 0;
            Count = 0;
            Counter = true;
        }

        public VSCodeToolStripRenderer(VSCodeTheme Theme, Boolean Settings)
        {
            this.Theme = Theme;
            this.GetTheme(Theme);
            this.ControlBox = new VSCodeControlBox(VSCodeIconTheme.Classic);
            this.Settings = Settings;

            Index = 0;
            Count = 0;
            Counter = true;
        }

        #region Variables

        private Int32 Index;
        private Int32 Count;
        private Boolean Counter;

        #endregion

        #region Properties

        private Boolean Settings { get; set; }
        public VSCodeTheme Theme { get; private set; }
        public VSCodeControlBox ControlBox { get; private set; }

        public Color BackColor { get; private set; }
        public Color ForeColor { get; private set; }
        public Color DisabledForeColor { get; private set; }
        public Color SelectColor { get; private set; }

        public Color DropDownMenuBackColor { get; private set; }
        public Color DropDownMenuForeColor { get; private set; }
        public Color DropDownMenuSelectForeColor { get; private set; }
        public Color DropDownMenuSelectColor { get; private set; }

        public Color SeparatorColor { get; private set; }
        public Color CheckColor { get; private set; }
        public Color SelectCheckColor { get; private set; }
        public Color ArrowColor { get; private set; }
        public Color SelectArrowColor { get; private set; }

        public Color CloseColor { get; private set; }
        public Color WindowBackColor { get; private set; }
        public Color SidebarBackColor { get; private set; }

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
            String ShortcutKeys = KC.ConvertToString(Keys).ToUpper();

            return (ShortcutKeys != "NONE") ? ShortcutKeys : "";
        }

        /// <summary>
        /// Задаёт цветовую палитру, в зависимости от выбранной темы.
        /// </summary>
        /// <param name="Theme"></param>
        private void GetTheme(VSCodeTheme Theme)
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
        }

        /// <summary>
        /// Получает список <see cref="ToolStripItem"/> родительского <see cref="ToolStripItem"/>.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        private ToolStripItem[] GetChildren(ToolStripItem Item)
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
        private ToolStripItem[] GetChildren(ToolStripItemCollection Collection)
        {
            List<ToolStripItem> Items = new List<ToolStripItem>();
            foreach (ToolStripItem Item in Collection)
                Items.Add(Item);

            return Items.ToArray();
        }

        private Int32 GetCount(ToolStripItem[] Items)
        {
            Int32 _count = 0;
            foreach (ToolStripItem Item in Items)
            {
                _count += 1;
                _count += GetCount(GetChildren(Item));
            }

            return _count;
        }

        /// <summary>
        /// Задаёт Margin первому и последнему элементам списка.
        /// </summary>
        /// <param name="Items"></param>
        private void UMargin(ToolStripItem[] Items)
        {
            switch (Items.Length)
            {
                case 0:
                    break;

                case 1:
                    Items[0].Margin = new Padding(0, 1, 0, 2);
                    break;

                default:
                    Items[0].Margin = new Padding(0, 1, 0, 0);
                    Items[Items.Length - 1].Margin = new Padding(0, 0, 0, 2);
                    break;
            }

            if (Items.Length > 0)
            {
                foreach (ToolStripItem Item in Items)
                    UMargin(GetChildren(Item));
            }
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
            if (this.Settings)
            {
                Form Parent = null;

                // Настройка "MenuStrip"
                if (!e.Item.IsOnDropDown && e.Item.GetCurrentParent() is MenuStrip)
                {
                    MenuStrip _menuStrip = e.Item.GetCurrentParent() as MenuStrip;
                    if (_menuStrip.Parent is Form) Parent = _menuStrip.Parent as Form;
                    foreach (ToolStripItem _item in GetChildren(_menuStrip.Items)) UMargin(GetChildren(_item));

                    _menuStrip.Padding = new Padding(0);
                    
                    if (Counter)
                    {
                        Count = GetCount(GetChildren(_menuStrip.Items));
                        Counter = false;
                    }
                }

                // Настройка "ContextMenuStrip"
                if (e.Item.GetCurrentParent() is ContextMenuStrip)
                {
                    ContextMenuStrip _contextMenuStrip = e.Item.GetCurrentParent() as ContextMenuStrip;
                    UMargin(GetChildren(_contextMenuStrip.Items));

                    if (Counter)
                    {
                        Count = GetCount(GetChildren(_contextMenuStrip.Items));
                        Counter = false;
                    }
                }

                switch (e.Item.Name)
                {
                    case "mmMinimum":
                        e.Item.Image = ControlBox.Minimum(Theme);
                        e.Item.Padding = new Padding(12, 0, 12, 0);
                        break;

                    case "mmMaximum":
                        e.Item.Image = (Parent != null) ? ControlBox.Maximum(Theme, Parent.WindowState) : ControlBox.Maximum(Theme, FormWindowState.Normal);
                        e.Item.Padding = new Padding(12, 0, 12, 0);
                        break;

                    case "mmClose":
                        e.Item.Image = ControlBox.Close(Theme);
                        e.Item.Padding = new Padding(12, 0, 12, 0);
                        break;

                    default:
                        if (e.Item.IsOnDropDown)
                            e.Item.Padding = new Padding(0);
                        break;
                }

                Index++;
                if (Count == Index) this.Settings = false;
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
            Int32 W = TextRenderer.MeasureText(
                ShortcutKeys,
                new Font("Segoe UI", 7.5F, FontStyle.Regular)).Width;

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
                    (e.Item as ToolStripMenuItem).Font,
                    new Rectangle(18, 0, e.Item.Width, e.Item.Height),
                    (e.Item.Enabled) ? _selectForeColor : _disabledFontColor,
                    (e.Item.Enabled) ? _selectColor : _backColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

                if ((e.Item as ToolStripMenuItem).ShowShortcutKeys)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        ShortcutKeys,
                        (e.Item as ToolStripMenuItem).Font,
                        new Rectangle(e.Item.Width - W - 17, 0, W, e.Item.Height),
                        (e.Item.Enabled) ? _selectForeColor : _disabledFontColor,
                        (e.Item.Enabled) ? _selectColor : _backColor,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right | TextFormatFlags.EndEllipsis);
                }
            }

            else if (e.Item.IsOnDropDown && e.Item.Selected == false)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    e.Item.Text,
                    (e.Item as ToolStripMenuItem).Font,
                    new Rectangle(18, 0, e.Item.Width, e.Item.Height),
                    (e.Item.Enabled) ? _foreColor : _disabledFontColor,
                    _backColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

                if ((e.Item as ToolStripMenuItem).ShowShortcutKeys)
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        ShortcutKeys,
                        (e.Item as ToolStripMenuItem).Font,
                        new Rectangle(e.Item.Width - W - 17, 0, W, e.Item.Height),
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
                    (e.Item as ToolStripMenuItem).Font,
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

            e.Graphics.FillRectangle(
                new SolidBrush(this.SeparatorColor),
                new Rectangle(6, 3, e.Item.Width - 12, 1));
        }

        // Отрисовка стрелки
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            Rectangle _arrowRectangle = new Rectangle(e.ArrowRectangle.Location, e.ArrowRectangle.Size);
            _arrowRectangle.Inflate(-3, -6);

            Point[] _lines = new Point[]
            {
                new Point(_arrowRectangle.Left + 2, _arrowRectangle.Top),
                new Point(_arrowRectangle.Right + 2, _arrowRectangle.Top + _arrowRectangle.Height / 2),
                new Point(_arrowRectangle.Left + 2, _arrowRectangle.Top + _arrowRectangle.Height)
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

            Point[] _lines = new Point[]
            {
                new Point(_imageRectangle.Left/* - 1*/, (_imageRectangle.Bottom - _imageRectangle.Height / 2)),
                new Point((_imageRectangle.Left + _imageRectangle.Width / 3)/* - 1*/,  _imageRectangle.Bottom),
                new Point(_imageRectangle.Right - 2, _imageRectangle.Top - 1)
            };

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawLines(new Pen((e.Item.IsOnDropDown && e.Item.Selected) ? this.SelectCheckColor : this.CheckColor), _lines);

            e.Graphics.SmoothingMode = SmoothingMode.Default;
        }
    }
}
