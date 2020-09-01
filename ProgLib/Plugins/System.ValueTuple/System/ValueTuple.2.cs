using System;
using System.Collections;
using System.Collections.Generic;

namespace System
{
	/// <summary>Represents a 1-tuple, or singleton, as a value type.</summary>
	/// <typeparam name="T1">The type of the tuple's only component.</typeparam>
	public struct ValueTuple<T1> : IEquatable<ValueTuple<T1>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1>>, ITupleInternal
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ValueTuple`1" /> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		public ValueTuple(T1 item1)
		{
			this.Item1 = item1;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`1" /> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// The <paramref name="obj" /> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="T:System.ValueTuple`1" /> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1> && this.Equals((ValueTuple<T1>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`1" />
		/// instance is equal to a specified <see cref="T:System.ValueTuple`1" />.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// The <paramref name="other" /> parameter is considered to be equal to the current instance if each of its field
		/// is equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1> other)
		{
			return ValueTuple<T1>.s_t1Comparer.Equals(this.Item1, other.Item1);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null || !(other is ValueTuple<T1>))
			{
				return false;
			}
			ValueTuple<T1> valueTuple = (ValueTuple<T1>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			ValueTuple<T1> valueTuple = (ValueTuple<T1>)other;
			return Comparer<T1>.Default.Compare(this.Item1, valueTuple.Item1);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other" />.
		/// Returns less than zero if this instance is less than <paramref name="other" />, zero if this
		/// instance is equal to <paramref name="other" />, and greater than zero if this instance is greater 
		/// than <paramref name="other" />.
		/// </returns>
		public int CompareTo(ValueTuple<T1> other)
		{
			return Comparer<T1>.Default.Compare(this.Item1, other.Item1);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			ValueTuple<T1> valueTuple = (ValueTuple<T1>)other;
			return comparer.Compare(this.Item1, valueTuple.Item1);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="T:System.ValueTuple`1" /> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple<T1>.s_t1Comparer.GetHashCode(this.Item1);
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.Item1);
		}

		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.Item1);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="T:System.ValueTuple`1" /> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="T:System.ValueTuple`1" /> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1)</c>,
		/// where <c>Item1</c> represents the value of <see cref="F:System.ValueTuple`1.Item1" />. If the field is <see langword="null" />,
		/// it is represented as <see cref="F:System.String.Empty" />.
		/// </remarks>
		public override string ToString()
		{
			string str = "(";
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			string str2;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					str2 = null;
					goto IL_3A;
				}
			}
			str2 = ptr.ToString();
			IL_3A:
			return str + str2 + ")";
		}

		string ITupleInternal.ToStringEnd()
		{
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			string str;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					str = null;
					goto IL_35;
				}
			}
			str = ptr.ToString();
			IL_35:
			return str + ")";
		}

		int ITupleInternal.Size
		{
			get
			{
				return 1;
			}
		}

		private static readonly EqualityComparer<T1> s_t1Comparer = EqualityComparer<T1>.Default;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`1" /> instance's first component.
		/// </summary>
		public T1 Item1;
	}
}
