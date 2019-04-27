using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_AUTOWAH
	{
		public BASS_BFX_AUTOWAH()
		{
		}

		public BASS_BFX_AUTOWAH(float DryMix, float WetMix, float Feedback, float Rate, float Range, float Freq)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fRate = Rate;
			this.fRange = Range;
			this.fFreq = Freq;
		}

		public BASS_BFX_AUTOWAH(float DryMix, float WetMix, float Feedback, float Rate, float Range, float Freq, BASSFXChan chans)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fRate = Rate;
			this.fRange = Range;
			this.fFreq = Freq;
			this.lChannel = chans;
		}

		public void Preset_Default()
		{
			this.fDryMix = -1f;
			this.fWetMix = 1f;
			this.fFeedback = 0.06f;
			this.fRate = 0.2f;
			this.fRange = 6f;
			this.fFreq = 100f;
		}

		public void Preset_SlowAutoWah()
		{
			this.fDryMix = 0.5f;
			this.fWetMix = 1.5f;
			this.fFeedback = 0.5f;
			this.fRate = 2f;
			this.fRange = 4.3f;
			this.fFreq = 50f;
		}

		public void Preset_FastAutoWah()
		{
			this.fDryMix = 0.5f;
			this.fWetMix = 1.5f;
			this.fFeedback = 0.5f;
			this.fRate = 5f;
			this.fRange = 5.3f;
			this.fFreq = 50f;
		}

		public void Preset_HiFastAutoWah()
		{
			this.fDryMix = 0.5f;
			this.fWetMix = 1.5f;
			this.fFeedback = 0.5f;
			this.fRate = 5f;
			this.fRange = 4.3f;
			this.fFreq = 500f;
		}

		public float fDryMix;

		public float fWetMix;

		public float fFeedback;

		public float fRate;

		public float fRange;

		public float fFreq;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
