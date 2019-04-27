using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_ECHO
	{
		public BASS_DX8_ECHO()
		{
		}

		public BASS_DX8_ECHO(float WetDryMix, float Feedback, float LeftDelay, float RightDelay, bool PanDelay)
		{
			this.fWetDryMix = WetDryMix;
			this.fFeedback = Feedback;
			this.fLeftDelay = LeftDelay;
			this.fRightDelay = RightDelay;
			this.lPanDelay = PanDelay;
		}

		public void Preset_Default()
		{
			this.fWetDryMix = 50f;
			this.fFeedback = 0f;
			this.fLeftDelay = 333f;
			this.fRightDelay = 333f;
			this.lPanDelay = false;
		}

		public void Preset_Small()
		{
			this.fWetDryMix = 50f;
			this.fFeedback = 20f;
			this.fLeftDelay = 100f;
			this.fRightDelay = 100f;
			this.lPanDelay = false;
		}

		public void Preset_Long()
		{
			this.fWetDryMix = 50f;
			this.fFeedback = 20f;
			this.fLeftDelay = 700f;
			this.fRightDelay = 700f;
			this.lPanDelay = false;
		}

		public float fWetDryMix;

		public float fFeedback;

		public float fLeftDelay = 333f;

		public float fRightDelay = 333f;

		[MarshalAs(UnmanagedType.Bool)]
		public bool lPanDelay;
	}
}
