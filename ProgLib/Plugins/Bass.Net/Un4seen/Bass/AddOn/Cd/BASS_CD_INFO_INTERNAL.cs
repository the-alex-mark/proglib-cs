using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Cd
{
	[Serializable]
	internal struct BASS_CD_INFO_INTERNAL
	{
		public IntPtr vendor;

		public IntPtr product;

		public IntPtr rev;

		public int letter;

		public BASSCDRWFlags rwflags;

		[MarshalAs(UnmanagedType.Bool)]
		public bool canopen;

		[MarshalAs(UnmanagedType.Bool)]
		public bool canlock;

		public int maxspeed;

		public int cache;

		[MarshalAs(UnmanagedType.Bool)]
		public bool cdtext;
	}
}
