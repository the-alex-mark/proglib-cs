using System;
using System.Runtime.InteropServices;
using Un4seen.Bass;

namespace Un4seen.BassWasapi
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_WASAPI_INFO
	{
		public override string ToString()
		{
			return string.Format("{0}, {1}Hz, {2}", (this.format == BASSWASAPIFormat.BASS_WASAPI_FORMAT_FLOAT || this.format == BASSWASAPIFormat.BASS_WASAPI_FORMAT_32BIT) ? "32-bit" : ((this.format == BASSWASAPIFormat.BASS_WASAPI_FORMAT_24BIT) ? "24-bit" : ((this.format == BASSWASAPIFormat.BASS_WASAPI_FORMAT_16BIT) ? "16-bit" : ((this.format == BASSWASAPIFormat.BASS_WASAPI_FORMAT_8BIT) ? "8-bit" : "Unknown"))), this.freq, Utils.ChannelNumberToString(this.chans));
		}

		public bool IsExclusive
		{
			get
			{
				return (this.initflags & BASSWASAPIInit.BASS_WASAPI_EXCLUSIVE) > BASSWASAPIInit.BASS_WASAPI_SHARED;
			}
		}

		public bool IsEventDriven
		{
			get
			{
				return (this.initflags & BASSWASAPIInit.BASS_WASAPI_EVENT) > BASSWASAPIInit.BASS_WASAPI_SHARED;
			}
		}

		public BASSWASAPIInit initflags;

		public int freq;

		public int chans;

		public BASSWASAPIFormat format;

		public int buflen;

		private float volmax;

		private float volmin;

		private float volstep;
	}
}
