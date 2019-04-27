using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	public sealed class BASS_PLUGININFO
	{
		private BASS_PLUGININFO()
		{
		}

		internal BASS_PLUGININFO(int Version, BASS_PLUGINFORM[] Formats)
		{
			this.version = Version;
			this.formatc = Formats.Length;
			this.formats = Formats;
		}

		internal BASS_PLUGININFO(IntPtr pluginInfoPtr)
		{
			if (pluginInfoPtr == IntPtr.Zero)
			{
				return;
			}
			bass_plugininfo bass_plugininfo = (bass_plugininfo)Marshal.PtrToStructure(pluginInfoPtr, typeof(bass_plugininfo));
			this.version = bass_plugininfo.version;
			this.formatc = bass_plugininfo.formatc;
			this.formats = new BASS_PLUGINFORM[this.formatc];
			this.ReadArrayStructure(this.formatc, bass_plugininfo.formats);
		}

		internal BASS_PLUGININFO(int ver, int count, IntPtr fPtr)
		{
			this.version = ver;
			this.formatc = count;
			if (fPtr == IntPtr.Zero)
			{
				return;
			}
			this.formats = new BASS_PLUGINFORM[count];
			this.ReadArrayStructure(this.formatc, fPtr);
		}

		private unsafe void ReadArrayStructure(int count, IntPtr p)
		{
			for (int i = 0; i < count; i++)
			{
				try
				{
					this.formats[i] = (BASS_PLUGINFORM)Marshal.PtrToStructure(p, typeof(BASS_PLUGINFORM));
					p = new IntPtr((void*)((byte*)p.ToPointer() + Marshal.SizeOf(this.formats[i])));
				}
				catch
				{
					break;
				}
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}", this.version, this.formatc);
		}

		public int version;

		public int formatc;

		public BASS_PLUGINFORM[] formats;
	}
}
