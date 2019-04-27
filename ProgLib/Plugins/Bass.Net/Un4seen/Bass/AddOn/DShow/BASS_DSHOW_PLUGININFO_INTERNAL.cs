using System;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	internal struct BASS_DSHOW_PLUGININFO_INTERNAL
	{
		public int version;

		public int decoderType;

		public IntPtr plgdescription;
	}
}
