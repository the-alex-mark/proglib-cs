using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Minimal
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    public partial class MinimalButton : System.Windows.Forms.Button
    {
        public MinimalButton()
        {
            Size = new Size(140, 27);
            Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);

            BackColor = Color.Gainsboro;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 1;
            FlatAppearance.BorderColor = Drawing.MetroColors.Blue;
        }

        #region Variables

        private Boolean _onMouseEnter;
        private Boolean _onMouseDown;

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _onMouseEnter = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _onMouseEnter = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _onMouseDown = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _onMouseDown = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            
            if (_onMouseEnter)
            {
                // Отрисовка при наведении
                if (FlatAppearance.MouseOverBackColor != null)
                    e.Graphics.FillRectangle(new SolidBrush(FlatAppearance.MouseOverBackColor), new Rectangle(0, 0, Width - 1, Height - 1));
                
            }
            else e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, 0, Width - 1, Height - 1));

            if (_onMouseDown)
            {
                if (FlatAppearance.MouseDownBackColor != null)
                {
                    // Отрисовка при нажатии
                    e.Graphics.FillRectangle(new SolidBrush(FlatAppearance.MouseDownBackColor), new Rectangle(0, 0, Width - 1, Height - 1));
                }
            }

            // Отрисовка текста
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(0, 0, Width - 1, Height - 1),
                ForeColor,
                TextAlign.ToTextFormatFlags() | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);

            if (FlatAppearance.BorderSize > 0)
            {
                // Отрисовка границ
                e.Graphics.DrawRectangle(new Pen(FlatAppearance.BorderColor, 1), new Rectangle(0, 0, Width - 1, Height - 1));
            }
        }
    }
}
