using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ProgLib.Audio.Visualization
{
    public class iSpectrum : Control
    {
        public iSpectrum()
        {
            Size = new Size(163, 78);
            BackProgressColor = Color.Transparent;
            ProgressColor = Color.Green;

            this.Count = 10;
        }

        #region Global Variables

        List<iProgressBar> _progressBars;

        #endregion

        #region Methods

        private void Create(Int32 Count)
        {
            Controls.Clear();

            _progressBars = new List<iProgressBar>();
            for (int i = 0; i < Count; i++)
            {
                _progressBars.Add(
                    new iProgressBar
                    {
                        BackColor = Color.Red,
                        ProgressColor = ProgressColor,
                        Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                        Orientation = iProgressBar.OrientationType.Vertical,
                        Maximum = 255,
                        Size = new Size(3, 57),
                        Location = (i == 0) ? new Point(2, 1) : new Point(_progressBars[i - 1].Location.X + 4, 1),

                        Parent = this
                    });
            }
        }

        public void Set(List<Byte> Data)
        {
            if (Data.Count == this.Count)
            {
                for (int i = 0; i < Data.Count; i++)
                {
                    _progressBars[i].BackColor = BackProgressColor;
                    _progressBars[i].ProgressColor = ProgressColor;

                    _progressBars[i].Value = Data[i];
                }
            }
            else throw new Exception("Количетсво данных не совпадают! Имя параметра: \"Data\"");
        }

        #endregion

        public Int32 Count { get { return _progressBars.Count; } set { this.Create(value); } }
        public Color BackProgressColor { get; set; }
        public Color ProgressColor { get; set; }
    }
}
