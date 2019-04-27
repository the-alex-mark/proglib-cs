using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_PEAKEQ
	{
		public BASS_BFX_PEAKEQ()
		{
		}

		public BASS_BFX_PEAKEQ(int Band, float Bandwidth, float Q, float Center, float Gain)
		{
			this.lBand = Band;
			this.fBandwidth = Bandwidth;
			this.fQ = Q;
			this.fCenter = Center;
			this.fGain = Gain;
		}

		public BASS_BFX_PEAKEQ(int Band, float Bandwidth, float Q, float Center, float Gain, BASSFXChan chans)
		{
			this.lBand = Band;
			this.fBandwidth = Bandwidth;
			this.fQ = Q;
			this.fCenter = Center;
			this.fGain = Gain;
			this.lChannel = chans;
		}

		public int lBand;

		public float fBandwidth = 1f;

		public float fQ;

		public float fCenter = 1000f;

		public float fGain;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
