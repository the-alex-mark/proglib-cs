using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_PHASER
	{
		public BASS_BFX_PHASER()
		{
		}

		public BASS_BFX_PHASER(float DryMix, float WetMix, float Feedback, float Rate, float Range, float Freq)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fRate = Rate;
			this.fRange = Range;
			this.fFreq = Freq;
		}

		public BASS_BFX_PHASER(float DryMix, float WetMix, float Feedback, float Rate, float Range, float Freq, BASSFXChan chans)
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

		public void Preset_PhaseShift()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = 0f;
			this.fRate = 1f;
			this.fRange = 4f;
			this.fFreq = 100f;
		}

		public void Preset_SlowInvertPhaseShiftWithFeedback()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = -0.999f;
			this.fFeedback = -0.6f;
			this.fRate = 0.2f;
			this.fRange = 6f;
			this.fFreq = 100f;
		}

		public void Preset_BasicPhase()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = -0f;
			this.fRate = 1f;
			this.fRange = 4.3f;
			this.fFreq = 50f;
		}

		public void Preset_PhaseWithFeedback()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = 0f;
			this.fRate = 1f;
			this.fRange = 4.3f;
			this.fFreq = 50f;
		}

		public void Preset_MediumPhase()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = 0f;
			this.fRate = 1f;
			this.fRange = 7f;
			this.fFreq = 100f;
		}

		public void Preset_FastPhase()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = 0f;
			this.fRate = 1f;
			this.fRange = 7f;
			this.fFreq = 400f;
		}

		public void Preset_InvertWithInvertFeedback()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = -0.999f;
			this.fFeedback = -0.2f;
			this.fRate = 1f;
			this.fRange = 7f;
			this.fFreq = 200f;
		}

		public void Preset_TremoloWah()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = 0.6f;
			this.fRate = 1f;
			this.fRange = 4f;
			this.fFreq = 60f;
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
