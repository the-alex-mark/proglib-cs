using System;
using System.Collections.Generic;
using System.IO;

namespace Un4seen.Bass.AddOn.WaDsp
{
	public sealed class WINAMP_DSP
	{
		public string File
		{
			get
			{
				return this._file;
			}
		}

		public string Description
		{
			get
			{
				return this._description;
			}
		}

		public int ModuleCount
		{
			get
			{
				return this._modulecount;
			}
		}

		public string[] ModuleNames
		{
			get
			{
				return this._modulenames;
			}
		}

		private WINAMP_DSP()
		{
		}

		public WINAMP_DSP(string fileName)
		{
			this._file = fileName;
			if (BassWaDsp.BASS_WADSP_PluginInfoLoad(this._file))
			{
				this._description = BassWaDsp.BASS_WADSP_PluginInfoGetName();
				this._modulecount = BassWaDsp.BASS_WADSP_PluginInfoGetModuleCount();
				this._modulenames = BassWaDsp.BASS_WADSP_PluginInfoGetModuleNames();
				BassWaDsp.BASS_WADSP_PluginInfoFree();
			}
		}

		public bool Load()
		{
			if (this.IsLoaded)
			{
				BassWaDsp.BASS_WADSP_FreeDSP(this._plugin);
			}
			this._plugin = BassWaDsp.BASS_WADSP_Load(this._file, 5, 5, 100, 100, null);
			return this._plugin != 0;
		}

		public bool IsLoaded
		{
			get
			{
				return this._plugin != 0;
			}
		}

		public bool Unload()
		{
			if (this.IsStarted)
			{
				this.Stop();
			}
			if (this.IsLoaded)
			{
				BassWaDsp.BASS_WADSP_FreeDSP(this._plugin);
				this._plugin = 0;
				return true;
			}
			return false;
		}

		public int Start(int module, int channel, int prio)
		{
			if (this.IsLoaded && module >= 0 && module < this._modulecount && channel != 0)
			{
				if (this.IsStarted)
				{
					this.Stop();
				}
				this._dsp = 0;
				this._module = -1;
				BassWaDsp.BASS_WADSP_Start(this._plugin, module, channel);
				this._module = module;
				this._dsp = BassWaDsp.BASS_WADSP_ChannelSetDSP(this._plugin, channel, prio);
				return this._dsp;
			}
			return 0;
		}

		public bool Stop()
		{
			if (!this.IsLoaded)
			{
				return false;
			}
			if (this.IsStarted && !BassWaDsp.BASS_WADSP_ChannelRemoveDSP(this._plugin))
			{
				return false;
			}
			if (BassWaDsp.BASS_WADSP_Stop(this._plugin))
			{
				this._dsp = 0;
				this._module = -1;
				return true;
			}
			return false;
		}

		public bool IsStarted
		{
			get
			{
				return this._dsp != 0;
			}
		}

		public int StartedModule
		{
			get
			{
				return this._module;
			}
		}

		public bool SetSongTitle(string title)
		{
			return this.IsLoaded && this.IsStarted && BassWaDsp.BASS_WADSP_SetSongTitle(this._plugin, title);
		}

		public bool SetFilename(string filename)
		{
			return this.IsLoaded && this.IsStarted && BassWaDsp.BASS_WADSP_SetFileName(this._plugin, filename);
		}

		public bool ShowEditor()
		{
			return this.IsLoaded && this._module >= 0 && BassWaDsp.BASS_WADSP_Config(this._plugin);
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", this._description, Path.GetFileNameWithoutExtension(this._file));
		}

		public static List<WINAMP_DSP> PlugIns
		{
			get
			{
				return WINAMP_DSP._plugins;
			}
		}

		public static void FindPlugins(string path)
		{
			foreach (string text in Directory.GetFiles(path, "dsp_*.dll"))
			{
				if (!WINAMP_DSP.containsPlugin(text))
				{
					WINAMP_DSP winamp_DSP = new WINAMP_DSP(text);
					if (winamp_DSP._modulecount > 0)
					{
						WINAMP_DSP._plugins.Add(winamp_DSP);
					}
				}
			}
		}

		private static bool containsPlugin(string file)
		{
			bool result = false;
			using (List<WINAMP_DSP>.Enumerator enumerator = WINAMP_DSP._plugins.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current._file.ToLower().Equals(file.ToLower()))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private string _file = string.Empty;

		private string _description = string.Empty;

		private int _modulecount;

		private string[] _modulenames;

		private int _plugin;

		private int _dsp;

		private int _module = -1;

		private static List<WINAMP_DSP> _plugins = new List<WINAMP_DSP>();
	}
}
