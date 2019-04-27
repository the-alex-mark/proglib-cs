using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use BASS_FX_BFX_COMPRESSOR2 instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_BFX_COMPRESSOR
	{
		public BASS_BFX_COMPRESSOR()
		{
		}

		public BASS_BFX_COMPRESSOR(float Threshold, float Attacktime, float Releasetime)
		{
			this.fThreshold = Threshold;
			this.fAttacktime = Attacktime;
			this.fReleasetime = Releasetime;
		}

		public BASS_BFX_COMPRESSOR(float Threshold, float Attacktime, float Releasetime, BASSFXChan chans)
		{
			this.fThreshold = Threshold;
			this.fAttacktime = Attacktime;
			this.fReleasetime = Releasetime;
			this.lChannel = chans;
		}

		public void Preset_50Attack15msRelease1sec()
		{
			this.fThreshold = 0.5f;
			this.fAttacktime = 15f;
			this.fReleasetime = 1000f;
		}

		public void Preset_80Attack1msRelease05sec()
		{
			this.fThreshold = 0.8f;
			this.fAttacktime = 1f;
			this.fReleasetime = 500f;
		}

		public void Preset_Soft()
		{
			this.fThreshold = 0.89f;
			this.fAttacktime = 20f;
			this.fReleasetime = 350f;
		}

		public void Preset_SoftHigh()
		{
			this.fThreshold = 0.7f;
			this.fAttacktime = 10f;
			this.fReleasetime = 200f;
		}

		public void Preset_Medium()
		{
			this.fThreshold = 0.5f;
			this.fAttacktime = 5f;
			this.fReleasetime = 250f;
		}

		public void Preset_Hard()
		{
			this.fThreshold = 0.25f;
			this.fAttacktime = 2.2f;
			this.fReleasetime = 400f;
		}

		public float fThreshold;

		public float fAttacktime;

		public float fReleasetime;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
