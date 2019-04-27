using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_PARAMEQ
	{
		public BASS_DX8_PARAMEQ()
		{
		}

		public BASS_DX8_PARAMEQ(float Center, float Bandwidth, float Gain)
		{
			this.fCenter = Center;
			this.fBandwidth = Bandwidth;
			this.fGain = Gain;
		}

		public void Preset_Default()
		{
			this.fCenter = 100f;
			this.fBandwidth = 18f;
			this.fGain = 0f;
		}

		public void Preset_Low()
		{
			this.fCenter = 125f;
			this.fBandwidth = 18f;
			this.fGain = 0f;
		}

		public void Preset_Mid()
		{
			this.fCenter = 1000f;
			this.fBandwidth = 18f;
			this.fGain = 0f;
		}

		public void Preset_High()
		{
			this.fCenter = 8000f;
			this.fBandwidth = 18f;
			this.fGain = 0f;
		}

		public float fCenter = 100f;

		public float fBandwidth = 18f;

		public float fGain;
	}
}
