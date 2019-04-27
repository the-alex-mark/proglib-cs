using System;

namespace Un4seen.BassAsio
{
	[Serializable]
	public sealed class BASS_ASIO_DEVICEINFO
	{
		public override string ToString()
		{
			return this.name;
		}

		internal BASS_ASIO_DEVICEINFO_INTERNAL _internal;

		public string name = string.Empty;

		public string driver = string.Empty;
	}
}
