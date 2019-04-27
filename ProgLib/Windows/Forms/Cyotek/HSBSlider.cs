using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Drawing;

namespace ProgLib.Windows.Forms.Cyotek
{
    public partial class HSBSlider : UserControl
    {
        public HSBSlider()
        {
            InitializeComponent();

            Width = 243;
            Height = 205;

            Color = Color.Red;
        }

        public Color Color
        {
            get { return new HSB(SBSlider.Hue, (double)SBSlider.Saturation / 100, (double)SBSlider.Brightness / 100).ToColor(); }
            set
            {
                HueSlider.Value = value.ToHSB().H;
                
                SBSlider.Hue = value.ToHSB().H;
                SBSlider.Saturation = (int)(value.ToHSB().S * 100);
                SBSlider.Brightness = (int)(value.ToHSB().B * 100);
            }
        }

        private void HueSlider_Scroll(Object sender, ScrollEventArgs e)
        {
            SBSlider.Hue = HueSlider.Value;
        }

        private void SBSlider_Resize(Object sender, EventArgs e)
        {
            SBSlider.Size = new Size(HueSlider.Height - 5, HueSlider.Height - 5);
        }

        private void HSBSlider_Resize(Object sender, EventArgs e)
        {
            if (Width >= (SBSlider.Width + HueSlider.Width + 5 + 10))
            {
                
            }
            else { Width = (SBSlider.Width + HueSlider.Width + 5 + 10); }

            HueSlider.Location = new Point(SBSlider.Width + 5 + 5, 3);
        }

        private void HueSlider_Resize(Object sender, EventArgs e)
        {
            SBSlider.Size = new Size(HueSlider.Height - 5, HueSlider.Height - 5);
        }
    }
}
