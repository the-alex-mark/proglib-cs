using System;

namespace Un4seen.Bass.AddOn.Sfx
{
	[Serializable]
	public sealed class BASS_SFX_PLUGININFO
	{
		public override string ToString()
		{
			return this.name;
		}

		internal BASS_SFX_PLUGININFO_INTERNAL _internal;

		public string name = string.Empty;

		public string clsid = string.Empty;
	}
}
