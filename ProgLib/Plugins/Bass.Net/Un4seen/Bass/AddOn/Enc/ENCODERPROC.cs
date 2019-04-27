using System;

namespace Un4seen.Bass.AddOn.Enc
{
	public delegate int ENCODERPROC(int handle, int channel, IntPtr buffer, int length, int maxout, IntPtr user);
}
