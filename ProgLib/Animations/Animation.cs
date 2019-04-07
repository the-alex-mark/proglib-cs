using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgLib.Animations.Metro;

namespace ProgLib.Animations
{
    public static class Animation
    {
        /// <summary>
        /// Перемещает указанный <see cref="System.Windows.Forms.Control"/>.
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Location"></param>
        /// <param name="Duration"></param>
        public static void Move(Control Control, Point Location, TransitionType Type, Int32 Duration)
        {
            new MoveAnimation().Start(Control, Location, Type, Duration);
        }

        /// <summary>
        /// Изменяет размер указанного <see cref="System.Windows.Forms.Control"/>.
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Size"></param>
        /// <param name="Duration"></param>
        public static void Size(Control Control, Size Size, TransitionType Type, Int32 Duration)
        {
            new ExpandAnimation().Start(Control, Size, Type, Duration);
        }

        public static void MoveAndSize(Control Control, Size Size, TransitionType Type, Int32 Duration)
        {
            Animation.Move(Control, Control.Location, Type, Duration);
            Animation.Size(Control, Size, Type, Duration);
        }

        /// <summary>
        /// Изменяет цвет свойства указанного <see cref="System.Windows.Forms.Control"/>.
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Property"></param>
        /// <param name="Color"></param>
        /// <param name="Duration"></param>
        public static void Color(Control Control, String Property, Color Color, Int32 Duration)
        {
            new ColorBlendAnimation().Start(Control, Property, Color, Duration);
        }
    }
}
