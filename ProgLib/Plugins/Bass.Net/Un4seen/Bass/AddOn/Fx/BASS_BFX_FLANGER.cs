using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use BASS_FX_BFX_CHORUS instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_FLANGER
	{
		public BASS_BFX_FLANGER()
		{
		}

		public BASS_BFX_FLANGER(float WetDry, float Speed)
		{
			this.fWetDry = WetDry;
			this.fSpeed = Speed;
		}

		public BASS_BFX_FLANGER(float WetDry, float Speed, BASSFXChan chans)
		{
			this.fWetDry = WetDry;
			this.fSpeed = Speed;
			this.lChannel = chans;
		}

		public void Preset_Default()
		{
			this.fWetDry = 1f;
			this.fSpeed = 0.01f;
		}

		public float fWetDry = 1f;

		public float fSpeed = 0.01f;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
