using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Windows.Material
{
    public interface IMaterialControl
    {
        int Depth { get; set; }
        MouseState MouseState { get; set; }
    }

    public enum MouseState
    {
        HOVER,
        DOWN,
        OUT
    }
}
