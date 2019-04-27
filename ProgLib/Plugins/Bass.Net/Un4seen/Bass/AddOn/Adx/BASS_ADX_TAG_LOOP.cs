using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Adx
{
	[Serializable]
	public struct BASS_ADX_TAG_LOOP
	{
		[MarshalAs(UnmanagedType.Bool)]
		public bool LoopEnabled;

		public long SampleStart;

		public long ByteStart;

		public long SampleEnd;

		public long ByteEnd;
	}
}
