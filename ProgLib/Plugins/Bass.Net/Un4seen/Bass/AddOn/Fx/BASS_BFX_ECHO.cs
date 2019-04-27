using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_ECHO
	{
		public BASS_BFX_ECHO()
		{
		}

		public BASS_BFX_ECHO(float Level, int Delay)
		{
			this.fLevel = Level;
			this.lDelay = Delay;
		}

		public float fLevel;

		public int lDelay = 1200;
	}
}
