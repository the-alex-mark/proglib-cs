using System;
using Un4seen.Bass.AddOn.Tags;

namespace Un4seen.Bass.Misc
{
	public interface IStreamingServer
	{
		bool UseBASS { get; }

		bool IsConnected { get; }

		IBaseEncoder Encoder { get; }

		StreamingServer.STREAMINGERROR LastError { get; set; }

		string LastErrorMessage { get; set; }

		string SongTitle { get; set; }

		string SongUrl { get; set; }

		bool ForceUTF8TitleUpdates { get; set; }

		bool Connect();

		bool Disconnect();

		bool Login();

		int SendData(IntPtr buffer, int length);

		bool UpdateTitle(string song, string url);

		bool UpdateTitle(TAG_INFO tag, string url);

		int GetListeners(string password);

		string GetStats(string password);
	}
}
