using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_ECHO3
	{
		public BASS_BFX_ECHO3()
		{
		}

		public BASS_BFX_ECHO3(float DryMix, float WetMix, float Delay)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fDelay = Delay;
		}

		public BASS_BFX_ECHO3(float DryMix, float WetMix, float Delay, BASSFXChan chans)
		{
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fDelay = Delay;
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

		public float fDelay;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
