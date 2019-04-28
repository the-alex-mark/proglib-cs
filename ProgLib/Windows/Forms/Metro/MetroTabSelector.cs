using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Animations.Material;

namespace ProgLib.Windows.Forms.Metro
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public partial class MetroTabSelector : Control
    {
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        private MetroTabControl _baseTabControl;
        public MetroTabControl BaseTabControl
        {
            get { return _baseTabControl; }
            set
            {
                _baseTabControl = value;
                if (_baseTabControl == null) return;
                _previousSelectedTabIndex = _baseTabControl.SelectedIndex;
                _baseTabControl.Deselected += (sender, args) =>
                {
                    _previousSelectedTabIndex = _baseTabControl.SelectedIndex;
                };
                _baseTabControl.SelectedIndexChanged += (sender, args) =>
                {
                    _animationManager.SetProgress(0);
                    _animationManager.StartNewAnimation(AnimationDirection.In);
                };
                _baseTabControl.ControlAdded += delegate
                {
                    Invalidate();
                };
                _baseTabControl.ControlRemoved += delegate
                {
                    Invalidate();
                };
            }
        }

        private int _previousSelectedTabIndex;
        private Point _animationSource;
        private readonly AnimationManager _animationManager;

        private List<Rectangle> _tabRects;
        private const int TAB_HEADER_PADDING = 10;
        private const int TAB_INDICATOR_HEIGHT = 2;

        public MetroTabSelector()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            Height = 200;
            _theme = Theme.Dark;
            _styleColor = Drawing.MetroColors.Blue;

            _animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseOut,
                Increment = 0.04
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();
        }
        
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

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(MetroPaint.BackColor.Form(_theme));

            if (_baseTabControl == null) return;
            if (!_animationManager.IsAnimating() || _tabRects == null || _tabRects.Count != _baseTabControl.TabCount)
                UpdateTabRects();

            e.Graphics.DrawLine(
                new Pen((_theme == Theme.Dark) ? Color.FromArgb(68, 68, 68) : Color.FromArgb(204, 204, 204), 3),
                new Point(0, Height - 1),
                new Point(Width, Height - 1));

            // Отрисовка текста
            foreach (TabPage tabPage in _baseTabControl.TabPages)
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    tabPage.Text,
                    Font,
                    _tabRects[_baseTabControl.TabPages.IndexOf(tabPage)],
                    ForeColor,
                    BackColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);

                e.Graphics.SetClip(_tabRects[_baseTabControl.SelectedIndex]);
                e.Graphics.DrawLine(new Pen(_styleColor, 3), new Point(0, Height - 1), new Point(tabPage.Width, Height - 1));
                e.Graphics.ResetClip();

                tabPage.BackColor = MetroPaint.BackColor.Form(_theme);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_tabRects == null) UpdateTabRects();
            for (int i = 0; i < _tabRects.Count; i++)
            {
                if (_tabRects[i].Contains(e.Location))
                {
                    _baseTabControl.SelectedIndex = i;
                }
            }

            _animationSource = e.Location;
        }
        private void UpdateTabRects()
        {
            _tabRects = new List<Rectangle>();

            // Если отсутствует элемент управления базовой вкладки, ректы не должны вычисляться
            // Если в элементе управления "базовая вкладка" нет страниц вкладок, список должен быть пустым, который уже был установлен; выйдите из пустоты
            if (_baseTabControl == null || _baseTabControl.TabCount == 0) return;

            // Вычисление границ каждого заголовка вкладки, указанного в Базовом элементе управления вкладками
            using (Bitmap B = new Bitmap(1, 1))
            {
                using (Graphics G = Graphics.FromImage(B))
                {
                    _tabRects.Add(new Rectangle(0, 0, TAB_HEADER_PADDING * 2 + (int)G.MeasureString(_baseTabControl.TabPages[0].Text, Font).Width, Height));
                    for (int i = 1; i < _baseTabControl.TabPages.Count; i++)
                    {
                        _tabRects.Add(new Rectangle(_tabRects[i - 1].Right, 0, TAB_HEADER_PADDING * 2 + (int)G.MeasureString(_baseTabControl.TabPages[i].Text, Font).Width, Height));
                    }
                }
            }
        }
    }
}
