using System;

namespace Un4seen.Bass.Misc
{
	public enum BroadCastEventType
	{
		Connected,
		Disconnected,
		DisconnectError,
		ConnectionLost,
		ConnectionError,
		ReconnectTry,
		UnsuccessfulReconnectTry,
		EncoderStarted,
		EncoderStartError,
		EncoderStopped,
		EncoderStopError,
		EncoderRestartRequired,
		DataSend,
		LessDataSend,
		IsAlive,
		TitleUpdated,
		TitleUpdateError,
		Reconnected
	}
}
