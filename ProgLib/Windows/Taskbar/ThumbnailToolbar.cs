using System;
using System.Windows.Forms;

namespace ProgLib.Windows.Taskbar
{
	internal class ThumbnailToolbar : NativeWindow
	{
		public ThumbnailToolbar(IntPtr Handle, ThumbnailButton[] buttons)
		{
			this.buttons = buttons;
			base.AssignHandle(Handle);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 273 && NativeMethods.GetHiWord(m.WParam.ToInt64(), 16) == 6144)
			{
				Int32 loWord = NativeMethods.GetLoWord(m.WParam.ToInt64());
				foreach (ThumbnailButton thumbnailButton in this.buttons)
				{
					if (thumbnailButton.Id == loWord)
						thumbnailButton.FireClickEvent();
				}
			}

			base.WndProc(ref m);
		}

		private ThumbnailButton[] buttons;
	}
}
