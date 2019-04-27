using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BASS_TAG_BEXT
	{
		public string Description
		{
			get
			{
				if (this.description == null)
				{
					return string.Empty;
				}
				Encoding encoding = Encoding.ASCII;
				if (BassNet.UseBrokenLatin1Behavior)
				{
					encoding = Encoding.Default;
				}
				string[] array = encoding.GetString(this.description, 0, this.description.Length).TrimEnd(new char[1]).Split(new char[1], 2, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].TrimEnd(new char[]
					{
						'\0',
						' '
					});
				}
				return string.Join(";", array);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.description = new byte[256];
					return;
				}
				string[] array = value.Split(new char[]
				{
					';'
				}, 2, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Trim();
				}
				string text = string.Empty;
				if (array.Length > 1)
				{
					if (array[0] != null && array[0].Length >= 64)
					{
						array[0] = array[0].Remove(63);
					}
					if (array[0] != null)
					{
						array[0] = array[0].PadRight(64, '\0');
						text = array[0] + array[1];
					}
					else
					{
						text = array[1];
					}
					if (text.Length > 256)
					{
						text = text.Remove(256);
					}
				}
				else
				{
					text = array[0];
					if (text.Length > 256)
					{
						text = text.Remove(256);
					}
				}
				this.description = new byte[256];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(text, 0, (text.Length > 256) ? 256 : text.Length, this.description, 0);
					return;
				}
				Encoding.ASCII.GetBytes(text, 0, (text.Length > 256) ? 256 : text.Length, this.description, 0);
			}
		}

		public string Originator
		{
			get
			{
				if (this.originator == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.originator, 0, this.originator.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.originator, 0, this.originator.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.originator = new byte[32];
					return;
				}
				this.originator = new byte[32];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, this.originator, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, this.originator, 0);
			}
		}

		public string OriginatorReference
		{
			get
			{
				if (this.originatorReference == null)
				{
					return string.Empty;
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.originatorReference, 0, this.originatorReference.Length).TrimEnd(new char[1]);
				}
				return Encoding.ASCII.GetString(this.originatorReference, 0, this.originatorReference.Length).TrimEnd(new char[1]);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.originatorReference = new byte[32];
					return;
				}
				this.originatorReference = new byte[32];
				if (BassNet.UseBrokenLatin1Behavior)
				{
					Encoding.Default.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, this.originatorReference, 0);
					return;
				}
				Encoding.ASCII.GetBytes(value, 0, (value.Length > 32) ? 32 : value.Length, this.originatorReference, 0);
			}
		}

		public string OriginationDate
		{
			get
			{
				if (this.originationDate == null)
				{
					return "0000-01-01";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.originationDate, 0, this.originationDate.Length).TrimEnd(new char[1]).Replace(' ', '-').Replace(':', '-').Replace('.', '-').Replace('_', '-');
				}
				return Encoding.ASCII.GetString(this.originationDate, 0, this.originationDate.Length).TrimEnd(new char[1]).Replace(' ', '-').Replace(':', '-').Replace('.', '-').Replace('_', '-');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (BassNet.UseBrokenLatin1Behavior)
					{
						this.originationDate = Encoding.Default.GetBytes("0000-01-01");
						return;
					}
					this.originationDate = Encoding.ASCII.GetBytes("0000-01-01");
					return;
				}
				else
				{
					this.originationDate = new byte[10];
					if (BassNet.UseBrokenLatin1Behavior)
					{
						Encoding.Default.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, this.originationDate, 0);
						return;
					}
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 10) ? 10 : value.Length, this.originationDate, 0);
					return;
				}
			}
		}

		public string OriginationTime
		{
			get
			{
				if (this.originationTime == null)
				{
					return "00:00:00";
				}
				if (BassNet.UseBrokenLatin1Behavior)
				{
					return Encoding.Default.GetString(this.originationTime, 0, this.originationTime.Length).TrimEnd(new char[1]).Replace(' ', ':').Replace('-', ':').Replace('.', ':').Replace('_', ':');
				}
				return Encoding.ASCII.GetString(this.originationTime, 0, this.originationTime.Length).TrimEnd(new char[1]).Replace(' ', ':').Replace('-', ':').Replace('.', ':').Replace('_', ':');
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					if (BassNet.UseBrokenLatin1Behavior)
					{
						this.originationTime = Encoding.Default.GetBytes("00:00:00");
						return;
					}
					this.originationTime = Encoding.ASCII.GetBytes("00:00:00");
					return;
				}
				else
				{
					this.originationTime = new byte[8];
					if (BassNet.UseBrokenLatin1Behavior)
					{
						Encoding.Default.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, this.originationTime, 0);
						return;
					}
					Encoding.ASCII.GetBytes(value, 0, (value.Length > 8) ? 8 : value.Length, this.originationTime, 0);
					return;
				}
			}
		}

		public string UMID
		{
			get
			{
				return Utils.ByteToHex(this.umid);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.umid = new byte[64];
					return;
				}
				this.umid = Utils.HexToByte(value, 64);
			}
		}

		public override string ToString()
		{
			return this.Description;
		}

		public unsafe string GetCodingHistory(IntPtr tag)
		{
			if (tag == IntPtr.Zero)
			{
				return null;
			}
			int num;
			return Utils.IntPtrAsStringLatin1(new IntPtr((void*)((byte*)tag.ToPointer() + 602)), out num);
		}

		public byte[] AsByteArray(string codingHistory)
		{
			if (string.IsNullOrEmpty(codingHistory))
			{
				codingHistory = new string('\0', 256);
			}
			else
			{
				if (!codingHistory.EndsWith("\r\n"))
				{
					codingHistory += "\r\n";
				}
				if (!codingHistory.EndsWith("\0"))
				{
					codingHistory += "\0";
				}
				if (codingHistory.Length % 2 == 1)
				{
					codingHistory += "\0";
				}
				int num = codingHistory.Length % 256;
				if (num > 0)
				{
					codingHistory += new string('\0', num);
				}
			}
			byte[] array = BassNet.UseBrokenLatin1Behavior ? Encoding.Default.GetBytes(codingHistory) : Encoding.ASCII.GetBytes(codingHistory);
			int num2 = Marshal.SizeOf(typeof(BASS_TAG_BEXT));
			byte[] array2 = new byte[num2];
			GCHandle gchandle = GCHandle.Alloc(array2, GCHandleType.Pinned);
			Marshal.StructureToPtr(this, gchandle.AddrOfPinnedObject(), false);
			gchandle.Free();
			byte[] array3 = new byte[num2 + array.Length];
			Array.Copy(array2, 0, array3, 0, num2);
			Array.Copy(array, 0, array3, num2, array.Length);
			return array3;
		}

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		private byte[] description;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] originator;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		private byte[] originatorReference;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		private byte[] originationDate;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		private byte[] originationTime;

		public long TimeReference;

		public short Version;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] umid;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 190)]
		public byte[] Reserved;
	}
}
