using System;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	public sealed class BASS_DSHOW_VIDEOCOLORS
	{
		public override string ToString()
		{
			return string.Format("Contrast={0:F}, Brightness={1:F}, Hue={2:F}, Saturation={3:F}", new object[]
			{
				this.Contrast,
				this.Brightness,
				this.Hue,
				this.Saturation
			});
		}

		public float Contrast;

		public float Brightness;

		public float Hue;

		public float Saturation;
	}
}
