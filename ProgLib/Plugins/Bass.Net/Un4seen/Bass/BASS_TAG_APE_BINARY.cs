using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_APE_BINARY
	{
		public string Key
		{
			get
			{
				if (this._key == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringAnsi(this._key);
			}
		}

		public byte[] Data
		{
			get
			{
				if (this._data == IntPtr.Zero || this._length == 0)
				{
					return null;
				}
				byte[] array = new byte[this._length];
				Marshal.Copy(this._data, array, 0, this._length);
				return array;
			}
		}

		public int Length
		{
			get
			{
				return this._length;
			}
		}

		public override string ToString()
		{
			return this.Key;
		}

		private BASS_TAG_APE_BINARY()
		{
		}

		public static BASS_TAG_APE_BINARY GetTag(int handle, int index)
		{
			IntPtr intPtr = Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_APE_BINARY + index);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_APE_BINARY)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_APE_BINARY));
		}

		public static BASS_TAG_APE_BINARY FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_APE_BINARY)Marshal.PtrToStructure(p, typeof(BASS_TAG_APE_BINARY));
		}

		private IntPtr _key = IntPtr.Zero;

		private IntPtr _data = IntPtr.Zero;

		private int _length;
	}
}
