using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_ECHO2
	{
		public BASS_BFX_ECHO2()
		{
		}

		public BASS_BFX_ECHO2(float DryMix, float WetMix, float Feedback, float Delay)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fDelay = Delay;
		}

		public BASS_BFX_ECHO2(float DryMix, float WetMix, float Feedback, float Delay, BASSFXChan chans)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fDelay = Delay;
			this.lChannel = chans;
		}

		public void Preset_SmallEcho()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = 0f;
			this.fDelay = 0.2f;
		}

		public void Preset_ManyEchoes()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = 0.7f;
			this.fDelay = 0.5f;
		}

		public void Preset_ReverseEchoes()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fFeedback = -0.7f;
			this.fDelay = 0.8f;
		}

		public float fDryMix;

		public float fWetMix;

		public float fFeedback;

		public float fDelay;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
