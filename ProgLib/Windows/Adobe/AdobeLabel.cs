using ProgLib.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Adobe
{
    public partial class AdobeLabel : Control
    {
        public AdobeLabel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(223, 25);
            ForeColor = SystemColors.ControlText;
            
            _caption = "";
            _text = Name;
            _captionWidth = 45;
            _radius = 5;
            _borderColor = SystemColors.ButtonShadow;
            _captionBackColor = Color.FromArgb(202, 202, 202);
            _textBackColor = Color.FromArgb(224, 224, 224);
            _captionColor = SystemColors.ControlText;
            _alignment = Alignment.Left;
            _showIcon = true;
        }

        private String _caption, _text;
        private Color _borderColor, _captionBackColor, _textBackColor, _captionColor;
        private Int32 _captionWidth, _radius;
        private Boolean _showIcon;
        private Alignment _alignment;

        [Category("Внешний вид"), Description("Название")]
        public String Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                Invalidate();
            }
        }
        public override String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Цвет обводки элемента управления")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Цвет фона названия")]
        public Color CaptionBackColor
        {
            get { return _captionBackColor; }
            set
            {
                _captionBackColor = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Цвет фона текста")]
        public Color TextBackColor
        {
            get { return _textBackColor; }
            set
            {
                _textBackColor = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Цвет фона текста")]
        public Color CaptionColor
        {
            get { return _captionColor; }
            set
            {
                _captionColor = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Длина названия")]
        public Int32 CaptionWidth
        {
            get { return _captionWidth; }
            set
            {
                _captionWidth = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Радиус округления углов элемента управления")]
        public Int32 BorderRadius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Положение текста")]
        public Alignment Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                Invalidate();
            }
        }

        [Category("Внешний вид"), Description("Отображения иконки копирования текста")]
        public Boolean ShowIcon
        {
            get { return _showIcon; }
            set
            {
                _showIcon = value;
                Invalidate();
            }
        }

        protected virtual GraphicsPath Ellipse(Radius Radius, Rectangle Rectangle)
        {
            GraphicsPath GP = new GraphicsPath();

            if (Radius.LeftTop != 0)
                GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, Radius.LeftTop * 2, Radius.LeftTop * 2), 180, 90);
            else GP.AddLine(new Point(Rectangle.X, Rectangle.Y), new Point(Rectangle.X, Rectangle.Y));

            if (Radius.RightTop != 0)
                GP.AddArc(new Rectangle(Rectangle.Width - Radius.RightTop * 2, Rectangle.Y, Radius.RightTop * 2, Radius.RightTop * 2), 270, 90);
            else GP.AddLine(new Point(Rectangle.Width, Rectangle.Y), new Point(Rectangle.Width, Rectangle.Y));

            if (Radius.RightBottom != 0)
                GP.AddArc(new Rectangle(Rectangle.Width - Radius.RightBottom * 2, Rectangle.Height - Radius.RightBottom * 2, Radius.RightBottom * 2, Radius.RightBottom * 2), 0, 90);
            else GP.AddLine(new Point(Rectangle.Width, Rectangle.Height), new Point(Rectangle.Width, Rectangle.Height));

            if (Radius.LeftBottom != 0)
                GP.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - Radius.LeftBottom * 2, Radius.LeftBottom * 2, Radius.LeftBottom * 2), 90, 90);
            else GP.AddLine(new Point(Rectangle.X, Rectangle.Height), new Point(Rectangle.X, Rectangle.Height));

            GP.CloseFigure();

            return GP;
        }
        protected virtual Image Copy(Color Border)
        {
            Bitmap Image = new Bitmap(18, 18);
            using (Graphics G = Graphics.FromImage(Image))
            {
                G.Clear(Color.Transparent);

                G.DrawRectangle(new Pen(Border, 1), new Rectangle(6, 4, 6, 7));
                //for (int X = 5; X < 11; X++)
                //{
                //    for (int Y = 6; Y < 13; Y++)
                //    {
                //        if (Border == Image.GetPixel(X, Y))
                //            Image.SetPixel(X, Y, Color.Transparent);
                //    }
                //}
                G.FillRectangle(new SolidBrush(_textBackColor), new Rectangle(4, 6, 6, 7));
                G.DrawRectangle(new Pen(Border, 1), new Rectangle(4, 6, 6, 7));

                //G.DrawRectangle(new Pen(Color.Red), new Rectangle(0, 0, 17, 17));
            }

            return Image;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_showIcon)
            {
                if (new Rectangle(Width - 21, (Height / 2) - 9, 18, 18).Contains(PointToClient(Cursor.Position)) && e.Button == MouseButtons.Left)
                {
                    if (Text != "" && Text != null)
                        Clipboard.SetText(Text);
                }
            }

            base.OnMouseDown(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.FillPath(new SolidBrush(_captionBackColor), Ellipse(new Radius(_radius, 0, 0, _radius), new Rectangle(0, 0, _captionWidth + 3, Height - 1)));
            e.Graphics.DrawString(_caption, new Font(Font.FontFamily, Font.Size, FontStyle.Bold), new SolidBrush(_captionColor), new Rectangle(0, 0, _captionWidth + 3, Height - 1), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            e.Graphics.FillPath(new SolidBrush(_textBackColor), Ellipse(new Radius(0, _radius, _radius, 0), new Rectangle(_captionWidth + 2, 0, Width - 1, Height - 1)));
            e.Graphics.DrawString(_text, Font, new SolidBrush(ForeColor), new Rectangle(_captionWidth + 8, 0, Width - _captionWidth - 13, Height - 1), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = (StringAlignment)_alignment });

            if (_showIcon)
                e.Graphics.DrawImage(Copy(_borderColor), new Point(Width - 21, (Height / 2) - 9));

            e.Graphics.DrawLine(new Pen(_borderColor, 1), new Point(_captionWidth + 2, 0), new Point(_captionWidth + 2, Height - 1));
            e.Graphics.DrawPath(new Pen(_borderColor, 1), Ellipse(new Radius(_radius, _radius, _radius, _radius), new Rectangle(0, 0, Width - 1, Height - 1)));
        }
    }
}
