using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgLib.Audio.Visualization
{
    public class SpectrumAnalyzerEventArgs : EventArgs
    {
        public SpectrumAnalyzerEventArgs(List<Byte> Data)
        {
            this.Data = Data;
        }

        public List<Byte> Data { get; }
    }
}
