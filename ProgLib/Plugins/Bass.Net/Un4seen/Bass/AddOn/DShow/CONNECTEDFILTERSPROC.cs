using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.DShow
{
	[return: MarshalAs(UnmanagedType.Bool)]
	public delegate bool CONNECTEDFILTERSPROC(IntPtr filter, [MarshalAs(UnmanagedType.LPStr)] string filterName, [MarshalAs(UnmanagedType.Bool)] bool hasPropertyPage, IntPtr user);
}
