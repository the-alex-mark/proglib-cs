using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ProgLib.Animation.Metro
{
    public delegate void AnimationAction();
    public delegate Boolean AnimationFinishedEvaluator();
    public enum TransitionType
    {
        Linear,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuart,
        EaseInExpo,
        EaseOutExpo,
        NotApplicable
    }

    internal class DelayedCall : IDisposable
    {
        public delegate void Callback();

        protected static List<DelayedCall> dcList;

        protected System.Timers.Timer timer;

        protected object timerLock;

        private DelayedCall.Callback callback;

        protected bool cancelled;

        protected SynchronizationContext context;

        private DelayedCall<object>.Callback oldCallback;

        private object oldData;

        public static int RegisteredCount
        {
            get
            {
                int count;
                lock (DelayedCall.dcList)
                {
                    count = DelayedCall.dcList.Count;
                }
                return count;
            }
        }

        public static bool IsAnyWaiting
        {
            get
            {
                lock (DelayedCall.dcList)
                {
                    foreach (DelayedCall current in DelayedCall.dcList)
                    {
                        if (current.IsWaiting)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public bool IsWaiting
        {
            get
            {
                bool result;
                lock (this.timerLock)
                {
                    result = (this.timer.Enabled && !this.cancelled);
                }
                return result;
            }
        }

        public int Milliseconds
        {
            get
            {
                int result;
                lock (this.timerLock)
                {
                    result = (int)this.timer.Interval;
                }
                return result;
            }
            set
            {
                lock (this.timerLock)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException("Milliseconds", "The new timeout must be 0 or greater.");
                    }
                    if (value == 0)
                    {
                        this.Cancel();
                        this.FireNow();
                        DelayedCall.Unregister(this);
                    }
                    else
                    {
                        this.timer.Interval = (double)value;
                    }
                }
            }
        }

        static DelayedCall()
        {
            DelayedCall.dcList = new List<DelayedCall>();
        }

        protected DelayedCall()
        {
            this.timerLock = new object();
        }

        [Obsolete("Use the static method DelayedCall.Create instead.")]
        public DelayedCall(DelayedCall.Callback cb) : this()
        {
            DelayedCall.PrepareDCObject(this, 0, false);
            this.callback = cb;
        }

        [Obsolete("Use the static method DelayedCall.Create instead.")]
        public DelayedCall(DelayedCall<object>.Callback cb, object data) : this()
        {
            DelayedCall.PrepareDCObject(this, 0, false);
            this.oldCallback = cb;
            this.oldData = data;
        }

        [Obsolete("Use the static method DelayedCall.Start instead.")]
        public DelayedCall(DelayedCall.Callback cb, int milliseconds) : this()
        {
            DelayedCall.PrepareDCObject(this, milliseconds, false);
            this.callback = cb;
            if (milliseconds > 0)
            {
                this.Start();
            }
        }

        [Obsolete("Use the static method DelayedCall.Start instead.")]
        public DelayedCall(DelayedCall<object>.Callback cb, int milliseconds, object data) : this()
        {
            DelayedCall.PrepareDCObject(this, milliseconds, false);
            this.oldCallback = cb;
            this.oldData = data;
            if (milliseconds > 0)
            {
                this.Start();
            }
        }

        [Obsolete("Use the method Restart of the generic class instead.")]
        public void Reset(object data)
        {
            this.Cancel();
            this.oldData = data;
            this.Start();
        }

        [Obsolete("Use the method Restart of the generic class instead.")]
        public void Reset(int milliseconds, object data)
        {
            this.Cancel();
            this.oldData = data;
            this.Reset(milliseconds);
        }

        [Obsolete("Use the method Restart instead.")]
        public void SetTimeout(int milliseconds)
        {
            this.Reset(milliseconds);
        }

        public static DelayedCall Create(DelayedCall.Callback cb, int milliseconds)
        {
            DelayedCall delayedCall = new DelayedCall();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, false);
            delayedCall.callback = cb;
            return delayedCall;
        }

        public static DelayedCall CreateAsync(DelayedCall.Callback cb, int milliseconds)
        {
            DelayedCall delayedCall = new DelayedCall();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, true);
            delayedCall.callback = cb;
            return delayedCall;
        }

        public static DelayedCall Start(DelayedCall.Callback cb, int milliseconds)
        {
            DelayedCall delayedCall = DelayedCall.Create(cb, milliseconds);
            if (milliseconds > 0)
            {
                delayedCall.Start();
            }
            else if (milliseconds == 0)
            {
                delayedCall.FireNow();
            }
            return delayedCall;
        }

        public static DelayedCall StartAsync(DelayedCall.Callback cb, int milliseconds)
        {
            DelayedCall delayedCall = DelayedCall.CreateAsync(cb, milliseconds);
            if (milliseconds > 0)
            {
                delayedCall.Start();
            }
            else if (milliseconds == 0)
            {
                delayedCall.FireNow();
            }
            return delayedCall;
        }

        protected static void PrepareDCObject(DelayedCall dc, int milliseconds, bool async)
        {
            if (milliseconds < 0)
            {
                throw new ArgumentOutOfRangeException("milliseconds", "The new timeout must be 0 or greater.");
            }
            dc.context = null;
            if (!async)
            {
                dc.context = SynchronizationContext.Current;
                if (dc.context == null)
                {
                    throw new InvalidOperationException("Cannot delay calls synchronously on a non-UI thread. Use the *Async methods instead.");
                }
            }
            if (dc.context == null)
            {
                dc.context = new SynchronizationContext();
            }
            dc.timer = new System.Timers.Timer();
            if (milliseconds > 0)
            {
                dc.timer.Interval = (double)milliseconds;
            }
            dc.timer.AutoReset = false;
            dc.timer.Elapsed += new ElapsedEventHandler(dc.Timer_Elapsed);
            DelayedCall.Register(dc);
        }

        protected static void Register(DelayedCall dc)
        {
            lock (DelayedCall.dcList)
            {
                if (!DelayedCall.dcList.Contains(dc))
                {
                    DelayedCall.dcList.Add(dc);
                }
            }
        }

        protected static void Unregister(DelayedCall dc)
        {
            lock (DelayedCall.dcList)
            {
                DelayedCall.dcList.Remove(dc);
            }
        }

        public static void CancelAll()
        {
            lock (DelayedCall.dcList)
            {
                foreach (DelayedCall current in DelayedCall.dcList)
                {
                    current.Cancel();
                }
            }
        }

        public static void FireAll()
        {
            lock (DelayedCall.dcList)
            {
                foreach (DelayedCall current in DelayedCall.dcList)
                {
                    current.Fire();
                }
            }
        }

        public static void DisposeAll()
        {
            lock (DelayedCall.dcList)
            {
                while (DelayedCall.dcList.Count > 0)
                {
                    DelayedCall.dcList[0].Dispose();
                }
            }
        }

        protected virtual void Timer_Elapsed(object o, ElapsedEventArgs e)
        {
            this.FireNow();
            DelayedCall.Unregister(this);
        }

        public void Dispose()
        {
            DelayedCall.Unregister(this);
            this.timer.Dispose();
        }

        public void Start()
        {
            lock (this.timerLock)
            {
                this.cancelled = false;
                this.timer.Start();
                DelayedCall.Register(this);
            }
        }

        public void Cancel()
        {
            lock (this.timerLock)
            {
                this.cancelled = true;
                DelayedCall.Unregister(this);
                this.timer.Stop();
            }
        }

        public void Fire()
        {
            lock (this.timerLock)
            {
                if (!this.IsWaiting)
                {
                    return;
                }
                this.timer.Stop();
            }
            this.FireNow();
        }

        public void FireNow()
        {
            this.OnFire();
            DelayedCall.Unregister(this);
        }

        protected virtual void OnFire()
        {
            this.context.Post(delegate (object param0)
            {
                lock (this.timerLock)
                {
                    if (this.cancelled)
                    {
                        return;
                    }
                }
                if (this.callback != null)
                {
                    this.callback();
                }
                if (this.oldCallback != null)
                {
                    this.oldCallback(this.oldData);
                }
            }, null);
        }

        public void Reset()
        {
            lock (this.timerLock)
            {
                this.Cancel();
                this.Start();
            }
        }

        public void Reset(int milliseconds)
        {
            lock (this.timerLock)
            {
                this.Cancel();
                this.Milliseconds = milliseconds;
                this.Start();
            }
        }
    }
    internal class DelayedCall<T> : DelayedCall
    {
        public new delegate void Callback(T data);

        private DelayedCall<T>.Callback callback;

        private T data;

        public static DelayedCall<T> Create(DelayedCall<T>.Callback cb, T data, int milliseconds)
        {
            DelayedCall<T> delayedCall = new DelayedCall<T>();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, false);
            delayedCall.callback = cb;
            delayedCall.data = data;
            return delayedCall;
        }

        public static DelayedCall<T> CreateAsync(DelayedCall<T>.Callback cb, T data, int milliseconds)
        {
            DelayedCall<T> delayedCall = new DelayedCall<T>();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, true);
            delayedCall.callback = cb;
            delayedCall.data = data;
            return delayedCall;
        }

        public static DelayedCall<T> Start(DelayedCall<T>.Callback cb, T data, int milliseconds)
        {
            DelayedCall<T> delayedCall = DelayedCall<T>.Create(cb, data, milliseconds);
            delayedCall.Start();
            return delayedCall;
        }

        public static DelayedCall<T> StartAsync(DelayedCall<T>.Callback cb, T data, int milliseconds)
        {
            DelayedCall<T> delayedCall = DelayedCall<T>.CreateAsync(cb, data, milliseconds);
            delayedCall.Start();
            return delayedCall;
        }

        protected override void OnFire()
        {
            this.context.Post(delegate (object param0)
            {
                lock (this.timerLock)
                {
                    if (this.cancelled)
                    {
                        return;
                    }
                }
                if (this.callback != null)
                {
                    this.callback(this.data);
                }
            }, null);
        }

        public void Reset(T data, int milliseconds)
        {
            lock (this.timerLock)
            {
                base.Cancel();
                this.data = data;
                base.Milliseconds = milliseconds;
                base.Start();
            }
        }
    }
    internal class DelayedCall<T1, T2> : DelayedCall
    {
        public new delegate void Callback(T1 data1, T2 data2);

        private DelayedCall<T1, T2>.Callback callback;

        private T1 data1;

        private T2 data2;

        public static DelayedCall<T1, T2> Create(DelayedCall<T1, T2>.Callback cb, T1 data1, T2 data2, int milliseconds)
        {
            DelayedCall<T1, T2> delayedCall = new DelayedCall<T1, T2>();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, false);
            delayedCall.callback = cb;
            delayedCall.data1 = data1;
            delayedCall.data2 = data2;
            return delayedCall;
        }

        public static DelayedCall<T1, T2> CreateAsync(DelayedCall<T1, T2>.Callback cb, T1 data1, T2 data2, int milliseconds)
        {
            DelayedCall<T1, T2> delayedCall = new DelayedCall<T1, T2>();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, true);
            delayedCall.callback = cb;
            delayedCall.data1 = data1;
            delayedCall.data2 = data2;
            return delayedCall;
        }

        public static DelayedCall<T1, T2> Start(DelayedCall<T1, T2>.Callback cb, T1 data1, T2 data2, int milliseconds)
        {
            DelayedCall<T1, T2> delayedCall = DelayedCall<T1, T2>.Create(cb, data1, data2, milliseconds);
            delayedCall.Start();
            return delayedCall;
        }

        public static DelayedCall<T1, T2> StartAsync(DelayedCall<T1, T2>.Callback cb, T1 data1, T2 data2, int milliseconds)
        {
            DelayedCall<T1, T2> delayedCall = DelayedCall<T1, T2>.CreateAsync(cb, data1, data2, milliseconds);
            delayedCall.Start();
            return delayedCall;
        }

        protected override void OnFire()
        {
            this.context.Post(delegate (object param0)
            {
                lock (this.timerLock)
                {
                    if (this.cancelled)
                    {
                        return;
                    }
                }
                if (this.callback != null)
                {
                    this.callback(this.data1, this.data2);
                }
            }, null);
        }

        public void Reset(T1 data1, T2 data2, int milliseconds)
        {
            lock (this.timerLock)
            {
                base.Cancel();
                this.data1 = data1;
                this.data2 = data2;
                base.Milliseconds = milliseconds;
                base.Start();
            }
        }
    }
    internal class DelayedCall<T1, T2, T3> : DelayedCall
    {
        public new delegate void Callback(T1 data1, T2 data2, T3 data3);

        private DelayedCall<T1, T2, T3>.Callback callback;

        private T1 data1;

        private T2 data2;

        private T3 data3;

        public static DelayedCall<T1, T2, T3> Create(DelayedCall<T1, T2, T3>.Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
        {
            DelayedCall<T1, T2, T3> delayedCall = new DelayedCall<T1, T2, T3>();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, false);
            delayedCall.callback = cb;
            delayedCall.data1 = data1;
            delayedCall.data2 = data2;
            delayedCall.data3 = data3;
            return delayedCall;
        }

        public static DelayedCall<T1, T2, T3> CreateAsync(DelayedCall<T1, T2, T3>.Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
        {
            DelayedCall<T1, T2, T3> delayedCall = new DelayedCall<T1, T2, T3>();
            DelayedCall.PrepareDCObject(delayedCall, milliseconds, true);
            delayedCall.callback = cb;
            delayedCall.data1 = data1;
            delayedCall.data2 = data2;
            delayedCall.data3 = data3;
            return delayedCall;
        }

        public static DelayedCall<T1, T2, T3> Start(DelayedCall<T1, T2, T3>.Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
        {
            DelayedCall<T1, T2, T3> delayedCall = DelayedCall<T1, T2, T3>.Create(cb, data1, data2, data3, milliseconds);
            delayedCall.Start();
            return delayedCall;
        }

        public static DelayedCall<T1, T2, T3> StartAsync(DelayedCall<T1, T2, T3>.Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
        {
            DelayedCall<T1, T2, T3> delayedCall = DelayedCall<T1, T2, T3>.CreateAsync(cb, data1, data2, data3, milliseconds);
            delayedCall.Start();
            return delayedCall;
        }

        protected override void OnFire()
        {
            this.context.Post(delegate (object param0)
            {
                lock (this.timerLock)
                {
                    if (this.cancelled)
                    {
                        return;
                    }
                }
                if (this.callback != null)
                {
                    this.callback(this.data1, this.data2, this.data3);
                }
            }, null);
        }

        public void Reset(T1 data1, T2 data2, T3 data3, int milliseconds)
        {
            lock (this.timerLock)
            {
                base.Cancel();
                this.data1 = data1;
                this.data2 = data2;
                this.data3 = data3;
                base.Milliseconds = milliseconds;
                base.Start();
            }
        }
    }
}
