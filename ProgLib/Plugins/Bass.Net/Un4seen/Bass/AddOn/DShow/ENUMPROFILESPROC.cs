using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.DShow
{
	[return: MarshalAs(UnmanagedType.Bool)]
	public delegate bool ENUMPROFILESPROC([MarshalAs(UnmanagedType.LPStr)] string profile, IntPtr user);
}
