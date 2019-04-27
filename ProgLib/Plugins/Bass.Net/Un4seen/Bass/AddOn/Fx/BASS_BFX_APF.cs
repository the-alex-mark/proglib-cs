using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use BASS_FX_BFX_BQF with BASS_BFX_BQF_ALLPASS filter instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_APF
	{
		public BASS_BFX_APF()
		{
		}

		public BASS_BFX_APF(float Gain, float Delay)
		{
			this.fGain = Gain;
			this.fDelay = Delay;
		}

		public BASS_BFX_APF(float Gain, float Delay, BASSFXChan chans)
		{
			this.fGain = Gain;
			this.fDelay = Delay;
			this.lChannel = chans;
		}

		public void Preset_Default()
		{
			this.fGain = -0.5f;
			this.fDelay = 0.5f;
		}

		public void Preset_SmallRever()
		{
			this.fGain = 0.799f;
			this.fDelay = 0.2f;
		}

		public void Preset_RobotVoice()
		{
			this.fGain = 0.6f;
			this.fDelay = 0.05f;
		}

		public void Preset_LongReverberation()
		{
			this.fGain = 0.599f;
			this.fDelay = 1.3f;
		}

		public float fGain;

		public float fDelay;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
