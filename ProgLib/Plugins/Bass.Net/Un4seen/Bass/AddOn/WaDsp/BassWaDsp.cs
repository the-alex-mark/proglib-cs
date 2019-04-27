using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.WaDsp
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassWaDsp
	{
		private BassWaDsp()
		{
		}

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_GetVersion();

		public static Version BASS_WADSP_GetVersion(int fieldcount)
		{
			if (fieldcount < 1)
			{
				fieldcount = 1;
			}
			if (fieldcount > 4)
			{
				fieldcount = 4;
			}
			int num = BassWaDsp.BASS_WADSP_GetVersion();
			Version result = new Version(2, 4);
			switch (fieldcount)
			{
			case 1:
				result = new Version(num >> 24 & 255, 0);
				break;
			case 2:
				result = new Version(num >> 24 & 255, num >> 16 & 255);
				break;
			case 3:
				result = new Version(num >> 24 & 255, num >> 16 & 255, num >> 8 & 255);
				break;
			case 4:
				result = new Version(num >> 24 & 255, num >> 16 & 255, num >> 8 & 255, num & 255);
				break;
			}
			return result;
		}

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_Init(IntPtr win);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_Free();

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_FreeDSP(int plugin);

		[DllImport("bass_wadsp")]
		public static extern IntPtr BASS_WADSP_GetFakeWinampWnd(int plugin);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_SetSongTitle(int plugin, [MarshalAs(UnmanagedType.LPStr)] [In] string title);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_SetFileName(int plugin, [MarshalAs(UnmanagedType.LPStr)] [In] string file);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_Load([MarshalAs(UnmanagedType.LPWStr)] [In] string dspfile, int x, int y, int width, int height, WINAMPWINPROC proc);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_Config(int plugin);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_Start(int plugin, int module, int handle);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_Stop(int plugin);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_SetChannel(int plugin, int handle);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_GetModule(int plugin);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ChannelSetDSP(int plugin, int handle, int priority);

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_ChannelRemoveDSP(int plugin);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesDSP(int plugin, IntPtr buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesDSP(int plugin, byte[] buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesDSP(int plugin, short[] buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesDSP(int plugin, int[] buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesDSP(int plugin, float[] buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesSTREAM(int plugin, IntPtr buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesSTREAM(int plugin, byte[] buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesSTREAM(int plugin, short[] buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesSTREAM(int plugin, int[] buffer, int length);

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_ModifySamplesSTREAM(int plugin, float[] buffer, int length);

		[DllImport("bass_wadsp", EntryPoint = "BASS_WADSP_GetName")]
		private static extern IntPtr BASS_WADSP_GetNamePtr(int plugin);

		public static string BASS_WADSP_GetName(int plugin)
		{
			IntPtr intPtr = BassWaDsp.BASS_WADSP_GetNamePtr(plugin);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_GetModuleCount(int plugin);

		[DllImport("bass_wadsp", EntryPoint = "BASS_WADSP_GetModuleName")]
		private static extern IntPtr BASS_WADSP_GetModuleNamePtr(int plugin, int module);

		public static string BASS_WADSP_GetModuleName(int plugin, int module)
		{
			IntPtr intPtr = BassWaDsp.BASS_WADSP_GetModuleNamePtr(plugin, module);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string[] BASS_WADSP_GetModuleNames(int plugin)
		{
			List<string> list = new List<string>();
			int num = 0;
			string text = string.Empty;
			while (text != null)
			{
				text = BassWaDsp.BASS_WADSP_GetModuleName(plugin, num);
				num++;
				if (text != null)
				{
					list.Add(text);
				}
			}
			Bass.BASS_GetCPU();
			return list.ToArray();
		}

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_PluginInfoFree();

		[DllImport("bass_wadsp")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WADSP_PluginInfoLoad([MarshalAs(UnmanagedType.LPWStr)] [In] string dspfile);

		[DllImport("bass_wadsp", EntryPoint = "BASS_WADSP_PluginInfoGetName")]
		private static extern IntPtr BASS_WADSP_PluginInfoGetNamePtr();

		public static string BASS_WADSP_PluginInfoGetName()
		{
			IntPtr intPtr = BassWaDsp.BASS_WADSP_PluginInfoGetNamePtr();
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		[DllImport("bass_wadsp")]
		public static extern int BASS_WADSP_PluginInfoGetModuleCount();

		[DllImport("bass_wadsp", EntryPoint = "BASS_WADSP_PluginInfoGetModuleName")]
		private static extern IntPtr BASS_WADSP_PluginInfoGetModuleNamePtr(int module);

		public static string BASS_WADSP_PluginInfoGetModuleName(int module)
		{
			IntPtr intPtr = BassWaDsp.BASS_WADSP_PluginInfoGetModuleNamePtr(module);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		public static string[] BASS_WADSP_PluginInfoGetModuleNames()
		{
			List<string> list = new List<string>();
			int num = 0;
			string text = string.Empty;
			while (text != null)
			{
				text = BassWaDsp.BASS_WADSP_PluginInfoGetModuleName(num);
				num++;
				if (text != null)
				{
					list.Add(text);
				}
			}
			Bass.BASS_GetCPU();
			return list.ToArray();
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("bass_wadsp", ref BassWaDsp._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bass_wadsp"), ref BassWaDsp._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassWaDsp._myModuleHandle);
		}

		public const int BASSWADSPVERSION = 516;

		private static int _myModuleHandle;

		private const string _myModuleName = "bass_wadsp";
	}
}
