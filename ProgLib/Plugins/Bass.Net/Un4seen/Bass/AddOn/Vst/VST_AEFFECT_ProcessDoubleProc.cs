using System;

namespace Un4seen.Bass.AddOn.Vst
{
	public delegate void VST_AEFFECT_ProcessDoubleProc(IntPtr effect, IntPtr inputs, IntPtr outputs, int sampleframes);
}
