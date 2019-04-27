using System;

namespace Un4seen.BassWasapi
{
	[Serializable]
	public class BassWasapiHandlerEventArgs : EventArgs
	{
		public BassWasapiHandlerEventArgs(BassWasapiHandlerSyncType syncType, int data)
		{
			this._syncType = syncType;
		}

		public BassWasapiHandlerSyncType SyncType
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

		private readonly BassWasapiHandlerSyncType _syncType = BassWasapiHandlerSyncType.SourceResumed;

		private readonly int _data;
	}
}
