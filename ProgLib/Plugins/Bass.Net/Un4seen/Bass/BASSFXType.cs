using System;

namespace Un4seen.Bass
{
	public enum BASSFXType
	{
		BASS_FX_DX8_CHORUS,
		BASS_FX_DX8_COMPRESSOR,
		BASS_FX_DX8_DISTORTION,
		BASS_FX_DX8_ECHO,
		BASS_FX_DX8_FLANGER,
		BASS_FX_DX8_GARGLE,
		BASS_FX_DX8_I3DL2REVERB,
		BASS_FX_DX8_PARAMEQ,
		BASS_FX_DX8_REVERB,
		BASS_FX_BFX_ROTATE = 65536,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
		BASS_FX_BFX_ECHO,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_CHORUS instead")]
		BASS_FX_BFX_FLANGER,
		BASS_FX_BFX_VOLUME,
		BASS_FX_BFX_PEAKEQ,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_FREEVERB instead")]
		BASS_FX_BFX_REVERB,
		[Obsolete("This effect is obsolete; use 2x BASS_FX_BFX_BQF with BASS_BFX_BQF_LOWPASS filter and appropriate fQ values instead")]
		BASS_FX_BFX_LPF,
		BASS_FX_BFX_MIX,
		BASS_FX_BFX_DAMP,
		BASS_FX_BFX_AUTOWAH,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
		BASS_FX_BFX_ECHO2,
		BASS_FX_BFX_PHASER,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_ECHO4 instead")]
		BASS_FX_BFX_ECHO3,
		BASS_FX_BFX_CHORUS,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_BQF with BASS_BFX_BQF_ALLPASS filter instead")]
		BASS_FX_BFX_APF,
		[Obsolete("This effect is obsolete; use BASS_FX_BFX_COMPRESSOR2 instead")]
		BASS_FX_BFX_COMPRESSOR,
		BASS_FX_BFX_DISTORTION,
		BASS_FX_BFX_COMPRESSOR2,
		BASS_FX_BFX_VOLUME_ENV,
		BASS_FX_BFX_BQF,
		BASS_FX_BFX_ECHO4,
		BASS_FX_BFX_PITCHSHIFT,
		BASS_FX_BFX_FREEVERB
	}
}
