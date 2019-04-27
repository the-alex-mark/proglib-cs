using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Dsd
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassDsd
	{
		private BassDsd()
		{
		}

		[DllImport("bassdsd", EntryPoint = "BASS_DSD_StreamCreateFile")]
		private static extern int BASS_DSD_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [MarshalAs(UnmanagedType.LPWStr)] [In] string file, long offset, long length, BASSFlag flags, int freq);

		public static int BASS_DSD_StreamCreateFile(string file, long offset, long length, BASSFlag flags, int freq)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassDsd.BASS_DSD_StreamCreateFileUnicode(false, file, offset, length, flags, freq);
		}

		[DllImport("bassdsd", EntryPoint = "BASS_DSD_StreamCreateFile")]
		private static extern int BASS_DSD_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, long length, BASSFlag flags, int freq);

		public static int BASS_DSD_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags, int freq)
		{
			return BassDsd.BASS_DSD_StreamCreateFileMemory(true, memory, offset, length, flags, freq);
		}

		[DllImport("bassdsd")]
		public static extern int BASS_DSD_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user, int freq);

		[DllImport("bassdsd", EntryPoint = "BASS_DSD_StreamCreateURL")]
		private static extern int BASS_DSD_StreamCreateURLAscii([MarshalAs(UnmanagedType.LPStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user, int freq);

		[DllImport("bassdsd", EntryPoint = "BASS_DSD_StreamCreateURL")]
		private static extern int BASS_DSD_StreamCreateURLUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user, int freq);

		public static int BASS_DSD_StreamCreateURL(string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user, int freq)
		{
			flags |= BASSFlag.BASS_UNICODE;
			int num = BassDsd.BASS_DSD_StreamCreateURLUnicode(url, offset, flags, proc, user, freq);
			if (num == 0)
			{
				flags &= (BASSFlag.BASS_SAMPLE_8BITS | BASSFlag.BASS_SAMPLE_MONO | BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_SOFTWARE | BASSFlag.BASS_SAMPLE_MUTEMAX | BASSFlag.BASS_SAMPLE_VAM | BASSFlag.BASS_SAMPLE_FX | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE | BASSFlag.BASS_RECORD_ECHOCANCEL | BASSFlag.BASS_RECORD_AGC | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_RESTRATE | BASSFlag.BASS_STREAM_BLOCK | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_SPEAKER_FRONT | BASSFlag.BASS_SPEAKER_REAR | BASSFlag.BASS_SPEAKER_REAR2 | BASSFlag.BASS_SPEAKER_LEFT | BASSFlag.BASS_SPEAKER_RIGHT | BASSFlag.BASS_SPEAKER_PAIR8 | BASSFlag.BASS_ASYNCFILE | BASSFlag.BASS_SAMPLE_OVER_VOL | BASSFlag.BASS_WV_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 | BASSFlag.BASS_AC3_DOWNMIX_4 | BASSFlag.BASS_AC3_DYNAMIC_RANGE | BASSFlag.BASS_AAC_FRAME960);
				num = BassDsd.BASS_DSD_StreamCreateURLAscii(url, offset, flags, proc, user, freq);
			}
			return num;
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("bassdsd", ref BassDsd._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bassdsd"), ref BassDsd._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassDsd._myModuleHandle);
		}

		public static string SupportedStreamExtensions = "*.dsf;*.dff;*.dsd";

		public static string SupportedStreamName = "Direct Stream Digital";

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "bassdsd";
	}
}
