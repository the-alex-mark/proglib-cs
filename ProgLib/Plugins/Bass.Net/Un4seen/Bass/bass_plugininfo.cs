using System;

namespace Un4seen.Bass
{
	[Serializable]
	internal struct bass_plugininfo
	{
		public int version;

		public int formatc;

		public IntPtr formats;
	}
}
