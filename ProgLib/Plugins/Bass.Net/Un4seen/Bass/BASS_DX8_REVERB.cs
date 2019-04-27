using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_REVERB
	{
		public BASS_DX8_REVERB()
		{
		}

		public BASS_DX8_REVERB(float InGain, float ReverbMix, float ReverbTime, float HighFreqRTRatio)
		{
			this.fInGain = InGain;
			this.fReverbMix = ReverbMix;
			this.fReverbTime = ReverbTime;
			this.fHighFreqRTRatio = HighFreqRTRatio;
		}

		public void Preset_Default()
		{
			this.fInGain = -3f;
			this.fReverbMix = -6f;
			this.fReverbTime = 1000f;
			this.fHighFreqRTRatio = 0.5f;
		}

		public float fInGain;

		public float fReverbMix;

		public float fReverbTime = 1000f;

		public float fHighFreqRTRatio = 0.001f;
	}
}
