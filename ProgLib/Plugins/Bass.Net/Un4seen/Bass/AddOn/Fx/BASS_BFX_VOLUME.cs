using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_VOLUME
	{
		public BASS_BFX_VOLUME()
		{
		}

		public BASS_BFX_VOLUME(float Volume)
		{
			this.fVolume = Volume;
		}

		public BASS_BFX_VOLUME(float Volume, BASSFXChan chans)
		{
			this.fVolume = Volume;
			this.lChannel = chans;
		}

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;

		public float fVolume = 1f;
	}
}
