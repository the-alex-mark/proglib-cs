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
    [DefaultEvent("ValueChanged")]
    public partial class AdobeNumericUpDown : Control
    {
        public TextBox TB = new TextBox();
        public AdobeNumericUpDown()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            
            _value = 0;
            _minimum = 0;
            _maximum = 100;
            _caption = "";
            _captionWidth = 20;
            _radius = 5;
            _borderColor = SystemColors.ButtonShadow;
            _captionBackColor = Color.FromArgb(202, 202, 202);
            _textBackColor = Color.FromArgb(224, 224, 224);
            _captionColor = SystemColors.ControlText;
            _alignment = Alignment.Left;
            
            TB.Parent = this;
            Controls.Add(TB);

            TB.BorderStyle = BorderStyle.None;
            TB.TextAlign = HorizontalAlignment.Left;
            TB.Font = Font;

            ForeColor = SystemColors.ControlText;
            TB.BackColor = _textBackColor;
            Text = null;
            Size = new Size(70, 26);
            DoubleBuffered = true;
            TB.KeyPress += box_KeyPress;
            TB.TextChanged += box_TextChanged;
            TB.MouseDoubleClick += box_MouseDoubleClick;
        }

        private String _caption;
        private Color _borderColor, _captionBackColor, _textBackColor, _captionColor;
        private Int32 _captionWidth, _radius, _value, _minimum, _maximum;
        private Alignment _alignment;
        public event EventHandler ValueChanged;
        
        [Browsable(false), DefaultValue("0")]
        public new String Text
        {
            get;
            set;
        }
        public String Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                Invalidate();
            }
        }
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }
        public Color CaptionBackColor
        {
            get { return _captionBackColor; }
            set
            {
                _captionBackColor = value;
                Invalidate();
            }
        }
        public Color TextBackColor
        {
            get { return _textBackColor; }
            set
            {
                _textBackColor = value;
                Invalidate();
            }
        }
        public Color CaptionColor
        {
            get { return _captionColor; }
            set
            {
                _captionColor = value;
                Invalidate();
            }
        }
        public Int32 CaptionWidth
        {
            get { return _captionWidth; }
            set
            {
                _captionWidth = value;
                Invalidate();
            }
        }
        public Int32 BorderRadius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                Invalidate();
            }
        }
        public Alignment Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                Invalidate();
            }
        }
        [DefaultValue("0")]
        public Int32 Value
        {
            get { return _value; }
            set
            {
                if (value >= Minimum && value <= Maximum)
                {
                    _value = value;
                    TB.Text = Text = _value.ToString();
                }

                OnValueChanged(new EventArgs());
                Invalidate();
            }
        }
        public Int32 Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                Invalidate();
            }
        }
        public Int32 Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                Invalidate();
            }
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
        
        void box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;

            TB.SelectAll();
        }

        void box_TextChanged(object sender, EventArgs e)
        {
            if (TB.Text != "")
                Value = Math.Max(Minimum, Math.Min(Maximum, System.Convert.ToInt32(TB.Text)));
        }
        public void SelectAll()
        {
            TB.SelectAll();
        }
        //private void box_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.A)
        //    {
        //        TB.SelectionStart = 0;
        //        TB.SelectionLength = Text.Length;
        //    }
        //}
        private void box_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.Handled = !Char.IsDigit(e.KeyChar))
            //    MessageBox.Show("Нажата неверная кнопка!");

            if (e.KeyChar != 8 && e.KeyChar != 46 && (e.KeyChar < 48 || e.KeyChar > 57))
            {
                e.Handled = true;
            }
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            TB.Text = Value.ToString();
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            TB.Font = Font;
            Invalidate();
        }
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            TB.ForeColor = ForeColor;
            Invalidate();
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (new Rectangle(Width - 15, 1, 11, (Height / 2) - 1).Contains(e.Location))
                {
                    if (System.Convert.ToInt32(Text) < Maximum)
                        Value = (Value + 1);
                }

                if (new Rectangle(Width - 15, (Height / 2), 11, (Height / 2) - 2).Contains(e.Location))
                {
                    if (System.Convert.ToInt32(Text) > Minimum)
                        Value = (Value - 1);
                }
            }

            base.OnMouseDown(e);
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
        protected virtual Image Arrow(Color Border)
        {
            Bitmap Image = new Bitmap(7, 6);
            using (Graphics G = Graphics.FromImage(Image))
            {
                G.Clear(Color.Transparent);

                Point[] Lines = new Point[]
                {
                    new Point(1, 1),
                    new Point(3, 3),
                    new Point(5, 1)
                };
                G.DrawLines(new Pen(Border, 1), Lines);
                //G.DrawRectangle(new Pen(Color.Red), new Rectangle(0, 0, 17, 17));
            }

            return Image;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            e.Graphics.FillPath(new SolidBrush(_captionBackColor), Ellipse(new Radius(_radius, 0, 0, _radius), new Rectangle(0, 0, _captionWidth + 3, Height - 1)));
            e.Graphics.DrawString(_caption, new Font(Font.FontFamily, Font.Size, FontStyle.Bold), new SolidBrush(_captionColor), new Rectangle(0, 0, _captionWidth + 3, Height - 1), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            e.Graphics.FillPath(new SolidBrush(_textBackColor), Ellipse(new Radius(0, _radius, _radius, 0), new Rectangle(_captionWidth + 2, 0, Width - 1, Height - 1)));


            e.Graphics.DrawLine(new Pen(_borderColor, 1), new Point(_captionWidth + 2, 0), new Point(_captionWidth + 2, Height - 1));
            e.Graphics.DrawPath(new Pen(_borderColor, 1), Ellipse(new Radius(_radius, _radius, _radius, _radius), new Rectangle(0, 0, Width - 1, Height - 1)));



            if (TB.Height >= Height - 4) Height = TB.Height + 4;
            TB.Location = new Point(_captionWidth + 8, Height / 2 - TB.Font.Height / 2);
            TB.Width = Width - (_captionWidth + 24);

            TB.TextAlign = (HorizontalAlignment)_alignment;

            Image IMG = Arrow(ForeColor);
            e.Graphics.DrawImage(IMG, new Point(Width - 13, (Height / 2) + 1));
            IMG.RotateFlip(RotateFlipType.Rotate180FlipNone);
            e.Graphics.DrawImage(IMG, new Point(Width - 13, (Height / 2) - 7));

            base.OnPaint(e);
        }
    }
}
