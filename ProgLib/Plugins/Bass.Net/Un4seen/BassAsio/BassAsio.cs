using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Un4seen.Bass;

namespace Un4seen.BassAsio
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassAsio
	{
		static BassAsio()
		{
			try
			{
				BassAsio._isUnicode = BassAsio.BASS_ASIO_SetUnicode(true);
			}
			catch
			{
				BassAsio._isUnicode = false;
			}
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
				BassAsio.InitBassAsio();
			}
		}

		private BassAsio()
		{
		}

		private static void InitBassAsio()
		{
			if (!BassNet.OmitCheckVersion)
			{
				BassAsio.CheckVersion();
			}
		}

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_ASIO_SetUnicode([MarshalAs(UnmanagedType.Bool)] bool unicode);

		[DllImport("bassasio")]
		public static extern BASSError BASS_ASIO_ErrorGetCode();

		[DllImport("bassasio", EntryPoint = "BASS_ASIO_GetDeviceInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_ASIO_GetDeviceInfoInternal([In] int device, [In] [Out] ref BASS_ASIO_DEVICEINFO_INTERNAL info);

		public static bool BASS_ASIO_GetDeviceInfo(int device, BASS_ASIO_DEVICEINFO info)
		{
			bool flag = BassAsio.BASS_ASIO_GetDeviceInfoInternal(device, ref info._internal);
			if (flag)
			{
				if (BassAsio._isUnicode)
				{
					info.name = Marshal.PtrToStringUni(info._internal.name);
					info.driver = Marshal.PtrToStringUni(info._internal.driver);
					return flag;
				}
				info.name = Utils.IntPtrAsStringAnsi(info._internal.name);
				info.driver = Utils.IntPtrAsStringAnsi(info._internal.driver);
			}
			return flag;
		}

		public static BASS_ASIO_DEVICEINFO BASS_ASIO_GetDeviceInfo(int device)
		{
			BASS_ASIO_DEVICEINFO bass_ASIO_DEVICEINFO = new BASS_ASIO_DEVICEINFO();
			if (BassAsio.BASS_ASIO_GetDeviceInfo(device, bass_ASIO_DEVICEINFO))
			{
				return bass_ASIO_DEVICEINFO;
			}
			return null;
		}

		public static BASS_ASIO_DEVICEINFO[] BASS_ASIO_GetDeviceInfos()
		{
			List<BASS_ASIO_DEVICEINFO> list = new List<BASS_ASIO_DEVICEINFO>();
			int num = 0;
			BASS_ASIO_DEVICEINFO item;
			while ((item = BassAsio.BASS_ASIO_GetDeviceInfo(num)) != null)
			{
				list.Add(item);
				num++;
			}
			BassAsio.BASS_ASIO_GetCPU();
			return list.ToArray();
		}

		public static int BASS_ASIO_GetDeviceCount()
		{
			BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();
			int num = 0;
			while (BassAsio.BASS_ASIO_GetDeviceInfo(num, info))
			{
				num++;
			}
			BassAsio.BASS_ASIO_GetCPU();
			return num;
		}

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_SetDevice(int device);

		[DllImport("bassasio")]
		public static extern int BASS_ASIO_GetDevice();

		[DllImport("bassasio", EntryPoint = "BASS_ASIO_AddDevice")]
		private static extern int BASS_ASIO_AddDevicePtr(IntPtr clsid, [MarshalAs(UnmanagedType.LPStr)] [In] string driver, [MarshalAs(UnmanagedType.LPStr)] [In] string name);

		[DllImport("bassasio", EntryPoint = "BASS_ASIO_AddDevice")]
		private static extern int BASS_ASIO_AddDeviceGuid([MarshalAs(UnmanagedType.LPStruct)] Guid clsid, [MarshalAs(UnmanagedType.LPStr)] [In] string driver, [MarshalAs(UnmanagedType.LPStr)] [In] string name);

		[DllImport("bassasio", EntryPoint = "BASS_ASIO_AddDevice")]
		private static extern int BASS_ASIO_AddDevicePtrUnicode(IntPtr clsid, [MarshalAs(UnmanagedType.LPWStr)] [In] string driver, [MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		[DllImport("bassasio", EntryPoint = "BASS_ASIO_AddDevice")]
		private static extern int BASS_ASIO_AddDeviceGuidUnicode([MarshalAs(UnmanagedType.LPStruct)] Guid clsid, [MarshalAs(UnmanagedType.LPWStr)] [In] string driver, [MarshalAs(UnmanagedType.LPWStr)] [In] string name);

		public static int BASS_ASIO_AddDevice(Guid clsid, string driver, string name)
		{
			if (clsid == Guid.Empty)
			{
				if (BassAsio._isUnicode)
				{
					return BassAsio.BASS_ASIO_AddDevicePtrUnicode(IntPtr.Zero, driver, name);
				}
				return BassAsio.BASS_ASIO_AddDevicePtr(IntPtr.Zero, driver, name);
			}
			else
			{
				if (BassAsio._isUnicode)
				{
					return BassAsio.BASS_ASIO_AddDeviceGuidUnicode(clsid, driver, name);
				}
				return BassAsio.BASS_ASIO_AddDeviceGuid(clsid, driver, name);
			}
		}

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_Init(int device, BASSASIOInit flags);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_Free();

		[DllImport("bassasio")]
		public static extern int BASS_ASIO_GetVersion();

		public static Version BASS_ASIO_GetVersion(int fieldcount)
		{
			if (fieldcount < 1)
			{
				fieldcount = 1;
			}
			if (fieldcount > 4)
			{
				fieldcount = 4;
			}
			int num = BassAsio.BASS_ASIO_GetVersion();
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

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_Stop();

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_GetInfo([In] [Out] BASS_ASIO_INFO info);

		public static BASS_ASIO_INFO BASS_ASIO_GetInfo()
		{
			BASS_ASIO_INFO bass_ASIO_INFO = new BASS_ASIO_INFO();
			if (BassAsio.BASS_ASIO_GetInfo(bass_ASIO_INFO))
			{
				return bass_ASIO_INFO;
			}
			return null;
		}

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_SetRate(double rate);

		[DllImport("bassasio", EntryPoint = "BASS_ASIO_Start")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_ASIO_StartOld(int buflen);

		[DllImport("bassasio", EntryPoint = "BASS_ASIO_Start")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_ASIO_StartInternal(int buflen, int threads);

		public static bool BASS_ASIO_Start(int buflen)
		{
			return BassAsio.BASS_ASIO_Start(buflen, 0);
		}

		public static bool BASS_ASIO_Start(int buflen, int threads)
		{
			if (BassAsio.BASS_ASIO_GetVersion() >= 16908546)
			{
				return BassAsio.BASS_ASIO_StartInternal(buflen, threads);
			}
			return BassAsio.BASS_ASIO_StartOld(buflen);
		}

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_IsStarted();

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ControlPanel();

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_CheckRate(double rate);

		[DllImport("bassasio")]
		public static extern double BASS_ASIO_GetRate();

		[DllImport("bassasio")]
		public static extern int BASS_ASIO_GetLatency([MarshalAs(UnmanagedType.Bool)] bool input);

		[DllImport("bassasio")]
		public static extern float BASS_ASIO_GetCPU();

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_Monitor(int input, int output, int gain, int state, int pan);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_SetNotify(ASIONOTIFYPROC proc, IntPtr user);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_Future(int selector, IntPtr param);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_Future(BASSASIOFuture selector, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object param);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_SetDSD([MarshalAs(UnmanagedType.Bool)] bool dsd);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelGetInfo([MarshalAs(UnmanagedType.Bool)] bool input, int channel, [In] [Out] BASS_ASIO_CHANNELINFO info);

		public static BASS_ASIO_CHANNELINFO BASS_ASIO_ChannelGetInfo(bool input, int channel)
		{
			BASS_ASIO_CHANNELINFO bass_ASIO_CHANNELINFO = new BASS_ASIO_CHANNELINFO();
			if (BassAsio.BASS_ASIO_ChannelGetInfo(input, channel, bass_ASIO_CHANNELINFO))
			{
				return bass_ASIO_CHANNELINFO;
			}
			return null;
		}

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelEnable([MarshalAs(UnmanagedType.Bool)] bool input, int channel, ASIOPROC proc, IntPtr user);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelJoin([MarshalAs(UnmanagedType.Bool)] bool input, int channel, int channel2);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelSetFormat([MarshalAs(UnmanagedType.Bool)] bool input, int channel, BASSASIOFormat format);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelSetRate([MarshalAs(UnmanagedType.Bool)] bool input, int channel, double rate);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelReset([MarshalAs(UnmanagedType.Bool)] bool input, int channel, BASSASIOReset flags);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelPause([MarshalAs(UnmanagedType.Bool)] bool input, int channel);

		[DllImport("bassasio")]
		public static extern BASSASIOActive BASS_ASIO_ChannelIsActive([MarshalAs(UnmanagedType.Bool)] bool input, int channel);

		[DllImport("bassasio")]
		public static extern BASSASIOFormat BASS_ASIO_ChannelGetFormat([MarshalAs(UnmanagedType.Bool)] bool input, int channel);

		[DllImport("bassasio")]
		public static extern double BASS_ASIO_ChannelGetRate([MarshalAs(UnmanagedType.Bool)] bool input, int channel);

		[DllImport("bassasio")]
		public static extern float BASS_ASIO_ChannelGetLevel([MarshalAs(UnmanagedType.Bool)] bool input, int channel);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelEnableMirror(int channel, [MarshalAs(UnmanagedType.Bool)] bool input2, int channel2);

		[DllImport("bassasio")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_ASIO_ChannelSetVolume([MarshalAs(UnmanagedType.Bool)] bool input, int channel, float volume);

		[DllImport("bassasio")]
		public static extern float BASS_ASIO_ChannelGetVolume([MarshalAs(UnmanagedType.Bool)] bool input, int channel);

		public static bool LoadMe()
		{
			bool flag = Utils.LoadLib("bassasio", ref BassAsio._myModuleHandle);
			if (flag)
			{
				BassAsio.InitBassAsio();
			}
			return flag;
		}

		public static bool LoadMe(string path)
		{
			bool flag = Utils.LoadLib(Path.Combine(path, "bassasio"), ref BassAsio._myModuleHandle);
			if (flag)
			{
				BassAsio.InitBassAsio();
			}
			return flag;
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassAsio._myModuleHandle);
		}

		private static void CheckVersion()
		{
			try
			{
				if (Utils.HighWord(BassAsio.BASS_ASIO_GetVersion()) != 259)
				{
					Version version = BassAsio.BASS_ASIO_GetVersion(2);
					Version version2 = new Version(1, 3);
					FileVersionInfo fileVersionInfo = null;
					ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
					for (int i = modules.Count - 1; i >= 0; i--)
					{
						ProcessModule processModule = modules[i];
						if (processModule.ModuleName.ToLower().Equals("bassasio".ToLower()))
						{
							fileVersionInfo = processModule.FileVersionInfo;
							break;
						}
					}
					if (fileVersionInfo != null)
					{
						MessageBox.Show(string.Format("An incorrect version of BASSASIO was loaded!\r\n\r\nVersion loaded: {0}.{1}\r\nVersion expected: {2}.{3}\r\n\r\nFile: {4}\r\nFileVersion: {5}\r\nDescription: {6}\r\nCompany: {7}\r\nLanguage: {8}", new object[]
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
						}), "Incorrect BASSASIO Version", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						MessageBox.Show(string.Format("An incorrect version of BASSASIO was loaded!\r\n\r\nVersion loaded: {0}.{1}\r\nVersion expected: {2}.{3}", new object[]
						{
							version.Major,
							version.Minor,
							version2.Major,
							version2.Minor
						}), "Incorrect BASSASIO Version", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			catch
			{
			}
		}

		private static bool _isUnicode = true;

		public const int BASSASIOVERSION = 259;

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "bassasio";
	}
}
