using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_COMPRESSOR
	{
		public BASS_DX8_COMPRESSOR()
		{
		}

		public BASS_DX8_COMPRESSOR(float Gain, float Attack, float Release, float Threshold, float Ratio, float Predelay)
		{
			this.fGain = Gain;
			this.fAttack = Attack;
			this.fRelease = Release;
			this.fThreshold = Threshold;
			this.fRatio = Ratio;
			this.fPredelay = Predelay;
		}

		public void Preset_Default()
		{
			this.fGain = 0f;
			this.fAttack = 10f;
			this.fRelease = 200f;
			this.fThreshold = -20f;
			this.fRatio = 3f;
			this.fPredelay = 4f;
		}

		public void Preset_Soft()
		{
			this.fGain = 0f;
			this.fAttack = 12f;
			this.fRelease = 800f;
			this.fThreshold = -20f;
			this.fRatio = 3f;
			this.fPredelay = 4f;
		}

		public void Preset_Soft2()
		{
			this.fGain = 2f;
			this.fAttack = 20f;
			this.fRelease = 800f;
			this.fThreshold = -20f;
			this.fRatio = 4f;
			this.fPredelay = 4f;
		}

		public void Preset_Medium()
		{
			this.fGain = 4f;
			this.fAttack = 5f;
			this.fRelease = 600f;
			this.fThreshold = -20f;
			this.fRatio = 5f;
			this.fPredelay = 3f;
		}

		public void Preset_Hard()
		{
			this.fGain = 2f;
			this.fAttack = 2f;
			this.fRelease = 400f;
			this.fThreshold = -20f;
			this.fRatio = 8f;
			this.fPredelay = 2f;
		}

		public void Preset_Hard2()
		{
			this.fGain = 6f;
			this.fAttack = 2f;
			this.fRelease = 200f;
			this.fThreshold = -20f;
			this.fRatio = 10f;
			this.fPredelay = 2f;
		}

		public void Preset_HardCommercial()
		{
			this.fGain = 4f;
			this.fAttack = 5f;
			this.fRelease = 300f;
			this.fThreshold = -16f;
			this.fRatio = 9f;
			this.fPredelay = 2f;
		}

		public float fGain;

		public float fAttack = 10f;

		public float fRelease = 200f;

		public float fThreshold = -20f;

		public float fRatio = 3f;

		public float fPredelay = 4f;
	}
}
