using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ProgLib.Windows.Taskbar
{
	public sealed class TaskbarHelper
	{
		private TaskbarHelper()
		{
			if (TaskbarHelper.taskbar == null)
			{
				TaskbarHelper.taskbar = (NativeMethods.ITaskbarList3)new TaskbarHelper.TaskbarInstance();
				TaskbarHelper.taskbar.HrInit();
			}
		}

		internal NativeMethods.ITaskbarList3 NativeInterface
		{
			get { return TaskbarHelper.taskbar; }
		}

		public void SetProgressValue(IntPtr Handle, double value, double max)
		{
			TaskbarHelper.taskbar.SetProgressValue(Handle, (ulong)value, (ulong)max);
		}

		public void SetProgressValue(double value, double max)
		{
			this.SetProgressValue(this.MainHandle, value, max);
		}

		public void SetProgressState(IntPtr Handle, TBPFLAG state)
		{
			TaskbarHelper.taskbar.SetProgressState(Handle, state);
		}

		public void SetProgressState(TBPFLAG state)
		{
			this.SetProgressState(this.MainHandle, state);
		}

		public void SetOverlayIcon(IntPtr Handle, Icon icon, string description)
		{
			TaskbarHelper.taskbar.SetOverlayIcon(Handle, icon.Handle, description);
		}

		public void SetOverlayIcon(Icon icon, string description)
		{
			this.SetOverlayIcon(this.MainHandle, icon, description);
		}

		public void AddThumbnailButtons(IntPtr Handle, params ThumbnailButton[] buttons)
		{
			if (!this.buttonsAdded)
			{
				this.buttonsAdded = true;
				if (buttons == null || buttons.Length == 0)
				{
					throw new ArgumentException("Массив кнопок пуст", "buttons");
				}
				if (buttons.Length > 7)
				{
					throw new ArgumentException("Количество кнопок достигает предела 7 кнопок", "buttons");
				}
				List<NativeMethods.THUMBBUTTON> list = new List<NativeMethods.THUMBBUTTON>();
				ThumbnailToolbar thumbnailToolbar = new ThumbnailToolbar(Handle, buttons);
				foreach (ThumbnailButton thumbnailButton in buttons)
				{
					thumbnailButton.ParentHandle = thumbnailToolbar.Handle;
					list.Add(thumbnailButton.NativeButton);
				}
				NativeMethods.THUMBBUTTON[] array = list.ToArray();
				TaskbarHelper.taskbar.ThumbBarAddButtons(Handle, array.Length, array);
			}
		}

		public static TaskbarHelper Instance
		{
			get
			{
				if (!TaskbarHelper.isSupported)
				{
					throw new UnsupportedWindowsException("Windows 7");
				}
				return new TaskbarHelper();
			}
		}

		public static bool isSupported
		{
			get
			{
				return Environment.OSVersion.Version >= new Version(6, 1);
			}
		}

		private IntPtr MainHandle
		{
			get
			{
				if (this._MainHandle == IntPtr.Zero)
				{
					Process currentProcess = Process.GetCurrentProcess();
					if (currentProcess == null || currentProcess.MainWindowHandle == IntPtr.Zero)
					{
						throw new InvalidOperationException("Главное допустимое окно обязательно");
					}
					this._MainHandle = currentProcess.MainWindowHandle;
				}
				return this._MainHandle;
			}
		}

		private static NativeMethods.ITaskbarList3 taskbar;

		private IntPtr _MainHandle;

		private bool buttonsAdded;

		[Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
		[ClassInterface(ClassInterfaceType.None)]
		//[ComImport]
		private class TaskbarInstance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern TaskbarInstance();
		}
	}
}
