using System;

namespace Un4seen.Bass.Misc
{
	public class BroadCastEventArgs : EventArgs
	{
		public BroadCastEventArgs(BroadCastEventType pEventType, object pData)
		{
			this._eventType = pEventType;
			this._data = pData;
			this._now = DateTime.Now;
		}

		public BroadCastEventType EventType
		{
			get
			{
				return this._eventType;
			}
		}

		public object Data
		{
			get
			{
				return this._data;
			}
		}

		public DateTime DateTime
		{
			get
			{
				return this._now;
			}
		}

		private readonly BroadCastEventType _eventType;

		private readonly object _data;

		private readonly DateTime _now;
	}
}
