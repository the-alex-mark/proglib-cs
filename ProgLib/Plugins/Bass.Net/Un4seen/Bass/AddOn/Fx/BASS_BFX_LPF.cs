using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use 2x BASS_FX_BFX_BQF with BASS_BFX_BQF_LOWPASS filter and appropriate fQ values instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_LPF
	{
		public BASS_BFX_LPF()
		{
		}

		public BASS_BFX_LPF(float Resonance, float CutOffFreq)
		{
			this.fResonance = Resonance;
			this.fCutOffFreq = CutOffFreq;
		}

		public BASS_BFX_LPF(float Resonance, float CutOffFreq, BASSFXChan chans)
		{
			this.fResonance = Resonance;
			this.fCutOffFreq = CutOffFreq;
			this.lChannel = chans;
		}

		public float fResonance = 2f;

		public float fCutOffFreq = 200f;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
