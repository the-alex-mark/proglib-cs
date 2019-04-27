using System;

namespace Un4seen.Bass.AddOn.DShow
{
	public delegate void DVPPROC(int handle, int channel, IntPtr buffer, int length, BASSDSHOWDVPType dataType, int width, int height, IntPtr user);
}
