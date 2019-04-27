using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Cdg
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassCdg
	{
		private BassCdg()
		{
		}

		[DllImport("bass_cdg")]
		public static extern void BASS_CDG_SetConfig(BASSCDGConfig config, int value);

		[DllImport("bass_cdg")]
		public static extern void BASS_CDG_SetVideoWindow(int handle, IntPtr win);

		[DllImport("bass_cdg")]
		public static extern IntPtr BASS_CDG_GetCurrentBitmap(int handle);

		public static bool LoadMe()
		{
			return Utils.LoadLib("bass_cdg", ref BassCdg._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bass_cdg"), ref BassCdg._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassCdg._myModuleHandle);
		}

		public static string SupportedStreamExtensions = "*.cdg";

		public static string SupportedStreamName = "CD+Graphics";

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "bass_cdg";
	}
}
