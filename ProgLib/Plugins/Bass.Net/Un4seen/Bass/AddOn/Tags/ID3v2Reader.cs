using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Un4seen.Bass.AddOn.Tags
{
	[SuppressUnmanagedCodeSecurity]
	internal class ID3v2Reader
	{
		public ID3v2Reader(IntPtr pID3v2)
		{
			if (Utils.IntPtrAsStringAnsi(pID3v2, 3) == "ID3")
			{
				this.offset += 3;
				this.majorVersion = Marshal.ReadByte(pID3v2, this.offset);
				this.offset++;
				this.minorVersion = Marshal.ReadByte(pID3v2, this.offset);
				this.offset++;
				this.tagFlags = Marshal.ReadByte(pID3v2, this.offset);
				this.offset++;
				int num = this.ReadSynchsafeInt32(pID3v2, this.offset);
				this.offset += 4;
				bool flag = (this.tagFlags & 64) > 0;
				int num2 = 0;
				if (flag)
				{
					num2 = this.ReadSynchsafeInt32(pID3v2, this.offset);
				}
				this.buffer = new byte[num + 10];
				Marshal.Copy(pID3v2, this.buffer, 0, num + 10);
				this.stream = new MemoryStream(this.buffer);
				this.stream.Position = (long)(10 + num2);
				int num3 = num + 10;
				this.lastTagPos = num3 - 10;
			}
			else
			{
				this.majorVersion = this.DefaultMajorVersion;
				this.minorVersion = this.DefaultMinorVersion;
				this.stream = null;
			}
			this.frameId = null;
			this.frameValue = null;
		}

		public void Close()
		{
			if (this.stream != null)
			{
				this.stream.Close();
				this.stream.Dispose();
				this.stream = null;
			}
		}

		public bool Read()
		{
			this.frameId = null;
			this.frameValue = null;
			if (this.stream == null)
			{
				return false;
			}
			if (this.stream.Position > (long)this.lastTagPos)
			{
				return false;
			}
			this.frameId = this.ReadFrameId();
			int num = this.ReadFrameLength();
			if (num > 16777216)
			{
				return false;
			}
			this.frameFlags = 0;
			if (this.majorVersion > 2)
			{
				this.frameFlags = this.ReadFrameFlags();
			}
			if (num == 0)
			{
				this.frameValue = string.Empty;
			}
			else
			{
				this.frameValue = this.ReadFrameValue(num, this.frameFlags);
			}
			return true;
		}

		public string GetKey()
		{
			return this.frameId;
		}

		public object GetValue()
		{
			return this.frameValue;
		}

		public short GetFlags()
		{
			return this.frameFlags;
		}

		public byte MajorVersion
		{
			get
			{
				return this.majorVersion;
			}
		}

		public byte MinorVersion
		{
			get
			{
				return this.minorVersion;
			}
		}

		private string ReadMagic(IntPtr p)
		{
			byte[] bytes = new byte[3];
			this.stream.Read(bytes, 0, 3);
			return Encoding.ASCII.GetString(bytes, 0, 3);
		}

		private string ReadFrameId()
		{
			int num = 4;
			if (this.majorVersion == 2)
			{
				num = 3;
			}
			byte[] bytes = new byte[num];
			this.stream.Read(bytes, 0, num);
			return Encoding.ASCII.GetString(bytes, 0, num).TrimEnd(new char[1]);
		}

		private int ReadFrameLength()
		{
			int result;
			if (this.majorVersion == 4)
			{
				result = this.ReadSynchsafeInt32();
			}
			else if (this.majorVersion == 3)
			{
				result = this.ReadInt32();
			}
			else
			{
				if (this.majorVersion != 2)
				{
					throw new NotSupportedException("Unsupported ID3v2 version detected. Don't know how to deal with this version.");
				}
				result = this.ReadInt24();
			}
			return result;
		}

		private short ReadFrameFlags()
		{
			int num = this.stream.ReadByte();
			int num2 = this.stream.ReadByte();
			return (short)(num << 8 | num2);
		}

		private byte[] UnsyncBuffer(byte[] buffer)
		{
			List<byte> list = new List<byte>(buffer);
			for (int i = list.Count - 2; i >= 0; i--)
			{
				if (list[i] == 255 && list[i + 1] == 0)
				{
					list.RemoveAt(i + 1);
				}
			}
			return list.ToArray();
		}

		private object ReadFrameValue(int frameLength, short frameFlags)
		{
			byte[] array = new byte[frameLength];
			this.stream.Read(array, 0, frameLength);
			int num = 0;
			if ((frameFlags & 1) != 0)
			{
				this.GetSynchsafeInt32(array[0], array[1], array[2], array[3]);
				num = 4;
			}
			if ((this.tagFlags & 128) != 0 || (frameFlags & 2) != 0)
			{
				array = this.UnsyncBuffer(array);
				frameLength = array.Length;
			}
			if (this.frameId == "COM" || this.frameId == "COMM" || this.frameId == "USER" || this.frameId == "ULT" || this.frameId == "USLT")
			{
				Encoding encoding = this.GetFrameEncoding(array[num]);
				num += 4;
				if (array[num - 4] == 1 && frameLength > 6)
				{
					if (array[num] == 254 && array[num + 1] == 255)
					{
						encoding = Encoding.BigEndianUnicode;
						num += 2;
					}
					else
					{
						if (array[num] != 255 || array[num + 1] != 254)
						{
							return string.Empty;
						}
						encoding = Encoding.Unicode;
						num += 2;
					}
				}
				string text = encoding.GetString(array, num, frameLength - num).TrimEnd(new char[1]);
				string[] array2 = text.Split(new char[1]);
				if (array2 != null && array2.Length > 1)
				{
					if (array2[0].TrimWithBOM().Length > 0 && array2[1].TrimWithBOM().Length > 0)
					{
						text = "(" + array2[0].TrimWithBOM() + "):" + array2[1].TrimWithBOM();
					}
					else
					{
						text = array2[1].TrimWithBOM();
					}
				}
				return text.TrimWithBOM();
			}
			if (this.frameId == "WXXX" || this.frameId == "WXX" || this.frameId == "TXXX" || this.frameId == "TXX")
			{
				Encoding encoding2 = this.GetFrameEncoding(array[num]);
				num++;
				if (array[num - 1] == 1 && frameLength > 6)
				{
					if (array[num] == 254 && array[num + 1] == 255)
					{
						encoding2 = Encoding.BigEndianUnicode;
						num += 2;
					}
					else
					{
						if (array[num] != 255 || array[num + 1] != 254)
						{
							return string.Empty;
						}
						encoding2 = Encoding.Unicode;
						num += 2;
					}
				}
				string text2 = encoding2.GetString(array, num, frameLength - num).TrimEnd(new char[1]);
				string[] array3 = text2.Split(new char[1]);
				if (array3 != null && array3.Length > 1)
				{
					if (array3[0].TrimWithBOM().Length > 0)
					{
						text2 = array3[0].TrimWithBOM() + ":" + array3[1].TrimWithBOM();
					}
					else
					{
						text2 = array3[1].TrimWithBOM();
					}
				}
				return text2.TrimWithBOM();
			}
			if (this.frameId[0] == 'T')
			{
				Encoding encoding3 = this.GetFrameEncoding(array[num]);
				num++;
				if (array[num - 1] == 1 && frameLength > 3)
				{
					if (array[num] == 254 && array[num + 1] == 255)
					{
						encoding3 = Encoding.BigEndianUnicode;
						num += 2;
					}
					else
					{
						if (array[num] != 255 || array[num + 1] != 254)
						{
							return string.Empty;
						}
						encoding3 = Encoding.Unicode;
						num += 2;
					}
				}
				return encoding3.GetString(array, num, frameLength - num).TrimEnd(new char[1]).TrimWithBOM();
			}
			if (this.frameId[0] == 'W')
			{
				string text3 = Encoding.Default.GetString(array, num, frameLength).TrimEnd(new char[1]).TrimEnd(new char[1]);
				string[] array4 = text3.Split(new char[1]);
				if (array4 != null && array4.Length > 1)
				{
					text3 = array4[0].TrimWithBOM();
				}
				return text3.TrimWithBOM();
			}
			if (this.frameId == "UFI" || this.frameId == "LNK" || this.frameId == "UFID" || this.frameId == "LINK")
			{
				string text4 = Encoding.Default.GetString(array, num, frameLength).TrimEnd(new char[1]);
				string[] array5 = text4.Split(new char[1]);
				if (array5 != null && array5.Length > 1)
				{
					if (array5[0].TrimWithBOM().Length > 0)
					{
						text4 = array5[0].TrimWithBOM() + ":" + array5[1].TrimWithBOM();
					}
					else
					{
						text4 = array5[1].TrimWithBOM();
					}
				}
				return text4.TrimWithBOM();
			}
			if (this.frameId == "POP" || this.frameId == "POPM")
			{
				byte b = 0;
				for (int i = 0; i < frameLength; i++)
				{
					if (array[i + num] == 0 && i < frameLength - 1)
					{
						b = array[i + num + 1];
						break;
					}
				}
				return b;
			}
			return array;
		}

		private int ReadSynchsafeInt32(IntPtr p, int offset)
		{
			byte[] array = new byte[]
			{
				Marshal.ReadByte(p, offset),
				Marshal.ReadByte(p, offset + 1),
				Marshal.ReadByte(p, offset + 2),
				Marshal.ReadByte(p, offset + 3)
			};
			if ((array[0] & 128) != 0 || (array[1] & 128) != 0 || (array[2] & 128) != 0 || (array[3] & 128) != 0)
			{
				throw new FormatException("Found invalid syncsafe integer");
			}
			return (int)array[0] << 21 | (int)array[1] << 14 | (int)array[2] << 7 | (int)array[3];
		}

		private int ReadSynchsafeInt32()
		{
			byte[] array = new byte[4];
			this.stream.Read(array, 0, 4);
			return this.GetSynchsafeInt32(array[0], array[1], array[2], array[3]);
		}

		private int GetSynchsafeInt32(byte first, byte second, byte third, byte forth)
		{
			if ((first & 128) != 0 || (second & 128) != 0 || (third & 128) != 0 || (forth & 128) != 0)
			{
				throw new FormatException("Found invalid syncsafe integer");
			}
			return (int)first << 21 | (int)second << 14 | (int)third << 7 | (int)forth;
		}

		private int ReadInt32(IntPtr p, int offset)
		{
			byte[] array = new byte[]
			{
				Marshal.ReadByte(p, offset),
				Marshal.ReadByte(p, offset + 1),
				Marshal.ReadByte(p, offset + 2),
				Marshal.ReadByte(p, offset + 3)
			};
			return (int)array[0] << 24 | (int)array[1] << 16 | (int)array[2] << 8 | (int)array[3];
		}

		private int ReadInt32()
		{
			byte[] array = new byte[4];
			this.stream.Read(array, 0, 4);
			return (int)array[0] << 24 | (int)array[1] << 16 | (int)array[2] << 8 | (int)array[3];
		}

		private int ReadInt24(IntPtr p, int offset)
		{
			byte[] array = new byte[]
			{
				Marshal.ReadByte(p, offset),
				Marshal.ReadByte(p, offset + 1),
				Marshal.ReadByte(p, offset + 2)
			};
			return (int)array[0] << 16 | (int)array[1] << 8 | (int)array[2];
		}

		private int ReadInt24()
		{
			byte[] array = new byte[3];
			this.stream.Read(array, 0, 3);
			return (int)array[0] << 16 | (int)array[1] << 8 | (int)array[2];
		}

		private Encoding GetFrameEncoding(byte frameEncoding)
		{
			switch (frameEncoding)
			{
			case 0:
				if (!BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.GetEncoding("latin1");
				}
				return Encoding.Default;
			case 1:
				return Encoding.Unicode;
			case 2:
				return Encoding.BigEndianUnicode;
			case 3:
				return Encoding.UTF8;
			default:
				if (!BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.GetEncoding("latin1");
				}
				return Encoding.Default;
			}
		}

		private int GetDataLength()
		{
			return 0;
		}

		public TagPicture GetPicture(byte[] frameValue, short frameFlags, int index, bool v2)
		{
			if (frameValue == null)
			{
				return null;
			}
			TagPicture result = null;
			try
			{
				int num = 0;
				int num2 = frameValue.Length;
				if ((frameFlags & 1) != 0)
				{
					this.GetSynchsafeInt32(frameValue[0], frameValue[1], frameValue[2], frameValue[3]);
					num = 4;
				}
				Encoding frameEncoding = this.GetFrameEncoding(frameValue[num]);
				num++;
				string mimeType;
				if (v2)
				{
					mimeType = "Unknown";
					if (frameValue[num] == 74 && frameValue[num + 1] == 80 && frameValue[num + 2] == 71)
					{
						mimeType = "image/jpeg";
					}
					else if (frameValue[num] == 71 && frameValue[num + 1] == 73 && frameValue[num + 2] == 70)
					{
						mimeType = "image/gif";
					}
					else if (frameValue[num] == 66 && frameValue[num + 1] == 80 && frameValue[num + 2] == 77)
					{
						mimeType = "image/bmp";
					}
					else if (frameValue[num] == 80 && frameValue[num + 1] == 78 && frameValue[num + 2] == 71)
					{
						mimeType = "image/png";
					}
					num += 3;
				}
				else
				{
					mimeType = this.ReadTextZero(frameValue, ref num);
					num++;
				}
				byte b = frameValue[num];
				TagPicture.PICTURE_TYPE pictureType;
				try
				{
					pictureType = (TagPicture.PICTURE_TYPE)b;
				}
				catch
				{
					pictureType = TagPicture.PICTURE_TYPE.Unknown;
				}
				num++;
				string description = this.ReadTextZero(frameValue, ref num, frameEncoding);
				num++;
				int num3 = frameValue.Length - num;
				if (num3 > 0)
				{
					byte[] array = new byte[num3];
					Array.Copy(frameValue, num, array, 0, num3);
					if ((frameFlags & 2) != 0)
					{
						List<byte> list = new List<byte>(num3);
						for (int i = 0; i < num3; i++)
						{
							list.Add(array[i]);
							if (i < num3 - 1 && array[i] == 255 && array[i + 1] == 0)
							{
								i++;
							}
						}
						array = list.ToArray();
					}
					result = new TagPicture(index, mimeType, pictureType, description, array);
				}
			}
			catch
			{
			}
			return result;
		}

		private string ReadTextZero(byte[] frameValue, ref int offset)
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				char value;
				while ((value = (char)frameValue[offset]) != '\0')
				{
					stringBuilder.Append(value);
					offset++;
				}
			}
			catch
			{
			}
			return stringBuilder.ToString();
		}

		private string ReadTextZero(byte[] frameValue, ref int offset, Encoding encoding)
		{
			string result = string.Empty;
			try
			{
				if (frameValue[0] == 1)
				{
					if (frameValue[offset] == 254 && frameValue[offset + 1] == 255)
					{
						encoding = Encoding.BigEndianUnicode;
					}
					else
					{
						if (frameValue[offset] != 255 || frameValue[offset + 1] != 254)
						{
							return result;
						}
						encoding = Encoding.Unicode;
					}
				}
				int num = 1;
				if (frameValue[0] == 1 || frameValue[0] == 2)
				{
					num = 2;
				}
				int num2 = offset;
				for (;;)
				{
					if (num == 1)
					{
						if (frameValue[num2] == 0)
						{
							goto IL_90;
						}
						num2++;
					}
					else if (num == 2)
					{
						if (frameValue[num2] == 0 && frameValue[num2 + 1] == 0)
						{
							break;
						}
						num2++;
					}
				}
				num2++;
				IL_90:
				result = encoding.GetString(frameValue, offset, num2 - offset);
				offset = num2;
				if (num == 2)
				{
					offset++;
				}
			}
			catch
			{
			}
			return result;
		}

		private Stream stream;

		private byte majorVersion;

		private byte minorVersion;

		private byte tagFlags;

		private int lastTagPos;

		private string frameId;

		private short frameFlags;

		private object frameValue;

		private int offset;

		private byte[] buffer;

		private byte DefaultMajorVersion = 3;

		private byte DefaultMinorVersion;
	}
}
