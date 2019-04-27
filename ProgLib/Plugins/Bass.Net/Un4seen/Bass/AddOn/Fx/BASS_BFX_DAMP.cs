using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_DAMP
	{
		public BASS_BFX_DAMP()
		{
		}

		public BASS_BFX_DAMP(float Target, float Quiet, float Rate, float Gain, float Delay)
		{
			this.fTarget = Target;
			this.fQuiet = Quiet;
			this.fRate = Rate;
			this.fGain = Gain;
			this.fDelay = Delay;
		}

		public BASS_BFX_DAMP(float Target, float Quiet, float Rate, float Gain, float Delay, BASSFXChan chans)
		{
			this.fTarget = Target;
			this.fQuiet = Quiet;
			this.fRate = Rate;
			this.fGain = Gain;
			this.fDelay = Delay;
			this.lChannel = chans;
		}

		public void Preset_Soft()
		{
			this.fTarget = 0.92f;
			this.fQuiet = 0.02f;
			this.fRate = 0.01f;
			this.fGain = 1f;
			this.fDelay = 0.5f;
		}

		public void Preset_Medium()
		{
			this.fTarget = 0.94f;
			this.fQuiet = 0.03f;
			this.fRate = 0.01f;
			this.fGain = 1f;
			this.fDelay = 0.35f;
		}

		public void Preset_Hard()
		{
			this.fTarget = 0.98f;
			this.fQuiet = 0.04f;
			this.fRate = 0.02f;
			this.fGain = 2f;
			this.fDelay = 0.2f;
		}

		public float fTarget = 1f;

		public float fQuiet;

		public float fRate;

		public float fGain;

		public float fDelay;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
