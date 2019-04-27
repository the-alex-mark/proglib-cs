using System;
using System.Runtime.InteropServices;
using System.Text;

namespace radio42.Multimedia.Midi
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class MIDIHDR
	{
		public bool IsDone
		{
			get
			{
				return (this.flags & MIDIHeader.MHDR_DONE) > MIDIHeader.MHDR_NONE;
			}
		}

		public bool IsPrepared
		{
			get
			{
				return (this.flags & MIDIHeader.MHDR_PREPARED) > MIDIHeader.MHDR_NONE;
			}
		}

		public bool IsStreamBuffer
		{
			get
			{
				return (this.flags & MIDIHeader.MHDR_ISSTRM) > MIDIHeader.MHDR_NONE;
			}
		}

		public override string ToString()
		{
			if (this.data == IntPtr.Zero || !this.IsPrepared)
			{
				return null;
			}
			if (this.bytesRecorded <= 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(this.bytesRecorded);
			byte[] array = new byte[this.bytesRecorded];
			Marshal.Copy(this.data, array, 0, array.Length);
			foreach (byte b in array)
			{
				stringBuilder.Append(string.Format("{0:X2} ", b));
			}
			return stringBuilder.ToString();
		}

		public byte[] GetData()
		{
			if (this.data == IntPtr.Zero || !this.IsPrepared)
			{
				return null;
			}
			if (this.bytesRecorded <= 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[this.bytesRecorded];
			Marshal.Copy(this.data, array, 0, array.Length);
			return array;
		}

		public void Reset()
		{
			try
			{
				if (this.data != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.data);
				}
			}
			catch
			{
			}
			this.data = IntPtr.Zero;
			this.bufferLength = 0;
			this.bytesRecorded = 0;
			this.flags = MIDIHeader.MHDR_NONE;
			this.next = IntPtr.Zero;
			this.reserved = IntPtr.Zero;
			this.offset = 0;
			for (int i = 0; i < this.reservedArray.Length; i++)
			{
				this.reservedArray[i] = IntPtr.Zero;
			}
		}

		public IntPtr data = IntPtr.Zero;

		public int bufferLength;

		public int bytesRecorded;

		public IntPtr user = IntPtr.Zero;

		public MIDIHeader flags;

		public IntPtr next = IntPtr.Zero;

		public IntPtr reserved = IntPtr.Zero;

		public int offset;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.SysInt)]
		public IntPtr[] reservedArray = new IntPtr[8];
	}
}
