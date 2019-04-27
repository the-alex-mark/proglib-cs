using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Un4seen.Bass.AddOn.Enc
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public sealed class ACMFORMAT
	{
		public ACMFORMAT()
		{
			int num = BassEnc.BASS_Encode_GetACMFormat(0, IntPtr.Zero, 0, null, BASSACMFormat.BASS_ACM_NONE);
			this.waveformatex = new WAVEFORMATEX();
			this.waveformatex.cbSize = (short)(num - 18);
			if (this.waveformatex.cbSize >= 0)
			{
				this.extension = new byte[(int)this.waveformatex.cbSize];
			}
		}

		public ACMFORMAT(int length)
		{
			this.waveformatex = new WAVEFORMATEX();
			this.waveformatex.cbSize = (short)(length - 18);
			if (this.waveformatex.cbSize >= 0)
			{
				this.extension = new byte[(int)this.waveformatex.cbSize];
			}
		}

		public unsafe ACMFORMAT(IntPtr codec)
		{
			this.waveformatex = (WAVEFORMATEX)Marshal.PtrToStructure(codec, typeof(WAVEFORMATEX));
			this.extension = new byte[(int)this.waveformatex.cbSize];
			codec = new IntPtr((void*)((byte*)codec.ToPointer() + 18));
			Marshal.Copy(codec, this.extension, 0, (int)this.waveformatex.cbSize);
		}

		public override string ToString()
		{
			return this.waveformatex.ToString();
		}

		public int FormatLength
		{
			get
			{
				int num = 18;
				if (this.extension != null)
				{
					num += this.extension.Length;
				}
				return num;
			}
		}

		public static bool SaveToFile(ACMFORMAT form, string fileName)
		{
			if (form == null)
			{
				return false;
			}
			bool result = false;
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			Stream stream = null;
			try
			{
				stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
				binaryFormatter.Serialize(stream, form);
				result = true;
			}
			catch
			{
			}
			finally
			{
				if (stream != null)
				{
					stream.Flush();
					stream.Close();
				}
			}
			return result;
		}

		public static ACMFORMAT LoadFromFile(string fileName)
		{
			ACMFORMAT acmformat = null;
			IFormatter formatter = new BinaryFormatter();
			Stream stream = null;
			try
			{
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				bool flag = false;
				while (!flag)
				{
					try
					{
						acmformat = (formatter.Deserialize(stream) as ACMFORMAT);
						if (acmformat != null)
						{
							flag = true;
						}
					}
					catch
					{
						flag = true;
					}
				}
			}
			catch
			{
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
					stream.Dispose();
					stream = null;
				}
			}
			return acmformat;
		}

		public WAVEFORMATEX waveformatex;

		public byte[] extension;
	}
}
