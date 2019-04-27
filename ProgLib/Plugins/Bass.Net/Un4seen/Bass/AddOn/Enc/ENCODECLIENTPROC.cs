using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Enc
{
	[return: MarshalAs(UnmanagedType.Bool)]
	public delegate bool ENCODECLIENTPROC(int handle, [MarshalAs(UnmanagedType.Bool)] bool connect, [MarshalAs(UnmanagedType.LPStr)] [In] string client, IntPtr headers, IntPtr user);
}
