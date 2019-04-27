using System;
using System.Runtime.InteropServices;

namespace Un4seen.Bass.AddOn.Fx
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BASS_BFX_VOLUME_ENV : IDisposable
	{
		public BASS_BFX_VOLUME_ENV()
		{
		}

		public BASS_BFX_VOLUME_ENV(int nodeCount)
		{
			this.lNodeCount = nodeCount;
			this.pNodes = new BASS_BFX_ENV_NODE[this.lNodeCount];
			for (int i = 0; i < this.lNodeCount; i++)
			{
				this.pNodes[i].pos = 0.0;
				this.pNodes[i].val = 1f;
			}
		}

		public BASS_BFX_VOLUME_ENV(params BASS_BFX_ENV_NODE[] nodes)
		{
			if (nodes == null)
			{
				this.lNodeCount = 0;
				this.pNodes = null;
				return;
			}
			this.lNodeCount = nodes.Length;
			this.pNodes = new BASS_BFX_ENV_NODE[this.lNodeCount];
			for (int i = 0; i < this.lNodeCount; i++)
			{
				this.pNodes[i].pos = nodes[i].pos;
				this.pNodes[i].val = nodes[i].val;
			}
		}

		~BASS_BFX_VOLUME_ENV()
		{
			this.Dispose();
		}

		internal void Set()
		{
			if (this.lNodeCount > 0)
			{
				if (this.hgc.IsAllocated)
				{
					this.hgc.Free();
					this.ptr = IntPtr.Zero;
				}
				this.hgc = GCHandle.Alloc(this.pNodes, GCHandleType.Pinned);
				this.ptr = this.hgc.AddrOfPinnedObject();
				return;
			}
			if (this.hgc.IsAllocated)
			{
				this.hgc.Free();
			}
			this.ptr = IntPtr.Zero;
		}

		internal void Get()
		{
			if (this.ptr != IntPtr.Zero && this.lNodeCount > 0)
			{
				this.pNodes = new BASS_BFX_ENV_NODE[this.lNodeCount];
				this.ReadArrayStructure(this.lNodeCount, this.ptr);
				return;
			}
			this.pNodes = null;
		}

		private unsafe void ReadArrayStructure(int count, IntPtr p)
		{
			for (int i = 0; i < count; i++)
			{
				this.pNodes[i] = (BASS_BFX_ENV_NODE)Marshal.PtrToStructure(p, typeof(BASS_BFX_ENV_NODE));
				p = new IntPtr((void*)((byte*)p.ToPointer() + Marshal.SizeOf(this.pNodes[i])));
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

		public BASSFXChan lChannel = BASSFXChan.BASS_BFX_CHANALL;

		public int lNodeCount;

		private IntPtr ptr = IntPtr.Zero;

		[MarshalAs(UnmanagedType.Bool)]
		public bool bFollow = true;

		private GCHandle hgc;

		public BASS_BFX_ENV_NODE[] pNodes;
	}
}
