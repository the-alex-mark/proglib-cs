using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.VSCode
{
    public class VSCodeToolStripRenderer : ToolStripRenderer
    {
        public VSCodeToolStripRenderer(VSCodeTheme Theme)
        {
            this.GetTheme(Theme);
            this.Settings = false;
        }

        public VSCodeToolStripRenderer(VSCodeTheme Theme, Boolean Settings)
        {
            this.GetTheme(Theme);
            this.Settings = Settings;
        }

        #region Properties

        private Boolean Settings { get; set; }
        public Color ForeColor { get; set; }
        public Color SelectForeColor { get; set; }
        public Color DisabledForeColor { get; set; }
        public Color SelectColor { get; set; }
        public Color DropDownMenuSelectColor { get; set; }
        public Color BackColor { get; set; }
        public Color DropDownMenuBackColor { get; set; }
        public Color SeparatorColor { get; set; }
        public Color CheckColor { get; set; }
        public Color SelectCheckColor { get; set; }
        public Color ArrowColor { get; set; }
        public Color SelectArrowColor { get; set; }
        public Color CloseColor { get; set; }

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
                    this.ForeColor = Color.Black;
                    this.SelectForeColor = Color.White;
                    this.DisabledForeColor = Color.FromArgb(90, 90, 90);
                    this.SelectColor = Color.FromArgb(198, 198, 198);
                    this.DropDownMenuSelectColor = Color.FromArgb(36, 119, 206);
                    this.BackColor = Color.FromArgb(221, 221, 221);
                    this.DropDownMenuBackColor = Color.FromArgb(243, 243, 243);
                    this.SeparatorColor = Color.FromArgb(207, 207, 207);
                    this.CheckColor = Color.Black;
                    this.SelectCheckColor = Color.White;
                    this.ArrowColor = Color.Black;
                    this.SelectArrowColor = Color.White;
                    this.CloseColor = Color.FromArgb(232, 38, 55);
                    break;

                case VSCodeTheme.QuietLight:
                    this.ForeColor = Color.Black;
                    this.SelectForeColor = Color.Black;
                    this.DisabledForeColor = Color.FromArgb(80, 80, 80);
                    this.SelectColor = Color.FromArgb(176, 164, 193);
                    this.DropDownMenuSelectColor = Color.FromArgb(196, 217, 177);
                    this.BackColor = Color.FromArgb(196, 183, 215);
                    this.DropDownMenuBackColor = Color.FromArgb(240, 240, 240);
                    this.SeparatorColor = Color.FromArgb(201, 201, 201);
                    this.CheckColor = Color.Black;
                    this.SelectCheckColor = Color.Black;
                    this.ArrowColor = Color.Black;
                    this.SelectArrowColor = Color.Black;
                    this.CloseColor = Color.FromArgb(228, 33, 52);
                    break;
            }
        }

        private ToolStripItem[] GetAllChildren(ToolStripItem Item)
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

        private void SettingsUpMargin(ToolStripItem[] Items)
        {
            foreach (ToolStripItem Item in Items)
            {
                ToolStripItem[] _items = GetAllChildren(Item);

                switch (_items.Length)
                {
                    case 0:
                        break;

                    case 1:
                        _items[0].Margin = new Padding(0, 1, 0, 2);
                        break;

                    default:
                        _items[0].Margin = new Padding(0, 1, 0, 0);
                        _items[_items.Length - 1].Margin = new Padding(0, 0, 0, 2);
                        break;
                }

                SettingsUpMargin(_items);
            }
        }

        #endregion

        // Отрисовка фона Item
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            // Настройка "MenuStrip"
            if (this.Settings)
            {
                if (!e.Item.IsOnDropDown && e.Item.GetCurrentParent() is MenuStrip)
                {
                    MenuStrip _menuStrip = e.Item.GetCurrentParent() as MenuStrip;
                    _menuStrip.BackColor = this.BackColor;
                    _menuStrip.Padding = new Padding(0);

                    List<ToolStripItem> Items = new List<ToolStripItem>();
                    foreach (ToolStripItem Item in _menuStrip.Items) Items.Add(Item);
                    if (Items.Count > 0) SettingsUpMargin(Items.ToArray());
                }

                switch (e.Item.Name)
                {
                    case "mmMinimum":
                    case "mmMaximum":
                    case "mmClose":
                        e.Item.Padding = new Padding(12, 0, 12, 0);
                        break;

                    default:
                        if (e.Item.IsOnDropDown)
                            e.Item.Padding = new Padding(0);
                        break;
                }
            }
            else
            {
                if (!e.Item.IsOnDropDown && e.Item.GetCurrentParent() is MenuStrip)
                    (e.Item.GetCurrentParent() as MenuStrip).BackColor = this.BackColor;
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

            Color _foreColor = this.ForeColor;
            Color _selectForeColor = this.SelectForeColor;

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
                    (e.Item.Enabled) ? _foreColor : _disabledFontColor,
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
