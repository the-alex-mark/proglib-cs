using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace Un4seen.Bass.AddOn.DShow
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassDShow
	{
		private BassDShow()
		{
		}

		[DllImport("xVideo", EntryPoint = "xVideo_GetVersion")]
		public static extern int BASS_DSHOW_GetVersion();

		public static Version BASS_DSHOW_GetVersion(int fieldcount)
		{
			if (fieldcount < 1)
			{
				fieldcount = 1;
			}
			if (fieldcount > 4)
			{
				fieldcount = 4;
			}
			int num = BassDShow.BASS_DSHOW_GetVersion();
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

		[DllImport("xVideo", EntryPoint = "xVideo_ErrorGetCode")]
		public static extern BASSDSHOWError BASS_DSHOW_ErrorGetCode();

		[DllImport("xVideo", EntryPoint = "xVideo_Init")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_DSHOW_Init(IntPtr hWnd, int flags);

		public static bool BASS_DSHOW_Init(IntPtr hWnd, BASSDSHOWInit flags)
		{
			if (!BassNet.OmitCheckVersion)
			{
				BassDShow.CheckVersion();
			}
			return BassDShow.BASS_DSHOW_Init(hWnd, (int)flags);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_Free")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_Free();

		[DllImport("xVideo", EntryPoint = "xVideo_StreamCreateFile")]
		private static extern int BASS_DSHOW_StreamCreateFileUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string file, int pos, IntPtr win, BASSFlag flags);

		public static int BASS_DSHOW_StreamCreateFile(string file, int pos, IntPtr win, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassDShow.BASS_DSHOW_StreamCreateFileUnicode(file, pos, win, flags);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_StreamCreateFileMem")]
		public static extern int BASS_DSHOW_StreamCreateFile(IntPtr memory, long length, IntPtr win, BASSFlag flags);

		[DllImport("xVideo", EntryPoint = "xVideo_StreamCreateFileUser")]
		public static extern int BASS_DSHOW_StreamCreateFileUser(BASSFlag flags, IntPtr win, BASS_FILEPROCS procs, IntPtr user);

		[DllImport("xVideo", EntryPoint = "xVideo_StreamCreateFilter")]
		private static extern int BASS_DSHOW_StreamCreateFilterUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string file, [MarshalAs(UnmanagedType.LPStr)] [In] string filter, IntPtr win, BASSFlag flags);

		public static int BASS_DSHOW_StreamCreateFilter(string file, string filter, IntPtr win, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassDShow.BASS_DSHOW_StreamCreateFilterUnicode(file, filter, win, flags);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelSetPosition")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelSetPosition(int handle, double pos, BASSDSHOWMode mode);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetPosition")]
		public static extern double BASS_DSHOW_ChannelGetPosition(int handle, BASSDSHOWMode mode);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetLength")]
		public static extern double BASS_DSHOW_ChannelGetLength(int handle, BASSDSHOWMode mode);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelPlay")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelPlay(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelPause")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelPause(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelStop")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelStop(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_StreamFree")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_StreamFree(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetInfo")]
		public static extern void BASS_DSHOW_ChannelGetInfo(int handle, [In] [Out] ref BASS_DSHOW_CHANNELINFO info);

		public static BASS_DSHOW_CHANNELINFO BASS_DSHOW_ChannelGetInfo(int handle)
		{
			BASS_DSHOW_CHANNELINFO result = new BASS_DSHOW_CHANNELINFO();
			BassDShow.BASS_DSHOW_ChannelGetInfo(handle, ref result);
			return result;
		}

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelAddWindow")]
		public static extern int BASS_DSHOW_ChannelAddWindow(int handle, IntPtr hWnd);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelAddWindow")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelRemoveWindow(int handle, int window);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelResizeWindow")]
		public static extern void BASS_DSHOW_ChannelResizeWindow(int handle, int window, int left, int top, int right, int bottom);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelSetFullscreen")]
		public static extern void BASS_DSHOW_ChannelSetFullscreen(int handle, [MarshalAs(UnmanagedType.Bool)] [In] bool fullScreen);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelSetWindow")]
		public static extern void BASS_DSHOW_ChannelSetWindow(int handle, int window, IntPtr hWnd);

		[DllImport("xVideo", EntryPoint = "xVideo_SetConfig")]
		public static extern void BASS_DSHOW_SetConfig(BASSDSHOWConfig option, int newvalue);

		[DllImport("xVideo", EntryPoint = "xVideo_SetConfig")]
		public static extern void BASS_DSHOW_SetConfig(BASSDSHOWConfig option, BASSDSHOWConfigFlag newvalue);

		[DllImport("xVideo", EntryPoint = "xVideo_GetConfig")]
		public static extern int BASS_DSHOW_GetConfig(BASSDSHOWConfig option);

		[DllImport("xVideo", EntryPoint = "xVideo_GetGraph")]
		public static extern IntPtr BASS_DSHOW_GetGraph(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetBitmap")]
		public static extern IntPtr BASS_DSHOW_ChannelGetBitmapPtr(int handle);

		public static Image BASS_DSHOW_ChannelGetBitmap(int handle)
		{
			IntPtr intPtr = BassDShow.BASS_DSHOW_ChannelGetBitmapPtr(handle);
			if (intPtr != IntPtr.Zero)
			{
				return Image.FromHbitmap(intPtr);
			}
			return null;
		}

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelColorRange")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelColorRange(int handle, BASSDSHOWColorControl id, [In] [Out] ref BASS_DSHOW_COLORRANGE ctrl);

		public static BASS_DSHOW_COLORRANGE BASS_DSHOW_ChannelColorRange(int handle, BASSDSHOWColorControl id)
		{
			BASS_DSHOW_COLORRANGE result = new BASS_DSHOW_COLORRANGE();
			if (BassDShow.BASS_DSHOW_ChannelColorRange(handle, id, ref result))
			{
				return result;
			}
			return null;
		}

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelSetColors")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelSetColors(int handle, BASSDSHOWColorControl id, [In] BASS_DSHOW_VIDEOCOLORS colors);

		[DllImport("xVideo", EntryPoint = "xVideo_SetVideoAlpha")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_SetVideoAlpha(int handle, int win, int layer, float alpha);

		[DllImport("xVideo", EntryPoint = "xVideo_GetVideoAlpha")]
		public static extern float BASS_DSHOW_GetVideoAlpha(int handle, int win, int layer);

		[DllImport("xVideo", EntryPoint = "xVideo_MIXChannelResize")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_MIXChannelResize(int handle, int layer, int left, int top, int right, int bottom);

		[DllImport("xVideo", EntryPoint = "xVideo_CallbackItemByIndex")]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public static extern string BASS_DSHOW_CallbackItemByIndex(BASSDSHOWCallbackItem callbackType, int index);

		[DllImport("xVideo", EntryPoint = "xVideo_CaptureGetDevices")]
		public static extern int BASS_DSHOW_CaptureGetDevices(BASSDSHOWCapture deviceType, ENUMDEVICESPROC proc, IntPtr user);

		[DllImport("xVideo", EntryPoint = "xVideo_CaptureDeviceProfiles")]
		public static extern int BASS_DSHOW_CaptureDeviceProfiles(int device, BASSDSHOWCapture deviceType, ENUMPROFILESPROC proc, IntPtr user);

		[DllImport("xVideo", EntryPoint = "xVideo_GetAudioRenderers")]
		public static extern int BASS_DSHOW_GetAudioRenderers(ENUMDEVICESPROC proc, IntPtr user);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelOverlayBMP")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelOverlayBMP(int handle, [In] BASS_DSHOW_VIDEOBITMAP bitmap);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelAddFile")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_DSHOW_ChannelAddFileUnicode(int handle, [MarshalAs(UnmanagedType.LPWStr)] [In] string filename, BASSFlag flags);

		public static bool BASS_DSHOW_ChannelAddFile(int handle, string filename)
		{
			return BassDShow.BASS_DSHOW_ChannelAddFileUnicode(handle, filename, BASSFlag.BASS_UNICODE);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetConnectedFilters")]
		public static extern int BASS_DSHOW_ChannelGetConnectedFilters(int handle, CONNECTEDFILTERSPROC proc, IntPtr user);

		[DllImport("xVideo", EntryPoint = "xVideo_ShowFilterPropertyPage")]
		public static extern void BASS_DSHOW_ShowFilterPropertyPage(int handle, int filter, IntPtr parent);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetState")]
		public static extern BASSDSHOWState BASS_DSHOW_ChannelGetState(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelSetAttribute")]
		public static extern void BASS_DSHOW_ChannelSetAttribute(int handle, BASSDSHOWAttribute attrib, float newvalue);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetAttribute")]
		public static extern float BASS_DSHOW_ChannelGetAttribute(int handle, BASSDSHOWAttribute attrib);

		[DllImport("xVideo", EntryPoint = "xVideo_CaptureCreate")]
		public static extern int BASS_DSHOW_CaptureCreate(int audio, int video, int audioprofile, int videoprofile, BASSFlag flags);

		[DllImport("xVideo", EntryPoint = "xVideo_CaptureFree")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_CaptureFree(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelRepaint")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelRepaint(int handle, IntPtr win, IntPtr hdc);

		[DllImport("xVideo", EntryPoint = "xVideo_LoadPlugin")]
		private static extern int BASS_DSHOW_LoadPluginUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string file, BASSFlag flags);

		public static int BASS_DSHOW_LoadPlugin(string file)
		{
			return BassDShow.BASS_DSHOW_LoadPluginUnicode(file, BASSFlag.BASS_UNICODE);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_LoadPluginDS")]
		private static extern int BASS_DSHOW_LoadPluginDSUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string guid, [MarshalAs(UnmanagedType.LPWStr)] [In] string name, BASSFlag flags);

		public static int BASS_DSHOW_LoadPluginDS(string guid, string name)
		{
			return BassDShow.BASS_DSHOW_LoadPluginDSUnicode(guid, name, BASSFlag.BASS_UNICODE);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_PluginGetInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_DSHOW_PluginGetInfoInternal(int plugin, [In] [Out] ref BASS_DSHOW_PLUGININFO_INTERNAL info);

		public bool BASS_DSHOW_PluginGetInfo(int plugin, BASS_DSHOW_PLUGININFO info)
		{
			bool flag = BassDShow.BASS_DSHOW_PluginGetInfoInternal(plugin, ref info._internal);
			if (flag)
			{
				info.version = info._internal.version;
				info.decoderType = info._internal.decoderType;
				info.plgdescription = Utils.IntPtrAsStringAnsi(info._internal.plgdescription);
			}
			return flag;
		}

		public BASS_DSHOW_PLUGININFO BASS_DSHOW_PluginGetInfo(int plugin)
		{
			BASS_DSHOW_PLUGININFO bass_DSHOW_PLUGININFO = new BASS_DSHOW_PLUGININFO();
			if (this.BASS_DSHOW_PluginGetInfo(plugin, bass_DSHOW_PLUGININFO))
			{
				return bass_DSHOW_PLUGININFO;
			}
			return null;
		}

		[DllImport("xVideo", EntryPoint = "xVideo_RemovePlugin")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_RemovePlugin(int plugin);

		[DllImport("xVideo", EntryPoint = "xVideo_StreamCreateDVD")]
		private static extern int BASS_DSHOW_StreamCreateDVDUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string dvd, IntPtr win, BASSFlag flags);

		public static int BASS_DSHOW_StreamCreateDVD(string dvd, IntPtr win, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassDShow.BASS_DSHOW_StreamCreateDVDUnicode(dvd, win, flags);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_DVDGetProperty")]
		public static extern int BASS_DSHOW_DVDGetProperty(int handle, BASSDSHOWDVDGetProperty prop, int value);

		[DllImport("xVideo", EntryPoint = "xVideo_DVDSetProperty")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_DVDSetProperty(int handle, BASSDSHOWDVDSetProperty prop, int newvalue);

		[DllImport("xVideo", EntryPoint = "xVideo_DVDChannelMenu")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_DVDChannelMenu(int handle, BASSDSHOWDVDMenu option, int value1, int value2);

		[DllImport("xVideo", EntryPoint = "xVideo_Register")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_DSHOW_RegisterUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string email, [MarshalAs(UnmanagedType.LPWStr)] [In] string reg, BASSFlag flags);

		public static bool BASS_DSHOW_Register(string email, string reg)
		{
			return BassDShow.BASS_DSHOW_RegisterUnicode(email, reg, BASSFlag.BASS_UNICODE);
		}

		[DllImport("xVideo", EntryPoint = "xVideo_VCamStreamChannel")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_VCamStreamChannel(int handle, [MarshalAs(UnmanagedType.Bool)] [In] bool stream);

		[DllImport("xVideo", EntryPoint = "xVideo_VCamPush")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_VCamPush(int win, IntPtr data);

		[DllImport("xVideo", EntryPoint = "xVideo_VCamPush")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_VCamPush(int win, byte[] data);

		[DllImport("xVideo", EntryPoint = "xVideo_VCamPush")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_VCamPush(int win, long[] data);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelCallBackSync")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelSetSync(int handle, VIDEOSYNCPROC proc, IntPtr user);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelSetDVP")]
		public static extern int BASS_DSHOW_ChannelSetDVP(int handle, DVPPROC proc, IntPtr user);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelRemoveDVP")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelRemoveDVP(int handle, int dvp);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelStreamsCount")]
		public static extern int BASS_DSHOW_ChannelStreamsCount(int handle);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelGetStream")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_DSHOW_ChannelGetStream(int handle, int index, [In] [Out] ref BASS_DSHOW_STREAMS_INTERNAL info);

		public static BASS_DSHOW_STREAMS BASS_DSHOW_ChannelGetStream(int handle, int index)
		{
			BASS_DSHOW_STREAMS bass_DSHOW_STREAMS = new BASS_DSHOW_STREAMS();
			if (BassDShow.BASS_DSHOW_ChannelGetStream(handle, index, ref bass_DSHOW_STREAMS._internal))
			{
				bass_DSHOW_STREAMS.format = bass_DSHOW_STREAMS._internal.format;
				bass_DSHOW_STREAMS.name = Utils.IntPtrAsStringAnsi(bass_DSHOW_STREAMS._internal.name);
				bass_DSHOW_STREAMS.index = bass_DSHOW_STREAMS._internal.index;
				bass_DSHOW_STREAMS.enabled = bass_DSHOW_STREAMS._internal.enabled;
				return bass_DSHOW_STREAMS;
			}
			return null;
		}

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelEnableStream")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_DSHOW_ChannelEnableStream(int handle, int index);

		[DllImport("xVideo", EntryPoint = "xVideo_ChannelEnumerateStreams")]
		public static extern int BASS_DSHOW_ChannelEnumerateStreams(int handle, VIDEOSTREAMSPROC proc, IntPtr user);

		public static bool LoadMe()
		{
			return Utils.LoadLib("xVideo", ref BassDShow._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "xVideo"), ref BassDShow._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassDShow._myModuleHandle);
		}

		private static void CheckVersion()
		{
			try
			{
				if (Utils.HighWord(BassDShow.BASS_DSHOW_GetVersion()) != 258)
				{
					Version version = BassDShow.BASS_DSHOW_GetVersion(2);
					Version version2 = new Version(1, 2);
					FileVersionInfo fileVersionInfo = null;
					ProcessModuleCollection modules = Process.GetCurrentProcess().Modules;
					for (int i = modules.Count - 1; i >= 0; i--)
					{
						ProcessModule processModule = modules[i];
						if (processModule.ModuleName.ToLower().Equals("xVideo".ToLower()))
						{
							fileVersionInfo = processModule.FileVersionInfo;
							break;
						}
					}
					if (fileVersionInfo != null)
					{
						MessageBox.Show(string.Format("An incorrect version of BASS_DSHOW was loaded!\r\n\r\nVersion loaded: {0}.{1}\r\nVersion expected: {2}.{3}\r\n\r\nFile: {4}\r\nFileVersion: {5}\r\nDescription: {6}\r\nCompany: {7}\r\nLanguage: {8}", new object[]
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
						}), "Incorrect BASS_DSHOW Version", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else
					{
						MessageBox.Show(string.Format("An incorrect version of BASS_DSHOW was loaded!\r\n\r\nVersion loaded: {0}.{1}\r\nVersion expected: {2}.{3}", new object[]
						{
							version.Major,
							version.Minor,
							version2.Major,
							version2.Minor
						}), "Incorrect BASS_DSHOW Version", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			catch
			{
			}
		}

		public static string SupportedStreamExtensions = "*.avi;*.mpg;*.mpeg;*.mov;*.vob";

		public static string SupportedStreamName = "Video Files";

		public const int BASSDSHOWVERSION = 258;

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "xVideo";
	}
}
