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

namespace ProgLib.Windows.Forms.Material
{
    [ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public partial class MaterialTabSelector : Control
    {
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        private MaterialTabControl _baseTabControl;
        public MaterialTabControl BaseTabControl
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
        private const int TAB_HEADER_PADDING = 24;
        private const int TAB_INDICATOR_HEIGHT = 2;

        public MaterialTabSelector()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            Height = 200;
            _IndicatorColor = Color.FromArgb(255, 128, 128);
            _animateColor = Color.FromArgb(255, 128, 128);

            _animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseOut,
                Increment = 0.04
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();
        }

        private Color _IndicatorColor, _animateColor;

        [Category("Appearance"), Description("Цвет индикатора вкладок")]
        public Color IndicatorColor
        {
            get { return _IndicatorColor; }
            set
            {
                _IndicatorColor = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("Цвет анимации")]
        public Color AnimateColor
        {
            get { return _animateColor; }
            set
            {
                _animateColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            if (_baseTabControl == null) return;
            if (!_animationManager.IsAnimating() || _tabRects == null || _tabRects.Count != _baseTabControl.TabCount)
                UpdateTabRects();

            Double animationProgress = _animationManager.GetProgress();

            // Отрисовка анимации
            if (_animationManager.IsAnimating())
            {
                Int32 rippleSize = (int)(animationProgress * _tabRects[_baseTabControl.SelectedIndex].Width * 1.75);

                e.Graphics.SetClip(_tabRects[_baseTabControl.SelectedIndex]);
                e.Graphics.FillEllipse(
                    new SolidBrush(Color.FromArgb((int)(51 - (animationProgress * 50)), _animateColor)),
                    new Rectangle(_animationSource.X - rippleSize / 2, _animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                e.Graphics.ResetClip();
            }

            // Отрисовка текста
            foreach (TabPage tabPage in _baseTabControl.TabPages)
            {
                e.Graphics.DrawString(
                    tabPage.Text,
                    Font,
                    new SolidBrush(ForeColor),
                    _tabRects[_baseTabControl.TabPages.IndexOf(tabPage)],
                    new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

            // Анимация индикатора вкладок
            Int32 previousSelectedTabIndexIfHasOne = _previousSelectedTabIndex == -1 ? _baseTabControl.SelectedIndex : _previousSelectedTabIndex;
            Rectangle previousActiveTabRect = _tabRects[previousSelectedTabIndexIfHasOne];
            Rectangle activeTabPageRect = _tabRects[_baseTabControl.SelectedIndex];

            e.Graphics.FillRectangle(
                new SolidBrush(_IndicatorColor),
                previousActiveTabRect.X + (int)((activeTabPageRect.X - previousActiveTabRect.X) * animationProgress),
                activeTabPageRect.Bottom - 2,
                previousActiveTabRect.Width + (int)((activeTabPageRect.Width - previousActiveTabRect.Width) * animationProgress),
                TAB_INDICATOR_HEIGHT);
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
