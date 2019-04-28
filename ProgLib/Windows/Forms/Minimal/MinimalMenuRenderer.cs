using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    public class MinimalMenuRenderer : ToolStripRenderer
    {
        public MinimalMenuRenderer()
        {
            ForeColor = Color.White;
            BackColor = SystemColors.ControlLight;
            DropDownMenuBackColor = SystemColors.ControlDark;
            BorderColor = Color.Black;
            SeparatorColor = SystemColors.ControlDark;
            SelectColor = SystemColors.ControlDark;
        }

        private Rectangle _rectangle;

        public Color ForeColor
        {
            get; set;
        }
        public Color BackColor
        {
            get; set;
        }
        public Color DropDownMenuBackColor
        {
            get; set;
        }
        public Color BorderColor
        {
            get; set;
        }
        public Color SeparatorColor
        {
            get; set;
        }
        public Color SelectColor
        {
            get; set;
        }
        
        // Выделенный Item
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            base.OnRenderMenuItemBackground(e);

            if (e.Item.Enabled)
            {
                if (e.Item.IsOnDropDown == false && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(
                        new SolidBrush(SelectColor),
                        new Rectangle(1, 0, e.Item.Width - 1, e.Item.Height - 1));

                    e.Item.ForeColor = ForeColor;
                }
                else { e.Item.ForeColor = ForeColor; }

                if (e.Item.Selected) _rectangle = new Rectangle(0, 0, e.Item.Width, e.Item.Height);

                if (e.Item.IsOnDropDown && e.Item.Selected)
                {
                    e.Graphics.FillRectangle(
                        new SolidBrush(SelectColor),
                        new Rectangle(1, 0, e.Item.Width, e.Item.Height));

                    e.Item.ForeColor = ForeColor;
                }

                // Верхнее меню
                if ((e.Item as ToolStripMenuItem).DropDown.Visible && e.Item.IsOnDropDown == false)
                {
                    e.Graphics.FillRectangle(
                        new SolidBrush(DropDownMenuBackColor),
                        new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1));

                    e.Graphics.DrawRectangle(
                        new Pen(BorderColor),
                        new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height));

                    e.Item.ForeColor = ForeColor;
                }
            }
        }

        // Разделитель
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            base.OnRenderSeparator(e);

            e.Graphics.FillRectangle(
                new SolidBrush(SeparatorColor),
                new Rectangle(30, 3, e.Item.Width - 35, 1));
        }

        //// Картинка
        //protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        //{
        //    base.OnRenderItemCheck(e);

        //    if (e.Item.Selected)
        //    {
        //        e.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(4, 2, 18, 18));
        //        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(220, 220, 220)), new Rectangle(5, 3, 16, 16));
        //        e.Graphics.DrawImage(e.Image, new Point(5, 3));
        //    }
        //    else
        //    {
        //        e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(4, 2, 18, 18));
        //        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(80, 90, 90)), new Rectangle(5, 3, 16, 16));
        //        e.Graphics.DrawImage(e.Image, new Point(5, 3));
        //    }
        //}

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            base.OnRenderImageMargin(e);

            e.Graphics.FillRectangle(
                new SolidBrush(DropDownMenuBackColor),
                new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height));

            e.Graphics.FillRectangle(
                new SolidBrush(DropDownMenuBackColor),
                new Rectangle(0, 0, 26, e.AffectedBounds.Height));

            e.Graphics.DrawLine(new Pen(DropDownMenuBackColor), 28, 0, 28, e.AffectedBounds.Height);

            e.Graphics.DrawLine(new Pen(BorderColor), new Point(_rectangle.Width - 1, 0), new Point(e.ToolStrip.Width, 0));

            // Обводка выпадающего меню
            e.Graphics.DrawRectangle(
                new Pen(BorderColor),
                new Rectangle(0, -1, e.ToolStrip.Width - 1, e.ToolStrip.Height));
        }
    }
}
