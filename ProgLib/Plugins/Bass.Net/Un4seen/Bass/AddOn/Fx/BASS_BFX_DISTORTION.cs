using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_DISTORTION
	{
		public BASS_BFX_DISTORTION()
		{
		}

		public BASS_BFX_DISTORTION(float Drive, float DryMix, float WetMix, float Feedback, float Volume)
		{
			this.fDrive = Drive;
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fVolume = Volume;
		}

		public BASS_BFX_DISTORTION(float Drive, float DryMix, float WetMix, float Feedback, float Volume, BASSFXChan chans)
		{
			this.fDrive = Drive;
			this.fDryMix = DryMix;
			this.fWetMix = WetMix;
			this.fFeedback = Feedback;
			this.fVolume = Volume;
			this.lChannel = chans;
		}

		public void Preset_HardDistortion()
		{
			this.fDrive = 1f;
			this.fDryMix = 0f;
			this.fWetMix = 1f;
			this.fFeedback = 0f;
			this.fVolume = 1f;
		}

		public void Preset_VeryHardDistortion()
		{
			this.fDrive = 5f;
			this.fDryMix = 0f;
			this.fWetMix = 1f;
			this.fFeedback = 0.1f;
			this.fVolume = 1f;
		}

		public void Preset_MediumDistortion()
		{
			this.fDrive = 0.2f;
			this.fDryMix = 1f;
			this.fWetMix = 1f;
			this.fFeedback = 0.1f;
			this.fVolume = 1f;
		}

		public float fDrive;

		public float fDryMix;

		public float fWetMix;

		public float fFeedback;

		public float fVolume;

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;
	}
}
