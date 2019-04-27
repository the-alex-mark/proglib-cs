using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public sealed class WAVEFORMATEXT
	{
		public WAVEFORMATEXT(int length)
		{
			this.waveformatex = new WAVEFORMATEX();
			this.waveformatex.cbSize = (short)(length - 18);
			if (this.waveformatex.cbSize >= 0)
			{
				this.extension = new byte[(int)this.waveformatex.cbSize];
			}
		}

		public unsafe WAVEFORMATEXT(IntPtr codec)
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

		public WAVEFORMATEX waveformatex;

		public byte[] extension;
	}
}
