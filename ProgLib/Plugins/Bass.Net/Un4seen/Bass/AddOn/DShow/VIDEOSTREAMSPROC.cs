using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.DShow
{
	[return: MarshalAs(UnmanagedType.Bool)]
	public delegate bool VIDEOSTREAMSPROC(int format, [MarshalAs(UnmanagedType.LPStr)] string name, int index, [MarshalAs(UnmanagedType.Bool)] bool enabled, IntPtr user);
}
