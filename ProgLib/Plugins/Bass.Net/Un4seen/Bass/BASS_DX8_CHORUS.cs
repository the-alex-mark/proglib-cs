using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_CHORUS
	{
		public BASS_DX8_CHORUS()
		{
		}

		public BASS_DX8_CHORUS(float WetDryMix, float Depth, float Feedback, float Frequency, int Waveform, float Delay, BASSFXPhase Phase)
		{
			this.fWetDryMix = WetDryMix;
			this.fDepth = Depth;
			this.fFeedback = Feedback;
			this.fFrequency = Frequency;
			this.lWaveform = Waveform;
			this.fDelay = Delay;
			this.lPhase = Phase;
		}

		public void Preset_Default()
		{
			this.fWetDryMix = 50f;
			this.fDepth = 25f;
			this.fFeedback = 0f;
			this.fFrequency = 0f;
			this.lWaveform = 1;
			this.fDelay = 0f;
			this.lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
		}

		public void Preset_A()
		{
			this.fWetDryMix = 60f;
			this.fDepth = 60f;
			this.fFeedback = 25f;
			this.fFrequency = 5f;
			this.lWaveform = 1;
			this.fDelay = 8f;
			this.lPhase = BASSFXPhase.BASS_FX_PHASE_90;
		}

		public void Preset_B()
		{
			this.fWetDryMix = 75f;
			this.fDepth = 80f;
			this.fFeedback = 50f;
			this.fFrequency = 7f;
			this.lWaveform = 0;
			this.fDelay = 15f;
			this.lPhase = BASSFXPhase.BASS_FX_PHASE_NEG_90;
		}

		public float fWetDryMix;

		public float fDepth = 25f;

		public float fFeedback;

		public float fFrequency;

		public int lWaveform = 1;

		public float fDelay;

		public BASSFXPhase lPhase = BASSFXPhase.BASS_FX_PHASE_ZERO;
	}
}
