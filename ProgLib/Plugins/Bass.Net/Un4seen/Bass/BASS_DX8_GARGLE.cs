using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_GARGLE
	{
		public BASS_DX8_GARGLE()
		{
		}

		public BASS_DX8_GARGLE(int RateHz, int WaveShape)
		{
			this.dwRateHz = RateHz;
			this.dwWaveShape = WaveShape;
		}

		public void Preset_Default()
		{
			this.dwRateHz = 100;
			this.dwWaveShape = 1;
		}

		public int dwRateHz = 500;

		public int dwWaveShape = 1;
	}
}
