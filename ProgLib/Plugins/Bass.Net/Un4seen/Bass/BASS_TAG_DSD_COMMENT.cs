using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class BASS_TAG_DSD_COMMENT
	{
		public string CommentText
		{
			get
			{
				if (this._commentCount == 0)
				{
					return string.Empty;
				}
				return Encoding.ASCII.GetString(this._commentText, 0, this._commentCount);
			}
		}

		public override string ToString()
		{
			return this.CommentText;
		}

		private BASS_TAG_DSD_COMMENT()
		{
		}

		public static BASS_TAG_DSD_COMMENT GetTag(int handle, int index)
		{
			return BASS_TAG_DSD_COMMENT.FromIntPtr(Bass.BASS_ChannelGetTags(handle, BASSTag.BASS_TAG_DSD_COMMENT + index));
		}

		public static BASS_TAG_DSD_COMMENT FromIntPtr(IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				return null;
			}
			BASS_TAG_DSD_COMMENT bass_TAG_DSD_COMMENT = (BASS_TAG_DSD_COMMENT)Marshal.PtrToStructure(p, typeof(BASS_TAG_DSD_COMMENT));
			if (bass_TAG_DSD_COMMENT._commentCount > 0)
			{
				byte[] array = new byte[bass_TAG_DSD_COMMENT._commentCount];
				Marshal.Copy(IntPtr.Add(p, Marshal.OffsetOf(typeof(BASS_TAG_DSD_COMMENT), "_commentText").ToInt32()), array, 0, bass_TAG_DSD_COMMENT._commentCount);
				Array.Resize<byte>(ref bass_TAG_DSD_COMMENT._commentText, bass_TAG_DSD_COMMENT._commentCount);
				Buffer.BlockCopy(array, 0, bass_TAG_DSD_COMMENT._commentText, 0, bass_TAG_DSD_COMMENT._commentCount);
			}
			return bass_TAG_DSD_COMMENT;
		}

		public short TimeStampYear;

		public byte TimeStampMonth;

		public byte TimeStampDay;

		public byte TimeStampHour;

		public byte TimeStampMinutes;

		public short CommentType;

		public short CommentRef;

		private int _commentCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		private byte[] _commentText;
	}
}
