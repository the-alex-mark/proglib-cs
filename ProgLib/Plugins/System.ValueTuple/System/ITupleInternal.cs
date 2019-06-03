using System;
using System.Collections;

namespace System
{
	/// <summary>
	/// Helper so we can call some tuple methods recursively without knowing the underlying types.
	/// </summary>
	internal interface ITupleInternal
	{
		int GetHashCode(IEqualityComparer comparer);

		int Size { get; }

		string ToStringEnd();
	}
}
