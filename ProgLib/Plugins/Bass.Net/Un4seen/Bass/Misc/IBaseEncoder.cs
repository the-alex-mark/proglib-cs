using System;
using Un4seen.Bass.AddOn.Enc;
using Un4seen.Bass.AddOn.Tags;

namespace Un4seen.Bass.Misc
{
	public interface IBaseEncoder
	{
		int ChannelHandle { get; set; }

		int ChannelBitwidth { get; }

		int ChannelSampleRate { get; }

		int ChannelNumChans { get; }

		BASSChannelType EncoderType { get; }

		string DefaultOutputExtension { get; }

		bool SupportsSTDOUT { get; }

		int EncoderHandle { get; set; }

		bool IsActive { get; }

		bool IsPaused { get; }

		string EncoderCommandLine { get; }

		string EncoderDirectory { get; set; }

		bool EncoderExists { get; }

		string InputFile { get; set; }

		bool Force16Bit { get; set; }

		bool NoLimit { get; set; }

		bool UseAsyncQueue { get; set; }

		string OutputFile { get; set; }

		int EffectiveBitrate { get; }

		TAG_INFO TAGs { get; set; }

		bool Start(ENCODEPROC proc, IntPtr user, bool paused);

		bool Stop();

		bool Pause(bool paused);
	}
}
