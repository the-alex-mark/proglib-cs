using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Animations.Metro
{
    public sealed class ColorBlendAnimation : AnimationBase
    {
        private double percent = 1.0;

        public void Start(Control control, string property, Color targetColor, int duration)
        {
            if (duration == 0)
            {
                duration = 1;
            }
            base.Start(control, this.transitionType, 2 * duration, delegate
            {
                Color propertyValue = this.GetPropertyValue(property, control);
                Color color = this.DoColorBlend(propertyValue, targetColor, 0.1 * (this.percent / 2.0));
                PropertyInfo property2 = control.GetType().GetProperty(property);
                MethodInfo setMethod = property2.GetSetMethod(true);
                setMethod.Invoke(control, new object[]
                {
                    color
                });
            }, delegate
            {
                Color propertyValue = this.GetPropertyValue(property, control);
                return propertyValue.A.Equals(targetColor.A) && propertyValue.R.Equals(targetColor.R) && propertyValue.G.Equals(targetColor.G) && propertyValue.B.Equals(targetColor.B);
            });
        }

        private Color DoColorBlend(Color startColor, Color targetColor, double ratio)
        {
            this.percent += 0.2;
            int alpha = (int)Math.Round((double)startColor.A * (1.0 - ratio) + (double)targetColor.A * ratio);
            int red = (int)Math.Round((double)startColor.R * (1.0 - ratio) + (double)targetColor.R * ratio);
            int green = (int)Math.Round((double)startColor.G * (1.0 - ratio) + (double)targetColor.G * ratio);
            int blue = (int)Math.Round((double)startColor.B * (1.0 - ratio) + (double)targetColor.B * ratio);
            return System.Drawing.Color.FromArgb(alpha, red, green, blue);
        }

        private Color GetPropertyValue(string pName, Control control)
        {
            Type type = control.GetType();
            BindingFlags invokeAttr = BindingFlags.GetProperty;
            Binder binder = null;
            object[] args = null;
            object obj = type.InvokeMember(pName, invokeAttr, binder, control, args);
            return (Color)obj;
        }
    }
}
