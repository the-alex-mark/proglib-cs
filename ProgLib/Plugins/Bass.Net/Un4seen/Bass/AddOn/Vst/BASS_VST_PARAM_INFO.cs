using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Vst
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_VST_PARAM_INFO
	{
		public override string ToString()
		{
			return string.Format("{0} = {1}", this.name, this.display);
		}

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string name = string.Empty;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string unit = string.Empty;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string display = string.Empty;

		public float defaultValue;
	}
}
