using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Un4seen.Bass;

namespace Un4seen.BassWasapi
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassWasapi
	{
		static BassWasapi()
		{
			string text = string.Empty;
			try
			{
				text = new StackFrame(1).GetMethod().Name;
			}
			catch
			{
			}
			if (!text.Equals("LoadMe"))
			{
				BassWasapi.InitBassWasapi();
			}
		}

		private BassWasapi()
		{
		}

		private static void InitBassWasapi()
		{
			if (!BassNet.OmitCheckVersion)
			{
				BassWasapi.CheckVersion();
			}
		}

		[DllImport("basswasapi")]
		public static extern int BASS_WASAPI_GetVersion();

		public static Version BASS_WASAPI_GetVersion(int fieldcount)
		{
			if (fieldcount < 1)
			{
				fieldcount = 1;
			}
			if (fieldcount > 4)
			{
				fieldcount = 4;
			}
			int num = BassWasapi.BASS_WASAPI_GetVersion();
			Version result = new Version(2, 3);
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

		[DllImport("basswasapi", EntryPoint = "BASS_WASAPI_GetDeviceInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_WASAPI_GetDeviceInfoInternal([In] int device, [In] [Out] ref BASS_WASAPI_DEVICEINFO_INTERNAL info);

		public static bool BASS_WASAPI_GetDeviceInfo(int device, BASS_WASAPI_DEVICEINFO info)
		{
			bool result;
			try
			{
				bool flag = BassWasapi.BASS_WASAPI_GetDeviceInfoInternal(device, ref info._internal);
				if (flag)
				{
					if (info._internal.name != IntPtr.Zero)
					{
						if (Bass.Bass._configUTF8)
						{
							info.name = Utils.IntPtrAsStringUtf8(info._internal.name);
						}
						else
						{
							info.name = Utils.IntPtrAsStringAnsi(info._internal.name);
						}
					}
					else
					{
						info.name = string.Empty;
					}
					if (info._internal.id != IntPtr.Zero)
					{
						if (Bass.Bass._configUTF8)
						{
							info.id = Utils.IntPtrAsStringUtf8(info._internal.id);
						}
						else
						{
							info.id = Utils.IntPtrAsStringAnsi(info._internal.id);
						}
					}
					else
					{
						info.id = string.Empty;
					}
					info.type = info._internal.type;
					info.flags = info._internal.flags;
					info.minperiod = info._internal.minperiod;
					info.defperiod = info._internal.defperiod;
					info.mixfreq = info._internal.mixfreq;
					info.mixchans = info._internal.mixchans;
				}
				result = flag;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static BASS_WASAPI_DEVICEINFO BASS_WASAPI_GetDeviceInfo(int device)
		{
			BASS_WASAPI_DEVICEINFO bass_WASAPI_DEVICEINFO = new BASS_WASAPI_DEVICEINFO();
			if (BassWasapi.BASS_WASAPI_GetDeviceInfo(device, bass_WASAPI_DEVICEINFO))
			{
				return bass_WASAPI_DEVICEINFO;
			}
			return null;
		}

		public static BASS_WASAPI_DEVICEINFO[] BASS_WASAPI_GetDeviceInfos()
		{
			List<BASS_WASAPI_DEVICEINFO> list = new List<BASS_WASAPI_DEVICEINFO>();
			int num = 0;
			BASS_WASAPI_DEVICEINFO item;
			while ((item = BassWasapi.BASS_WASAPI_GetDeviceInfo(num)) != null)
			{
				list.Add(item);
				num++;
			}
			BassWasapi.BASS_WASAPI_GetCPU();
			return list.ToArray();
		}

		public static int BASS_WASAPI_GetDeviceCount()
		{
			BASS_WASAPI_DEVICEINFO info = new BASS_WASAPI_DEVICEINFO();
			int num = 0;
			while (BassWasapi.BASS_WASAPI_GetDeviceInfo(num, info))
			{
				num++;
			}
			BassWasapi.BASS_WASAPI_GetCPU();
			return num;
		}

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_SetDevice(int device);

		[DllImport("basswasapi")]
		public static extern int BASS_WASAPI_GetDevice();

		[DllImport("basswasapi")]
		public static extern BASSWASAPIFormat BASS_WASAPI_CheckFormat(int device, int freq, int chans, BASSWASAPIInit flags);

		public static BASSWASAPIFormat BASS_WASAPI_CheckFormat(int device, int freq, int chans, BASSWASAPIFormat format)
		{
			return BassWasapi.BASS_WASAPI_CheckFormat(device, freq, chans, (BASSWASAPIInit)Utils.MakeLong(1, (int)format));
		}

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_Init(int device, int freq, int chans, BASSWASAPIInit flags, float buffer, float period, WASAPIPROC proc, IntPtr user);

		public static bool BASS_WASAPI_Init(int device, int freq, int chans, BASSWASAPIInit flags, BASSWASAPIFormat format, float buffer, float period, WASAPIPROC proc, IntPtr user)
		{
			flags |= BASSWASAPIInit.BASS_WASAPI_EXCLUSIVE;
			return BassWasapi.BASS_WASAPI_Init(device, freq, chans, (BASSWASAPIInit)Utils.MakeLong((int)flags, (int)format), buffer, period, proc, user);
		}

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_Free();

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_GetInfo([In] [Out] BASS_WASAPI_INFO info);

		public static BASS_WASAPI_INFO BASS_WASAPI_GetInfo()
		{
			BASS_WASAPI_INFO bass_WASAPI_INFO = new BASS_WASAPI_INFO();
			if (BassWasapi.BASS_WASAPI_GetInfo(bass_WASAPI_INFO))
			{
				return bass_WASAPI_INFO;
			}
			return null;
		}

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_Start();

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_Stop([MarshalAs(UnmanagedType.Bool)] bool reset);

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_IsStarted();

		[DllImport("basswasapi")]
		public static extern float BASS_WASAPI_GetCPU();

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern float BASS_WASAPI_Lock([MarshalAs(UnmanagedType.Bool)] bool state);

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_SetVolume(BASSWASAPIVolume curve, float volume);

		[DllImport("basswasapi")]
		public static extern float BASS_WASAPI_GetVolume(BASSWASAPIVolume curve);

		[DllImport("basswasapi")]
		public static extern int BASS_WASAPI_PutData(IntPtr buffer, int length);

		[DllImport("basswasapi")]
		public static extern int BASS_WASAPI_PutData(float[] buffer, int length);

		[DllImport("basswasapi")]
		public static extern int BASS_WASAPI_GetLevel();

		[DllImport("basswasapi", EntryPoint = "BASS_WASAPI_GetLevelEx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_GetLevel([In] [Out] float[] levels, float length, BASSLevel flags);

		public static float[] BASS_WASAPI_GetLevel(float length = 0.02f, BASSLevel flags = BASSLevel.BASS_LEVEL_ALL)
		{
			BASS_WASAPI_INFO bass_WASAPI_INFO = BassWasapi.BASS_WASAPI_GetInfo();
			if (bass_WASAPI_INFO == null)
			{
				return null;
			}
			int num = bass_WASAPI_INFO.chans;
			if ((flags & BASSLevel.BASS_LEVEL_MONO) == BASSLevel.BASS_LEVEL_MONO)
			{
				num = 1;
			}
			else if ((flags & BASSLevel.BASS_LEVEL_STEREO) == BASSLevel.BASS_LEVEL_STEREO)
			{
				num = 2;
			}
			float[] array = new float[num];
			if (BassWasapi.BASS_WASAPI_GetLevel(array, length, flags))
			{
				return array;
			}
			return null;
		}

		[DllImport("basswasapi")]
		public static extern int BASS_WASAPI_GetData(IntPtr buffer, int length);

		[DllImport("basswasapi")]
		public static extern int BASS_WASAPI_GetData([In] [Out] float[] buffer, int length);

		[DllImport("basswasapi")]
		public static extern float BASS_WASAPI_GetDeviceLevel(int device, int chan);

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_SetMute(BASSWASAPIVolume mode, [MarshalAs(UnmanagedType.Bool)] bool mute);

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_GetMute(BASSWASAPIVolume mode);

		[DllImport("basswasapi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_WASAPI_SetNotify(WASAPINOTIFYPROC proc, IntPtr user);

		public static bool LoadMe()
		{
			bool flag = Utils.LoadLib("basswasapi", ref BassWasapi._myModuleHandle);
			if (flag)
			{
				BassWasapi.InitBassWasapi();
			}
			return flag;
		}

		public static bool LoadMe(string path)
		{
			bool flag = Utils.LoadLib(Path.Combine(path, "basswasapi"), ref BassWasapi._myModuleHandle);
			if (flag)
			{
				BassWasapi.InitBassWasapi();
			}
			return flag;
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassWasapi._myModuleHandle);
		}

		private static void CheckVersion()
		{
			try
			{
				if (Utils.HighWord(BassWasapi.BASS_WASAPI_GetVersion()) != 516)
				{
					Version version = BassWasapi.BASS_WASAPI_GetVersion(2);
					Version version2 = new Version(2, 4);
					FileVersionInfo fileVersionInfo = null;
					ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
					for (int i = modules.Count - 1; i >= 0; i--)
					{
						ProcessModule processModule = modules[i];
						if (processModule.ModuleName.ToLower().Equals("basswasapi".ToLower()))
						{
							fileVersionInfo = processModule.FileVersionInfo;
							break;
						}
					}
					if (fileVersionInfo != null)
					{
						MessageBox.Show(string.Format("An incorrect version of BASSWASAPI was loaded!\r\n\r\nVersion loaded: {0}.{1}\r\nVersion expected: {2}.{3}\r\n\r\nFile: {4}\r\nFileVersion: {5}\r\nDescription: {6}\r\nCompany: {7}\r\nLanguage: {8}", new object[]
						{
							version.Major,
							version.Minor,
							version2.Major,
							version2.Minor,
							fileVersionInfo.FileName,
							fileVersionInfo.FileVersion,
							fileVersionInfo.FileDescription,
							fileVersionInfo.CompanyName + " " + fileVersionInfo.LegalCopyright,
							fileVersionInfo.Language
						}), "Incorrect BASSWASAPI Version", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						MessageBox.Show(string.Format("An incorrect version of BASSWASAPI was loaded!\r\n\r\nVersion loaded: {0}.{1}\r\nVersion expected: {2}.{3}", new object[]
						{
							version.Major,
							version.Minor,
							version2.Major,
							version2.Minor
						}), "Incorrect BASSWASAPI Version", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			catch
			{
			}
		}

		public const int BASSWASAPIVERSION = 516;

		private static int _myModuleHandle;

		private const string _myModuleName = "basswasapi";
	}
}
