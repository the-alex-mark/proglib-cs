﻿using System;

namespace Un4seen.Bass
{
	public enum BASSStreamFilePosition
	{
		BASS_FILEPOS_CURRENT,
		BASS_FILEPOS_DOWNLOAD,
		BASS_FILEPOS_END,
		BASS_FILEPOS_START,
		BASS_FILEPOS_CONNECTED,
		BASS_FILEPOS_BUFFER,
		BASS_FILEPOS_SOCKET,
		BASS_FILEPOS_ASYNCBUF,
		BASS_FILEPOS_WMA_BUFFER = 1000,
		BASS_FILEPOS_HLS_SEGMENT = 65536
	}
}