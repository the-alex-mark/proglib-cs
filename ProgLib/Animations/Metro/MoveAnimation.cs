using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Animations.Metro
{
    public sealed class MoveAnimation : AnimationBase
    {
        public void Start(Control control, Point targetPoint, TransitionType transitionType, int duration)
        {
            base.Start(control, transitionType, duration, delegate
            {
                int x = this.DoMoveAnimation(control.Location.X, targetPoint.X);
                int y = this.DoMoveAnimation(control.Location.Y, targetPoint.Y);
                control.Location = new Point(x, y);
            }, () => control.Location.Equals(targetPoint));
        }

        private int DoMoveAnimation(int startPos, int targetPos)
        {
            float t = (float)this.counter - (float)this.startTime;
            float b = (float)startPos;
            float c = (float)targetPos - (float)startPos;
            float d = (float)this.targetTime - (float)this.startTime;
            return base.MakeTransition(t, b, d, c);
        }
    }
}
