﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Flac
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassFlac
	{
		private BassFlac()
		{
		}

		[DllImport("bassflac", EntryPoint = "BASS_FLAC_StreamCreateFile")]
		private static extern int BASS_FLAC_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [MarshalAs(UnmanagedType.LPWStr)] [In] string file, long offset, long length, BASSFlag flags);

		public static int BASS_FLAC_StreamCreateFile(string file, long offset, long length, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassFlac.BASS_FLAC_StreamCreateFileUnicode(false, file, offset, length, flags);
		}

		[DllImport("bassflac", EntryPoint = "BASS_FLAC_StreamCreateFile")]
		private static extern int BASS_FLAC_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, long length, BASSFlag flags);

		public static int BASS_FLAC_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags)
		{
			return BassFlac.BASS_FLAC_StreamCreateFileMemory(true, memory, offset, length, flags);
		}

		[DllImport("bassflac")]
		public static extern int BASS_FLAC_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user);

		[DllImport("bassflac", EntryPoint = "BASS_FLAC_StreamCreateURL")]
		private static extern int BASS_FLAC_StreamCreateURLAscii([MarshalAs(UnmanagedType.LPStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

		[DllImport("bassflac", EntryPoint = "BASS_FLAC_StreamCreateURL")]
		private static extern int BASS_FLAC_StreamCreateURLUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user);

		public static int BASS_FLAC_StreamCreateURL(string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user)
		{
			flags |= BASSFlag.BASS_UNICODE;
			int num = BassFlac.BASS_FLAC_StreamCreateURLUnicode(url, offset, flags, proc, user);
			if (num == 0)
			{
				flags &= (BASSFlag.BASS_SAMPLE_8BITS | BASSFlag.BASS_SAMPLE_MONO | BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_SOFTWARE | BASSFlag.BASS_SAMPLE_MUTEMAX | BASSFlag.BASS_SAMPLE_VAM | BASSFlag.BASS_SAMPLE_FX | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE | BASSFlag.BASS_RECORD_ECHOCANCEL | BASSFlag.BASS_RECORD_AGC | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_RESTRATE | BASSFlag.BASS_STREAM_BLOCK | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_SPEAKER_FRONT | BASSFlag.BASS_SPEAKER_REAR | BASSFlag.BASS_SPEAKER_REAR2 | BASSFlag.BASS_SPEAKER_LEFT | BASSFlag.BASS_SPEAKER_RIGHT | BASSFlag.BASS_SPEAKER_PAIR8 | BASSFlag.BASS_ASYNCFILE | BASSFlag.BASS_SAMPLE_OVER_VOL | BASSFlag.BASS_WV_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 | BASSFlag.BASS_AC3_DOWNMIX_4 | BASSFlag.BASS_AC3_DYNAMIC_RANGE | BASSFlag.BASS_AAC_FRAME960);
				num = BassFlac.BASS_FLAC_StreamCreateURLAscii(url, offset, flags, proc, user);
			}
			return num;
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("bassflac", ref BassFlac._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bassflac"), ref BassFlac._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassFlac._myModuleHandle);
		}

		public static string SupportedStreamExtensions = "*.flac";

		public static string SupportedStreamName = "Free Lossless Audio";

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "bassflac";
	}
}
