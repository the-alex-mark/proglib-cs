using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_FILEPROCS
	{
		public BASS_FILEPROCS(FILECLOSEPROC closeCallback, FILELENPROC lengthCallback, FILEREADPROC readCallback, FILESEEKPROC seekCallback)
		{
			this.close = closeCallback;
			this.length = lengthCallback;
			this.read = readCallback;
			this.seek = seekCallback;
		}

		public FILECLOSEPROC close;

		public FILELENPROC length;

		public FILEREADPROC read;

		public FILESEEKPROC seek;
	}
}
