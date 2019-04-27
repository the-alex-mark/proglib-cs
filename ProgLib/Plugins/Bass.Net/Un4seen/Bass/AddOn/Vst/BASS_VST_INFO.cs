using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Vst
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_VST_INFO
	{
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.productName) && !string.IsNullOrEmpty(this.vendorName))
			{
				return string.Format("{0} ({1}, {2})", this.effectName, this.productName, this.vendorName);
			}
			if (string.IsNullOrEmpty(this.vendorName) && !string.IsNullOrEmpty(this.productName))
			{
				return string.Format("{0} ({1})", this.effectName, this.productName);
			}
			if (string.IsNullOrEmpty(this.productName) && !string.IsNullOrEmpty(this.vendorName))
			{
				return string.Format("{0} ({1})", this.effectName, this.vendorName);
			}
			return string.Format("{0}", string.IsNullOrEmpty(this.effectName) ? "Unknown" : this.effectName);
		}

		public int channelHandle;

		public int uniqueID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string effectName = string.Empty;

		public int effectVersion;

		public int effectVstVersion;

		public int hostVstVersion;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string productName = string.Empty;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string vendorName = string.Empty;

		public int vendorVersion;

		public int chansIn;

		public int chansOut;

		public int initialDelay;

		[MarshalAs(UnmanagedType.Bool)]
		public bool hasEditor;

		public int editorWidth;

		public int editorHeight;

		public IntPtr aeffect = IntPtr.Zero;

		[MarshalAs(UnmanagedType.Bool)]
		public bool isInstrument;

		public int dspHandle;
	}
}
