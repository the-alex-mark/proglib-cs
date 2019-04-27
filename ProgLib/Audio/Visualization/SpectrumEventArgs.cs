using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio.Visualization
{
    public class SpectrumEventArgs : EventArgs
    {
        public SpectrumEventArgs(List<Byte> Data)
        {
            this.Data = Data;
        }

        public List<Byte> Data { get; }
    }
}
