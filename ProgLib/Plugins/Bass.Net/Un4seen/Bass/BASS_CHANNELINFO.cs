using System;

namespace Un4seen.Bass
{
	[Serializable]
	public sealed class BASS_CHANNELINFO
	{
		public bool IsDecodingChannel
		{
			get
			{
				return (this.flags & BASSFlag.BASS_STREAM_DECODE) > BASSFlag.BASS_DEFAULT;
			}
		}

		public bool Is32bit
		{
			get
			{
				return (this.flags & BASSFlag.BASS_SAMPLE_FLOAT) > BASSFlag.BASS_DEFAULT;
			}
		}

		public bool Is8bit
		{
			get
			{
				return (this.flags & BASSFlag.BASS_SAMPLE_8BITS) > BASSFlag.BASS_DEFAULT;
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}Hz, {2}, {3}bit", new object[]
			{
				Utils.BASSChannelTypeToString(this.ctype),
				this.freq,
				Utils.ChannelNumberToString(this.chans),
				(this.origres == 0) ? (this.Is32bit ? 32 : (this.Is8bit ? 8 : 16)) : this.origres
			});
		}

		internal BASS_CHANNELINFO_INTERNAL _internal;

		public int freq;

		public int chans;

		public BASSFlag flags;

		public BASSChannelType ctype;

		public int origres;

		public int plugin;

		public int sample;

		public string filename = string.Empty;
	}
}
