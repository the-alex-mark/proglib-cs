using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	/// Represents a 6-tuple, or sixtuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4, T5, T6> : IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1, T2, T3, T4, T5, T6>>, ITupleInternal
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ValueTuple`6" /> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`6" /> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// The <paramref name="obj" /> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="T:System.ValueTuple`6" /> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1, T2, T3, T4, T5, T6> && this.Equals((ValueTuple<T1, T2, T3, T4, T5, T6>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`6" />
		/// instance is equal to a specified <see cref="T:System.ValueTuple`6" />.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// The <paramref name="other" /> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6> other)
		{
			return ValueTuple<T1, T2, T3, T4, T5, T6>.s_t1Comparer.Equals(this.Item1, other.Item1) && ValueTuple<T1, T2, T3, T4, T5, T6>.s_t2Comparer.Equals(this.Item2, other.Item2) && ValueTuple<T1, T2, T3, T4, T5, T6>.s_t3Comparer.Equals(this.Item3, other.Item3) && ValueTuple<T1, T2, T3, T4, T5, T6>.s_t4Comparer.Equals(this.Item4, other.Item4) && ValueTuple<T1, T2, T3, T4, T5, T6>.s_t5Comparer.Equals(this.Item5, other.Item5) && ValueTuple<T1, T2, T3, T4, T5, T6>.s_t6Comparer.Equals(this.Item6, other.Item6);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null || !(other is ValueTuple<T1, T2, T3, T4, T5, T6>))
			{
				return false;
			}
			ValueTuple<T1, T2, T3, T4, T5, T6> valueTuple = (ValueTuple<T1, T2, T3, T4, T5, T6>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4) && comparer.Equals(this.Item5, valueTuple.Item5) && comparer.Equals(this.Item6, valueTuple.Item6);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5, T6>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			return this.CompareTo((ValueTuple<T1, T2, T3, T4, T5, T6>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other" />.
		/// Returns less than zero if this instance is less than <paramref name="other" />, zero if this
		/// instance is equal to <paramref name="other" />, and greater than zero if this instance is greater 
		/// than <paramref name="other" />.
		/// </returns>
		public int CompareTo(ValueTuple<T1, T2, T3, T4, T5, T6> other)
		{
			int num = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T2>.Default.Compare(this.Item2, other.Item2);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T3>.Default.Compare(this.Item3, other.Item3);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T4>.Default.Compare(this.Item4, other.Item4);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T5>.Default.Compare(this.Item5, other.Item5);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T6>.Default.Compare(this.Item6, other.Item6);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5, T6>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			ValueTuple<T1, T2, T3, T4, T5, T6> valueTuple = (ValueTuple<T1, T2, T3, T4, T5, T6>)other;
			int num = comparer.Compare(this.Item1, valueTuple.Item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item2, valueTuple.Item2);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item3, valueTuple.Item3);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item4, valueTuple.Item4);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item5, valueTuple.Item5);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Item6, valueTuple.Item6);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="T:System.ValueTuple`6" /> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6>.s_t1Comparer.GetHashCode(this.Item1), ValueTuple<T1, T2, T3, T4, T5, T6>.s_t2Comparer.GetHashCode(this.Item2), ValueTuple<T1, T2, T3, T4, T5, T6>.s_t3Comparer.GetHashCode(this.Item3), ValueTuple<T1, T2, T3, T4, T5, T6>.s_t4Comparer.GetHashCode(this.Item4), ValueTuple<T1, T2, T3, T4, T5, T6>.s_t5Comparer.GetHashCode(this.Item5), ValueTuple<T1, T2, T3, T4, T5, T6>.s_t6Comparer.GetHashCode(this.Item6));
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6));
		}

		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="T:System.ValueTuple`6" /> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="T:System.ValueTuple`6" /> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3, Item4, Item5, Item6)</c>.
		/// If any field value is <see langword="null" />, it is represented as <see cref="F:System.String.Empty" />.
		/// </remarks>
		public override string ToString()
		{
			string[] array = new string[13];
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
					goto IL_46;
				}
			}
			text = ptr.ToString();
			IL_46:
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
					goto IL_86;
				}
			}
			text2 = ptr2.ToString();
			IL_86:
			array[num2] = text2;
			array[4] = ", ";
			int num3 = 5;
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			string text3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					text3 = null;
					goto IL_C6;
				}
			}
			text3 = ptr3.ToString();
			IL_C6:
			array[num3] = text3;
			array[6] = ", ";
			int num4 = 7;
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			string text4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					text4 = null;
					goto IL_106;
				}
			}
			text4 = ptr4.ToString();
			IL_106:
			array[num4] = text4;
			array[8] = ", ";
			int num5 = 9;
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			string text5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					text5 = null;
					goto IL_14A;
				}
			}
			text5 = ptr5.ToString();
			IL_14A:
			array[num5] = text5;
			array[10] = ", ";
			int num6 = 11;
			ref T6 ptr6 = ref this.Item6;
			T6 t6 = default(T6);
			string text6;
			if (t6 == null)
			{
				t6 = this.Item6;
				ptr6 = ref t6;
				if (t6 == null)
				{
					text6 = null;
					goto IL_18F;
				}
			}
			text6 = ptr6.ToString();
			IL_18F:
			array[num6] = text6;
			array[12] = ")";
			return string.Concat(array);
		}

		string ITupleInternal.ToStringEnd()
		{
			string[] array = new string[12];
			int num = 0;
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
					goto IL_3E;
				}
			}
			text = ptr.ToString();
			IL_3E:
			array[num] = text;
			array[1] = ", ";
			int num2 = 2;
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
					goto IL_7E;
				}
			}
			text2 = ptr2.ToString();
			IL_7E:
			array[num2] = text2;
			array[3] = ", ";
			int num3 = 4;
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			string text3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					text3 = null;
					goto IL_BE;
				}
			}
			text3 = ptr3.ToString();
			IL_BE:
			array[num3] = text3;
			array[5] = ", ";
			int num4 = 6;
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			string text4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					text4 = null;
					goto IL_FE;
				}
			}
			text4 = ptr4.ToString();
			IL_FE:
			array[num4] = text4;
			array[7] = ", ";
			int num5 = 8;
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			string text5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					text5 = null;
					goto IL_141;
				}
			}
			text5 = ptr5.ToString();
			IL_141:
			array[num5] = text5;
			array[9] = ", ";
			int num6 = 10;
			ref T6 ptr6 = ref this.Item6;
			T6 t6 = default(T6);
			string text6;
			if (t6 == null)
			{
				t6 = this.Item6;
				ptr6 = ref t6;
				if (t6 == null)
				{
					text6 = null;
					goto IL_186;
				}
			}
			text6 = ptr6.ToString();
			IL_186:
			array[num6] = text6;
			array[11] = ")";
			return string.Concat(array);
		}

		int ITupleInternal.Size
		{
			get
			{
				return 6;
			}
		}

		private static readonly EqualityComparer<T1> s_t1Comparer = EqualityComparer<T1>.Default;

		private static readonly EqualityComparer<T2> s_t2Comparer = EqualityComparer<T2>.Default;

		private static readonly EqualityComparer<T3> s_t3Comparer = EqualityComparer<T3>.Default;

		private static readonly EqualityComparer<T4> s_t4Comparer = EqualityComparer<T4>.Default;

		private static readonly EqualityComparer<T5> s_t5Comparer = EqualityComparer<T5>.Default;

		private static readonly EqualityComparer<T6> s_t6Comparer = EqualityComparer<T6>.Default;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`6" /> instance's first component.
		/// </summary>
		public T1 Item1;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`6" /> instance's second component.
		/// </summary>
		public T2 Item2;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`6" /> instance's third component.
		/// </summary>
		public T3 Item3;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`6" /> instance's fourth component.
		/// </summary>
		public T4 Item4;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`6" /> instance's fifth component.
		/// </summary>
		public T5 Item5;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`6" /> instance's sixth component.
		/// </summary>
		public T6 Item6;
	}
}
