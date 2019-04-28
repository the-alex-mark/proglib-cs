using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms.Metro
{
    public class MetroCollection
    {
        public static void Update(Theme Theme, params Control[] Controls)
        {
            System.Threading.Thread T = new System.Threading.Thread(delegate ()
            {
                List<MetroButton> MetroButtons = new List<MetroButton>();
                List<MetroCheckBox> MetroCheckBoxs = new List<MetroCheckBox>();
                List<MetroTabSelector> MetroTabSelectors = new List<MetroTabSelector>();
                List<MetroTile> MetroTiles = new List<MetroTile>();
                List<MetroToggle> MetroToggles = new List<MetroToggle>();
                List<MetroRadioButton> MetroRadioButtons = new List<MetroRadioButton>();

                foreach (Control Control in Controls)
                {
                    try { MetroButtons.Add((MetroButton)Control); } catch { }
                    try { MetroCheckBoxs.Add((MetroCheckBox)Control); } catch { }
                    try { MetroTabSelectors.Add((MetroTabSelector)Control); } catch { }
                    try { MetroTiles.Add((MetroTile)Control); } catch { }
                    try { MetroToggles.Add((MetroToggle)Control); } catch { }
                    try { MetroRadioButtons.Add((MetroRadioButton)Control); } catch { }
                }

                Update(Theme, MetroButtons.ToArray());
                Update(Theme, MetroCheckBoxs.ToArray());
                Update(Theme, MetroTabSelectors.ToArray());
                Update(Theme, MetroTiles.ToArray());
                Update(Theme, MetroToggles.ToArray());
                Update(Theme, MetroRadioButtons.ToArray());
            });
            T.Start();
        }
        public static void Update(Color StyleColor, params Control[] Controls)
        {
            System.Threading.Thread T = new System.Threading.Thread(delegate ()
            {
                List<MetroButton> MetroButtons = new List<MetroButton>();
                List<MetroCheckBox> MetroCheckBoxs = new List<MetroCheckBox>();
                List<MetroTabSelector> MetroTabSelectors = new List<MetroTabSelector>();
                List<MetroTile> MetroTiles = new List<MetroTile>();
                List<MetroToggle> MetroToggles = new List<MetroToggle>();
                List<MetroRadioButton> MetroRadioButtons = new List<MetroRadioButton>();

                foreach (Control Control in Controls)
                {
                    try { MetroButtons.Add((MetroButton)Control); } catch { }
                    try { MetroCheckBoxs.Add((MetroCheckBox)Control); } catch { }
                    try { MetroTabSelectors.Add((MetroTabSelector)Control); } catch { }
                    try { MetroTiles.Add((MetroTile)Control); } catch { }
                    try { MetroToggles.Add((MetroToggle)Control); } catch { }
                    try { MetroRadioButtons.Add((MetroRadioButton)Control); } catch { }
                }

                Update(StyleColor, MetroButtons.ToArray());
                Update(StyleColor, MetroCheckBoxs.ToArray());
                Update(StyleColor, MetroTabSelectors.ToArray());
                Update(StyleColor, MetroTiles.ToArray());
                Update(StyleColor, MetroToggles.ToArray());
                Update(StyleColor, MetroRadioButtons.ToArray());
            });
            T.Start();
        }
        public static void Update(Theme Theme, Color StyleColor, params Control[] Controls)
        {
            Update(Theme, Controls);
            Update(StyleColor, Controls);
        }

        public static void Update(Theme Theme, params MetroButton[] Controls)
        {
            foreach (MetroButton Control in Controls)
            {
                Control.Theme = Theme;
            }
        }
        public static void Update(Color StyleColor, params MetroButton[] Controls)
        {
            foreach (MetroButton Control in Controls)
            {
                Control.StyleColor = StyleColor;
            }
        }
        public static void Update(Theme Theme, Color StyleColor, params MetroButton[] Controls)
        {
            foreach (MetroButton Control in Controls)
            {
                Control.Theme = Theme;
                Control.StyleColor = StyleColor;
            }
        }

        public static void Update(Theme Theme, params MetroCheckBox[] Controls)
        {
            foreach (MetroCheckBox Control in Controls)
            {
                Control.Theme = Theme;
            }
        }
        public static void Update(Color StyleColor, params MetroCheckBox[] Controls)
        {
            foreach (MetroCheckBox Control in Controls)
            {
                Control.StyleColor = StyleColor;
            }
        }
        public static void Update(Theme Theme, Color StyleColor, params MetroCheckBox[] Controls)
        {
            foreach (MetroCheckBox Control in Controls)
            {
                Control.Theme = Theme;
                Control.StyleColor = StyleColor;
            }
        }

        public static void Update(Theme Theme, params MetroTabSelector[] Controls)
        {
            foreach (MetroTabSelector Control in Controls)
            {
                Control.Theme = Theme;
            }
        }
        public static void Update(Color StyleColor, params MetroTabSelector[] Controls)
        {
            foreach (MetroTabSelector Control in Controls)
            {
                Control.StyleColor = StyleColor;
            }
        }
        public static void Update(Theme Theme, Color StyleColor, params MetroTabSelector[] Controls)
        {
            foreach (MetroTabSelector Control in Controls)
            {
                Control.Theme = Theme;
                Control.StyleColor = StyleColor;
            }
        }

        public static void Update(Theme Theme, params MetroTile[] Controls)
        {
            foreach (MetroTile Control in Controls)
            {
                Control.Theme = Theme;
            }
        }
        public static void Update(Color StyleColor, params MetroTile[] Controls)
        {
            foreach (MetroTile Control in Controls)
            {
                Control.StyleColor = StyleColor;
            }
        }
        public static void Update(Theme Theme, Color StyleColor, params MetroTile[] Controls)
        {
            foreach (MetroTile Control in Controls)
            {
                Control.Theme = Theme;
                Control.StyleColor = StyleColor;
            }
        }

        public static void Update(Theme Theme, params MetroToggle[] Controls)
        {
            foreach (MetroToggle Control in Controls)
            {
                Control.Theme = Theme;
            }
        }
        public static void Update(Color StyleColor, params MetroToggle[] Controls)
        {
            foreach (MetroToggle Control in Controls)
            {
                Control.StyleColor = StyleColor;
            }
        }
        public static void Update(Theme Theme, Color StyleColor, params MetroToggle[] Controls)
        {
            foreach (MetroToggle Control in Controls)
            {
                Control.Theme = Theme;
                Control.StyleColor = StyleColor;
            }
        }

        public static void Update(Theme Theme, params MetroRadioButton[] Controls)
        {
            foreach (MetroRadioButton Control in Controls)
            {
                Control.Theme = Theme;
            }
        }
        public static void Update(Color StyleColor, params MetroRadioButton[] Controls)
        {
            foreach (MetroRadioButton Control in Controls)
            {
                Control.StyleColor = StyleColor;
            }
        }
        public static void Update(Theme Theme, Color StyleColor, params MetroRadioButton[] Controls)
        {
            foreach (MetroRadioButton Control in Controls)
            {
                Control.Theme = Theme;
                Control.StyleColor = StyleColor;
            }
        }
    }
}