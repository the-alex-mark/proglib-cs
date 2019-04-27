using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BASS_TAG_FLAC_CUE_TRACK
	{
		public long Offset
		{
			get
			{
				return this._offset;
			}
		}

		public int Number
		{
			get
			{
				return this._number;
			}
		}

		public string ISRC
		{
			get
			{
				if (this._isrc == IntPtr.Zero)
				{
					return null;
				}
				return Utils.IntPtrAsStringAnsi(this._isrc);
			}
		}

		public BASS_TAG_FLAC_CUE_TRACK.CUESHEETTrackType Flags
		{
			get
			{
				return (BASS_TAG_FLAC_CUE_TRACK.CUESHEETTrackType)this._flags;
			}
		}

		public int NumIndexes
		{
			get
			{
				return this._nindexes;
			}
		}

		public unsafe BASS_TAG_FLAC_CUE_TRACK_INDEX[] Indexes
		{
			get
			{
				if (this._nindexes > 0 && this._indexes != IntPtr.Zero)
				{
					BASS_TAG_FLAC_CUE_TRACK_INDEX[] array = new BASS_TAG_FLAC_CUE_TRACK_INDEX[this._nindexes];
					IntPtr indexes = this._indexes;
					for (int i = 0; i < this._nindexes; i++)
					{
						array[i] = (BASS_TAG_FLAC_CUE_TRACK_INDEX)Marshal.PtrToStructure(indexes, typeof(BASS_TAG_FLAC_CUE_TRACK_INDEX));
						indexes = new IntPtr((void*)((byte*)indexes.ToPointer() + Marshal.SizeOf(array[i])));
					}
					return array;
				}
				return null;
			}
		}

		private BASS_TAG_FLAC_CUE_TRACK()
		{
		}

		private long _offset;

		private int _number;

		private IntPtr _isrc = IntPtr.Zero;

		private int _flags;

		private int _nindexes;

		private IntPtr _indexes = IntPtr.Zero;

		[Flags]
		public enum CUESHEETTrackType
		{
			TAG_FLAC_CUE_TRACK_AUDIO = 0,
			TAG_FLAC_CUE_TRACK_DATA = 1,
			TAG_FLAC_CUE_TRACK_PRE = 2
		}
	}
}
