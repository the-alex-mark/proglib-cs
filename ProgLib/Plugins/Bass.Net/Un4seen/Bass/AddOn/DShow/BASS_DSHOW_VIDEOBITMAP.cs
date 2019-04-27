using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.DShow
{
	[Serializable]
	public sealed class BASS_DSHOW_VIDEOBITMAP
	{
		[MarshalAs(UnmanagedType.Bool)]
		public bool visible = true;

		public int inLeft;

		public int inTop;

		public int inRight;

		public int inBottom;

		public float outLeft;

		public float outTop;

		public float outRight;

		public float outBottom;

		public float alphavalue = 1f;

		public int transColor = Color.Empty.ToArgb();

		public IntPtr bmp = IntPtr.Zero;
	}
}
