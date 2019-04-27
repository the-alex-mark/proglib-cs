using System;

namespace Un4seen.BassAsio
{
	public enum BASSASIOFuture
	{
		BASS_ASIO_FUTURE_EnableTimeCodeRead = 1,
		BASS_ASIO_FUTURE_DisableTimeCodeRead,
		BASS_ASIO_FUTURE_SetInputMonitor,
		BASS_ASIO_FUTURE_Transport,
		BASS_ASIO_FUTURE_SetInputGain,
		BASS_ASIO_FUTURE_GetInputMeter,
		BASS_ASIO_FUTURE_SetOutputGain,
		BASS_ASIO_FUTURE_GetOutputMeter,
		BASS_ASIO_FUTURE_CanInputMonitor,
		BASS_ASIO_FUTURE_CanTimeInfo,
		BASS_ASIO_FUTURE_CanTimeCode,
		BASS_ASIO_FUTURE_CanTransport,
		BASS_ASIO_FUTURE_CanInputGain,
		BASS_ASIO_FUTURE_CanInputMeter,
		BASS_ASIO_FUTURE_CanOutputGain,
		BASS_ASIO_FUTURE_CanOutputMeter,
		BASS_ASIO_FUTURE_SetIoFormat = 588323169,
		BASS_ASIO_FUTURE_GetIoFormat = 588323203,
		BASS_ASIO_FUTURE_CanDoIoFormat = 588324868
	}
}
