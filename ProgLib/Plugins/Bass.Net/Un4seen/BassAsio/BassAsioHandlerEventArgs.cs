using System;

namespace Un4seen.BassAsio
{
	[Serializable]
	public class BassAsioHandlerEventArgs : EventArgs
	{
		public BassAsioHandlerEventArgs(BassAsioHandlerSyncType syncType, int data)
		{
			this._syncType = syncType;
		}

		public BassAsioHandlerSyncType SyncType
		{
			get
			{
				return this._syncType;
			}
		}

		public int Data
		{
			get
			{
				return this._data;
			}
		}

		private readonly BassAsioHandlerSyncType _syncType = BassAsioHandlerSyncType.SourceResumed;

		private readonly int _data;
	}
}
