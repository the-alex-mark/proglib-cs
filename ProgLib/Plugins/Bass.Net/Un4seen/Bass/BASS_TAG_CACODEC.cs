using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	public sealed class BASS_TAG_CACODEC
	{
		public BASS_TAG_CACODEC()
		{
		}

		public BASS_TAG_CACODEC(IntPtr p)
		{
			try
			{
				BASS_TAG_CACODEC.TAG_CA_CODEC tag_CA_CODEC = (BASS_TAG_CACODEC.TAG_CA_CODEC)Marshal.PtrToStructure(p, typeof(BASS_TAG_CACODEC.TAG_CA_CODEC));
				this.ftype = tag_CA_CODEC.ftype;
				this.atype = tag_CA_CODEC.atype;
				this.name = Utils.IntPtrAsStringAnsi(tag_CA_CODEC.name);
			}
			catch
			{
			}
		}

		public override string ToString()
		{
			return this.name;
		}

		public int ftype;

		public int atype;

		public string name = string.Empty;

		[Serializable]
		private struct TAG_CA_CODEC
		{
			public int ftype;

			public int atype;

			public IntPtr name;
		}
	}
}
