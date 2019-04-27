using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_FLAC_CUE
	{
		public string Catalog
		{
			get
			{
				if (this._catalog == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringAnsi(this._catalog);
			}
		}

		public int LeadIn
		{
			get
			{
				return this._leadin;
			}
		}

		public bool IsCD
		{
			get
			{
				return this._iscd;
			}
		}

		public int NumTracks
		{
			get
			{
				return this._ntracks;
			}
		}

		public unsafe BASS_TAG_FLAC_CUE_TRACK[] Tracks
		{
			get
			{
				if (this._ntracks > 0 && this._tracks != IntPtr.Zero)
				{
					BASS_TAG_FLAC_CUE_TRACK[] array = new BASS_TAG_FLAC_CUE_TRACK[this._ntracks];
					IntPtr tracks = this._tracks;
					for (int i = 0; i < this._ntracks; i++)
					{
						array[i] = (BASS_TAG_FLAC_CUE_TRACK)Marshal.PtrToStructure(tracks, typeof(BASS_TAG_FLAC_CUE_TRACK));
						tracks = new IntPtr((void*)((byte*)tracks.ToPointer() + Marshal.SizeOf(array[i])));
					}
					return array;
				}
				return null;
			}
		}

		private BASS_TAG_FLAC_CUE()
		{
		}

		public static BASS_TAG_FLAC_CUE GetTag(int handle)
		{
			IntPtr intPtr = Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_FLAC_CUE);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_CUE)Marshal.PtrToStructure(intPtr, typeof(BASS_TAG_FLAC_CUE));
		}

		public static BASS_TAG_FLAC_CUE FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			return (BASS_TAG_FLAC_CUE)Marshal.PtrToStructure(p, typeof(BASS_TAG_FLAC_CUE));
		}

		private IntPtr _catalog = IntPtr.Zero;

		private int _leadin;

		[MarshalAs(UnmanagedType.Bool)]
		private bool _iscd;

		private int _ntracks;

		private IntPtr _tracks = IntPtr.Zero;
	}
}
