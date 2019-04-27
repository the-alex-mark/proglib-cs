using System;

namespace Un4seen.Bass.AddOn.Cd
{
	public delegate void CDDATAPROC(int handle, int pos, BASSCDDATAType type, IntPtr buffer, int length, IntPtr user);
}
