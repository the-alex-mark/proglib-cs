using ProgLib.Animations.Material;
using ProgLib.Drawing;
using ProgLib.Windows.Forms.Material;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.VSCode
{
    public class VSCodeTabSelector : Control
    {
        public VSCodeTabSelector()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            SelectColor = MetroColors.Blue;
            this.Theme = VSCodeTheme.Red;
            Height = 48;

            _animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseOut,
                Increment = 0.04
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();
        }

        #region Variables

        private TabControl _baseTabControl;

        private Int32 _previousSelectedTabIndex;
        private Point _animationSource;
        private readonly AnimationManager _animationManager;

        private List<Rectangle> _tabRects;
        private Color _selectColor;
        private VSCodeTheme _theme;
        private const Int32 TAB_HEADER_PADDING = 24;
        private const Int32 TAB_INDICATOR_HEIGHT = 2;

        #endregion

        #region Properties

        public Color SelectColor
        {
            get { return _selectColor; }
            set
            {
                _selectColor = value;
                Invalidate();
            }
        }

        public VSCodeTheme Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;

                SetTheme(_theme);
                Invalidate();
            }
        }

        public TabControl BaseTabControl
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

        #endregion

        #region Methods

        public Color SelectTabColor { get; private set; }
        public Color SelectForeColor { get; private set; }
        public Color TabColor { get; private set; }

        private void SetTheme(VSCodeTheme Theme)
        {
            switch (Theme)
            {
                case VSCodeTheme.QuietLight:
                    this.BackColor = Color.FromArgb(243, 243, 243);
                    this.ForeColor = Color.FromArgb(106, 106, 106);
                    this.SelectForeColor = Color.FromArgb(51, 51, 51);

                    this.TabColor = Color.FromArgb(236, 236, 236);
                    this.SelectTabColor = Color.FromArgb(245, 245, 245);
                    break;

                case VSCodeTheme.Red:
                    this.BackColor = Color.FromArgb(51, 0, 0);
                    this.ForeColor = Color.FromArgb(152, 133, 133);
                    this.SelectForeColor = Color.White;

                    this.TabColor = Color.FromArgb(48, 10, 10);
                    this.SelectTabColor = Color.FromArgb(73, 0, 0);
                    break;
            }
        }

        private void UpdateTabRects()
        {
            _tabRects = new List<Rectangle>();

            // Если нет базового элемента управления tab, rects не должны вычисляться
            // Если в элементе управления "базовая вкладка" нет страниц вкладок, список должен быть пустым, который уже установлен; выйдите из пустоты
            if (_baseTabControl == null || _baseTabControl.TabCount == 0) return;

            // Вычислить границы каждого заголовка вкладки, указанного в элементе управления базовая вкладка
            using (var b = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(b))
                {
                    _tabRects.Add(new Rectangle(0, 0, TAB_HEADER_PADDING * 2 + (int)g.MeasureString(_baseTabControl.TabPages[0].Text, Font).Width, Height));
                    for (int i = 1; i < _baseTabControl.TabPages.Count; i++)
                    {
                        _tabRects.Add(new Rectangle(_tabRects[i - 1].Right, 0, TAB_HEADER_PADDING * 2 + (int)g.MeasureString(_baseTabControl.TabPages[i].Text, Font).Width, Height));
                    }
                }
            }
        }

        #endregion

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_tabRects == null) UpdateTabRects();
            for (var i = 0; i < _tabRects.Count; i++)
            {
                if (_tabRects[i].Contains(e.Location))
                {
                    _baseTabControl.SelectedIndex = i;
                }
            }

            _animationSource = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (_baseTabControl == null) return;

            if (_tabRects == null || _tabRects.Count != _baseTabControl.TabCount)
                UpdateTabRects();
            
            foreach (TabPage tabPage in _baseTabControl.TabPages)
            {
                Int32 currentTabIndex = _baseTabControl.TabPages.IndexOf(tabPage);
                Rectangle currentRectangle = new Rectangle(_tabRects[currentTabIndex].X, 0, _tabRects[currentTabIndex].Width, Height);

                // Отрисовка фона
                e.Graphics.FillRectangle(new SolidBrush((_tabRects[_baseTabControl.SelectedIndex] == currentRectangle) ? SelectTabColor : TabColor), currentRectangle);

                // Отисовка иконки закрытия
                if (currentTabIndex == _baseTabControl.SelectedIndex)
                {
                    VSCodeControlBox ControlBox = new VSCodeControlBox(VSCodeIconTheme.Minimal);
                    e.Graphics.DrawImage(ControlBox.Close(this.Theme), new PointF((currentRectangle.X + currentRectangle.Width) - 16, (Height / 2) - 6));
                }

                // Отрисовка заголовка
                TextRenderer.DrawText(
                    e.Graphics,
                    tabPage.Text,
                    Font,
                    currentRectangle,
                    (currentTabIndex == _baseTabControl.SelectedIndex) ? SelectForeColor : ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis);
            }
        }
    }
}
