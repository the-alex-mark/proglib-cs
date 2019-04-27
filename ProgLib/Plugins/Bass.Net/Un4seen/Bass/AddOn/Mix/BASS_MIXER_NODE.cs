using System;

namespace Un4seen.Bass.AddOn.Mix
{
	[Serializable]
	public struct BASS_MIXER_NODE
	{
		public BASS_MIXER_NODE(long Pos, float Val)
		{
			this.pos = Pos;
			this.val = Val;
		}

		public override string ToString()
		{
			return string.Format("Pos={0}, Val={1}", this.pos, this.val);
		}

		public long pos;

		public float val;
	}
}
