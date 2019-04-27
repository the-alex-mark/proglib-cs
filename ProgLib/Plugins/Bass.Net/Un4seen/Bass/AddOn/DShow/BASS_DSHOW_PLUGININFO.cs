using System;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	public sealed class BASS_DSHOW_PLUGININFO
	{
		public override string ToString()
		{
			return this.plgdescription;
		}

		public bool IsAudio
		{
			get
			{
				return this.decoderType == 1;
			}
		}

		public bool IsVideo
		{
			get
			{
				return this.decoderType == 2;
			}
		}

		internal BASS_DSHOW_PLUGININFO_INTERNAL _internal;

		public int version;

		public int decoderType;

		public string plgdescription;
	}
}
