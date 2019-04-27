using System;

namespace ProgLib.Windows.Taskbar
{
	public enum TBPFLAG
	{
		TBPF_NOPROGRESS,
		TBPF_INDETERMINATE,
		TBPF_NORMAL,
		TBPF_ERROR = 4,
		TBPF_PAUSED = 8
	}
}
