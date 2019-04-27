using System;
using System.ComponentModel;
using System.Security;
using System.Threading;

namespace Un4seen.Bass
{
	[SuppressUnmanagedCodeSecurity]
	public sealed class BASSTimer : IDisposable
	{
		public BASSTimer()
		{
			this._timerDelegate = new TimerCallback(this.timer_Tick);
		}

		public BASSTimer(int interval)
		{
			this._interval = interval;
			this._timerDelegate = new TimerCallback(this.timer_Tick);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					this.Stop();
				}
				catch
				{
				}
			}
			this.disposed = true;
		}

		~BASSTimer()
		{
			this.Dispose(false);
		}

		private void timer_Tick(object state)
		{
			if (this.Tick != null)
			{
				this.ProcessDelegate(this.Tick, new object[]
				{
					this,
					EventArgs.Empty
				});
			}
		}

		private void ProcessDelegate(Delegate del, params object[] args)
		{
			if (del == null || this._timer == null)
			{
				return;
			}
			Timer timer = this._timer;
			lock (timer)
			{
				foreach (Delegate del2 in del.GetInvocationList())
				{
					this.InvokeDelegate(del2, args);
				}
			}
		}

		private void InvokeDelegate(Delegate del, object[] args)
		{
			ISynchronizeInvoke synchronizeInvoke = del.Target as ISynchronizeInvoke;
			if (synchronizeInvoke != null)
			{
				if (!synchronizeInvoke.InvokeRequired)
				{
					del.DynamicInvoke(args);
					return;
				}
				try
				{
					synchronizeInvoke.BeginInvoke(del, args);
					return;
				}
				catch
				{
					return;
				}
			}
			del.DynamicInvoke(args);
		}

		public event EventHandler Tick;

		public int Interval
		{
			get
			{
				return this._interval;
			}
			set
			{
				if (value <= 0)
				{
					this._interval = -1;
				}
				else
				{
					this._interval = value;
				}
				if (this.Enabled)
				{
					Timer timer = this._timer;
					lock (timer)
					{
						this._timer.Change(this._interval, this._interval);
					}
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return this._enabled && this._timer != null;
			}
			set
			{
				if (value == this._enabled)
				{
					return;
				}
				if (value)
				{
					if (this._timer != null)
					{
						Timer timer = this._timer;
						lock (timer)
						{
							this._timer.Change(this._interval, this._interval);
							this._enabled = true;
							return;
						}
					}
					this.Start();
					return;
				}
				if (this._timer != null)
				{
					Timer timer = this._timer;
					lock (timer)
					{
						this._timer.Change(-1, -1);
						this._enabled = false;
						return;
					}
				}
				this.Stop();
			}
		}

		public void Start()
		{
			if (this._timer == null)
			{
				this._timer = new Timer(this._timerDelegate, null, this._interval, this._interval);
				this._enabled = true;
				return;
			}
			Timer timer = this._timer;
			lock (timer)
			{
				this._timer.Change(this._interval, this._interval);
				this._enabled = true;
			}
		}

		public void Stop()
		{
			if (this._timer != null)
			{
				Timer timer = this._timer;
				lock (timer)
				{
					this._timer.Change(-1, -1);
					this._timer.Dispose();
					this._enabled = false;
				}
				this._timer = null;
				return;
			}
			this._enabled = false;
		}

		private bool disposed;

		private Timer _timer;

		private int _interval = 50;

		private TimerCallback _timerDelegate;

		private bool _enabled;
	}
}
