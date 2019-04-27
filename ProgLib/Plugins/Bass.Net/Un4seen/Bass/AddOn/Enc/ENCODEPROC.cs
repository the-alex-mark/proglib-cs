using System;

namespace Un4seen.Bass.AddOn.Enc
{
	public delegate void ENCODEPROC(int handle, int channel, IntPtr buffer, int length, IntPtr user);
}
