using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_DISTORTION
	{
		public BASS_DX8_DISTORTION()
		{
		}

		public BASS_DX8_DISTORTION(float Gain, float Edge, float PostEQCenterFrequency, float PostEQBandwidth, float PreLowpassCutoff)
		{
			this.fGain = Gain;
			this.fEdge = Edge;
			this.fPostEQCenterFrequency = PostEQCenterFrequency;
			this.fPostEQBandwidth = PostEQBandwidth;
			this.fPreLowpassCutoff = PreLowpassCutoff;
		}

		public void Preset_Default()
		{
			this.fGain = 0f;
			this.fEdge = 50f;
			this.fPostEQCenterFrequency = 4000f;
			this.fPostEQBandwidth = 4000f;
			this.fPreLowpassCutoff = 4000f;
		}

		public float fGain;

		public float fEdge = 50f;

		public float fPostEQCenterFrequency = 4000f;

		public float fPostEQBandwidth = 4000f;

		public float fPreLowpassCutoff = 4000f;
	}
}
