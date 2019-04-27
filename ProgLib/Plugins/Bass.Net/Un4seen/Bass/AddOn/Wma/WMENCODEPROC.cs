using System;

namespace Un4seen.Bass.AddOn.Wma
{
	public delegate void WMENCODEPROC(int handle, BASSWMAEncodeCallback type, IntPtr buffer, int length, IntPtr user);
}
