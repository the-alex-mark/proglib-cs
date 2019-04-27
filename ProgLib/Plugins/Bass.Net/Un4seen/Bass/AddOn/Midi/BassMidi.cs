using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Un4seen.Bass.AddOn.Midi
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BassMidi
	{
		private BassMidi()
		{
		}

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_StreamCreate(int channels, BASSFlag flags, int freq);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamCreateFile")]
		private static extern int BASS_MIDI_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [MarshalAs(UnmanagedType.LPWStr)] [In] string file, long offset, long length, BASSFlag flags, int freq);

		public static int BASS_MIDI_StreamCreateFile(string file, long offset, long length, BASSFlag flags, int freq)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassMidi.BASS_MIDI_StreamCreateFileUnicode(false, file, offset, length, flags, freq);
		}

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamCreateFile")]
		private static extern int BASS_MIDI_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, IntPtr memory, long offset, long length, BASSFlag flags, int freq);

		public static int BASS_MIDI_StreamCreateFile(IntPtr memory, long offset, long length, BASSFlag flags, int freq)
		{
			return BassMidi.BASS_MIDI_StreamCreateFileMemory(true, memory, offset, length, flags, freq);
		}

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamCreateFile")]
		private static extern int BASS_MIDI_StreamCreateFileMemory([MarshalAs(UnmanagedType.Bool)] bool mem, byte[] memory, long offset, long length, BASSFlag flags, int freq);

		public static int BASS_MIDI_StreamCreateFile(byte[] memory, long offset, long length, BASSFlag flags, int freq)
		{
			return BassMidi.BASS_MIDI_StreamCreateFileMemory(true, memory, offset, length, flags, freq);
		}

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_StreamCreateFileUser(BASSStreamSystem system, BASSFlag flags, BASS_FILEPROCS procs, IntPtr user, int freq);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamCreateURL")]
		private static extern int BASS_MIDI_StreamCreateURLAscii([MarshalAs(UnmanagedType.LPStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user, int freq);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamCreateURL")]
		private static extern int BASS_MIDI_StreamCreateURLUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user, int freq);

		public static int BASS_MIDI_StreamCreateURL(string url, int offset, BASSFlag flags, DOWNLOADPROC proc, IntPtr user, int freq)
		{
			flags |= BASSFlag.BASS_UNICODE;
			int num = BassMidi.BASS_MIDI_StreamCreateURLUnicode(url, offset, flags, proc, user, freq);
			if (num == 0)
			{
				flags &= (BASSFlag.BASS_SAMPLE_8BITS | BASSFlag.BASS_SAMPLE_MONO | BASSFlag.BASS_SAMPLE_LOOP | BASSFlag.BASS_SAMPLE_3D | BASSFlag.BASS_SAMPLE_SOFTWARE | BASSFlag.BASS_SAMPLE_MUTEMAX | BASSFlag.BASS_SAMPLE_VAM | BASSFlag.BASS_SAMPLE_FX | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_RECORD_PAUSE | BASSFlag.BASS_RECORD_ECHOCANCEL | BASSFlag.BASS_RECORD_AGC | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_RESTRATE | BASSFlag.BASS_STREAM_BLOCK | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_SPEAKER_FRONT | BASSFlag.BASS_SPEAKER_REAR | BASSFlag.BASS_SPEAKER_REAR2 | BASSFlag.BASS_SPEAKER_LEFT | BASSFlag.BASS_SPEAKER_RIGHT | BASSFlag.BASS_SPEAKER_PAIR8 | BASSFlag.BASS_ASYNCFILE | BASSFlag.BASS_SAMPLE_OVER_VOL | BASSFlag.BASS_WV_STEREO | BASSFlag.BASS_AC3_DOWNMIX_2 | BASSFlag.BASS_AC3_DOWNMIX_4 | BASSFlag.BASS_AC3_DYNAMIC_RANGE | BASSFlag.BASS_AAC_FRAME960);
				num = BassMidi.BASS_MIDI_StreamCreateURLAscii(url, offset, flags, proc, user, freq);
			}
			return num;
		}

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_StreamCreateEvents([In] BASS_MIDI_EVENT[] events, int ppqn, BASSFlag flags, int freq);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamGetMark")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_MIDI_StreamGetMarkInternal(int handle, BASSMIDIMarker type, int index, [In] [Out] ref MIDI_MARK_INTERNAL mark);

		public static bool BASS_MIDI_StreamGetMark(int handle, BASSMIDIMarker type, int index, BASS_MIDI_MARK mark)
		{
			bool flag = BassMidi.BASS_MIDI_StreamGetMarkInternal(handle, type, index, ref mark._internal);
			if (flag)
			{
				mark.track = mark._internal.track;
				mark.pos = mark._internal.pos;
				mark.text = Utils.IntPtrAsStringAnsi(mark._internal.text);
			}
			return flag;
		}

		public static BASS_MIDI_MARK BASS_MIDI_StreamGetMark(int handle, BASSMIDIMarker type, int index)
		{
			BASS_MIDI_MARK bass_MIDI_MARK = new BASS_MIDI_MARK();
			if (BassMidi.BASS_MIDI_StreamGetMark(handle, type, index, bass_MIDI_MARK))
			{
				return bass_MIDI_MARK;
			}
			return null;
		}

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamGetMarks")]
		private static extern int BASS_MIDI_StreamGetMarksInternal(int handle, int track, BASSMIDIMarker type, [In] [Out] MIDI_MARK_INTERNAL[] marks);

		public static BASS_MIDI_MARK[] BASS_MIDI_StreamGetMarks(int handle, int track, BASSMIDIMarker type)
		{
			int num = BassMidi.BASS_MIDI_StreamGetMarksCount(handle, track, type);
			if (num <= 0)
			{
				return null;
			}
			MIDI_MARK_INTERNAL[] array = new MIDI_MARK_INTERNAL[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = default(MIDI_MARK_INTERNAL);
			}
			num = BassMidi.BASS_MIDI_StreamGetMarksInternal(handle, track, type, array);
			if (num > 0)
			{
				BASS_MIDI_MARK[] array2 = new BASS_MIDI_MARK[num];
				for (int j = 0; j < num; j++)
				{
					array2[j] = new BASS_MIDI_MARK();
					array2[j].track = array[j].track;
					array2[j].pos = array[j].pos;
					array2[j].text = Utils.IntPtrAsStringAnsi(array[j].text);
				}
				return array2;
			}
			return new BASS_MIDI_MARK[0];
		}

		public static int BASS_MIDI_StreamGetMarksCount(int handle, int track, BASSMIDIMarker type)
		{
			return BassMidi.BASS_MIDI_StreamGetMarksInternal(handle, track, type, null);
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_StreamEvent(int handle, int chan, BASSMIDIEvent eventtype, int param);

		public static bool BASS_MIDI_StreamEvent(int handle, int chan, BASSMIDIEvent eventtype, byte loParam, byte hiParam)
		{
			return BassMidi.BASS_MIDI_StreamEvent(handle, chan, eventtype, (int)Utils.MakeWord(loParam, hiParam));
		}

		[DllImport("bassmidi")]
		private static extern int BASS_MIDI_StreamEvents(int handle, BASSMIDIEventMode mode, BASS_MIDI_EVENT[] events, int length);

		public static int BASS_MIDI_StreamEvents(int handle, BASSMIDIEventMode flags, BASS_MIDI_EVENT[] events)
		{
			return BassMidi.BASS_MIDI_StreamEvents(handle, flags, events, events.Length);
		}

		[DllImport("bassmidi")]
		private static extern int BASS_MIDI_StreamEvents(int handle, BASSMIDIEventMode mode, byte[] events, int length);

		public static int BASS_MIDI_StreamEvents(int handle, BASSMIDIEventMode flags, int chan, byte[] events)
		{
			return BassMidi.BASS_MIDI_StreamEvents(handle, BASSMIDIEventMode.BASS_MIDI_EVENTS_RAW + chan | flags, events, events.Length);
		}

		[DllImport("bassmidi")]
		private static extern int BASS_MIDI_StreamEvents(int handle, BASSMIDIEventMode mode, IntPtr events, int length);

		public static int BASS_MIDI_StreamEvents(int handle, BASSMIDIEventMode flags, int chan, IntPtr events, int length)
		{
			return BassMidi.BASS_MIDI_StreamEvents(handle, BASSMIDIEventMode.BASS_MIDI_EVENTS_RAW + chan | flags, events, length);
		}

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_StreamGetEvent(int handle, int chan, BASSMIDIEvent eventtype);

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_StreamGetEvents(int handle, int track, BASSMIDIEvent filter, [In] [Out] BASS_MIDI_EVENT[] events);

		[DllImport("bassmidi", CharSet = CharSet.Unicode, EntryPoint = "BASS_MIDI_StreamGetEventsEx")]
		public static extern int BASS_MIDI_StreamGetEvents(int handle, int track, BASSMIDIEvent filter, [In] [Out] BASS_MIDI_EVENT[] events, int start, int count);

		public static BASS_MIDI_EVENT[] BASS_MIDI_StreamGetEvents(int handle, int track, BASSMIDIEvent filter)
		{
			int num = BassMidi.BASS_MIDI_StreamGetEventsCount(handle, track, filter);
			if (num >= 0)
			{
				BASS_MIDI_EVENT[] array = new BASS_MIDI_EVENT[num];
				BassMidi.BASS_MIDI_StreamGetEvents(handle, track, filter, array);
				return array;
			}
			return null;
		}

		public static int BASS_MIDI_StreamGetEventsCount(int handle, int track, BASSMIDIEvent filter)
		{
			return BassMidi.BASS_MIDI_StreamGetEvents(handle, track, filter, null);
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_StreamSetFonts(int handle, [In] BASS_MIDI_FONT[] fonts, int count);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamSetFonts")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_MIDI_StreamSetFontsEx(int handle, [In] BASS_MIDI_FONTEX[] fonts, int count);

		public static bool BASS_MIDI_StreamSetFonts(int handle, [In] BASS_MIDI_FONTEX[] fonts, int count)
		{
			return BassMidi.BASS_MIDI_StreamSetFontsEx(handle, fonts, count | 16777216);
		}

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_StreamGetFonts(int handle, [In] [Out] BASS_MIDI_FONT[] fonts, int count);

		public static BASS_MIDI_FONT[] BASS_MIDI_StreamGetFonts(int handle)
		{
			int num = BassMidi.BASS_MIDI_StreamGetFontsCount(handle);
			if (num >= 0)
			{
				BASS_MIDI_FONT[] array = new BASS_MIDI_FONT[num];
				BassMidi.BASS_MIDI_StreamGetFonts(handle, array, num);
				return array;
			}
			return null;
		}

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_StreamGetFonts")]
		private static extern int BASS_MIDI_StreamGetFontsEx(int handle, [In] [Out] BASS_MIDI_FONTEX[] fonts, int count);

		public static int BASS_MIDI_StreamGetFonts(int handle, [In] [Out] BASS_MIDI_FONTEX[] fonts, int count)
		{
			return BassMidi.BASS_MIDI_StreamGetFontsEx(handle, fonts, count | 16777216);
		}

		public static BASS_MIDI_FONTEX[] BASS_MIDI_StreamGetFontsEx(int handle)
		{
			int num = BassMidi.BASS_MIDI_StreamGetFontsCount(handle);
			if (num >= 0)
			{
				BASS_MIDI_FONTEX[] array = new BASS_MIDI_FONTEX[num];
				BassMidi.BASS_MIDI_StreamGetFonts(handle, array, num);
				return array;
			}
			return null;
		}

		public static int BASS_MIDI_StreamGetFontsCount(int handle)
		{
			return BassMidi.BASS_MIDI_StreamGetFontsEx(handle, null, 0);
		}

		public static int BASS_MIDI_StreamGetTrackCount(int handle)
		{
			int num = 0;
			while (Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_MIDI_TRACK + num) != IntPtr.Zero)
			{
				num++;
			}
			Bass.BASS_GetCPU();
			return num;
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_StreamLoadSamples(int handle);

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_StreamGetChannel(int handle, int channel);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_FontInit")]
		private static extern int BASS_MIDI_FontInitUnicode([MarshalAs(UnmanagedType.LPWStr)] [In] string file, BASSFlag flags);

		public static int BASS_MIDI_FontInit(string file, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassMidi.BASS_MIDI_FontInitUnicode(file, flags);
		}

		public static int BASS_MIDI_FontInit(string file)
		{
			return BassMidi.BASS_MIDI_FontInitUnicode(file, BASSFlag.BASS_UNICODE);
		}

		[DllImport("bassmidi")]
		public static extern int BASS_MIDI_FontInitUser(BASS_FILEPROCS procs, IntPtr user, BASSFlag flags);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_FontGetInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_MIDI_FontGetInfoInternal(int handle, [In] [Out] ref BASS_MIDI_FONTINFO_INTERNAL info);

		public static bool BASS_MIDI_FontGetInfo(int handle, BASS_MIDI_FONTINFO info)
		{
			bool flag = BassMidi.BASS_MIDI_FontGetInfoInternal(handle, ref info._internal);
			if (flag)
			{
				info.comment = Utils.IntPtrAsStringAnsi(info._internal.comment);
				info.copyright = Utils.IntPtrAsStringAnsi(info._internal.copyright);
				info.name = Utils.IntPtrAsStringAnsi(info._internal.name);
				info.presets = info._internal.presets;
				info.samload = info._internal.samload;
				info.samsize = info._internal.samsize;
				info.samtype = info._internal.samtype;
			}
			return flag;
		}

		public static BASS_MIDI_FONTINFO BASS_MIDI_FontGetInfo(int handle)
		{
			BASS_MIDI_FONTINFO bass_MIDI_FONTINFO = new BASS_MIDI_FONTINFO();
			if (BassMidi.BASS_MIDI_FontGetInfo(handle, bass_MIDI_FONTINFO))
			{
				return bass_MIDI_FONTINFO;
			}
			return null;
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_FontCompact(int handle);

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_FontFree(int handle);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_FontPack")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_MIDI_FontPackUnicode(int handle, string outfile, string encoder, BASSFlag flags);

		public static bool BASS_MIDI_FontPack(int handle, string outfile, string encoder, BASSFlag flags)
		{
			flags |= BASSFlag.BASS_UNICODE;
			return BassMidi.BASS_MIDI_FontPackUnicode(handle, outfile, encoder, flags);
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_MIDI_FontUnpack(int handle, string outfile, BASSFlag flags);

		public static bool BASS_MIDI_FontUnpack(int handle, string outfile)
		{
			return BassMidi.BASS_MIDI_FontUnpack(handle, outfile, BASSFlag.BASS_UNICODE);
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_FontLoad(int handle, int preset, int bank);

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_FontUnload(int handle, int preset, int bank);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_FontGetPreset")]
		private static extern IntPtr BASS_MIDI_FontGetPresetPtr(int handle, int preset, int bank);

		public static string BASS_MIDI_FontGetPreset(int handle, int preset, int bank)
		{
			IntPtr intPtr = BassMidi.BASS_MIDI_FontGetPresetPtr(handle, preset, bank);
			if (intPtr != IntPtr.Zero)
			{
				return Utils.IntPtrAsStringAnsi(intPtr);
			}
			return null;
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_MIDI_FontGetPresets(int handle, int[] presets);

		public static int[] BASS_MIDI_FontGetPresets(int handle)
		{
			BASS_MIDI_FONTINFO bass_MIDI_FONTINFO = BassMidi.BASS_MIDI_FontGetInfo(handle);
			if (bass_MIDI_FONTINFO == null)
			{
				return null;
			}
			int[] array = new int[bass_MIDI_FONTINFO.presets];
			if (BassMidi.BASS_MIDI_FontGetPresets(handle, array))
			{
				return array;
			}
			return null;
		}

		public static BASS_MIDI_FONT[] BASS_MIDI_FontGetPresetFonts(int handle)
		{
			BASS_MIDI_FONTINFO bass_MIDI_FONTINFO = BassMidi.BASS_MIDI_FontGetInfo(handle);
			if (bass_MIDI_FONTINFO == null)
			{
				return null;
			}
			int[] array = new int[bass_MIDI_FONTINFO.presets];
			if (BassMidi.BASS_MIDI_FontGetPresets(handle, array))
			{
				BASS_MIDI_FONT[] array2 = new BASS_MIDI_FONT[bass_MIDI_FONTINFO.presets];
				for (int i = 0; i < bass_MIDI_FONTINFO.presets; i++)
				{
					array2[i] = new BASS_MIDI_FONT(handle, Utils.LowWord32(array[i]), Utils.HighWord32(array[i]));
				}
				return array2;
			}
			return null;
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_FontSetVolume(int handle, float volume);

		[DllImport("bassmidi")]
		public static extern float BASS_MIDI_FontGetVolume(int handle);

		[DllImport("bassmidi", EntryPoint = "BASS_MIDI_InGetDeviceInfo")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool BASS_MIDI_InGetDeviceInfoInternal([In] int device, [In] [Out] ref BASS_MIDI_DEVICEINFO_INTERNAL info);

		public static bool BASS_MIDI_InGetDeviceInfo(int device, BASS_MIDI_DEVICEINFO info)
		{
			bool flag = BassMidi.BASS_MIDI_InGetDeviceInfoInternal(device, ref info._internal);
			if (flag)
			{
				if (Bass._configUTF8)
				{
					info.name = Utils.IntPtrAsStringUtf8(info._internal.name);
				}
				else
				{
					info.name = Utils.IntPtrAsStringAnsi(info._internal.name);
				}
				info.id = info._internal.id;
				info.flags = info._internal.flags;
			}
			return flag;
		}

		public static BASS_MIDI_DEVICEINFO BASS_MIDI_InGetDeviceInfo(int device)
		{
			BASS_MIDI_DEVICEINFO bass_MIDI_DEVICEINFO = new BASS_MIDI_DEVICEINFO();
			if (BassMidi.BASS_MIDI_InGetDeviceInfo(device, bass_MIDI_DEVICEINFO))
			{
				return bass_MIDI_DEVICEINFO;
			}
			return null;
		}

		public static BASS_MIDI_DEVICEINFO[] BASS_MIDI_InGetGeviceInfos()
		{
			List<BASS_MIDI_DEVICEINFO> list = new List<BASS_MIDI_DEVICEINFO>();
			int num = 0;
			BASS_MIDI_DEVICEINFO item;
			while ((item = BassMidi.BASS_MIDI_InGetDeviceInfo(num)) != null)
			{
				list.Add(item);
				num++;
			}
			Bass.BASS_GetCPU();
			return list.ToArray();
		}

		public static int BASS_MIDI_InGetDeviceInfos()
		{
			BASS_MIDI_DEVICEINFO info = new BASS_MIDI_DEVICEINFO();
			int num = 0;
			while (BassMidi.BASS_MIDI_InGetDeviceInfo(num, info))
			{
				num++;
			}
			Bass.BASS_GetCPU();
			return num;
		}

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_InInit(int device, MIDIINPROC proc, IntPtr user);

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_InFree(int device);

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_InStart(int device);

		[DllImport("bassmidi")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BASS_MIDI_InStop(int device);

		[DllImport("bassmidi", CharSet = CharSet.Unicode)]
		private static extern int BASS_MIDI_ConvertEvents([In] byte[] data, int length, [In] [Out] BASS_MIDI_EVENT[] events, int count, BASSMIDIEventMode flags);

		public static int BASS_MIDI_ConvertEvents(byte[] data, BASS_MIDI_EVENT[] events, int count, BASSMIDIEventMode flags)
		{
			return BassMidi.BASS_MIDI_ConvertEvents(data, data.Length, events, count, flags);
		}

		public static BASS_MIDI_EVENT[] BASS_MIDI_ConvertEvents(byte[] data, BASSMIDIEventMode flags)
		{
			int num = BassMidi.BASS_MIDI_ConvertEvents(data, null, 0, flags);
			if (num >= 0)
			{
				BASS_MIDI_EVENT[] array = new BASS_MIDI_EVENT[num];
				BassMidi.BASS_MIDI_ConvertEvents(data, array, num, flags);
				return array;
			}
			return null;
		}

		public static bool LoadMe()
		{
			return Utils.LoadLib("bassmidi", ref BassMidi._myModuleHandle);
		}

		public static bool LoadMe(string path)
		{
			return Utils.LoadLib(Path.Combine(path, "bassmidi"), ref BassMidi._myModuleHandle);
		}

		public static bool FreeMe()
		{
			return Utils.FreeLib(ref BassMidi._myModuleHandle);
		}

		public static string SupportedStreamExtensions = "*.midi;*.mid;*.rmi;*.kar";

		public static string SupportedStreamName = "MIDI Sound";

		private static int _myModuleHandle = 0;

		private const string _myModuleName = "bassmidi";
	}
}
