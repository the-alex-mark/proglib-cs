using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	internal struct BASS_DSHOW_STREAMS_INTERNAL
	{
		public int format;

		public IntPtr name;

		public int index;

		[MarshalAs(UnmanagedType.Bool)]
		public bool enabled;
	}
}
