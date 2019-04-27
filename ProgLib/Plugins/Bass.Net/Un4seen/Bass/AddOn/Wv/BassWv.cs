using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Wv
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassWv
	{
		private BassWv()
		{
		}

		[DllImport("basswv", EntryPoint = "BASS_WV_StreamCreateFile")]
		private static extern int BASS_WV_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [MarshalAs(UnmanagedType.LPWStr)] [In] string file, long offset, long length, BASSFlag flags);

		public static int BASS_WV_StreamCreateFile(string file, long offset, long length, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassWv.BASS_WV_StreamCreateFileUnicode(false, file, offset, length, flags);
		}

		[DllImport("basswv", EntryPoint = "BASS_WV_StreamCreateFile")]
		private static extern int BASS_WV_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, long length, BASSFlag flags);

		public static int BASS_WV_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags)
		{
			return BassWv.BASS_WV_StreamCreateFileMemory(true, memory, offset, length, flags);
		}

		[DllImport("basswv")]
		public static extern int BASS_WV_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user);

		[DllImport("basswv", EntryPoint = "BASS_WV_StreamCreateURL")]
		private static extern int BASS_WV_StreamCreateURLUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

		public static int BASS_WV_StreamCreateURL(string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassWv.BASS_WV_StreamCreateURLUnicode(url, offset, flags, proc, user);
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("basswv", ref BassWv._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "basswv"), ref BassWv._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassWv._myModuleHandle);
		}

		public static string SupportedStreamExtensions = "*.wv";

		public static string SupportedStreamName = "WavPack Audio";

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "basswv";
	}
}
