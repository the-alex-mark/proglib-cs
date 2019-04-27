using System;
using System.Diagnostics;
using System.Drawing;

namespace ProgLib.Windows.Taskbar
{
	public sealed class ThumbnailButton : IDisposable
	{
		public event EventHandler Click;

		public ThumbnailButton(Icon icon, string tip)
		{
			this.Id = ThumbnailButton.nextId;
			if (ThumbnailButton.nextId == int.MaxValue)
			{
				ThumbnailButton.nextId = 101;
			}
			else
			{
				ThumbnailButton.nextId++;
			}

			this.Icon = icon;
			this.Tip = tip;
			this.Enabled = true;
			this.nativeBtn = default(NativeMethods.THUMBBUTTON);
			this.BuildNativeButton();
			this.initialized = true;
		}

		internal IntPtr ParentHandle { get; set; }

		internal int Id { get; set; }

		internal NativeMethods.THUMBBUTTONFLAGS Flags { get; set; }

		internal NativeMethods.THUMBBUTTON NativeButton
		{
			get
			{
				return this.nativeBtn;
			}
		}

		internal void FireClickEvent()
		{
			EventHandler click = this.Click;
			if (click != null)
			{
				click(this, EventArgs.Empty);
			}
		}

		private void BuildNativeButton()
		{
			this.nativeBtn.iId = this.Id;
			this.nativeBtn.szTip = this.Tip;
			this.nativeBtn.hIcon = ((this.Icon != null) ? this.Icon.Handle : IntPtr.Zero);
			this.nativeBtn.dwFlags = this.Flags;
			this.nativeBtn.dwMask = NativeMethods.THUMBBUTTONMASK.THB_FLAGS;
			bool flag = this.Tip != null;
			if (flag)
			{
				this.nativeBtn.dwMask = (this.nativeBtn.dwMask | NativeMethods.THUMBBUTTONMASK.THB_TOOLTIP);
			}
			bool flag2 = this.Icon != null;
			if (flag2)
			{
				this.nativeBtn.dwMask = (this.nativeBtn.dwMask | NativeMethods.THUMBBUTTONMASK.THB_ICON);
			}
		}

		private void UpdateChanges()
		{
			bool flag;
			if (this.initialized)
			{
				IntPtr parentHandle = this.ParentHandle;
				flag = false;
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this.BuildNativeButton();
				TaskbarHelper.Instance.NativeInterface.ThumbBarUpdateButtons(this.ParentHandle, 1, new NativeMethods.THUMBBUTTON[]
				{
					this.nativeBtn
				});
			}
		}

		private bool IsFlagSet(NativeMethods.THUMBBUTTONFLAGS Flags, NativeMethods.THUMBBUTTONFLAGS Flag)
		{
			return (Flags & Flag) > NativeMethods.THUMBBUTTONFLAGS.THBF_ENABLED;
		}

		private void SetFlag(NativeMethods.THUMBBUTTONFLAGS Flags, NativeMethods.THUMBBUTTONFLAGS Flag, bool addOrRemove)
		{
			if (addOrRemove)
			{
				Flags |= Flag;
			}
			else
			{
				Flags &= ~Flag;
			}
		}

		public string Tip
		{
			get
			{
				return this._Tip;
			}
			set
			{
				bool flag = this._Tip != value;
				if (flag)
				{
					this._Tip = value;
					this.UpdateChanges();
				}
			}
		}

		public Icon Icon
		{
			get
			{
				return this._Icon;
			}
			set
			{
				bool flag = this._Icon != value;
				if (flag)
				{
					this._Icon = value;
					this.UpdateChanges();
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return !this.IsFlagSet(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_DISABLED);
			}
			set
			{
				bool flag = this._Enabled != value;
				if (flag)
				{
					this._Enabled = value;
					this.SetFlag(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_DISABLED, !value);
					this.UpdateChanges();
				}
			}
		}

		public bool DismissOnClick
		{
			get
			{
				return this.IsFlagSet(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_DISMISSONCLICK);
			}
			set
			{
				bool dismissOnClick = this._DismissOnClick;
				if (dismissOnClick)
				{
					this._DismissOnClick = value;
					this.SetFlag(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_DISMISSONCLICK, value);
					this.UpdateChanges();
				}
			}
		}

		public bool Visible
		{
			get
			{
				return !this.IsFlagSet(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_HIDDEN);
			}
			set
			{
				bool flag = this._Visible != value;
				if (flag)
				{
					this._Visible = value;
					this.SetFlag(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_HIDDEN, !value);
					this.UpdateChanges();
				}
			}
		}

		public bool IsInteractive
		{
			get
			{
				return !this.IsFlagSet(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_NONINTERACTIVE);
			}
			set
			{
				bool flag = this._IsInteractive != value;
				if (flag)
				{
					this._IsInteractive = value;
					this.SetFlag(this.Flags, NativeMethods.THUMBBUTTONFLAGS.THBF_NONINTERACTIVE, !value);
					this.UpdateChanges();
				}
			}
		}

		~ThumbnailButton()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Icon.Dispose();
				this.Tip = null;
			}
		}

		private NativeMethods.THUMBBUTTON nativeBtn;

		private bool initialized;

		private static int nextId = 101;

		private string _Tip;

		private Icon _Icon;

		private bool _Enabled;

		private bool _DismissOnClick;

		private bool _Visible;

		private bool _IsInteractive;
	}
}
