using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Winamp
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassWinamp
	{
		private BassWinamp()
		{
		}

		[DllImport("bass_winamp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WINAMP_GetIsSeekable(int handle);

		[DllImport("bass_winamp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WINAMP_GetUsesOutput(int handle);

		[DllImport("bass_winamp", EntryPoint = "BASS_WINAMP_GetFileInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_WINAMP_GetFileInfoIntPtr([MarshalAs(UnmanagedType.LPStr)] [In] string file, IntPtr title, ref int lenms);

		public static bool BASS_WINAMP_GetFileInfo(string file, ref string title, ref int lenms)
		{
			bool flag = false;
			title = new string('\0', 255);
			GCHandle gchandle = GCHandle.Alloc(title, GCHandleType.Pinned);
			try
			{
				flag = BassWinamp.BASS_WINAMP_GetFileInfoIntPtr(file, gchandle.AddrOfPinnedObject(), ref lenms);
				if (flag)
				{
					title = Utils.IntPtrAsStringAnsi(gchandle.AddrOfPinnedObject());
				}
				else
				{
					title = string.Empty;
				}
			}
			finally
			{
				gchandle.Free();
			}
			return flag;
		}

		[DllImport("bass_winamp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WINAMP_InfoDlg([MarshalAs(UnmanagedType.LPStr)] [In] string file, IntPtr win);

		[DllImport("bass_winamp", EntryPoint = "BASS_WINAMP_GetName")]
		private static extern IntPtr BASS_WINAMP_GetNamePtr(int handle);

		public static string BASS_WINAMP_GetName(int handle)
		{
			IntPtr intPtr = BassWinamp.BASS_WINAMP_GetNamePtr(handle);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		[DllImport("bass_winamp")]
		public static extern IntPtr BASS_WINAMP_GetExtentions(int handle);

		public static string BASS_WINAMP_GetExtentionsFilter(int handle)
		{
			IntPtr intPtr = BassWinamp.BASS_WINAMP_GetExtentions(handle);
			if (!(intPtr != IntPtr.Zero))
			{
				return null;
			}
			string[] array = Utils.IntPtrToArrayNullTermAnsi(intPtr);
			if (array != null && array.Length >= 0)
			{
				string text = "";
				foreach (string str in array)
				{
					text = text + str + "|";
				}
				return text.Substring(0, text.Length - 1);
			}
			return null;
		}

		[DllImport("bass_winamp", EntryPoint = "BASS_WINAMP_FindPlugins")]
		private static extern IntPtr BASS_WINAMP_FindPluginsPtr([MarshalAs(UnmanagedType.LPStr)] [In] string pluginpath, BASSWINAMPFindPlugin flags);

		public static string[] BASS_WINAMP_FindPlugins(string pluginpath, BASSWINAMPFindPlugin flags)
		{
			flags &= ~BASSWINAMPFindPlugin.BASS_WINAMP_FIND_COMMALIST;
			IntPtr intPtr = BassWinamp.BASS_WINAMP_FindPluginsPtr(pluginpath, flags);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrToArrayNullTermAnsi(intPtr);
			}
			return null;
		}

		[DllImport("bass_winamp")]
		public static extern int BASS_WINAMP_LoadPlugin([MarshalAs(UnmanagedType.LPStr)] [In] string file);

		[DllImport("bass_winamp")]
		public static extern void BASS_WINAMP_UnloadPlugin(int handle);

		[DllImport("bass_winamp")]
		public static extern int BASS_WINAMP_GetVersion(int handle);

		[DllImport("bass_winamp")]
		public static extern void BASS_WINAMP_ConfigPlugin(int handle, IntPtr win);

		[DllImport("bass_winamp")]
		public static extern void BASS_WINAMP_AboutPlugin(int handle, IntPtr win);

		[DllImport("bass_winamp", EntryPoint = "BASS_WINAMP_StreamCreate")]
		private static extern int BASS_WINAMP_StreamCreateAnsi([MarshalAs(UnmanagedType.LPStr)] [In] string file, BASSFlag flags);

		public static int BASS_WINAMP_StreamCreate(string file, BASSFlag flags)
		{
			flags &= (BASSFlag.BASS_SAMPLE_8BITS | BASSFlag.BASS_SAMPLE_MONO | BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_SOFTWARE | BASSFlag.BASS_SAMPLE_MUTEMAX | BASSFlag.BASS_SAMPLE_VAM | BASSFlag.BASS_SAMPLE_FX | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE | BASSFlag.BASS_RECORD_ECHOCANCEL | BASSFlag.BASS_RECORD_AGC | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_RESTRATE | BASSFlag.BASS_STREAM_BLOCK | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_SPEAKER_FRONT | BASSFlag.BASS_SPEAKER_REAR | BASSFlag.BASS_SPEAKER_REAR2 | BASSFlag.BASS_SPEAKER_LEFT | BASSFlag.BASS_SPEAKER_RIGHT | BASSFlag.BASS_SPEAKER_PAIR8 | BASSFlag.BASS_ASYNCFILE | BASSFlag.BASS_SAMPLE_OVER_VOL | BASSFlag.BASS_WV_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 | BASSFlag.BASS_AC3_DOWNMIX_4 | BASSFlag.BASS_AC3_DYNAMIC_RANGE | BASSFlag.BASS_AAC_FRAME960);
			return BassWinamp.BASS_WINAMP_StreamCreateAnsi(file, flags);
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("bass_winamp", ref BassWinamp._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bass_winamp"), ref BassWinamp._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassWinamp._myModuleHandle);
		}

		private static int _myModuleHandle;

		private const string _myModuleName = "bass_winamp";
	}
}
