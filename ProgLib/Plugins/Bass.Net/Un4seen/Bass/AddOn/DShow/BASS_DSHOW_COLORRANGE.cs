using System;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	public sealed class BASS_DSHOW_COLORRANGE
	{
		public override string ToString()
		{
			return string.Format("{0}: Min={1:F}, Max={2:F}, Default={3:F}, Step={4:F}", new object[]
			{
				this.type,
				this.MinValue,
				this.MaxValue,
				this.DefaultValue,
				this.StepSize
			});
		}

		public float MinValue;

		public float MaxValue;

		public float DefaultValue;

		public float StepSize;

		public BASSDSHOWColorControl type;
	}
}
