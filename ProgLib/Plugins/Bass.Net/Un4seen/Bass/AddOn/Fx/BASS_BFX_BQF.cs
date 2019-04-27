using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_BQF
	{
		public BASS_BFX_BQF()
		{
		}

		public BASS_BFX_BQF(BASSBFXBQF filter, float center, float gain, float bandwidth, float q, float s, BASSFXChan chans)
		{
			this.lFilter = filter;
			this.fCenter = center;
			this.fGain = gain;
			this.fBandwidth = bandwidth;
			this.fQ = q;
			this.fS = s;
			this.lChannel = chans;
		}

		public BASSBFXBQF lFilter = BASSBFXBQF.BASS_BFX_BQF_ALLPASS;

		public float fCenter = 200f;

		public float fGain;

		public float fBandwidth = 1f;

		public float fQ;

		public float fS;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
