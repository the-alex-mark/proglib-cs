using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_SAMPLE
	{
		public BASS_SAMPLE()
		{
		}

		public BASS_SAMPLE(int Freq, float Volume, float Pan, BASSFlag Flags, int Length, int Max, int OrigRes, int Chans, int MinGap, BASS3DMode Flag3D, float MinDist, float MaxDist, int IAngle, int OAngle, float OutVol, BASSVam FlagsVam, int Priority)
		{
			this.freq = Freq;
			this.volume = Volume;
			this.pan = Pan;
			this.flags = Flags;
			this.length = Length;
			this.max = Max;
			this.origres = OrigRes;
			this.chans = Chans;
			this.mingap = MinGap;
			this.mode3d = Flag3D;
			this.mindist = MinDist;
			this.maxdist = MaxDist;
			this.iangle = IAngle;
			this.oangle = OAngle;
			this.outvol = OutVol;
			this.vam = FlagsVam;
			this.priority = Priority;
		}

		public override string ToString()
		{
			return string.Format("Frequency={0}, Volume={1}, Pan={2}", this.freq, this.volume, this.pan);
		}

		public int freq = 44100;

		public float volume = 1f;

		public float pan;

		public BASSFlag flags;

		public int length;

		public int max = 1;

		public int origres;

		public int chans = 2;

		public int mingap;

		public BASS3DMode mode3d;

		public float mindist;

		public float maxdist;

		public int iangle;

		public int oangle;

		public float outvol = 1f;

		public BASSVam vam = BASSVam.BASS_VAM_HARDWARE;

		public int priority;
	}
}
