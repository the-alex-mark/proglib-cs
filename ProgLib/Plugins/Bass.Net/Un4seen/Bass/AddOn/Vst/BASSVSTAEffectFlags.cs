using System;

namespace Un4seen.Bass.AddOn.Vst
{
	[Flags]
	public enum BASSVSTAEffectFlags
	{
		effFlagsHasEditor = 1,
		effFlagsHasClip = 2,
		effFlagsHasVu = 4,
		effFlagsCanMono = 8,
		effFlagsCanReplacing = 16,
		effFlagsProgramChunks = 32,
		effFlagsIsSynth = 256,
		effFlagsNoSoundInStop = 512,
		effFlagsExtIsAsync = 1024,
		effFlagsExtHasBuffer = 2048,
		effFlagsCanDoubleReplacing = 4096
	}
}
