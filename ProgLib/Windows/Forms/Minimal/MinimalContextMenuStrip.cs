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
    public partial class MinimalContextMenuStrip : System.Windows.Forms.ContextMenuStrip
    {
        public MinimalContextMenuStrip()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            _foreColor = Color.Black;
            _backColor = SystemColors.ControlLight;
            _dropDownMenuBackColor = _backColor;
            _borderColor = Color.Black;
            _separatorColor = Color.FromArgb(255, 128, 128);
            _selectColor = Color.FromArgb(255, 128, 128);

            UpdateRenderer();
        }

        private MinimalContextMenuRenderer _minimalMenuRenderer;
        private Color _foreColor, _backColor, _dropDownMenuBackColor, _borderColor, _separatorColor, _selectColor;

        public new Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                UpdateRenderer();
            }
        }
        public new Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                UpdateRenderer();
            }
        }
        public Color DropDownMenuBackColor
        {
            get { return _dropDownMenuBackColor; }
            set
            {
                _dropDownMenuBackColor = value;
                UpdateRenderer();
            }
        }
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                UpdateRenderer();
            }
        }
        public Color SeparatorColor
        {
            get { return _separatorColor; }
            set
            {
                _separatorColor = value;
                UpdateRenderer();
            }
        }
        public Color SelectColor
        {
            get { return _selectColor; }
            set
            {
                _selectColor = value;
                UpdateRenderer();
            }
        }

        protected virtual void UpdateRenderer()
        {
            _minimalMenuRenderer = new MinimalContextMenuRenderer
            {
                ForeColor = _foreColor,
                BackColor = _backColor,
                DropDownMenuBackColor = _dropDownMenuBackColor,
                BorderColor = _borderColor,
                SeparatorColor = _separatorColor,
                SelectColor = _selectColor
            };

            Renderer = _minimalMenuRenderer;
            RenderMode = ToolStripRenderMode.ManagerRenderMode;
            ToolStripManager.Renderer = _minimalMenuRenderer;

            Invalidate();
        }
    }
}
