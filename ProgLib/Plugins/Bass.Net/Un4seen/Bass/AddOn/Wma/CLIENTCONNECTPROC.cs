using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Wma
{
	public delegate void CLIENTCONNECTPROC(int handle, [MarshalAs(UnmanagedType.Bool)] bool connect, [MarshalAs(UnmanagedType.LPStr)] string ip, IntPtr user);
}
