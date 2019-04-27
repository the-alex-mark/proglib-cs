using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Cyotek
{
    [DefaultProperty("Color")]
    [DefaultEvent("ColorChanged")]
    public class ScreenColorPicker : Control
    {
        #region Constants

        private static readonly Object _eventColorChanged = new Object();

        private static readonly Object _eventGridColorChanged = new Object();

        private static readonly Object _eventImageChanged = new Object();

        private static readonly Object _eventShowGridChanged = new Object();

        private static readonly Object _eventShowTextWithSnapshotChanged = new Object();

        private static readonly Object _eventZoomChanged = new Object();

        #endregion

        #region Fields

        private Color _color;

        private Color _gridColor;

        private Image _image;

        private Boolean _showGrid;

        private Boolean _showTextWithSnapshot;

        private int _zoom;

        #endregion

        #region Constructors

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref= "ScreenColorPicker"/>.
        /// </summary>
        public ScreenColorPicker()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, false);
            this.Zoom = 8;
            this.Color = Color.Empty;
            this.ShowTextWithSnapshot = false;
            this.TabStop = false;
            this.TabIndex = 0;
            this.ShowGrid = true;
            this.GridColor = SystemColors.ControlDark;
        }

        #endregion

        #region Events

        [Category("Property Changed")]
        public event EventHandler GridColorChanged
        {
            add { this.Events.AddHandler(_eventGridColorChanged, value); }
            remove { this.Events.RemoveHandler(_eventGridColorChanged, value); }
        }

        [Category("Property Changed")]
        public event EventHandler ImageChanged
        {
            add { this.Events.AddHandler(_eventImageChanged, value); }
            remove { this.Events.RemoveHandler(_eventImageChanged, value); }
        }

        [Category("Property Changed")]
        public event EventHandler ShowGridChanged
        {
            add { this.Events.AddHandler(_eventShowGridChanged, value); }
            remove { this.Events.RemoveHandler(_eventShowGridChanged, value); }
        }

        [Category("Property Changed")]
        public event EventHandler ShowTextWithSnapshotChanged
        {
            add { this.Events.AddHandler(_eventShowTextWithSnapshotChanged, value); }
            remove { this.Events.RemoveHandler(_eventShowTextWithSnapshotChanged, value); }
        }

        [Category("Property Changed")]
        public event EventHandler ZoomChanged
        {
            add { this.Events.AddHandler(_eventZoomChanged, value); }
            remove { this.Events.RemoveHandler(_eventZoomChanged, value); }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает или задает цвет сетки.
        /// </summary>
        [Category("Appearance"), Description("Возвращает или задает цвет сетки.")]
        [DefaultValue(typeof(Color), "ControlDark")]
        public virtual Color GridColor
        {
            get { return _gridColor; }
            set
            {
                if (this.GridColor != value)
                {
                    _gridColor = value;

                    this.OnGridColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Возвращает, если снимок доступен.
        /// </summary>
        [Browsable(false)]
        public Boolean HasSnapshot { get; protected set; }

        /// <summary>
        /// Возвращает или задает изображение.
        /// </summary>
        [Category("Appearance"), Description("Возвращает или задает изображение.")]
        [DefaultValue(typeof(Image), null)]
        public virtual Image Image
        {
            get { return _image; }
            set
            {
                if (this.Image != value)
                {
                    _image = value;

                    this.OnImageChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее, отображается ли пиксельная сетка.
        /// </summary>
        [Category("Appearance"), Description("Возвращает или задает значение, указывающее, отображается ли пиксельная сетка.")]
        [DefaultValue(true)]
        public virtual Boolean ShowGrid
        {
            get { return _showGrid; }
            set
            {
                if (this.ShowGrid != value)
                {
                    _showGrid = value;

                    this.OnShowGridChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее, должен ли отображаться текст при наличии моментального снимка.
        /// </summary>
        [Category("Appearance"), Description("Возвращает или задает значение, указывающее, должен ли отображаться текст при наличии моментального снимка.")]
        [DefaultValue(false)]
        public virtual Boolean ShowTextWithSnapshot
        {
            get { return _showTextWithSnapshot; }
            set
            {
                if (this.ShowTextWithSnapshot != value)
                {
                    _showTextWithSnapshot = value;

                    this.OnShowTextWithSnapshotChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Получает или задает порядок управления внутри контейнера.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(0)]
        public new Int32 TabIndex
        {
            get { return base.TabIndex; }
            set { base.TabIndex = value; }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее, может ли пользователь предоставить фокус этому элементу управления с помощью клавиши TAB.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false)]
        public new Boolean TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        /// <summary>
        /// Возвращает или задает уровень масштабирования снимка.
        /// </summary>
        [Category("Appearance"), Description("Возвращает или задает уровень масштабирования снимка.")]
        [DefaultValue(8)]
        public virtual Int32 Zoom
        {
            get { return _zoom; }
            set
            {
                if (this.Zoom != value)
                {
                    _zoom = value;

                    this.OnZoomChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее, что выполняется захват моментального снимка.
        /// </summary>
        protected Boolean IsCapturing { get; set; }

        /// <summary>
        /// Возвращает или задает значение, указывающее, следует ли выполнять операции перерисовки.
        /// </summary>
        protected Boolean LockUpdates { get; set; }

        /// <summary>
        /// Возвращает или задает образ моментального снимка.
        /// </summary>
        protected Bitmap SnapshotImage { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Создает снимок изображения.
        /// </summary>
        protected virtual void CreateSnapshotImage()
        {
            Size size;

            if (this.SnapshotImage != null)
            {
                this.SnapshotImage.Dispose();
                this.SnapshotImage = null;
            }

            size = this.GetSnapshotSize();
            if (!size.IsEmpty)
            {
                this.SnapshotImage = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Освобождает неуправляемые ресурсы, используемые <see cref="T:System.Windows.Forms.Control" /> и его дочерние элементы управления и при необходимости освобождает управляемые ресурсы.
        /// </summary>
        /// <param name="disposing">true, чтобы освободить управляемые и неуправляемые ресурсы; значение false, чтобы освободить только неуправляемые ресурсы.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (this.SnapshotImage != null)
                {
                    this.SnapshotImage.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Возвращает центральную точку на основе текущего уровня масштабирования.
        /// </summary>
        protected virtual Point GetCenterPoint()
        {
            int x;
            int y;

            x = this.ClientSize.Width / this.Zoom / 2;
            y = this.ClientSize.Height / this.Zoom / 2;

            return new Point(x, y);
        }

        /// <summary>
        /// Возвращает размер моментального снимка.
        /// </summary>
        protected virtual Size GetSnapshotSize()
        {
            int snapshotWidth;
            int snapshotHeight;

            snapshotWidth = (int)Math.Ceiling(this.ClientSize.Width / (double)this.Zoom);
            snapshotHeight = (int)Math.Ceiling(this.ClientSize.Height / (double)this.Zoom);

            return snapshotHeight != 0 && snapshotWidth != 0 ? new Size(snapshotWidth, snapshotHeight) : Size.Empty;
        }

        /// <summary>
        /// Вызывает событие <see cref="ColorChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnColorChanged(EventArgs e)
        {
            EventHandler handler;

            handler = (EventHandler)this.Events[_eventColorChanged];

            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.FontChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            this.Invalidate();
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.ForeColorChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            this.Invalidate();
        }

        /// <summary>
        /// Вызывает событие <see cref="GridColorChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGridColorChanged(EventArgs e)
        {
            EventHandler handler;

            this.Invalidate();

            handler = (EventHandler)this.Events[_eventGridColorChanged];

            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Вызывает событие <see cref="ImageChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnImageChanged(EventArgs e)
        {
            EventHandler handler;

            this.Invalidate();

            handler = (EventHandler)this.Events[_eventImageChanged];

            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.MouseDown"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && !this.IsCapturing)
            {
                //if (_eyedropperCursor == null)
                //{
                //    // ReSharper disable AssignNullToNotNullAttribute
                //    _eyedropperCursor = new Cursor(this.GetType().Assembly.GetManifestResourceStream(string.Concat(this.GetType().Namespace, ".Resources.eyedropper.cur")));
                //}
                // ReSharper restore AssignNullToNotNullAttribute

                //this.Cursor = _eyedropperCursor;
                this.IsCapturing = true;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.MouseMove" />.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.IsCapturing)
            {
                this.UpdateSnapshot();
            }
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.MouseUp" />.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (this.IsCapturing)
            {
                this.Cursor = Cursors.Default;
                this.IsCapturing = false;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.Paint" />.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //this.UpdateSnapshot();

            this.OnPaintBackground(e); // HACK: Самый простой способ поддержки таких вещей, как BackgroundImage, BackgroundImageLayout и т. д

            // draw the current snapshot, if present
            if (this.SnapshotImage != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                e.Graphics.DrawImage(this.SnapshotImage, new Rectangle(0, 0, this.SnapshotImage.Width * this.Zoom, this.SnapshotImage.Height * this.Zoom), new Rectangle(Point.Empty, this.SnapshotImage.Size), GraphicsUnit.Pixel);
            }

            this.PaintAdornments(e);
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.Resize"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.CreateSnapshotImage();
        }

        /// <summary>
        /// Вызывает событие <see cref="ShowGridChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnShowGridChanged(EventArgs e)
        {
            EventHandler handler;

            this.Invalidate();

            handler = (EventHandler)this.Events[_eventShowGridChanged];

            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Вызывает событие <see cref="ShowTextWithSnapshotChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnShowTextWithSnapshotChanged(EventArgs e)
        {
            EventHandler handler;

            this.Invalidate();

            handler = (EventHandler)this.Events[_eventShowTextWithSnapshotChanged];

            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Вызывает событие <see cref="E:System.Windows.Forms.Control.TextChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            this.Invalidate();
        }

        /// <summary>
        /// Вызывает событие <see cref="ZoomChanged"/>.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnZoomChanged(EventArgs e)
        {
            EventHandler handler;

            this.CreateSnapshotImage();

            handler = (EventHandler)this.Events[_eventZoomChanged];

            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Рисует украшения на элементе управления.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void PaintAdornments(PaintEventArgs e)
        {
            // Сетка
            if (this.ShowGrid)
            {
                this.PaintGrid(e);
            }

            // center marker
            if (this.HasSnapshot)
            {
                this.PaintCenterMarker(e);
            }

            // image
            if (this.Image != null && (!this.HasSnapshot || this.ShowTextWithSnapshot))
            {
                e.Graphics.DrawImage(this.Image, (this.ClientSize.Width - this.Image.Size.Width) / 2, (this.ClientSize.Height - this.Image.Size.Height) / 2);
            }

            // draw text
            if (!string.IsNullOrEmpty(this.Text) && (!this.HasSnapshot || this.ShowTextWithSnapshot))
            {
                TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor, this.BackColor, TextFormatFlags.ExpandTabs | TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis);
            }
        }

        /// <summary>
        /// Рисует центральный маркер.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void PaintCenterMarker(PaintEventArgs e)
        {
            Point center;

            center = this.GetCenterPoint();

            using (Pen pen = new Pen(this.ForeColor))
            {
                e.Graphics.DrawRectangle(pen, center.X * this.Zoom, center.Y * this.Zoom, this.Zoom + 2, this.Zoom + 2);
            }
        }

        /// <summary>
        /// Рисует пиксельную сетку.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void PaintGrid(PaintEventArgs e)
        {
            Rectangle viewport;
            int pixelSize;

            pixelSize = this.Zoom;
            viewport = this.ClientRectangle;

            using (Pen pen = new Pen(this.GridColor)
            {
                DashStyle = DashStyle.Dot
            })
            {
                for (int x = viewport.Left + 1; x < viewport.Right; x += pixelSize)
                {
                    e.Graphics.DrawLine(pen, x, viewport.Top, x, viewport.Bottom);
                }

                for (int y = viewport.Top + 1; y < viewport.Bottom; y += pixelSize)
                {
                    e.Graphics.DrawLine(pen, viewport.Left, y, viewport.Right, y);
                }

                e.Graphics.DrawRectangle(pen, viewport);
            }
        }

        /// <summary>
        /// Обновления моментального снимка.
        /// </summary>
        protected virtual void UpdateSnapshot()
        {
            Point cursor;

            cursor = MousePosition;
            cursor.X -= this.SnapshotImage.Width / 2;
            cursor.Y -= this.SnapshotImage.Height / 2;

            using (Graphics graphics = Graphics.FromImage(this.SnapshotImage))
            {
                Point center;

                // Сначала очистите изображение, если мышь находится вблизи границ экрана, поэтому для заполнения области недостаточно содержимого копии
                graphics.Clear(Color.Empty);

                // Копирование изображения с экрана
                graphics.CopyFromScreen(cursor, Point.Empty, this.SnapshotImage.Size);

                // Обновление активного цвета
                center = this.GetCenterPoint();
                this.Color = this.SnapshotImage.GetPixel(center.X, center.Y);

                // Принудительная перерисовка
                this.HasSnapshot = true;
                this.Refresh(); // Просто вызов Invalidate недостаточно, так как дисплей будет отставать
            }
        }

        #endregion

        #region IColorEditor Interface

        [Category("Property Changed")]
        public event EventHandler ColorChanged
        {
            add { this.Events.AddHandler(_eventColorChanged, value); }
            remove { this.Events.RemoveHandler(_eventColorChanged, value); }
        }

        /// <summary>
        /// Возвращает или задает цвет компонента.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(Color), "Empty")]
        public virtual Color Color
        {
            get { return _color; }
            set
            {
                if (this.Color != value)
                {
                    _color = value;

                    this.OnColorChanged(EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}
