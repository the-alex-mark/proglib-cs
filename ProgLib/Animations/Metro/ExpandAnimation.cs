using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Animations.Metro
{
    public sealed class ExpandAnimation : AnimationBase
    {
        public void Start(Control control, Size targetSize, TransitionType transitionType, int duration)
        {
            base.Start(control, transitionType, duration, delegate
            {
                int width = this.DoExpandAnimation(control.Width, targetSize.Width);
                int height = this.DoExpandAnimation(control.Height, targetSize.Height);
                control.Size = new Size(width, height);
            }, () => control.Size.Equals(targetSize));
        }

        private int DoExpandAnimation(int startSize, int targetSize)
        {
            float t = (float)this.counter - (float)this.startTime;
            float b = (float)startSize;
            float c = (float)targetSize - (float)startSize;
            float d = (float)this.targetTime - (float)this.startTime;
            return base.MakeTransition(t, b, d, c);
        }
    }
}
