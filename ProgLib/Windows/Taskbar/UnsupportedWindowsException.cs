using System;

namespace ProgLib.Windows.Taskbar
{
	public class UnsupportedWindowsException : Exception
	{
		public UnsupportedWindowsException()
		{
		}

		public UnsupportedWindowsException(string os) : base("Это требует" + os + " или позже")
		{
		}
	}
}
