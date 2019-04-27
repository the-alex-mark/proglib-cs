using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_ROTATE
	{
		public BASS_BFX_ROTATE()
		{
		}

		public BASS_BFX_ROTATE(float Rate, BASSFXChan chans)
		{
			this.fRate = Rate;
			this.lChannel = chans;
		}

		public float fRate;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
