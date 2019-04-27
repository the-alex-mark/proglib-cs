using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	public sealed class BASS_DSHOW_CHANNELINFO
	{
		public override string ToString()
		{
			if (this.Height == 0 && this.Width == 0 && this.nChannels != 0)
			{
				return string.Format("{0} Hz, {1}, {2} bits", this.freq, Utils.ChannelNumberToString(this.nChannels), this.wBits);
			}
			return string.Format("{0}x{1}", this.Width, this.Height);
		}

		public float AvgTimePerFrame;

		public int Height;

		public int Width;

		public int nChannels;

		public int freq;

		public int wBits;

		[MarshalAs(UnmanagedType.Bool)]
		public bool floatingpoint;
	}
}
