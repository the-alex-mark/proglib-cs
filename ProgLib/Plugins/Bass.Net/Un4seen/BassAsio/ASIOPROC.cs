using System;
using System.Runtime.InteropServices;

namespace Un4seen.BassAsio
{
	public delegate int ASIOPROC([MarshalAs(UnmanagedType.Bool)] bool input, int channel, IntPtr buffer, int length, IntPtr user);
}
