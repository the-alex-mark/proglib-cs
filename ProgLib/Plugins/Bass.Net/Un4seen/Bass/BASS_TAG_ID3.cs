using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BASS_TAG_ID3
	{
		public string ID
		{
			get
			{
				if (this.id == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(this.id, 0, this.id.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.id = new byte[64];
					return;
				}
				this.id = new byte[64];
				Encoding.Default.GetBytes(value, 0, (value.Length > 3) ? 3 : value.Length, this.id, 0);
			}
		}

		public string Title
		{
			get
			{
				if (this.title == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(this.title, 0, this.title.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.title = new byte[30];
					return;
				}
				this.title = new byte[30];
				Encoding.Default.GetBytes(value, 0, (value.Length > 30) ? 30 : value.Length, this.title, 0);
			}
		}

		public string Artist
		{
			get
			{
				if (this.artist == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(this.artist, 0, this.artist.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.artist = new byte[30];
					return;
				}
				this.artist = new byte[30];
				Encoding.Default.GetBytes(value, 0, (value.Length > 30) ? 30 : value.Length, this.artist, 0);
			}
		}

		public string Album
		{
			get
			{
				if (this.album == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(this.album, 0, this.album.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.album = new byte[30];
					return;
				}
				this.album = new byte[30];
				Encoding.Default.GetBytes(value, 0, (value.Length > 30) ? 30 : value.Length, this.album, 0);
			}
		}

		public string Year
		{
			get
			{
				if (this.year == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(this.year, 0, this.year.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.year = new byte[4];
					return;
				}
				this.year = new byte[4];
				Encoding.Default.GetBytes(value, 0, (value.Length > 4) ? 4 : value.Length, this.year, 0);
			}
		}

		public string Comment
		{
			get
			{
				if (this.comment == null)
				{
					return string.Empty;
				}
				string @string = Encoding.Default.GetString(this.comment, 0, this.comment.Length);
				int num = @string.IndexOf('\0');
				if (num >= 0)
				{
					return @string.Substring(0, num).TrimEnd(null);
				}
				return @string.TrimEnd(null);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.comment = new byte[28];
					return;
				}
				this.comment = new byte[28];
				Encoding.Default.GetBytes(value, 0, (value.Length > 28) ? 28 : value.Length, this.comment, 0);
			}
		}

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		private byte[] id;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		private byte[] title;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		private byte[] artist;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
		private byte[] album;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		private byte[] year;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
		private byte[] comment;

		internal byte Dummy;

		public byte Track;

		public byte Genre;
	}
}
