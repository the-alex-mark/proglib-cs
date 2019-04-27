using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Un4seen.Bass.AddOn.Aac;
using Un4seen.Bass.AddOn.Ac3;
using Un4seen.Bass.AddOn.Adx;
using Un4seen.Bass.AddOn.Alac;
using Un4seen.Bass.AddOn.Ape;
using Un4seen.Bass.AddOn.Cd;
using Un4seen.Bass.AddOn.Cdg;
using Un4seen.Bass.AddOn.Dsd;
using Un4seen.Bass.AddOn.Flac;
using Un4seen.Bass.AddOn.Midi;
using Un4seen.Bass.AddOn.Mpc;
using Un4seen.Bass.AddOn.Ofr;
using Un4seen.Bass.AddOn.Opus;
using Un4seen.Bass.AddOn.Spx;
using Un4seen.Bass.AddOn.Tta;
using Un4seen.Bass.AddOn.Wma;
using Un4seen.Bass.AddOn.Wv;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class Utils
	{
		private Utils()
		{
		}

		public static byte MakeByte(byte lowBits, byte highBits)
		{
			return (byte)((int)highBits << 4 | (int)lowBits);
		}

		public static short MakeWord(byte lowByte, byte highByte)
		{
			return (short)((int)highByte << 8 | (int)lowByte);
		}

		public static int MakeLong(short lowWord, short highWord)
		{
			return (int)highWord << 16 | ((int)lowWord & 65535);
		}

		public static int MakeLong(int lowWord, int highWord)
		{
			return highWord << 16 | (lowWord & 65535);
		}

		public static long MakeLong64(int lowWord, int highWord)
		{
			return (long)highWord << 32 | ((long)lowWord & (long)(-1));
		}

		public static long MakeLong64(long lowWord, long highWord)
		{
			return highWord << 32 | (lowWord & (long)-1);
		}

		public static short HighWord(int dWord)
		{
			return (short)(dWord >> 16 & 65535);
		}

		public static int HighWord32(int dWord)
		{
			return dWord >> 16 & 65535;
		}

		public static int HighWord(long qWord)
		{
			return (int)(qWord >> 32 & (long)-1);
		}

		public static short LowWord(int dWord)
		{
			return (short)(dWord & 65535);
		}

		public static int LowWord32(int dWord)
		{
			return dWord & 65535;
		}

		public static int LowWord(long qWord)
		{
			return (int)(qWord & (long)(-1));
		}

		public static double LevelToDB(int level, int maxLevel)
		{
			return 20.0 * Math.Log10((double)level / (double)maxLevel);
		}

		public static double LevelToDB(double level, double maxLevel)
		{
			return 20.0 * Math.Log10(level / maxLevel);
		}

		public static int DBToLevel(double dB, int maxLevel)
		{
			return (int)Math.Round((double)maxLevel * Math.Pow(10.0, dB / 20.0));
		}

		public static double DBToLevel(double dB, double maxLevel)
		{
			return maxLevel * Math.Pow(10.0, dB / 20.0);
		}

		public static string FixTimespan(double seconds)
		{
			return TimeSpan.FromSeconds(seconds).ToString();
		}

		public static string FixTimespan(double seconds, string format)
		{
			string result = string.Empty;
			try
			{
				DateTime dateTime = DateTime.Today.AddSeconds(seconds);
				uint num = PrivateImplementationDetails.ComputeStringHash(format);
				if (num <= 1029280109u)
				{
					if (num <= 501059810u)
					{
						if (num != 400247001u)
						{
							if (num != 468343215u)
							{
								if (num == 501059810u)
								{
									if (format == "SMPTE60")
									{
										result = string.Format("{0}:{1}:{2}:{3:F0}", new object[]
										{
											dateTime.Hour,
											dateTime.Minute,
											dateTime.Second,
											Math.Round((double)dateTime.Millisecond / 16.666666666666668)
										});
										goto IL_4E2;
									}
								}
							}
							else if (format == "SMPTE30")
							{
								result = string.Format("{0}:{1}:{2}:{3:F0}", new object[]
								{
									dateTime.Hour,
									dateTime.Minute,
									dateTime.Second,
									Math.Round((double)dateTime.Millisecond / 33.333333333333336)
								});
								goto IL_4E2;
							}
						}
						else if (format == "SMPTE50")
						{
							result = string.Format("{0}:{1}:{2}:{3:F0}", new object[]
							{
								dateTime.Hour,
								dateTime.Minute,
								dateTime.Second,
								Math.Round((double)dateTime.Millisecond / 20.0)
							});
							goto IL_4E2;
						}
					}
					else if (num <= 785917513u)
					{
						if (num != 524812473u)
						{
							if (num == 785917513u)
							{
								if (format == "HHMMSSFFF")
								{
									result = dateTime.ToString("HH:mm:ss.fff");
									goto IL_4E2;
								}
							}
						}
						else if (format == "MMSSF")
						{
							result = dateTime.ToString("mm:ss.f");
							goto IL_4E2;
						}
					}
					else if (num != 948343413u)
					{
						if (num == 1029280109u)
						{
							if (format == "MMSSFF")
							{
								result = dateTime.ToString("mm:ss.ff");
								goto IL_4E2;
							}
						}
					}
					else if (format == "HHMMSSFF")
					{
						result = dateTime.ToString("HH:mm:ss.ff");
						goto IL_4E2;
					}
				}
				else if (num <= 2598850565u)
				{
					if (num <= 1890435217u)
					{
						if (num != 1297605492u)
						{
							if (num == 1890435217u)
							{
								if (format == "HHMMSSF")
								{
									result = dateTime.ToString("HH:mm:ss.f");
									goto IL_4E2;
								}
							}
						}
						else if (format == "SMPTE")
						{
							result = dateTime.ToString("HH.mm.ss.f");
							goto IL_4E2;
						}
					}
					else if (num != 2582072946u)
					{
						if (num == 2598850565u)
						{
							if (format == "SMPTE25")
							{
								result = string.Format("{0}:{1}:{2}:{3:F0}", new object[]
								{
									dateTime.Hour,
									dateTime.Minute,
									dateTime.Second,
									Math.Round((double)dateTime.Millisecond / 40.0)
								});
								goto IL_4E2;
							}
						}
					}
					else if (format == "SMPTE24")
					{
						result = string.Format("{0}:{1}:{2}:{3:F0}", new object[]
						{
							dateTime.Hour,
							dateTime.Minute,
							dateTime.Second,
							Math.Round((double)dateTime.Millisecond / 41.666666666666664)
						});
						goto IL_4E2;
					}
				}
				else if (num <= 3204417201u)
				{
					if (num != 3182828429u)
					{
						if (num == 3204417201u)
						{
							if (format == "MMSSFFF")
							{
								result = dateTime.ToString("mm:ss.fff");
								goto IL_4E2;
							}
						}
					}
					else if (format == "HHMMSS")
					{
						result = dateTime.ToString("HH:mm:ss");
						goto IL_4E2;
					}
				}
				else if (num != 3464860741u)
				{
					if (num == 3740257559u)
					{
						if (format == "HHMM")
						{
							result = dateTime.ToString("HH:mm");
							goto IL_4E2;
						}
					}
				}
				else if (format == "MMSS")
				{
					result = dateTime.ToString("mm:ss");
					goto IL_4E2;
				}
				result = dateTime.ToString(format);
				IL_4E2:;
			}
			catch
			{
			}
			return result;
		}

		public static int FFTFrequency2Index(int frequency, int length, int samplerate)
		{
			int num = (int)Math.Round((double)length * (double)frequency / (double)samplerate);
			if (num > length / 2 - 1)
			{
				num = length / 2 - 1;
			}
			return num;
		}

		public static int FFTIndex2Frequency(int index, int length, int samplerate)
		{
			return (int)Math.Round((double)index * (double)samplerate / (double)length);
		}

		public static byte[] SampleTo8Bit(byte sample)
		{
			return new byte[]
			{
				sample
			};
		}

		public static byte[] SampleTo8Bit(short sample)
		{
			byte[] array = new byte[1];
			int num = (int)(sample / 256 + 128);
			if (num > 255)
			{
				num = 255;
			}
			else if (num < 0)
			{
				num = 0;
			}
			array[0] = (byte)num;
			return array;
		}

		public static byte[] SampleTo8Bit(float sample)
		{
			byte[] array = new byte[1];
			int num = (int)(sample * 128f) + 128;
			if (num > 255)
			{
				num = 255;
			}
			else if (num < 0)
			{
				num = 0;
			}
			array[0] = (byte)num;
			return array;
		}

		public static byte[] SampleTo16Bit(byte sample)
		{
			byte[] array = new byte[2];
			int num = (int)(sample - 128) * 256;
			if (num > 32767)
			{
				num = 32767;
			}
			else if (num < -32768)
			{
				num = -32768;
			}
			for (int i = 0; i < 2; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo16Bit(short sample)
		{
			byte[] array = new byte[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = (byte)(sample >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo16Bit(float sample)
		{
			byte[] array = new byte[2];
			int num = (int)(sample * 32768f);
			if (num > 32767)
			{
				num = 32767;
			}
			else if (num < -32768)
			{
				num = -32768;
			}
			for (int i = 0; i < 2; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo24Bit(byte sample)
		{
			byte[] array = new byte[3];
			int num = (int)(sample - 128) * 65536;
			if (num > 8388607)
			{
				num = 8388607;
			}
			else if (num < -8388608)
			{
				num = -8388608;
			}
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo24Bit(short sample)
		{
			byte[] array = new byte[3];
			int num = (int)(sample * 256);
			if (num > 8388607)
			{
				num = 8388607;
			}
			else if (num < -8388608)
			{
				num = -8388608;
			}
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static byte[] SampleTo24Bit(float sample)
		{
			byte[] array = new byte[3];
			int num = (int)(sample * 8388608f);
			if (num > 8388607)
			{
				num = 8388607;
			}
			else if (num < -8388608)
			{
				num = -8388608;
			}
			for (int i = 0; i < 3; i++)
			{
				array[i] = (byte)(num >> i * 8);
			}
			return array;
		}

		public static float SampleTo32Bit(byte sample)
		{
			return ((float)sample - 128f) / 128f;
		}

		public static float SampleTo32Bit(short sample)
		{
			return (float)sample / 32768f;
		}

		public static float SampleTo32Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num == 1)
			{
				return Utils.SampleTo32Bit(sample[0]);
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				num2 |= (int)sample[i] << i * 8;
			}
			if (sample[num - 1] > 127)
			{
				num2 -= (int)(Math.Pow(256.0, (double)num) / 2.0);
				return -1f + (float)((double)num2 / (Math.Pow(256.0, (double)num) / 2.0));
			}
			return (float)((double)num2 / (Math.Pow(256.0, (double)num) / 2.0));
		}

		public static int SampleTo24Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num < 3)
			{
				return 0;
			}
			int i = 0;
			int num2 = 0;
			for (int j = num - 3; j < num; j++)
			{
				i |= (int)sample[j] << num2 * 8;
				num2++;
			}
			while (i > 8388607)
			{
				i -= 8388608;
			}
			return i;
		}

		public static short SampleTo16Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num < 2)
			{
				return 0;
			}
			int num2 = 0;
			int num3 = 0;
			for (int i = num - 2; i < num; i++)
			{
				num2 |= (int)sample[i] << num3 * 8;
				num3++;
			}
			return (short)num2;
		}

		public static byte SampleTo8Bit(byte[] sample)
		{
			int num = sample.Length;
			if (num < 1)
			{
				return 0;
			}
			return sample[num - 1];
		}

		public static float Semitone2Samplerate(float origfreq, int semitones)
		{
			return origfreq * (float)Math.Pow(2.0, (double)((float)semitones / 12f));
		}

		public static float BPM2Seconds(float bpm)
		{
			if (bpm != 0f)
			{
				return 60f / bpm;
			}
			return -1f;
		}

		public static float Seconds2BPM(double seconds)
		{
			if (seconds != 0.0)
			{
				return (float)(60.0 / seconds);
			}
			return -1f;
		}

		public static string ByteToHex(byte[] buffer)
		{
			if (buffer == null || buffer.Length == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(buffer.Length * 3);
			foreach (byte value in buffer)
			{
				stringBuilder.Append(Convert.ToString(value, 16).PadLeft(2, '0').PadRight(3, ' '));
			}
			return stringBuilder.ToString().ToUpper();
		}

		public static byte[] HexToByte(string hexString, int length)
		{
			if (string.IsNullOrEmpty(hexString))
			{
				return null;
			}
			hexString = hexString.Replace(" ", "");
			byte[] array = (length < 0) ? new byte[hexString.Length / 2] : new byte[length];
			for (int i = 0; i < hexString.Length; i += 2)
			{
				array[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
				if (i / 2 == length)
				{
					break;
				}
			}
			return array;
		}

		public static bool Is64Bit
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		private static int IntPtrNullTermLength(IntPtr p)
		{
			int num = 0;
			while (Marshal.ReadByte(p, num) != 0)
			{
				num++;
			}
			return num;
		}

		public static string IntPtrAsStringAnsi(IntPtr ansiPtr)
		{
			int num;
			return Utils.IntPtrAsStringUtf8orLatin1(ansiPtr, out num);
		}

		public static string IntPtrAsStringAnsi(IntPtr ansiPtr, int len)
		{
			if (ansiPtr != IntPtr.Zero && len > 0)
			{
				byte[] array = new byte[len];
				Marshal.Copy(ansiPtr, array, 0, len);
				return Encoding.Default.GetString(array, 0, len);
			}
			return null;
		}

		public static string IntPtrAsStringUnicode(IntPtr unicodePtr)
		{
			if (unicodePtr != IntPtr.Zero)
			{
				return Marshal.PtrToStringUni(unicodePtr);
			}
			return null;
		}

		public static string IntPtrAsStringUnicode(IntPtr unicodePtr, int len)
		{
			if (unicodePtr != IntPtr.Zero)
			{
				return Marshal.PtrToStringUni(unicodePtr, len);
			}
			return null;
		}

		public static string IntPtrAsStringLatin1(IntPtr latin1Ptr, out int len)
		{
			len = 0;
			if (latin1Ptr != IntPtr.Zero)
			{
				len = Utils.IntPtrNullTermLength(latin1Ptr);
				if (len != 0)
				{
					byte[] array = new byte[len];
					Marshal.Copy(latin1Ptr, array, 0, len);
					if (!BassNet.UseBrokenLatin1Behavior)
					{
						return Encoding.GetEncoding("latin1").GetString(array, 0, len);
					}
					return Encoding.Default.GetString(array, 0, len);
				}
			}
			return null;
		}

		public static string IntPtrAsStringUtf8orLatin1(IntPtr utf8Ptr, out int len)
		{
			len = 0;
			if (utf8Ptr != IntPtr.Zero)
			{
				len = Utils.IntPtrNullTermLength(utf8Ptr);
				if (len != 0)
				{
					byte[] array = new byte[len];
					Marshal.Copy(utf8Ptr, array, 0, len);
					string text = BassNet.UseBrokenLatin1Behavior ? Encoding.Default.GetString(array, 0, len) : Encoding.GetEncoding("latin1").GetString(array, 0, len);
					try
					{
						string @string = new UTF8Encoding(false, true).GetString(array, 0, len);
						if (@string.Length < text.Length)
						{
							return @string;
						}
					}
					catch
					{
					}
					return text;
				}
			}
			return null;
		}

		public static string IntPtrAsStringUtf8(IntPtr utf8Ptr, out int len)
		{
			len = 0;
			if (utf8Ptr != IntPtr.Zero)
			{
				len = Utils.IntPtrNullTermLength(utf8Ptr);
				if (len != 0)
				{
					byte[] array = new byte[len];
					Marshal.Copy(utf8Ptr, array, 0, len);
					return Encoding.UTF8.GetString(array, 0, len);
				}
			}
			return null;
		}

		public static string IntPtrAsStringUtf8(IntPtr utf8Ptr)
		{
			if (utf8Ptr != IntPtr.Zero)
			{
				int num = Utils.IntPtrNullTermLength(utf8Ptr);
				if (num != 0)
				{
					byte[] array = new byte[num];
					Marshal.Copy(utf8Ptr, array, 0, num);
					return Encoding.UTF8.GetString(array, 0, num);
				}
			}
			return null;
		}

		public static object IntAsObject(IntPtr ptr, Type structureType)
		{
			return Marshal.PtrToStructure(ptr, structureType);
		}

		public static Version GetVersion()
		{
			return Assembly.GetExecutingAssembly().GetName().Version;
		}

		public unsafe static string[] IntPtrToArrayNullTermAnsi(IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
			{
				List<string> list = new List<string>();
				for (;;)
				{
					string text = Utils.IntPtrAsStringAnsi(pointer);
					if (string.IsNullOrEmpty(text))
					{
						break;
					}
					list.Add(text);
					pointer = new IntPtr((void*)((byte*)((byte*)pointer.ToPointer() + text.Length) + 1));
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		public unsafe static string[] IntPtrToArrayNullTermUtf8(IntPtr pointer)
		{
			if (pointer != IntPtr.Zero)
			{
				List<string> list = new List<string>();
				string item = string.Empty;
				for (;;)
				{
					int num = Utils.IntPtrNullTermLength(pointer);
					if (num <= 0)
					{
						break;
					}
					byte[] array = new byte[num];
					Marshal.Copy(pointer, array, 0, num);
					pointer = new IntPtr((void*)((byte*)((byte*)pointer.ToPointer() + num) + 1));
					item = Encoding.UTF8.GetString(array, 0, num);
					list.Add(item);
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		public unsafe static string[] IntPtrToArrayNullTermUnicode(IntPtr pointer)
		{
			if (!(pointer != IntPtr.Zero))
			{
				return null;
			}
			List<string> list = new List<string>();
			for (;;)
			{
				string text = Marshal.PtrToStringUni(pointer);
				if (text.Length == 0)
				{
					break;
				}
				list.Add(text);
				pointer = new IntPtr((void*)((byte*)((byte*)pointer.ToPointer() + 2 * text.Length) + 2));
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}

		public unsafe static int[] IntPtrToArrayNullTermInt32(IntPtr pointer)
		{
			if (!(pointer != IntPtr.Zero))
			{
				return null;
			}
			int num = 0;
			int* ptr = (int*)((void*)pointer);
			while (ptr[num] != 0)
			{
				num++;
			}
			if (num > 0)
			{
				int[] array = new int[num];
				Marshal.Copy(pointer, array, 0, num);
				return array;
			}
			return null;
		}

		public unsafe static short[] IntPtrToArrayNullTermInt16(IntPtr pointer)
		{
			if (!(pointer != IntPtr.Zero))
			{
				return null;
			}
			int num = 0;
			short* ptr = (short*)((void*)pointer);
			while (ptr[num] != 0)
			{
				num++;
			}
			if (num > 0)
			{
				short[] array = new short[num];
				Marshal.Copy(pointer, array, 0, num);
				return array;
			}
			return null;
		}

		public static void StringToNullTermAnsi(string text, IntPtr target)
		{
			if (target != IntPtr.Zero && text != null)
			{
				text += "\0";
				byte[] bytes = Encoding.Default.GetBytes(text);
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermUnicode(string text, IntPtr target)
		{
			if (target != IntPtr.Zero && text != null)
			{
				text += "\0";
				byte[] bytes = Encoding.Unicode.GetBytes(text);
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermUtf8(string text, IntPtr target)
		{
			if (target != IntPtr.Zero && text != null)
			{
				text += "\0";
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermAnsi(string[] text, IntPtr target, bool addCRLF)
		{
			if (target != IntPtr.Zero && text != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text2 in text)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append(text2);
						if (addCRLF && !text2.EndsWith("\r\n"))
						{
							stringBuilder.Append("\r\n");
						}
						stringBuilder.Append('\0');
					}
				}
				stringBuilder.Append('\0');
				byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermUnicode(string[] text, IntPtr target, bool addCRLF)
		{
			if (target != IntPtr.Zero && text != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text2 in text)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append(text2);
						if (addCRLF && !text2.EndsWith("\r\n"))
						{
							stringBuilder.Append("\r\n");
						}
						stringBuilder.Append('\0');
					}
				}
				stringBuilder.Append('\0');
				byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static void StringToNullTermUtf8(string[] text, IntPtr target, bool addCRLF)
		{
			if (target != IntPtr.Zero && text != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text2 in text)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append(text2);
						if (addCRLF && !text2.EndsWith("\r\n"))
						{
							stringBuilder.Append("\r\n");
						}
						stringBuilder.Append('\0');
					}
				}
				stringBuilder.Append('\0');
				byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
				Marshal.Copy(bytes, 0, target, bytes.Length);
			}
		}

		public static string ChannelNumberToString(int chans)
		{
			string result = chans.ToString();
			switch (chans)
			{
			case 1:
				result = "Mono";
				break;
			case 2:
				result = "Stereo";
				break;
			case 3:
				result = "2.1";
				break;
			case 4:
				result = "2.2";
				break;
			case 5:
				result = "4.1";
				break;
			case 6:
				result = "5.1";
				break;
			case 7:
				result = "5.2";
				break;
			case 8:
				result = "7.1";
				break;
			}
			return result;
		}

		public static string BASSChannelTypeToString(BASSChannelType ctype)
		{
			string result = "???";
			if ((ctype & BASSChannelType.BASS_CTYPE_STREAM_WAV) > BASSChannelType.BASS_CTYPE_UNKNOWN)
			{
				ctype = BASSChannelType.BASS_CTYPE_STREAM_WAV;
			}
			if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_AAC)
			{
				if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_WINAMP)
				{
					if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_MF)
					{
						switch (ctype)
						{
						case BASSChannelType.BASS_CTYPE_UNKNOWN:
							result = "Unknown";
							break;
						case BASSChannelType.BASS_CTYPE_SAMPLE:
							result = "Sample";
							break;
						case BASSChannelType.BASS_CTYPE_RECORD:
							result = "Recording";
							break;
						default:
							if (ctype != BASSChannelType.BASS_CTYPE_MUSIC_MO3)
							{
								switch (ctype)
								{
								case BASSChannelType.BASS_CTYPE_STREAM:
									result = "Custom Stream";
									break;
								case BASSChannelType.BASS_CTYPE_STREAM_OGG:
									result = "OGG";
									break;
								case BASSChannelType.BASS_CTYPE_STREAM_MP1:
									result = "MP1";
									break;
								case BASSChannelType.BASS_CTYPE_STREAM_MP2:
									result = "MP2";
									break;
								case BASSChannelType.BASS_CTYPE_STREAM_MP3:
									result = "MP3";
									break;
								case BASSChannelType.BASS_CTYPE_STREAM_AIFF:
									result = "AIFF";
									break;
								case BASSChannelType.BASS_CTYPE_STREAM_CA:
									result = "CoreAudio";
									break;
								case BASSChannelType.BASS_CTYPE_STREAM_MF:
									result = "MF";
									break;
								}
							}
							else
							{
								result = "MO3";
							}
							break;
						}
					}
					else if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_WMA)
					{
						if (ctype != BASSChannelType.BASS_CTYPE_STREAM_CD)
						{
							if (ctype == BASSChannelType.BASS_CTYPE_STREAM_WMA)
							{
								result = "WMA";
							}
						}
						else
						{
							result = "CDA";
						}
					}
					else if (ctype != BASSChannelType.BASS_CTYPE_STREAM_WMA_MP3)
					{
						if (ctype == BASSChannelType.BASS_CTYPE_STREAM_WINAMP)
						{
							result = "Winamp";
						}
					}
					else
					{
						result = "MP3";
					}
				}
				else if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_MIXER)
				{
					if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_OFR)
					{
						switch (ctype)
						{
						case BASSChannelType.BASS_CTYPE_STREAM_WV:
							result = "Wavpack";
							break;
						case BASSChannelType.BASS_CTYPE_STREAM_WV_H:
							result = "Wavpack";
							break;
						case BASSChannelType.BASS_CTYPE_STREAM_WV_L:
							result = "Wavpack";
							break;
						case BASSChannelType.BASS_CTYPE_STREAM_WV_LH:
							result = "Wavpack";
							break;
						default:
							if (ctype == BASSChannelType.BASS_CTYPE_STREAM_OFR)
							{
								result = "Optimfrog";
							}
							break;
						}
					}
					else if (ctype != BASSChannelType.BASS_CTYPE_STREAM_APE)
					{
						if (ctype == BASSChannelType.BASS_CTYPE_STREAM_MIXER)
						{
							result = "Mixer";
						}
					}
					else
					{
						result = "APE";
					}
				}
				else if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_FLAC)
				{
					if (ctype != BASSChannelType.BASS_CTYPE_STREAM_SPLIT)
					{
						if (ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC)
						{
							result = "FLAC";
						}
					}
					else
					{
						result = "Splitter";
					}
				}
				else if (ctype != BASSChannelType.BASS_CTYPE_STREAM_MPC)
				{
					if (ctype == BASSChannelType.BASS_CTYPE_STREAM_AAC)
					{
						result = "AAC";
					}
				}
				else
				{
					result = "MPC";
				}
			}
			else if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_VIDEO)
			{
				if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_MIDI)
				{
					if (ctype != BASSChannelType.BASS_CTYPE_STREAM_MP4)
					{
						if (ctype != BASSChannelType.BASS_CTYPE_STREAM_SPX)
						{
							if (ctype == BASSChannelType.BASS_CTYPE_STREAM_MIDI)
							{
								result = "MIDI";
							}
						}
						else
						{
							result = "Speex";
						}
					}
					else
					{
						result = "MP4";
					}
				}
				else if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_TTA)
				{
					if (ctype != BASSChannelType.BASS_CTYPE_STREAM_ALAC)
					{
						if (ctype == BASSChannelType.BASS_CTYPE_STREAM_TTA)
						{
							result = "TTA";
						}
					}
					else
					{
						result = "ALAC";
					}
				}
				else if (ctype != BASSChannelType.BASS_CTYPE_STREAM_AC3)
				{
					if (ctype == BASSChannelType.BASS_CTYPE_STREAM_VIDEO)
					{
						result = "Video";
					}
				}
				else
				{
					result = "AC3";
				}
			}
			else if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_AIX)
			{
				if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_DSD)
				{
					if (ctype != BASSChannelType.BASS_CTYPE_STREAM_OPUS)
					{
						if (ctype == BASSChannelType.BASS_CTYPE_STREAM_DSD)
						{
							result = "DSD";
						}
					}
					else
					{
						result = "OPUS";
					}
				}
				else if (ctype != BASSChannelType.BASS_CTYPE_STREAM_ADX)
				{
					if (ctype == BASSChannelType.BASS_CTYPE_STREAM_AIX)
					{
						result = "AIX";
					}
				}
				else
				{
					result = "ADX";
				}
			}
			else
			{
				if (ctype <= BASSChannelType.BASS_CTYPE_STREAM_WAV)
				{
					switch (ctype)
					{
					case BASSChannelType.BASS_CTYPE_MUSIC_MOD:
						return "MOD";
					case BASSChannelType.BASS_CTYPE_MUSIC_MTM:
						return "MTM";
					case BASSChannelType.BASS_CTYPE_MUSIC_S3M:
						return "S3M";
					case BASSChannelType.BASS_CTYPE_MUSIC_XM:
						return "XM";
					case BASSChannelType.BASS_CTYPE_MUSIC_IT:
						return "IT";
					default:
						if (ctype != BASSChannelType.BASS_CTYPE_STREAM_WAV)
						{
							return result;
						}
						break;
					}
				}
				else if (ctype != BASSChannelType.BASS_CTYPE_STREAM_WAV_PCM && ctype != BASSChannelType.BASS_CTYPE_STREAM_WAV_FLOAT)
				{
					return result;
				}
				result = "WAV";
			}
			return result;
		}

		public static string BASSTagTypeToString(BASSTag tagType)
		{
			if (tagType <= BASSTag.BASS_TAG_MUSIC_INST)
			{
				if (tagType <= BASSTag.BASS_TAG_APE_BINARY)
				{
					switch (tagType)
					{
					case BASSTag.BASS_TAG_ID3:
						return "ID3v1";
					case BASSTag.BASS_TAG_ID3V2:
					case BASSTag.BASS_TAG_LYRICS3:
						return "ID3v2";
					case BASSTag.BASS_TAG_OGG:
					case BASSTag.BASS_TAG_VENDOR:
						return "OGG";
					case BASSTag.BASS_TAG_HTTP:
					case BASSTag.BASS_TAG_ICY:
					case BASSTag.BASS_TAG_META:
					case BASSTag.BASS_TAG_WMA_META:
						return "META";
					case BASSTag.BASS_TAG_APE:
						break;
					case BASSTag.BASS_TAG_MP4:
						return "MP4";
					case BASSTag.BASS_TAG_WMA:
						return "WMA";
					case BASSTag.BASS_TAG_FLAC_CUE:
						goto IL_1C5;
					case BASSTag.BASS_TAG_MF:
						return "MF";
					case BASSTag.BASS_TAG_WAVEFORMAT:
						return "WAV";
					default:
						switch (tagType)
						{
						case BASSTag.BASS_TAG_RIFF_INFO:
							return "RIFF";
						case BASSTag.BASS_TAG_RIFF_BEXT:
							return "BWF";
						case BASSTag.BASS_TAG_RIFF_CART:
							return "RIFF CART";
						case BASSTag.BASS_TAG_RIFF_DISP:
							return "RIFF DISP";
						default:
							if (tagType != BASSTag.BASS_TAG_APE_BINARY)
							{
								goto IL_1C5;
							}
							break;
						}
						break;
					}
					return "APE";
				}
				if (tagType != BASSTag.BASS_TAG_MUSIC_NAME && tagType != BASSTag.BASS_TAG_MUSIC_MESSAGE && tagType != BASSTag.BASS_TAG_MUSIC_INST)
				{
					goto IL_1C5;
				}
			}
			else if (tagType <= BASSTag.BASS_TAG_ADX_LOOP)
			{
				if (tagType != BASSTag.BASS_TAG_MUSIC_SAMPLE)
				{
					if (tagType == BASSTag.BASS_TAG_MIDI_TRACK)
					{
						return "MIDI";
					}
					if (tagType != BASSTag.BASS_TAG_ADX_LOOP)
					{
						goto IL_1C5;
					}
					return "ADX";
				}
			}
			else if (tagType <= BASSTag.BASS_TAG_DSD_TITLE)
			{
				if (tagType == BASSTag.BASS_TAG_DSD_ARTIST)
				{
					return "DSD Artist";
				}
				if (tagType != BASSTag.BASS_TAG_DSD_TITLE)
				{
					goto IL_1C5;
				}
				return "DSD Title";
			}
			else
			{
				if (tagType == BASSTag.BASS_TAG_DSD_COMMENT)
				{
					return "DSD Comment";
				}
				if (tagType != BASSTag.BASS_TAG_HLS_EXTINF)
				{
					goto IL_1C5;
				}
				return "HLS";
			}
			return "MUSIC";
			IL_1C5:
			return "Unknown";
		}

		public static string BASSAddOnGetSupportedFileExtensions(string file)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(file))
			{
				return Bass.SupportedStreamExtensions;
			}
			if (file.ToLower() == "music")
			{
				return Bass.SupportedMusicExtensions;
			}
			string text = Path.GetFileNameWithoutExtension(file).ToLower();
			uint num = PrivateImplementationDetails.ComputeStringHash(text);
			if (num <= 2061274259u)
			{
				if (num <= 823504184u)
				{
					if (num <= 467664725u)
					{
						if (num != 116431425u)
						{
							if (num == 467664725u)
							{
								if (text == "bass_cdg")
								{
									result = BassCdg.SupportedStreamExtensions;
								}
							}
						}
						else if (text == "bass_ape")
						{
							result = BassApe.SupportedStreamExtensions;
						}
					}
					else if (num != 813142139u)
					{
						if (num == 823504184u)
						{
							if (text == "bass_ac3")
							{
								result = BassAc3.SupportedStreamExtensions;
							}
						}
					}
					else if (text == "bassmidi")
					{
						result = BassMidi.SupportedStreamExtensions;
					}
				}
				else if (num <= 1318759027u)
				{
					if (num != 1195663244u)
					{
						if (num == 1318759027u)
						{
							if (text == "bassalac")
							{
								result = BassAlac.SupportedStreamExtensions;
							}
						}
					}
					else if (text == "bass_spx")
					{
						result = BassSpx.SupportedStreamExtensions;
					}
				}
				else if (num != 1399761437u)
				{
					if (num == 2061274259u)
					{
						if (text == "basswv")
						{
							result = BassWv.SupportedStreamExtensions;
						}
					}
				}
				else if (text == "basswma")
				{
					result = BassWma.SupportedStreamExtensions;
				}
			}
			else if (num <= 3278768865u)
			{
				if (num <= 2633684781u)
				{
					if (num != 2362194717u)
					{
						if (num == 2633684781u)
						{
							if (text == "basscd")
							{
								result = BassCd.SupportedStreamExtensions;
							}
						}
					}
					else if (text == "bassdsd")
					{
						result = BassDsd.SupportedStreamExtensions;
					}
				}
				else if (num != 2948257496u)
				{
					if (num == 3278768865u)
					{
						if (text == "bassopus")
						{
							result = BassOpus.SupportedStreamExtensions;
						}
					}
				}
				else if (text == "bassflac")
				{
					result = BassFlac.SupportedStreamExtensions;
				}
			}
			else if (num <= 3623643491u)
			{
				if (num != 3508217414u)
				{
					if (num == 3623643491u)
					{
						if (text == "bass_mpc")
						{
							result = BassMpc.SupportedStreamExtensions;
						}
					}
				}
				else if (text == "bass_aac")
				{
					result = BassAac.SupportedStreamExtensions;
				}
			}
			else if (num != 3928099174u)
			{
				if (num != 3951651824u)
				{
					if (num == 4155915706u)
					{
						if (text == "bass_tta")
						{
							result = BassTta.SupportedStreamExtensions;
						}
					}
				}
				else if (text == "bass_ofr")
				{
					result = BassOfr.SupportedStreamExtensions;
				}
			}
			else if (text == "bass_adx")
			{
				result = BassAdx.SupportedStreamExtensions;
			}
			return result;
		}

		public static string BASSAddOnGetSupportedFileName(string file)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(file))
			{
				return Bass.SupportedStreamName;
			}
			if (file.ToLower() == "music")
			{
				return "MOD Music";
			}
			string text = Path.GetFileNameWithoutExtension(file).ToLower();
			uint num = PrivateImplementationDetails.ComputeStringHash(text);
			if (num <= 2061274259u)
			{
				if (num <= 823504184u)
				{
					if (num <= 467664725u)
					{
						if (num != 116431425u)
						{
							if (num == 467664725u)
							{
								if (text == "bass_cdg")
								{
									result = BassCdg.SupportedStreamName;
								}
							}
						}
						else if (text == "bass_ape")
						{
							result = BassApe.SupportedStreamName;
						}
					}
					else if (num != 813142139u)
					{
						if (num == 823504184u)
						{
							if (text == "bass_ac3")
							{
								result = BassAc3.SupportedStreamName;
							}
						}
					}
					else if (text == "bassmidi")
					{
						result = BassMidi.SupportedStreamName;
					}
				}
				else if (num <= 1318759027u)
				{
					if (num != 1195663244u)
					{
						if (num == 1318759027u)
						{
							if (text == "bassalac")
							{
								result = BassAlac.SupportedStreamName;
							}
						}
					}
					else if (text == "bass_spx")
					{
						result = BassSpx.SupportedStreamName;
					}
				}
				else if (num != 1399761437u)
				{
					if (num == 2061274259u)
					{
						if (text == "basswv")
						{
							result = BassWv.SupportedStreamName;
						}
					}
				}
				else if (text == "basswma")
				{
					result = BassWma.SupportedStreamName;
				}
			}
			else if (num <= 3278768865u)
			{
				if (num <= 2633684781u)
				{
					if (num != 2362194717u)
					{
						if (num == 2633684781u)
						{
							if (text == "basscd")
							{
								result = BassCd.SupportedStreamName;
							}
						}
					}
					else if (text == "bassdsd")
					{
						result = BassDsd.SupportedStreamName;
					}
				}
				else if (num != 2948257496u)
				{
					if (num == 3278768865u)
					{
						if (text == "bassopus")
						{
							result = BassOpus.SupportedStreamName;
						}
					}
				}
				else if (text == "bassflac")
				{
					result = BassFlac.SupportedStreamName;
				}
			}
			else if (num <= 3623643491u)
			{
				if (num != 3508217414u)
				{
					if (num == 3623643491u)
					{
						if (text == "bass_mpc")
						{
							result = BassMpc.SupportedStreamName;
						}
					}
				}
				else if (text == "bass_aac")
				{
					result = BassAac.SupportedStreamName;
				}
			}
			else if (num != 3928099174u)
			{
				if (num != 3951651824u)
				{
					if (num == 4155915706u)
					{
						if (text == "bass_tta")
						{
							result = BassTta.SupportedStreamName;
						}
					}
				}
				else if (text == "bass_ofr")
				{
					result = BassOfr.SupportedStreamName;
				}
			}
			else if (text == "bass_adx")
			{
				result = BassAdx.SupportedStreamName;
			}
			return result;
		}

		public static string BASSAddOnGetSupportedFileExtensions(Dictionary<int, string> plugins, bool includeBASS)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (includeBASS)
			{
				stringBuilder.Append(Utils.BASSAddOnGetSupportedFileExtensions(null));
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(";");
				}
				stringBuilder.Append(Utils.BASSAddOnGetSupportedFileExtensions("music"));
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(";");
				}
			}
			if (plugins != null)
			{
				foreach (string file in plugins.Values)
				{
					stringBuilder.Append(Utils.BASSAddOnGetSupportedFileExtensions(file));
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(";");
					}
				}
			}
			if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ';')
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == ';')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static string BASSAddOnGetSupportedFileFilter(Dictionary<int, string> plugins, string allFormatName)
		{
			return Utils.BASSAddOnGetSupportedFileFilter(plugins, allFormatName, true);
		}

		public static string BASSAddOnGetSupportedFileFilter(Dictionary<int, string> plugins, string allFormatName, bool includeBASS)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (includeBASS)
			{
				text = Utils.BASSAddOnGetSupportedFileName(null);
				text2 = Utils.BASSAddOnGetSupportedFileExtensions(null);
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						text,
						" (",
						text2,
						")|",
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder2.Append(text2);
				}
				text = Utils.BASSAddOnGetSupportedFileName("music");
				text2 = Utils.BASSAddOnGetSupportedFileExtensions("music");
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						"|",
						text,
						" (",
						text2,
						")|",
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder2.Append(";" + text2);
				}
			}
			if (plugins != null)
			{
				foreach (string file in plugins.Values)
				{
					text = Utils.BASSAddOnGetSupportedFileName(file);
					text2 = Utils.BASSAddOnGetSupportedFileExtensions(file);
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append(string.Concat(new string[]
						{
							"|",
							text,
							" (",
							text2,
							")|",
							text2
						}));
					}
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder2.Append(";" + text2);
					}
				}
			}
			if (!string.IsNullOrEmpty(allFormatName) && stringBuilder2.Length > 0)
			{
				stringBuilder.Insert(0, allFormatName + "|" + stringBuilder2.ToString() + "|");
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static string BASSAddOnGetSupportedFileFilter(Dictionary<int, string> plugins, string allFormatName, bool includeBASS, Dictionary<string, string> extra)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (includeBASS)
			{
				text = Utils.BASSAddOnGetSupportedFileName(null);
				text2 = Utils.BASSAddOnGetSupportedFileExtensions(null);
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						text,
						" (",
						text2,
						")|",
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder2.Append(text2);
				}
				text = Utils.BASSAddOnGetSupportedFileName("music");
				text2 = Utils.BASSAddOnGetSupportedFileExtensions("music");
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						"|",
						text,
						" (",
						text2,
						")|",
						text2
					}));
				}
				if (!string.IsNullOrEmpty(text2))
				{
					stringBuilder2.Append(";" + text2);
				}
			}
			if (plugins != null)
			{
				foreach (string file in plugins.Values)
				{
					text = Utils.BASSAddOnGetSupportedFileName(file);
					text2 = Utils.BASSAddOnGetSupportedFileExtensions(file);
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append(string.Concat(new string[]
						{
							"|",
							text,
							" (",
							text2,
							")|",
							text2
						}));
					}
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder2.Append(";" + text2);
					}
				}
			}
			if (extra != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in extra)
				{
					text = keyValuePair.Key;
					text2 = keyValuePair.Value;
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append(string.Concat(new string[]
						{
							"|",
							text,
							" (",
							text2,
							")|",
							text2
						}));
					}
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder2.Append(";" + text2);
					}
				}
			}
			if (!string.IsNullOrEmpty(allFormatName) && stringBuilder2.Length > 0)
			{
				stringBuilder.Insert(0, allFormatName + "|" + stringBuilder2.ToString() + "|");
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static string BASSAddOnGetPluginFileFilter(Dictionary<int, string> plugins, string allFormatName)
		{
			return Utils.BASSAddOnGetPluginFileFilter(plugins, allFormatName, true);
		}

		public static string BASSAddOnGetPluginFileFilter(Dictionary<int, string> plugins, string allFormatName, bool includeBASS)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (includeBASS)
			{
				foreach (BASS_PLUGINFORM bass_PLUGINFORM in Bass.BASS_PluginGetInfo(0).formats)
				{
					text = bass_PLUGINFORM.name;
					text2 = bass_PLUGINFORM.exts;
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append("|" + text + "|" + text2);
					}
					if (!string.IsNullOrEmpty(text2))
					{
						stringBuilder2.Append(";" + text2);
					}
				}
			}
			if (plugins != null)
			{
				foreach (int handle in plugins.Keys)
				{
					foreach (BASS_PLUGINFORM bass_PLUGINFORM2 in Bass.BASS_PluginGetInfo(handle).formats)
					{
						text = bass_PLUGINFORM2.name;
						text2 = bass_PLUGINFORM2.exts;
						if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
						{
							stringBuilder.Append("|" + text + "|" + text2);
						}
						if (!string.IsNullOrEmpty(text2))
						{
							stringBuilder2.Append(";" + text2);
						}
					}
				}
			}
			if (stringBuilder2.Length > 0 && stringBuilder2[0] == ';')
			{
				stringBuilder2.Remove(0, 1);
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			if (!string.IsNullOrEmpty(allFormatName) && stringBuilder2.Length > 0)
			{
				stringBuilder.Insert(0, allFormatName + "|" + stringBuilder2.ToString() + "|");
			}
			if (stringBuilder.Length > 0 && stringBuilder[0] == '|')
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static bool BASSAddOnIsFileSupported(Dictionary<int, string> plugins, string filename)
		{
			if (filename == null || filename == string.Empty)
			{
				return false;
			}
			string fileExt = Path.GetExtension(filename).ToLower();
			string empty = string.Empty;
			if (Utils.MatchExtensions(Utils.BASSAddOnGetSupportedFileExtensions(null).ToLower(), fileExt))
			{
				return true;
			}
			if (Utils.MatchExtensions(Utils.BASSAddOnGetSupportedFileExtensions("music").ToLower(), fileExt))
			{
				return true;
			}
			bool result = false;
			if (plugins != null)
			{
				using (Dictionary<int, string>.ValueCollection.Enumerator enumerator = plugins.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Utils.MatchExtensions(Utils.BASSAddOnGetSupportedFileExtensions(enumerator.Current).ToLower(), fileExt))
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		private static bool MatchExtensions(string addonExts, string fileExt)
		{
			bool result = false;
			string[] array = addonExts.Split(new char[]
			{
				';'
			});
			if (array != null && !string.IsNullOrEmpty(fileExt))
			{
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].EndsWith(fileExt))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public static short AbsSignMax(short val1, short val2)
		{
			if (val1 == -32768)
			{
				return val1;
			}
			if (val2 == -32768)
			{
				return val2;
			}
			if (Math.Abs(val1) < Math.Abs(val2))
			{
				return val2;
			}
			return val1;
		}

		public static float AbsSignMax(float val1, float val2)
		{
			if (Math.Abs(val1) < Math.Abs(val2))
			{
				return val2;
			}
			return val1;
		}

		public static double SampleDither(double sample, double factor, double max)
		{
			return sample += (Utils._autoRandomizer.NextDouble() - Utils._autoRandomizer.NextDouble()) * factor / max;
		}

		public static int GetLevel(short[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = 0;
			short num3 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				int num4 = Math.Abs((int)buffer[i]);
				if (num4 > 32767)
				{
					num4 = 32767;
				}
				if (i % 2 == 0)
				{
					if (num4 > (int)num2)
					{
						num2 = (short)num4;
					}
				}
				else if (num4 > (int)num3)
				{
					num3 = (short)num4;
				}
			}
			if (chans == 1)
			{
				num2 = (num3 = Math.Max(num2, num3));
			}
			return Utils.MakeLong(num2, num3);
		}

		public static long GetLevel2(short[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0L;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = short.MinValue;
			short num3 = short.MinValue;
			short num4 = short.MaxValue;
			short num5 = short.MaxValue;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				short num6 = buffer[i];
				if (i % 2 == 0)
				{
					if (num6 > num2)
					{
						num2 = num6;
					}
					if (num6 < num4)
					{
						num4 = num6;
					}
				}
				else
				{
					if (num6 > num3)
					{
						num3 = num6;
					}
					if (num6 < num5)
					{
						num5 = num6;
					}
				}
			}
			if (chans == 1)
			{
				num2 = (num3 = Math.Max(num2, num3));
				num4 = (num5 = Math.Min(num4, num5));
			}
			return Utils.MakeLong64(Utils.MakeLong(num4, num2), Utils.MakeLong(num5, num3));
		}

		public static int GetLevel(float[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = 0;
			short num3 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				int num4 = (int)Math.Round((double)Math.Abs(buffer[i] * 32768f));
				if (num4 > 32767)
				{
					num4 = 32767;
				}
				if (i % 2 == 0)
				{
					if (num4 > (int)num2)
					{
						num2 = (short)num4;
					}
				}
				else if (num4 > (int)num3)
				{
					num3 = (short)num4;
				}
			}
			if (chans == 1)
			{
				num2 = (num3 = Math.Max(num2, num3));
			}
			return Utils.MakeLong(num2, num3);
		}

		public static long GetLevel2(float[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0L;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = short.MinValue;
			short num3 = short.MinValue;
			short num4 = short.MaxValue;
			short num5 = short.MaxValue;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				int num6 = (int)Math.Round((double)buffer[i] * 32768.0);
				if (num6 > 32767)
				{
					num6 = 32767;
				}
				else if (num6 < -32768)
				{
					num6 = -32768;
				}
				if (i % 2 == 0)
				{
					if (num6 > (int)num2)
					{
						num2 = (short)num6;
					}
					if (num6 < (int)num4)
					{
						num4 = (short)num6;
					}
				}
				else
				{
					if (num6 > (int)num3)
					{
						num3 = (short)num6;
					}
					if (num6 < (int)num5)
					{
						num5 = (short)num6;
					}
				}
			}
			if (chans == 1)
			{
				num2 = (num3 = Math.Max(num2, num3));
				num4 = (num5 = Math.Min(num4, num5));
			}
			return Utils.MakeLong64(Utils.MakeLong(num4, num2), Utils.MakeLong(num5, num3));
		}

		public static int GetLevel(byte[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = 0;
			short num3 = 0;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				int num4 = Math.Abs((int)(buffer[i] - 128) * 256);
				if (num4 > 32767)
				{
					num4 = 32767;
				}
				if (i % 2 == 0)
				{
					if (num4 > (int)num2)
					{
						num2 = (short)num4;
					}
				}
				else if (num4 > (int)num3)
				{
					num3 = (short)num4;
				}
			}
			if (chans == 1)
			{
				num2 = (num3 = Math.Max(num2, num3));
			}
			return Utils.MakeLong(num2, num3);
		}

		public static long GetLevel2(byte[] buffer, int chans, int startIndex, int length)
		{
			if (buffer == null)
			{
				return 0L;
			}
			int num = buffer.Length;
			if (startIndex > num - 1 || startIndex < 0)
			{
				startIndex = 0;
			}
			if (length > num || length < 0)
			{
				length = num;
			}
			if (startIndex + length > num)
			{
				length = num - startIndex;
			}
			short num2 = short.MinValue;
			short num3 = short.MinValue;
			short num4 = short.MaxValue;
			short num5 = short.MaxValue;
			num = startIndex + length;
			for (int i = startIndex; i < num; i++)
			{
				short num6 = (short)((int)(buffer[i] - 128) * 256);
				if (i % 2 == 0)
				{
					if (num6 > num2)
					{
						num2 = num6;
					}
					if (num6 < num4)
					{
						num4 = num6;
					}
				}
				else
				{
					if (num6 > num3)
					{
						num3 = num6;
					}
					if (num6 < num5)
					{
						num5 = num6;
					}
				}
			}
			if (chans == 1)
			{
				num2 = (num3 = Math.Max(num2, num3));
				num4 = (num5 = Math.Min(num4, num5));
			}
			return Utils.MakeLong64(Utils.MakeLong(num4, num2), Utils.MakeLong(num5, num3));
		}

		public unsafe static int GetLevel(IntPtr buffer, int chans, int bps, int startIndex, int length)
		{
			if (buffer == IntPtr.Zero)
			{
				return 0;
			}
			if (bps == 16 || bps == 32 || bps == 8)
			{
				bps /= 8;
			}
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			short num = 0;
			short num2 = 0;
			int num3 = startIndex + length;
			if (bps == 2)
			{
				short* ptr = (short*)((void*)buffer);
				for (int i = startIndex; i < num3; i++)
				{
					int num4 = Math.Abs((int)ptr[i]);
					if (num4 > 32767)
					{
						num4 = 32767;
					}
					if (i % 2 == 0)
					{
						if (num4 > (int)num)
						{
							num = (short)num4;
						}
					}
					else if (num4 > (int)num2)
					{
						num2 = (short)num4;
					}
				}
			}
			else if (bps == 4)
			{
				float* ptr2 = (float*)((void*)buffer);
				for (int j = startIndex; j < num3; j++)
				{
					int num4 = (int)Math.Round((double)Math.Abs(ptr2[j] * 32768f));
					if (num4 > 32767)
					{
						num4 = 32767;
					}
					if (j % 2 == 0)
					{
						if (num4 > (int)num)
						{
							num = (short)num4;
						}
					}
					else if (num4 > (int)num2)
					{
						num2 = (short)num4;
					}
				}
			}
			else
			{
				byte* ptr3 = (byte*)((void*)buffer);
				for (int k = startIndex; k < num3; k++)
				{
					int num4 = Math.Abs((int)(ptr3[k] - 128) * 256);
					if (num4 > 32767)
					{
						num4 = 32767;
					}
					if (k % 2 == 0)
					{
						if (num4 > (int)num)
						{
							num = (short)num4;
						}
					}
					else if (num4 > (int)num2)
					{
						num2 = (short)num4;
					}
				}
			}
			if (chans == 1)
			{
				num = (num2 = Math.Max(num, num2));
			}
			return Utils.MakeLong(num, num2);
		}

		public unsafe static long GetLevel2(IntPtr buffer, int chans, int bps, int startIndex, int length)
		{
			if (buffer == IntPtr.Zero)
			{
				return 0L;
			}
			if (bps == 16 || bps == 32 || bps == 8)
			{
				bps /= 8;
			}
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			short num = short.MinValue;
			short num2 = short.MinValue;
			short num3 = short.MaxValue;
			short num4 = short.MaxValue;
			int num5 = startIndex + length;
			if (bps == 2)
			{
				short* ptr = (short*)((void*)buffer);
				for (int i = startIndex; i < num5; i++)
				{
					int num6 = (int)ptr[i];
					if (i % 2 == 0)
					{
						if (num6 > (int)num)
						{
							num = (short)num6;
						}
						if (num6 < (int)num3)
						{
							num3 = (short)num6;
						}
					}
					else
					{
						if (num6 > (int)num2)
						{
							num2 = (short)num6;
						}
						if (num6 < (int)num4)
						{
							num4 = (short)num6;
						}
					}
				}
			}
			else if (bps == 4)
			{
				float* ptr2 = (float*)((void*)buffer);
				for (int j = startIndex; j < num5; j++)
				{
					int num6 = (int)Math.Round((double)(ptr2[j] * 32768f));
					if (num6 > 32767)
					{
						num6 = 32767;
					}
					else if (num6 < -32768)
					{
						num6 = -32768;
					}
					if (j % 2 == 0)
					{
						if (num6 > (int)num)
						{
							num = (short)num6;
						}
						if (num6 < (int)num3)
						{
							num3 = (short)num6;
						}
					}
					else
					{
						if (num6 > (int)num2)
						{
							num2 = (short)num6;
						}
						if (num6 < (int)num4)
						{
							num4 = (short)num6;
						}
					}
				}
			}
			else
			{
				byte* ptr3 = (byte*)((void*)buffer);
				for (int k = startIndex; k < num5; k++)
				{
					int num6 = (int)(ptr3[k] - 128) * 256;
					if (k % 2 == 0)
					{
						if (num6 > (int)num)
						{
							num = (short)num6;
						}
						if (num6 < (int)num3)
						{
							num3 = (short)num6;
						}
					}
					else
					{
						if (num6 > (int)num2)
						{
							num2 = (short)num6;
						}
						if (num6 < (int)num4)
						{
							num4 = (short)num6;
						}
					}
				}
			}
			if (chans == 1)
			{
				num = (num2 = Math.Max(num, num2));
				num3 = (num4 = Math.Min(num3, num4));
			}
			return Utils.MakeLong64(Utils.MakeLong(num3, num), Utils.MakeLong(num4, num2));
		}

		public static long DecodeAllData(int channel, bool autoFree)
		{
			long num = 0L;
			byte[] buffer = new byte[131072];
			while (Bass.BASS_ChannelIsActive(channel) == BASSActive.BASS_ACTIVE_PLAYING)
			{
				int num2 = Bass.BASS_ChannelGetData(channel, buffer, 131072);
				if (num2 < 0)
				{
					break;
				}
				num += (long)num2;
			}
			if (autoFree)
			{
				Bass.BASS_StreamFree(channel);
			}
			return num;
		}

		public static bool DetectCuePoints(string filename, float blockSize, ref double cueInPos, ref double cueOutPos, double dBIn, double dBOut, int findZeroCrossing)
		{
			int num = Bass.BASS_StreamCreateFile(filename, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_DECODE);
			bool result = Utils.DetectCuePoints(num, blockSize, ref cueInPos, ref cueOutPos, dBIn, dBOut, findZeroCrossing);
			if (num != 0)
			{
				Bass.BASS_StreamFree(num);
			}
			return result;
		}

		public static bool DetectCuePoints(int decodingStream, float blockSize, ref double cueInPos, ref double cueOutPos, double dBIn, double dBOut, int findZeroCrossing)
		{
			if (decodingStream == 0)
			{
				return false;
			}
			long pos = Bass.BASS_ChannelGetPosition(decodingStream);
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (!Bass.BASS_ChannelGetInfo(decodingStream, bass_CHANNELINFO) || !bass_CHANNELINFO.IsDecodingChannel)
			{
				return false;
			}
			if (dBIn > 0.0)
			{
				dBIn = 0.0;
			}
			else if (dBIn < -90.0)
			{
				dBIn = -90.0;
			}
			if (dBOut > 0.0)
			{
				dBOut = 0.0;
			}
			else if (dBOut < -90.0)
			{
				dBOut = -90.0;
			}
			if (blockSize > 30f)
			{
				blockSize = 30f;
			}
			else if (blockSize < 0.1f)
			{
				blockSize = 0.1f;
			}
			float num = (float)Utils.DBToLevel(dBIn, 1.0);
			float num2 = (float)Utils.DBToLevel(dBOut, 1.0);
			long num3 = Bass.BASS_ChannelGetLength(decodingStream);
			long num4 = 0L;
			long num5 = num3;
			int num6 = (int)Bass.BASS_ChannelSeconds2Bytes(decodingStream, (double)blockSize);
			float[] buffer = new float[num6 / 4];
			int i = 0;
			long num7 = 0L;
			bool flag = false;
			int num8;
			while (!flag && num7 < num3)
			{
				num8 = Bass.BASS_ChannelGetData(decodingStream, buffer, num6);
				num4 = num7;
				i = 0;
				while (!flag && i < num8)
				{
					if (Utils.ScanSampleLevel(buffer, i / 4, bass_CHANNELINFO.chans) < num)
					{
						i += 4 * bass_CHANNELINFO.chans;
					}
					else
					{
						flag = true;
						num4 = num7 + (long)i;
					}
				}
				if (!flag)
				{
					num7 += (long)num8;
					if (num8 <= 0)
					{
						num7 = num3;
						num4 = num7;
					}
				}
			}
			if (flag && num4 < num3)
			{
				if (findZeroCrossing == 1)
				{
					while (i > 0 && !Utils.IsZeroCrossingPos(buffer, i / 4, i / 4 - bass_CHANNELINFO.chans, bass_CHANNELINFO.chans))
					{
						i -= 4 * bass_CHANNELINFO.chans;
						num4 -= (long)(4 * bass_CHANNELINFO.chans);
					}
					if (num4 < 0L)
					{
						num4 = 0L;
					}
				}
				else if (findZeroCrossing == 2)
				{
					while (i > 0 && Utils.ScanSampleLevel(buffer, i / 4, bass_CHANNELINFO.chans) > num / 2f)
					{
						i -= 4 * bass_CHANNELINFO.chans;
						num4 -= (long)(4 * bass_CHANNELINFO.chans);
					}
					if (num4 < 0L)
					{
						num4 = 0L;
					}
				}
			}
			else
			{
				num4 = 0L;
			}
			num8 = 0;
			num7 = num3;
			flag = false;
			while (!flag && num7 > 0L)
			{
				Bass.BASS_ChannelSetPosition(decodingStream, (num7 - (long)num6 >= 0L) ? (num7 - (long)num6) : 0L);
				num8 = Bass.BASS_ChannelGetData(decodingStream, buffer, num6);
				num5 = num7;
				i = num8;
				while (!flag && i > 0)
				{
					if (Utils.ScanSampleLevel(buffer, i / 4 - bass_CHANNELINFO.chans, bass_CHANNELINFO.chans) < num2)
					{
						i -= 4 * bass_CHANNELINFO.chans;
					}
					else
					{
						flag = true;
						num5 = num7 - (long)num8 + (long)i;
					}
				}
				if (!flag)
				{
					num7 -= (long)num8;
					if (num8 <= 0)
					{
						num7 = 0L;
						num5 = num3;
					}
				}
			}
			if (flag && num5 > 0L)
			{
				if (findZeroCrossing == 1)
				{
					while (i < num8 && !Utils.IsZeroCrossingPos(buffer, i / 4, i / 4 + bass_CHANNELINFO.chans, bass_CHANNELINFO.chans))
					{
						i += 4 * bass_CHANNELINFO.chans;
						num5 += (long)(4 * bass_CHANNELINFO.chans);
					}
					if (num5 > num3)
					{
						num5 = num3;
					}
				}
				else if (findZeroCrossing == 2)
				{
					while (i < num8)
					{
						if (Utils.ScanSampleLevel(buffer, i / 4, bass_CHANNELINFO.chans) <= num2 / 2f)
						{
							break;
						}
						i += 4 * bass_CHANNELINFO.chans;
						num5 += (long)(4 * bass_CHANNELINFO.chans);
					}
				}
			}
			else
			{
				num5 = num3;
			}
			cueInPos = Bass.BASS_ChannelBytes2Seconds(decodingStream, num4);
			cueOutPos = Bass.BASS_ChannelBytes2Seconds(decodingStream, num5);
			Bass.BASS_ChannelSetPosition(decodingStream, pos);
			return true;
		}

		public static double DetectNextLevel(int decodingStream, float blockSize, double startpos, double dB, bool reverse, bool findZeroCrossing)
		{
			if (decodingStream == 0)
			{
				return startpos;
			}
			BASS_CHANNELINFO bass_CHANNELINFO = new BASS_CHANNELINFO();
			if (!Bass.BASS_ChannelGetInfo(decodingStream, bass_CHANNELINFO) || !bass_CHANNELINFO.IsDecodingChannel)
			{
				return startpos;
			}
			if (dB > 0.0)
			{
				dB = 0.0;
			}
			else if (dB < -90.0)
			{
				dB = -90.0;
			}
			if (blockSize > 30f)
			{
				blockSize = 30f;
			}
			else if (blockSize < 0.1f)
			{
				blockSize = 0.1f;
			}
			float num = (float)Utils.DBToLevel(dB, 1.0);
			long num2 = Bass.BASS_ChannelGetLength(decodingStream);
			int num3 = (int)Bass.BASS_ChannelSeconds2Bytes(decodingStream, (double)blockSize);
			float[] buffer = new float[num3 / 4];
			int i = 0;
			int num4 = 0;
			long num5 = Bass.BASS_ChannelSeconds2Bytes(decodingStream, startpos);
			long num6 = num5;
			bool flag = false;
			if (reverse)
			{
				while (!flag && num5 > 0L)
				{
					Bass.BASS_ChannelSetPosition(decodingStream, (num5 - (long)num3 >= 0L) ? (num5 - (long)num3) : 0L);
					num4 = Bass.BASS_ChannelGetData(decodingStream, buffer, num3 | 1073741824);
					num6 = num5;
					i = num4;
					while (!flag && i > 0)
					{
						if (Utils.ScanSampleLevel(buffer, i / 4 - bass_CHANNELINFO.chans, bass_CHANNELINFO.chans) < num)
						{
							i -= 4 * bass_CHANNELINFO.chans;
						}
						else
						{
							flag = true;
							num6 = num5 - (long)num4 + (long)i;
						}
					}
					if (!flag)
					{
						num5 -= (long)num4;
						if (num4 <= 0)
						{
							num5 = 0L;
							num6 = num5;
						}
					}
				}
				if (flag && num6 > 0L)
				{
					if (findZeroCrossing)
					{
						while (i < num4)
						{
							if (Utils.IsZeroCrossingPos(buffer, i / 4, i / 4 + bass_CHANNELINFO.chans, bass_CHANNELINFO.chans))
							{
								break;
							}
							i += 4 * bass_CHANNELINFO.chans;
							num6 += (long)(4 * bass_CHANNELINFO.chans);
						}
					}
				}
				else
				{
					num6 = Bass.BASS_ChannelSeconds2Bytes(decodingStream, startpos);
				}
			}
			else
			{
				while (!flag && num5 < num2)
				{
					num4 = Bass.BASS_ChannelGetData(decodingStream, buffer, num3 | 1073741824);
					num6 = num5;
					i = 0;
					while (!flag && i < num4)
					{
						if (Utils.ScanSampleLevel(buffer, i / 4, bass_CHANNELINFO.chans) < num)
						{
							i += 4 * bass_CHANNELINFO.chans;
						}
						else
						{
							flag = true;
							num6 = num5 + (long)i;
						}
					}
					if (!flag)
					{
						num5 += (long)num4;
						if (num4 <= 0)
						{
							num5 = num2;
							num6 = num5;
						}
					}
				}
				if (flag && num6 < num2)
				{
					if (findZeroCrossing)
					{
						while (i > 0 && !Utils.IsZeroCrossingPos(buffer, i / 4, i / 4 - bass_CHANNELINFO.chans, bass_CHANNELINFO.chans))
						{
							i -= 4 * bass_CHANNELINFO.chans;
							startpos -= (double)(4 * bass_CHANNELINFO.chans);
						}
						if (startpos < 0.0)
						{
							startpos = 0.0;
						}
					}
				}
				else
				{
					num6 = Bass.BASS_ChannelSeconds2Bytes(decodingStream, startpos);
				}
			}
			return Bass.BASS_ChannelBytes2Seconds(decodingStream, num6);
		}

		private static float ScanSampleLevel(float[] buffer, int idx, int chans)
		{
			float num = 0f;
			for (int i = 0; i < chans; i++)
			{
				float num2;
				if (idx + i < buffer.Length)
				{
					num2 = Math.Abs(buffer[idx + i]);
				}
				else
				{
					num2 = 0f;
				}
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}

		private static bool IsZeroCrossingPos(float[] buffer, int idx1, int idx2, int chans)
		{
			bool result = false;
			try
			{
				if (chans > 1)
				{
					float num = buffer[idx1];
					float num2 = buffer[idx1 + 1];
					float num3 = buffer[idx2];
					float num4 = buffer[idx2 + 1];
					if ((num >= 0f && num3 <= 0f) || (num2 >= 0f && num4 <= 0f) || (num < 0f && num3 > 0f) || (num2 < 0f && num4 > 0f))
					{
						result = true;
					}
				}
				else
				{
					float num5 = buffer[idx1];
					float num6 = buffer[idx2];
					if ((num5 >= 0f && num6 <= 0f) || (num5 >= 0f && num6 <= 0f))
					{
						result = true;
					}
				}
			}
			catch
			{
			}
			return result;
		}

		public static float GetNormalizationGain(string filename, float blockSize, double startpos, double endpos, ref float peak)
		{
			int num = Bass.BASS_StreamCreateFile(filename, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
			if (num == 0)
			{
				return -1f;
			}
			float result = 1f;
			long num2 = 0L;
			long num3 = Bass.BASS_ChannelGetLength(num);
			if (startpos < 0.0)
			{
				startpos = 0.0;
			}
			if (endpos < 0.0)
			{
				num3 = long.MaxValue;
			}
			if (endpos > startpos && endpos <= Bass.BASS_ChannelBytes2Seconds(num, num3))
			{
				num2 = Bass.BASS_ChannelSeconds2Bytes(num, startpos);
				num3 = Bass.BASS_ChannelSeconds2Bytes(num, endpos);
			}
			if (num2 > 0L)
			{
				Bass.BASS_ChannelSetPosition(num, num2);
			}
			int num4 = (int)Bass.BASS_ChannelSeconds2Bytes(num, (double)blockSize);
			float[] array = new float[num4 / 4];
			float num5 = 0f;
			while (num2 < num3)
			{
				int num6 = Bass.BASS_ChannelGetData(num, array, num4 | 1073741824);
				if (num6 < 0)
				{
					num2 = num3;
				}
				else
				{
					num2 += (long)num6;
					num6 /= 4;
					for (int i = 0; i < num6; i++)
					{
						float num7 = Math.Abs(array[i]);
						if (num7 > num5)
						{
							num5 = num7;
						}
						if (num5 >= 1f)
						{
							break;
						}
					}
				}
				if (num5 >= 1f)
				{
					break;
				}
			}
			if (num5 < 1f && num5 > 0f)
			{
				result = 1f / num5;
			}
			peak = num5;
			if (num != 0)
			{
				Bass.BASS_StreamFree(num);
			}
			return result;
		}

		[DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
		private static extern void DMACopyMemory(IntPtr destination, IntPtr source, IntPtr length);

		public static void DMACopyMemory(IntPtr destination, IntPtr source, long length)
		{
			Utils.DMACopyMemory(destination, source, new IntPtr(length));
		}

		[DllImport("kernel32.dll", EntryPoint = "MoveMemory")]
		private static extern void DMAMoveMemory(IntPtr destination, IntPtr source, IntPtr length);

		public static void DMAMoveMemory(IntPtr destination, IntPtr source, long length)
		{
			Utils.DMAMoveMemory(destination, source, new IntPtr(length));
		}

		[DllImport("kernel32.dll", EntryPoint = "FillMemory")]
		private static extern void DMAFillMemory(IntPtr destination, IntPtr length, byte fill);

		public static void DMAFillMemory(IntPtr destination, long length, byte fill)
		{
			Utils.DMAFillMemory(destination, new IntPtr(length), fill);
		}

		[DllImport("kernel32.dll", EntryPoint = "ZeroMemory")]
		private static extern void DMAZeroMemory(IntPtr destination, IntPtr length);

		public static void DMAZeroMemory(IntPtr destination, long length)
		{
			Utils.DMAZeroMemory(destination, new IntPtr(length));
		}

		[DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
		public static extern int LIBLoadLibrary(string fileName);

		[DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool LIBFreeLibrary(int hModule);

		internal static bool LoadLib(string moduleName, ref int handle)
		{
			if (handle == 0)
			{
				handle = Utils.LIBLoadLibrary(moduleName);
			}
			return handle != 0;
		}

		internal static bool FreeLib(ref int handle)
		{
			return handle == 0 || Utils.LIBFreeLibrary(handle);
		}

		private static Random _autoRandomizer = new Random();
	}
}
