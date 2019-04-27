using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_FLAC_PICTURE
	{
		public string Mime
		{
			get
			{
				if (this._mime == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringAnsi(this._mime);
			}
		}

		public string Desc
		{
			get
			{
				if (this._desc == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringUtf8(this._desc);
			}
		}

		public int Width
		{
			get
			{
				return this._width;
			}
		}

		public int Height
		{
			get
			{
				return this._height;
			}
		}

		public int Depth
		{
			get
			{
				return this._depth;
			}
		}

		public int Colors
		{
			get
			{
				return this._colors;
			}
		}

		public int Length
		{
			get
			{
				return this._length;
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

		public Image Picture
		{
			get
			{
				try
				{
					if (this._data == IntPtr.Zero || this._length == 0)
					{
						return null;
					}
					return new ImageConverter().ConvertFrom(this.Data) as Image;
				}
				catch
				{
				}
				return null;
			}
		}

		public string ImageURL
		{
			get
			{
				if (this.Mime == "-->" && this._data != IntPtr.Zero && this._length > 0)
				{
					return Utils.IntPtrAsStringAnsi(this._data);
				}
				return null;
			}
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Mime))
			{
				return this.Desc;
			}
			return string.Format("{0} ({1})", this.Desc, this.Mime);
		}

		private BASS_TAG_FLAC_PICTURE()
		{
		}

		public static BASS_TAG_FLAC_PICTURE GetTag(int handle, int pictureIndex)
		{
			IntPtr intPtr = Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_ADX_LOOP + pictureIndex);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_PICTURE)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_FLAC_PICTURE));
		}

		public static BASS_TAG_FLAC_PICTURE FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_PICTURE)Marshal.PtrToStructure(p, typeof(BASS_TAG_FLAC_PICTURE));
		}

		private int _apic;

		private IntPtr _mime = IntPtr.Zero;

		private IntPtr _desc = IntPtr.Zero;

		private int _width;

		private int _height;

		private int _depth;

		private int _colors;

		private int _length;

		private IntPtr _data = IntPtr.Zero;
	}
}
