using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProgLib.Windows.Metro
{
    [ToolboxBitmap(typeof(System.Windows.Forms.Label))]
    public partial class MetroLabel : System.Windows.Forms.Label
    {
        public MetroLabel()
        {
            InitializeComponent();

            _theme = Theme.Light;
            _useStyleColor = false;
            _styleColor = Drawing.MetroColors.Blue;
        }

        private Theme _theme;
        private Color _styleColor;
        private Boolean _useStyleColor;

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

        [Category("Metro Appearance"), Description("Цвет оформления при Checked равном \"true\"")]
        public Boolean UseStyleColor
        {
            get { return _useStyleColor; }
            set
            {
                _useStyleColor = value;
                Invalidate();
            }
        }

        protected virtual TextFormatFlags AsTextFormatFlags(ContentAlignment Alignment)
        {
            switch (Alignment)
            {
                case ContentAlignment.BottomLeft: return TextFormatFlags.Bottom | TextFormatFlags.Left;
                case ContentAlignment.BottomCenter: return TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.BottomRight: return TextFormatFlags.Bottom | TextFormatFlags.Right;
                case ContentAlignment.MiddleLeft: return TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                case ContentAlignment.MiddleCenter: return TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.MiddleRight: return TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                case ContentAlignment.TopLeft: return TextFormatFlags.Top | TextFormatFlags.Left;
                case ContentAlignment.TopCenter: return TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                case ContentAlignment.TopRight: return TextFormatFlags.Top | TextFormatFlags.Right;
            }
            throw new InvalidEnumArgumentException();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                new Rectangle(0, 0, Width - 1, Height - 1),
                (Enabled) ? (_useStyleColor) ? _styleColor : MetroPaint.ForeColor.Label.Normal(_theme) : MetroPaint.ForeColor.Label.Disabled(_theme),
                BackColor,
                AsTextFormatFlags(TextAlign) | TextFormatFlags.EndEllipsis);
        }
    }
}