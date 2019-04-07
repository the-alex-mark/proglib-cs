using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib;

namespace ProgLib.Windows.Metro
{
    public partial class MetroTile : Control
    {
        public MetroTile()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(70, 70);
            _textAlign = ContentAlignment.BottomLeft;
            _number = 0;
            _numberSize = 28;
            _theme = Theme.Light;
            _styleColor = Drawing.MetroColors.Blue;
        }

        private ContentAlignment _textAlign;
        private Int32 _number, _numberSize;
        private Theme _theme;
        private Color _styleColor;

        [Category("Metro Appearance"), Description("Цветовая тема элемента управления")]
        public Theme Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                Invalidate();
            }
        }

        [Category("Metro Appearance"), Description("Цвет оформления при Checked равном \"true\"")]
        public Color StyleColor
        {
            get { return _styleColor; }
            set
            {
                _styleColor = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Выравнивание текста, который будет отображаться в данном элементе управления."), DefaultValue(ContentAlignment.BottomLeft)]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set
            {
                _textAlign = value;
                Invalidate();
            }
        }
        
        [Category("Metro Appearance"), Description("Номер Tile."), DefaultValue(0)]
        public Int32 Number
        {
            get { return _number; }
            set
            {
                _number = value;
                Invalidate();
            }
        }

        [Category("Metro Appearance"), Description("Размер текста с номером Tile."), DefaultValue(28)]
        public Int32 NumberSize
        {
            get { return _numberSize; }
            set
            {
                _numberSize = value;
                Invalidate();
            }
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(_styleColor);

            e.Graphics.DrawString(
                (_number != 0) ? _number.ToString() : "",
                new Font(Font.FontFamily, _numberSize, FontStyle.Regular),
                new SolidBrush((Enabled) ? MetroPaint.ForeColor.Tile.Normal(_theme) : MetroPaint.ForeColor.Tile.Disabled(_theme)),
                new Rectangle(0, 6, Width - 6, Height - (Height / 2)),
                new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near });
            
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(0, 0, Width - 1, Height - 1),
                (Enabled) ? MetroPaint.ForeColor.Tile.Normal(_theme) : MetroPaint.ForeColor.Tile.Disabled(_theme),
                _styleColor,
                _textAlign.ToTextFormatFlags() | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);
        }
    }
}