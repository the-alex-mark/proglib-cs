using System;
using System.Runtime.InteropServices;

namespace Un4seen.BassAsio
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ASIOIoFormat
	{
		public int formatType = -1;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 508)]
		public string future = string.Empty;
	}
}
