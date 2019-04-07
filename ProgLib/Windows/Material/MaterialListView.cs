using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Animations.Material;

namespace ProgLib.Windows.Material
{
    [ToolboxBitmap(typeof(System.Windows.Forms.ListView))]
    public partial class MaterialListView : System.Windows.Forms.ListView
    {
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        [Browsable(false)]
        public Point MouseLocation { get; set; }
        [Browsable(false)]
        private ListViewItem HoveredItem { get; set; }

        public MaterialListView()
        {
            GridLines = false;
            FullRowSelect = true;
            HeaderStyle = ColumnHeaderStyle.Nonclickable;
            View = View.Details;
            OwnerDraw = true;
            ResizeRedraw = true;
            BorderStyle = BorderStyle.None;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            //Fix for hovers, by default it doesn't redraw
            //TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
            MouseLocation = new Point(-1, -1);
            MouseState = MouseState.OUT;
            MouseEnter += delegate
            {
                MouseState = MouseState.HOVER;
            };
            MouseLeave += delegate
            {
                MouseState = MouseState.OUT;
                MouseLocation = new Point(-1, -1);
                HoveredItem = null;
                Invalidate();
            };
            MouseDown += delegate { MouseState = MouseState.DOWN; };
            MouseUp += delegate { MouseState = MouseState.HOVER; };
            MouseMove += delegate (object sender, MouseEventArgs args)
            {
                MouseLocation = args.Location;
                var currentHoveredItem = this.GetItemAt(MouseLocation.X, MouseLocation.Y);
                if (HoveredItem != currentHoveredItem)
                {
                    HoveredItem = currentHoveredItem;
                    Invalidate();
                }
            };

            ForeColor = SystemColors.ControlDarkDark;
            _headerForeColor = SystemColors.ControlText;
            _headerBackColor = BackColor;
            _hovertItemColor = Color.FromArgb(255, 200, 200);
            _selectItemColor = Color.FromArgb(255, 128, 128);
            _delimiterColor = SystemColors.ControlLight;
        }

        private Color _headerForeColor, _headerBackColor, _selectItemColor, _hovertItemColor, _delimiterColor;

        [Category("Appearance"), Description("Цвет текста заголовка столбцов")]
        public Color HeaderForeColor
        {
            get { return _headerForeColor; }
            set
            {
                _headerForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет фона заголовка столбцов")]
        public Color HeaderBackColor
        {
            get { return _headerBackColor; }
            set
            {
                _headerBackColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет фона Item при его выборе")]
        public Color SelectItemColor
        {
            get { return _selectItemColor; }
            set
            {
                _selectItemColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет фона Item при наведении на него")]
        public Color HovertItemColor
        {
            get { return _hovertItemColor; }
            set
            {
                _hovertItemColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет разделителя Item")]
        public Color DelimiterColor
        {
            get { return _delimiterColor; }
            set
            {
                _delimiterColor = value;
                Invalidate();
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(_headerBackColor), new Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height));
            e.Graphics.DrawString(
                e.Header.Text,
                Font,
                new SolidBrush(_headerForeColor),
                new Rectangle(e.Bounds.X + ITEM_PADDING, e.Bounds.Y + ITEM_PADDING, e.Bounds.Width - ITEM_PADDING * 2, e.Bounds.Height - ITEM_PADDING * 2),
                getStringFormat());
        }

        private const int ITEM_PADDING = 3;
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            // Мы рисуем текущую строку элементов (=элемент с подэлементами) на временном растровом изображении, а затем сразу рисуем растровое изображение. Это уменьшить мелькать.
            Bitmap B = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);

            using (Graphics G = Graphics.FromImage(B))
            {
                // Общий фон Item
                G.FillRectangle(new SolidBrush(BackColor), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));

                if (e.State.HasFlag(ListViewItemStates.Selected))
                {
                    // Фон - при выборе Item
                    G.FillRectangle(new SolidBrush(_selectItemColor), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
                }
                else if (e.Bounds.Contains(MouseLocation) && MouseState == MouseState.HOVER)
                {
                    // Фон - при наведении на Item
                    G.FillRectangle(new SolidBrush(_hovertItemColor), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
                }

                // Разделитель
                G.DrawLine(new Pen(_delimiterColor), e.Bounds.Left, 0, e.Bounds.Right, 0);

                // Отрисовка текста
                foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
                {
                    G.DrawString(
                        subItem.Text,
                        Font,
                        new SolidBrush(ForeColor),
                        new Rectangle(subItem.Bounds.X + ITEM_PADDING, ITEM_PADDING, subItem.Bounds.Width - 2 * ITEM_PADDING, subItem.Bounds.Height - 2 * ITEM_PADDING),
                        getStringFormat());
                }
            }
            
            e.Graphics.DrawImage((Image)B.Clone(), new Point(0, e.Item.Bounds.Location.Y));
            B.Dispose();
        }

        private StringFormat getStringFormat()
        {
            return new StringFormat
            {
                FormatFlags = StringFormatFlags.LineLimit,
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }
    }
}
