using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 with fFeedback enabled instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_REVERB
	{
		public BASS_BFX_REVERB()
		{
		}

		public BASS_BFX_REVERB(float Level, int Delay)
		{
			this.fLevel = Level;
			this.lDelay = Delay;
		}

		public float fLevel;

		public int lDelay = 1200;
	}
}
