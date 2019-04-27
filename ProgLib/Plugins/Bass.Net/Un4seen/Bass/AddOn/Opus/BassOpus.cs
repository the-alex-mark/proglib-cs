using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Opus
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassOpus
	{
		private BassOpus()
		{
		}

		[DllImport("bassopus", EntryPoint = "BASS_OPUS_StreamCreateFile")]
		private static extern int BASS_OPUS_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [MarshalAs(UnmanagedType.LPWStr)] [In] string file, long offset, long length, BASSFlag flags);

		public static int BASS_OPUS_StreamCreateFile(string file, long offset, long length, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassOpus.BASS_OPUS_StreamCreateFileUnicode(false, file, offset, length, flags);
		}

		[DllImport("bassopus", EntryPoint = "BASS_OPUS_StreamCreateFile")]
		private static extern int BASS_OPUS_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, long length, BASSFlag flags);

		public static int BASS_OPUS_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags)
		{
			return BassOpus.BASS_OPUS_StreamCreateFileMemory(true, memory, offset, length, flags);
		}

		[DllImport("bassopus")]
		public static extern int BASS_OPUS_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user);

		[DllImport("bassopus", EntryPoint = "BASS_OPUS_StreamCreateURL")]
		private static extern int BASS_OPUS_StreamCreateURLAscii([MarshalAs(UnmanagedType.LPStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

		[DllImport("bassopus", EntryPoint = "BASS_OPUS_StreamCreateURL")]
		private static extern int BASS_OPUS_StreamCreateURLUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

		public static int BASS_OPUS_StreamCreateURL(string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user)
		{
			flags |= BASSFlag.BASS_UNICODE;
			int num = BassOpus.BASS_OPUS_StreamCreateURLUnicode(url, offset, flags, proc, user);
			if (num == 0)
			{
				flags &= (BASSFlag.BASS_SAMPLE_8BITS | BASSFlag.BASS_SAMPLE_MONO | BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_SOFTWARE | BASSFlag.BASS_SAMPLE_MUTEMAX | BASSFlag.BASS_SAMPLE_VAM | BASSFlag.BASS_SAMPLE_FX | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE | BASSFlag.BASS_RECORD_ECHOCANCEL | BASSFlag.BASS_RECORD_AGC | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_RESTRATE | BASSFlag.BASS_STREAM_BLOCK | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_SPEAKER_FRONT | BASSFlag.BASS_SPEAKER_REAR | BASSFlag.BASS_SPEAKER_REAR2 | BASSFlag.BASS_SPEAKER_LEFT | BASSFlag.BASS_SPEAKER_RIGHT | BASSFlag.BASS_SPEAKER_PAIR8 | BASSFlag.BASS_ASYNCFILE | BASSFlag.BASS_SAMPLE_OVER_VOL | BASSFlag.BASS_WV_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 | BASSFlag.BASS_AC3_DOWNMIX_4 | BASSFlag.BASS_AC3_DYNAMIC_RANGE | BASSFlag.BASS_AAC_FRAME960);
				num = BassOpus.BASS_OPUS_StreamCreateURLAscii(url, offset, flags, proc, user);
			}
			return num;
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("bassopus", ref BassOpus._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bassopus"), ref BassOpus._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassOpus._myModuleHandle);
		}

		public static string SupportedStreamExtensions = "*.opus";

		public static string SupportedStreamName = "Opus Interactive Audio Codec";

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "bassopus";
	}
}
