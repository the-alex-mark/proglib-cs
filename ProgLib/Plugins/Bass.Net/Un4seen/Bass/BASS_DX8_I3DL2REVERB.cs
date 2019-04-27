using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_DX8_I3DL2REVERB
	{
		public BASS_DX8_I3DL2REVERB()
		{
		}

		public BASS_DX8_I3DL2REVERB(int Room, int RoomHF, float RoomRolloffFactor, float DecayTime, float DecayHFRatio, int Reflections, float ReflectionsDelay, int Reverb, float ReverbDelay, float Diffusion, float Density, float HFReference)
		{
			this.lRoom = Room;
			this.lRoomHF = RoomHF;
			this.flRoomRolloffFactor = RoomRolloffFactor;
			this.flDecayTime = DecayTime;
			this.flDecayHFRatio = DecayHFRatio;
			this.lReflections = Reflections;
			this.flReflectionsDelay = ReflectionsDelay;
			this.lReverb = Reverb;
			this.flReverbDelay = ReverbDelay;
			this.flDiffusion = Diffusion;
			this.flDensity = Density;
			this.flHFReference = HFReference;
		}

		public void Preset_Default()
		{
			this.lRoom = -1000;
			this.lRoomHF = 0;
			this.flRoomRolloffFactor = 0f;
			this.flDecayTime = 1.49f;
			this.flDecayHFRatio = 0.83f;
			this.lReflections = -2602;
			this.flReflectionsDelay = 0.007f;
			this.lReverb = 200;
			this.flReverbDelay = 0.011f;
			this.flDiffusion = 100f;
			this.flDensity = 100f;
			this.flHFReference = 5000f;
		}

		public int lRoom = -1000;

		public int lRoomHF;

		public float flRoomRolloffFactor;

		public float flDecayTime = 1.49f;

		public float flDecayHFRatio = 0.83f;

		public int lReflections = -2602;

		public float flReflectionsDelay = 0.007f;

		public int lReverb = 200;

		public float flReverbDelay = 0.011f;

		public float flDiffusion = 100f;

		public float flDensity = 100f;

		public float flHFReference = 5000f;
	}
}
