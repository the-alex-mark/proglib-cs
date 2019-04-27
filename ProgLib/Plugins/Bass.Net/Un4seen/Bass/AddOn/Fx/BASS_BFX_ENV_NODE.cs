using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BASS_BFX_ENV_NODE
	{
		public BASS_BFX_ENV_NODE(double Pos, float Val)
		{
			this.pos = Pos;
			this.val = Val;
		}

		public override string ToString()
		{
			return string.Format("Pos={0}, Val={1}", this.pos, this.val);
		}

		public double pos;

		public float val;
	}
}
