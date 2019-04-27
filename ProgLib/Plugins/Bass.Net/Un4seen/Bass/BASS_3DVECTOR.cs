using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_3DVECTOR
	{
		public BASS_3DVECTOR()
		{
		}

		public BASS_3DVECTOR(float X, float Y, float Z)
		{
			this.x = X;
			this.y = Y;
			this.z = Z;
		}

		public override string ToString()
		{
			return string.Format("X={0}, Y={1}, Z={2}", this.x, this.y, this.z);
		}

		public float x;

		public float y;

		public float z;
	}
}
