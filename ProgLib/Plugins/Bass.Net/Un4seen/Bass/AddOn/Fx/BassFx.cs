using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Fx
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassFx
	{
		private BassFx()
		{
		}

		[DllImport("bass_fx")]
		public static extern int BASS_FX_GetVersion();

		public static Version BASS_FX_GetVersion(int fieldcount)
		{
			if (fieldcount < 1)
			{
				fieldcount = 1;
			}
			if (fieldcount > 4)
			{
				fieldcount = 4;
			}
			int num = BassFx.BASS_FX_GetVersion();
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

		[DllImport("bass_fx")]
		public static extern int BASS_FX_TempoCreate(int channel, BASSFlag flags);

		[DllImport("bass_fx")]
		public static extern int BASS_FX_TempoGetSource(int channel);

		[DllImport("bass_fx")]
		public static extern float BASS_FX_TempoGetRateRatio(int chan);

		[DllImport("bass_fx")]
		public static extern int BASS_FX_ReverseCreate(int channel, float dec_block, BASSFlag flags);

		[DllImport("bass_fx")]
		public static extern int BASS_FX_ReverseGetSource(int channel);

		[DllImport("bass_fx")]
		public static extern float BASS_FX_BPM_DecodeGet(int channel, double startSec, double endSec, int minMaxBPM, BASSFXBpm flags, BPMPROGRESSPROC proc, IntPtr user);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_BeatDecodeGet(int channel, double startSec, double endSec, BASSFXBpm flags, BPMBEATPROC proc, IntPtr user);

		[Obsolete("This method is obsolete; users can have their own simple conversion function.")]
		[DllImport("bass_fx")]
		public static extern float BASS_FX_BPM_Translate(int handle, float val2tran, BASSFXBpmTrans trans);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_Free(int handle);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_CallbackSet(int handle, BPMPROC proc, double period, int minMaxBPM, BASSFXBpm flags, IntPtr user);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_CallbackReset(int handle);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_BeatCallbackSet(int handle, BPMBEATPROC proc, IntPtr user);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_BeatCallbackReset(int handle);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_BeatSetParameters(int handle, float bandwidth, float centerfreq, float beat_rtime);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_BeatGetParameters(int handle, ref float bandwidth, ref float centerfreq, ref float beat_rtime);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_BeatGetParameters(int handle, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object bandwidth, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object centerfreq, [MarshalAs(UnmanagedType.AsAny)] [In] [Out] object beat_rtime);

		[DllImport("bass_fx")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_FX_BPM_BeatFree(int handle);

		public static bool LoadMe()
		{
			return Utils.LoadLib("bass_fx", ref BassFx._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bass_fx"), ref BassFx._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassFx._myModuleHandle);
		}

		public const int BASSFXVERSION = 516;

		private static int _myModuleHandle;

		private const string _myModuleName = "bass_fx";
	}
}
