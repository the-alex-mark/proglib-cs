using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    public partial class MinimalContextMenuRenderer : ToolStripRenderer
    {
        public MinimalContextMenuRenderer()
        {
            ForeColor = Color.White;
            BackColor = SystemColors.ControlLight;
            DropDownMenuBackColor = SystemColors.ControlDark;
            BorderColor = Color.Black;
            SeparatorColor = SystemColors.ControlDark;
            SelectColor = SystemColors.ControlDark;
        }

        //private Rectangle _rectangle;

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

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle BOUNDS = new Rectangle(Point.Empty, e.Item.Size);
            if (e.Item.Selected)
            {
                using (Brush B = new SolidBrush(Color.FromArgb(202, 81, 0)))
                {
                    e.Graphics.FillRectangle(B, BOUNDS);
                }
            }

            e.Item.ForeColor = Color.White;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            Rectangle R = Rectangle.Inflate(e.AffectedBounds, 0, 0);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), R);
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), e.AffectedBounds);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            ToolStripDropDownMenu currentParent = e.Item.GetCurrentParent() as ToolStripDropDownMenu;
            int x = currentParent.Padding.Left;
            int y = e.Item.Height / 2;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(64, 64, 64)), x, y, e.Item.Bounds.Right, y);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(202, 81, 0)), 1f), new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            if (e.Item != null)
            {
                e.ArrowColor = Color.FromArgb(202, 81, 0);
            }
            base.OnRenderArrow(e);
        }
    }
}
