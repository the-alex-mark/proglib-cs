using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Animations.Metro
{
    public abstract class AnimationBase
    {
        private DelayedCall timer;

        private Control targetControl;

        private AnimationAction actionHandler;

        private AnimationFinishedEvaluator evaluatorHandler;

        protected TransitionType transitionType;

        protected int counter;

        protected int startTime;

        protected int targetTime;

        public event EventHandler AnimationCompleted;

        public bool IsCompleted
        {
            get
            {
                return this.timer == null || !this.timer.IsWaiting;
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.timer != null && this.timer.IsWaiting;
            }
        }

        private void OnAnimationCompleted()
        {
            if (this.AnimationCompleted != null)
            {
                this.AnimationCompleted(this, EventArgs.Empty);
            }
        }

        public void Cancel()
        {
            if (this.IsRunning)
            {
                this.timer.Cancel();
            }
        }

        protected void Start(Control control, TransitionType transitionType, int duration, AnimationAction actionHandler)
        {
            this.Start(control, transitionType, duration, actionHandler, null);
        }

        protected void Start(Control control, TransitionType transitionType, int duration, AnimationAction actionHandler, AnimationFinishedEvaluator evaluatorHandler)
        {
            this.targetControl = control;
            this.transitionType = transitionType;
            this.actionHandler = actionHandler;
            this.evaluatorHandler = evaluatorHandler;
            this.counter = 0;
            this.startTime = 0;
            this.targetTime = duration;
            this.timer = DelayedCall.Start(new DelayedCall.Callback(this.DoAnimation), duration);
        }

        private void DoAnimation()
        {
            if (this.evaluatorHandler == null || this.evaluatorHandler())
            {
                this.OnAnimationCompleted();
                return;
            }
            this.actionHandler();
            this.counter++;
            this.timer.Start();
        }

        protected int MakeTransition(float t, float b, float d, float c)
        {
            switch (this.transitionType)
            {
                case TransitionType.Linear:
                    return (int)(c * t / d + b);
                case TransitionType.EaseInQuad:
                    return (int)(c * (t /= d) * t + b);
                case TransitionType.EaseOutQuad:
                    return (int)(-c * (t /= d) * (t - 2f) + b);
                case TransitionType.EaseInOutQuad:
                    if ((t /= d / 2f) < 1f)
                    {
                        return (int)(c / 2f * t * t + b);
                    }
                    return (int)(-c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b);
                case TransitionType.EaseInCubic:
                    return (int)(c * (t /= d) * t * t + b);
                case TransitionType.EaseOutCubic:
                    return (int)(c * ((t = t / d - 1f) * t * t + 1f) + b);
                case TransitionType.EaseInOutCubic:
                    if ((t /= d / 2f) < 1f)
                    {
                        return (int)(c / 2f * t * t * t + b);
                    }
                    return (int)(c / 2f * ((t -= 2f) * t * t + 2f) + b);
                case TransitionType.EaseInQuart:
                    return (int)(c * (t /= d) * t * t * t + b);
                case TransitionType.EaseInExpo:
                    if (t == 0f)
                    {
                        return (int)b;
                    }
                    return (int)((double)c * Math.Pow(2.0, (double)(10f * (t / d - 1f))) + (double)b);
                case TransitionType.EaseOutExpo:
                    if (t == d)
                    {
                        return (int)(b + c);
                    }
                    return (int)((double)c * (-Math.Pow(2.0, (double)(-10f * t / d)) + 1.0) + (double)b);
                default:
                    return 0;
            }
        }
    }
}
