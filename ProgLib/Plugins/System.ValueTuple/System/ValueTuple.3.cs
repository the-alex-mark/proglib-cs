using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	/// Represents a 2-tuple, or pair, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1, T2>>, ITupleInternal
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ValueTuple`2" /> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		public ValueTuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`2" /> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
		///
		/// <remarks>
		/// The <paramref name="obj" /> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="T:System.ValueTuple`2" /> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1, T2> && this.Equals((ValueTuple<T1, T2>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`2" /> instance is equal to a specified <see cref="T:System.ValueTuple`2" />.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// The <paramref name="other" /> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1, T2> other)
		{
			return ValueTuple<T1, T2>.s_t1Comparer.Equals(this.Item1, other.Item1) && ValueTuple<T1, T2>.s_t2Comparer.Equals(this.Item2, other.Item2);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`2" /> instance is equal to a specified object based on a specified comparison method.
		/// </summary>
		/// <param name="other">The object to compare with this instance.</param>
		/// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
		///
		/// <remarks>
		/// This member is an explicit interface member implementation. It can be used only when the
		///  <see cref="T:System.ValueTuple`2" /> instance is cast to an <see cref="T:System.Collections.IStructuralEquatable" /> interface.
		///
		/// The <see cref="M:System.Collections.IEqualityComparer.Equals(System.Object,System.Object)" /> implementation is called only if <c>other</c> is not <see langword="null" />,
		///  and if it can be successfully cast (in C#) or converted (in Visual Basic) to a <see cref="T:System.ValueTuple`2" />
		///  whose components are of the same types as those of the current instance. The IStructuralEquatable.Equals(Object, IEqualityComparer) method
		///  first passes the <see cref="F:System.ValueTuple`2.Item1" /> values of the <see cref="T:System.ValueTuple`2" /> objects to be compared to the
		///  <see cref="M:System.Collections.IEqualityComparer.Equals(System.Object,System.Object)" /> implementation. If this method call returns <see langword="true" />, the method is
		///  called again and passed the <see cref="F:System.ValueTuple`2.Item2" /> values of the two <see cref="T:System.ValueTuple`2" /> instances.
		/// </remarks>
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null || !(other is ValueTuple<T1, T2>))
			{
				return false;
			}
			ValueTuple<T1, T2> valueTuple = (ValueTuple<T1, T2>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			return this.CompareTo((ValueTuple<T1, T2>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other" />.
		/// Returns less than zero if this instance is less than <paramref name="other" />, zero if this
		/// instance is equal to <paramref name="other" />, and greater than zero if this instance is greater 
		/// than <paramref name="other" />.
		/// </returns>
		public int CompareTo(ValueTuple<T1, T2> other)
		{
			int num = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T2>.Default.Compare(this.Item2, other.Item2);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			ValueTuple<T1, T2> valueTuple = (ValueTuple<T1, T2>)other;
			int num = comparer.Compare(this.Item1, valueTuple.Item1);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Item2, valueTuple.Item2);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="T:System.ValueTuple`2" /> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple.CombineHashCodes(ValueTuple<T1, T2>.s_t1Comparer.GetHashCode(this.Item1), ValueTuple<T1, T2>.s_t2Comparer.GetHashCode(this.Item2));
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2));
		}

		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="T:System.ValueTuple`2" /> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="T:System.ValueTuple`2" /> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2)</c>,
		/// where <c>Item1</c> and <c>Item2</c> represent the values of the <see cref="F:System.ValueTuple`2.Item1" />
		/// and <see cref="F:System.ValueTuple`2.Item2" /> fields. If either field value is <see langword="null" />,
		/// it is represented as <see cref="F:System.String.Empty" />.
		/// </remarks>
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			string text;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					text = null;
					goto IL_45;
				}
			}
			text = ptr.ToString();
			IL_45:
			array[num] = text;
			array[2] = ", ";
			int num2 = 3;
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			string text2;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					text2 = null;
					goto IL_85;
				}
			}
			text2 = ptr2.ToString();
			IL_85:
			array[num2] = text2;
			array[4] = ")";
			return string.Concat(array);
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
			string str2 = ", ";
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			string str3;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					str3 = null;
					goto IL_6F;
				}
			}
			str3 = ptr2.ToString();
			IL_6F:
			return str + str2 + str3 + ")";
		}

		int ITupleInternal.Size
		{
			get
			{
				return 2;
			}
		}

		private static readonly EqualityComparer<T1> s_t1Comparer = EqualityComparer<T1>.Default;

		private static readonly EqualityComparer<T2> s_t2Comparer = EqualityComparer<T2>.Default;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`2" /> instance's first component.
		/// </summary>
		public T1 Item1;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`2" /> instance's second component.
		/// </summary>
		public T2 Item2;
	}
}
