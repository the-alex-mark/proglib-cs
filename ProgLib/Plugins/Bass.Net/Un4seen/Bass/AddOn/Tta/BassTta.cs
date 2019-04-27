using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Tta
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassTta
	{
		private BassTta()
		{
		}

		[DllImport("bass_tta", EntryPoint = "BASS_TTA_StreamCreateFile")]
		private static extern int BASS_TTA_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [MarshalAs(UnmanagedType.LPWStr)] [In] string file, long offset, long length, BASSFlag flags);

		public static int BASS_TTA_StreamCreateFile(string file, long offset, long length, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassTta.BASS_TTA_StreamCreateFileUnicode(false, file, offset, length, flags);
		}

		[DllImport("bass_tta", EntryPoint = "BASS_TTA_StreamCreateFile")]
		private static extern int BASS_TTA_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, long length, BASSFlag flags);

		public static int BASS_TTA_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags)
		{
			return BassTta.BASS_TTA_StreamCreateFileMemory(true, memory, offset, length, flags);
		}

		[DllImport("bass_tta")]
		public static extern int BASS_TTA_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user);

		public static bool LoadMe()
		{
			return Utils.LoadLib("bass_tta", ref BassTta._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bass_tta"), ref BassTta._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassTta._myModuleHandle);
		}

		public static string SupportedStreamExtensions = "*.tta";

		public static string SupportedStreamName = "The True Audio";

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "bass_tta";
	}
}
