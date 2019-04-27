using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_CHORUS
	{
		public BASS_BFX_CHORUS()
		{
		}

		public BASS_BFX_CHORUS(float DryMix, float WetMix, float Feedback, float MinSweep, float MaxSweep, float Rate)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fMinSweep = MinSweep;
			this.fMaxSweep = MaxSweep;
			this.fRate = Rate;
		}

		public BASS_BFX_CHORUS(float DryMix, float WetMix, float Feedback, float MinSweep, float MaxSweep, float Rate, BASSFXChan chans)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fMinSweep = MinSweep;
			this.fMaxSweep = MaxSweep;
			this.fRate = Rate;
			this.lChannel = chans;
		}

		public void Preset_Flanger()
		{
			this.fDryMix = 1f;
			this.fWetMix = 0.35f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 5f;
			this.fRate = 1f;
		}

		public void Preset_ExaggeratedChorusLTMPitchSshiftedVoices()
		{
			this.fDryMix = 0.7f;
			this.fWetMix = 0.25f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 200f;
			this.fRate = 50f;
		}

		public void Preset_Motocycle()
		{
			this.fDryMix = 0.9f;
			this.fWetMix = 0.45f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 100f;
			this.fRate = 25f;
		}

		public void Preset_Devil()
		{
			this.fDryMix = 0.9f;
			this.fWetMix = 0.35f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 50f;
			this.fRate = 200f;
		}

		public void Preset_WhoSayTTNManyVoices()
		{
			this.fDryMix = 0.9f;
			this.fWetMix = 0.35f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 400f;
			this.fRate = 200f;
		}

		public void Preset_BackChipmunk()
		{
			this.fDryMix = 0.9f;
			this.fWetMix = -0.2f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 400f;
			this.fRate = 400f;
		}

		public void Preset_Water()
		{
			this.fDryMix = 0.9f;
			this.fWetMix = -0.4f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 2f;
			this.fRate = 1f;
		}

		public void Preset_ThisIsTheAirplane()
		{
			this.fDryMix = 0.3f;
			this.fWetMix = 0.4f;
			this.fFeedback = 0.5f;
			this.fMinSweep = 1f;
			this.fMaxSweep = 10f;
			this.fRate = 5f;
		}

		public float fDryMix;

		public float fWetMix;

		public float fFeedback;

		public float fMinSweep;

		public float fMaxSweep;

		public float fRate;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
