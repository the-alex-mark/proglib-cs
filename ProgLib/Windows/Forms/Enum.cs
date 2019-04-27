using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgLib.Windows.Forms
{
    public enum Theme
    {
        Dark,
        Light
    }

    public enum Enum
    {
        None,
        Hover,
        Down,
        Move
    }

    public enum ControlBoxButton
    {
        Minimize,
        Maximize,
        Close
    }

    public enum ControlBoxButtonState
    {
        None,
        Hover,
        Down
    }

    public enum ResizeDirection
    {
        None,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        BottomLeft,
        Left,
        TopLeft
    }

    public enum MessageBoxType
    {
        None,
        Information,
        Question,
        Warning,
        Error
    }

    public enum Gradient
    {
        None,
        Rainbow,
        Custom
    }
}