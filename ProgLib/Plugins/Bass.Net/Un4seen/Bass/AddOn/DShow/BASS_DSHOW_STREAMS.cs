using System;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	public sealed class BASS_DSHOW_STREAMS
	{
		public override string ToString()
		{
			return this.name;
		}

		public bool IsAudio
		{
			get
			{
				return this.format == 2;
			}
		}

		public bool IsVideo
		{
			get
			{
				return this.format == 1;
			}
		}

		internal BASS_DSHOW_STREAMS_INTERNAL _internal;

		public int format;

		public string name;

		public int index;

		public bool enabled;
	}
}
