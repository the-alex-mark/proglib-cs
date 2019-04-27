using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_MIX : IDisposable
	{
		public BASS_BFX_MIX(int numChans)
		{
			this.lChannel = new BASSFXChan[numChans];
			for (int i = 0; i < numChans; i++)
			{
				this.lChannel[i] = (BASSFXChan)(1 << i);
			}
		}

		public BASS_BFX_MIX(params BASSFXChan[] channels)
		{
			this.lChannel = new BASSFXChan[channels.Length];
			for (int i = 0; i < channels.Length; i++)
			{
				this.lChannel[i] = channels[i];
			}
		}

		~BASS_BFX_MIX()
		{
			this.Dispose();
		}

		internal void Set()
		{
			if (this.hgc.IsAllocated)
			{
				this.hgc.Free();
				this.ptr = IntPtr.Zero;
			}
			int[] array = new int[this.lChannel.Length];
			for (int i = 0; i < this.lChannel.Length; i++)
			{
				array[i] = (int)this.lChannel[i];
			}
			this.hgc = GCHandle.Alloc(array, GCHandleType.Pinned);
			this.ptr = this.hgc.AddrOfPinnedObject();
		}

		internal void Get()
		{
			if (this.ptr != IntPtr.Zero)
			{
				int[] array = new int[this.lChannel.Length];
				Marshal.Copy(this.ptr, array, 0, array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					this.lChannel[i] = (BASSFXChan)array[i];
				}
			}
		}

		public void Dispose()
		{
			if (this.hgc.IsAllocated)
			{
				this.hgc.Free();
				this.ptr = IntPtr.Zero;
			}
		}

		private IntPtr ptr = IntPtr.Zero;

		private GCHandle hgc;

		public BASSFXChan[] lChannel;
	}
}
