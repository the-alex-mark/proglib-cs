using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.DShow
{
	[return: MarshalAs(UnmanagedType.Bool)]
	public delegate bool ENUMDEVICESPROC([MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStruct)] Guid guid, IntPtr user);
}
