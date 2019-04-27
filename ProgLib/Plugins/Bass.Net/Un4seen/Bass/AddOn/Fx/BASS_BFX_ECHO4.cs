using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_ECHO4
	{
		public BASS_BFX_ECHO4()
		{
		}

		public BASS_BFX_ECHO4(float DryMix, float WetMix, float Delay)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fDelay = Delay;
		}

		public BASS_BFX_ECHO4(float DryMix, float WetMix, float Delay, float Feedback)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fDelay = Delay;
			this.fFeedback = Feedback;
		}

		public BASS_BFX_ECHO4(float DryMix, float WetMix, float Delay, float Feedback, bool Stereo)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fDelay = Delay;
			this.fFeedback = Feedback;
			this.bStereo = Stereo;
		}

		public BASS_BFX_ECHO4(float DryMix, float WetMix, float Delay, float Feedback, bool Stereo, BASSFXChan chans)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fDelay = Delay;
			this.fFeedback = Feedback;
			this.bStereo = Stereo;
			this.lChannel = chans;
		}

		public void Preset_SmallEcho()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.999f;
			this.fDelay = 0.2f;
		}

		public void Preset_DoubleKick()
		{
			this.fDryMix = 0.5f;
			this.fWetMix = 0.599f;
			this.fDelay = 0.5f;
		}

		public void Preset_LongEcho()
		{
			this.fDryMix = 0.999f;
			this.fWetMix = 0.699f;
			this.fDelay = 0.9f;
		}

		public float fDryMix;

		public float fWetMix;

		public float fFeedback;

		public float fDelay;

		[MarshalAs(UnmanagedType.Bool)]
		public bool bStereo;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
