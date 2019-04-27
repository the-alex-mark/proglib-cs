using System;

namespace Un4seen.Bass.AddOn.Midi
{
	public delegate void MIDIINPROC(int device, double time, IntPtr buffer, int length, IntPtr user);
}
