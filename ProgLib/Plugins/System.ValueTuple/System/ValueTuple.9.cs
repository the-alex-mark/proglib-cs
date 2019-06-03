using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	/// Represents an 8-tuple, or octuple, as a value type.
	/// </summary>
	/// <typeparam name="T1">The type of the tuple's first component.</typeparam>
	/// <typeparam name="T2">The type of the tuple's second component.</typeparam>
	/// <typeparam name="T3">The type of the tuple's third component.</typeparam>
	/// <typeparam name="T4">The type of the tuple's fourth component.</typeparam>
	/// <typeparam name="T5">The type of the tuple's fifth component.</typeparam>
	/// <typeparam name="T6">The type of the tuple's sixth component.</typeparam>
	/// <typeparam name="T7">The type of the tuple's seventh component.</typeparam>
	/// <typeparam name="TRest">The type of the tuple's eighth component.</typeparam>
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IEquatable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>, ITupleInternal where TRest : struct
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.ValueTuple`8" /> value type.
		/// </summary>
		/// <param name="item1">The value of the tuple's first component.</param>
		/// <param name="item2">The value of the tuple's second component.</param>
		/// <param name="item3">The value of the tuple's third component.</param>
		/// <param name="item4">The value of the tuple's fourth component.</param>
		/// <param name="item5">The value of the tuple's fifth component.</param>
		/// <param name="item6">The value of the tuple's sixth component.</param>
		/// <param name="item7">The value of the tuple's seventh component.</param>
		/// <param name="rest">The value of the tuple's eight component.</param>
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			if (!(rest is ITupleInternal))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleLastArgumentNotAValueTuple);
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
			this.Rest = rest;
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`8" /> instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// The <paramref name="obj" /> parameter is considered to be equal to the current instance under the following conditions:
		/// <list type="bullet">
		///     <item><description>It is a <see cref="T:System.ValueTuple`8" /> value type.</description></item>
		///     <item><description>Its components are of the same types as those of the current instance.</description></item>
		///     <item><description>Its components are equal to those of the current instance. Equality is determined by the default object equality comparer for each component.</description></item>
		/// </list>
		/// </remarks>
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> && this.Equals((ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)obj);
		}

		/// <summary>
		/// Returns a value that indicates whether the current <see cref="T:System.ValueTuple`8" />
		/// instance is equal to a specified <see cref="T:System.ValueTuple`8" />.
		/// </summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns><see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
		/// <remarks>
		/// The <paramref name="other" /> parameter is considered to be equal to the current instance if each of its fields
		/// are equal to that of the current instance, using the default comparer for that field's type.
		/// </remarks>
		public bool Equals(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other)
		{
			return ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t1Comparer.Equals(this.Item1, other.Item1) && ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t2Comparer.Equals(this.Item2, other.Item2) && ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t3Comparer.Equals(this.Item3, other.Item3) && ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t4Comparer.Equals(this.Item4, other.Item4) && ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t5Comparer.Equals(this.Item5, other.Item5) && ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.Equals(this.Item6, other.Item6) && ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.Equals(this.Item7, other.Item7) && ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_tRestComparer.Equals(this.Rest, other.Rest);
		}

		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null || !(other is ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				return false;
			}
			ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> valueTuple = (ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4) && comparer.Equals(this.Item5, valueTuple.Item5) && comparer.Equals(this.Item6, valueTuple.Item6) && comparer.Equals(this.Item7, valueTuple.Item7) && comparer.Equals(this.Rest, valueTuple.Rest);
		}

		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			return this.CompareTo((ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other);
		}

		/// <summary>Compares this instance to a specified instance and returns an indication of their relative values.</summary>
		/// <param name="other">An instance to compare.</param>
		/// <returns>
		/// A signed number indicating the relative values of this instance and <paramref name="other" />.
		/// Returns less than zero if this instance is less than <paramref name="other" />, zero if this
		/// instance is equal to <paramref name="other" />, and greater than zero if this instance is greater 
		/// than <paramref name="other" />.
		/// </returns>
		public int CompareTo(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other)
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
			num = Comparer<T6>.Default.Compare(this.Item6, other.Item6);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T7>.Default.Compare(this.Item7, other.Item7);
			if (num != 0)
			{
				return num;
			}
			return Comparer<TRest>.Default.Compare(this.Rest, other.Rest);
		}

		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				throw new ArgumentException(SR.ArgumentException_ValueTupleIncorrectType, "other");
			}
			ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> valueTuple = (ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other;
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
			num = comparer.Compare(this.Item6, valueTuple.Item6);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item7, valueTuple.Item7);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Rest, valueTuple.Rest);
		}

		/// <summary>
		/// Returns the hash code for the current <see cref="T:System.ValueTuple`8" /> instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			ITupleInternal tupleInternal = this.Rest as ITupleInternal;
			if (tupleInternal == null)
			{
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t1Comparer.GetHashCode(this.Item1), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t2Comparer.GetHashCode(this.Item2), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t3Comparer.GetHashCode(this.Item3), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t4Comparer.GetHashCode(this.Item4), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t5Comparer.GetHashCode(this.Item5), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.GetHashCode(this.Item6), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7));
			}
			int size = tupleInternal.Size;
			if (size >= 8)
			{
				return tupleInternal.GetHashCode();
			}
			switch (8 - size)
			{
			case 1:
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode());
			case 2:
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.GetHashCode(this.Item6), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode());
			case 3:
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t5Comparer.GetHashCode(this.Item5), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.GetHashCode(this.Item6), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode());
			case 4:
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t4Comparer.GetHashCode(this.Item4), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t5Comparer.GetHashCode(this.Item5), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.GetHashCode(this.Item6), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode());
			case 5:
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t3Comparer.GetHashCode(this.Item3), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t4Comparer.GetHashCode(this.Item4), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t5Comparer.GetHashCode(this.Item5), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.GetHashCode(this.Item6), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode());
			case 6:
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t2Comparer.GetHashCode(this.Item2), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t3Comparer.GetHashCode(this.Item3), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t4Comparer.GetHashCode(this.Item4), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t5Comparer.GetHashCode(this.Item5), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.GetHashCode(this.Item6), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode());
			case 7:
			case 8:
				return ValueTuple.CombineHashCodes(ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t1Comparer.GetHashCode(this.Item1), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t2Comparer.GetHashCode(this.Item2), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t3Comparer.GetHashCode(this.Item3), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t4Comparer.GetHashCode(this.Item4), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t5Comparer.GetHashCode(this.Item5), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t6Comparer.GetHashCode(this.Item6), ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>.s_t7Comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode());
			default:
				return -1;
			}
		}

		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			ITupleInternal tupleInternal = this.Rest as ITupleInternal;
			if (tupleInternal == null)
			{
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7));
			}
			int size = tupleInternal.Size;
			if (size >= 8)
			{
				return tupleInternal.GetHashCode(comparer);
			}
			switch (8 - size)
			{
			case 1:
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode(comparer));
			case 2:
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode(comparer));
			case 3:
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode(comparer));
			case 4:
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode(comparer));
			case 5:
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode(comparer));
			case 6:
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode(comparer));
			case 7:
			case 8:
				return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), tupleInternal.GetHashCode(comparer));
			default:
				return -1;
			}
		}

		int ITupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		/// <summary>
		/// Returns a string that represents the value of this <see cref="T:System.ValueTuple`8" /> instance.
		/// </summary>
		/// <returns>The string representation of this <see cref="T:System.ValueTuple`8" /> instance.</returns>
		/// <remarks>
		/// The string returned by this method takes the form <c>(Item1, Item2, Item3, Item4, Item5, Item6, Item7, Rest)</c>.
		/// If any field value is <see langword="null" />, it is represented as <see cref="F:System.String.Empty" />.
		/// </remarks>
		public override string ToString()
		{
			ITupleInternal tupleInternal = this.Rest as ITupleInternal;
			T1 t;
			T2 t2;
			T3 t3;
			T4 t4;
			T5 t5;
			T6 t6;
			T7 t7;
			if (tupleInternal == null)
			{
				string[] array = new string[17];
				array[0] = "(";
				int num = 1;
				ref T1 ptr = ref this.Item1;
				t = default(T1);
				string text;
				if (t == null)
				{
					t = this.Item1;
					ptr = ref t;
					if (t == null)
					{
						text = null;
						goto IL_5D;
					}
				}
				text = ptr.ToString();
				IL_5D:
				array[num] = text;
				array[2] = ", ";
				int num2 = 3;
				ref T2 ptr2 = ref this.Item2;
				t2 = default(T2);
				string text2;
				if (t2 == null)
				{
					t2 = this.Item2;
					ptr2 = ref t2;
					if (t2 == null)
					{
						text2 = null;
						goto IL_9D;
					}
				}
				text2 = ptr2.ToString();
				IL_9D:
				array[num2] = text2;
				array[4] = ", ";
				int num3 = 5;
				ref T3 ptr3 = ref this.Item3;
				t3 = default(T3);
				string text3;
				if (t3 == null)
				{
					t3 = this.Item3;
					ptr3 = ref t3;
					if (t3 == null)
					{
						text3 = null;
						goto IL_DD;
					}
				}
				text3 = ptr3.ToString();
				IL_DD:
				array[num3] = text3;
				array[6] = ", ";
				int num4 = 7;
				ref T4 ptr4 = ref this.Item4;
				t4 = default(T4);
				string text4;
				if (t4 == null)
				{
					t4 = this.Item4;
					ptr4 = ref t4;
					if (t4 == null)
					{
						text4 = null;
						goto IL_120;
					}
				}
				text4 = ptr4.ToString();
				IL_120:
				array[num4] = text4;
				array[8] = ", ";
				int num5 = 9;
				ref T5 ptr5 = ref this.Item5;
				t5 = default(T5);
				string text5;
				if (t5 == null)
				{
					t5 = this.Item5;
					ptr5 = ref t5;
					if (t5 == null)
					{
						text5 = null;
						goto IL_164;
					}
				}
				text5 = ptr5.ToString();
				IL_164:
				array[num5] = text5;
				array[10] = ", ";
				int num6 = 11;
				ref T6 ptr6 = ref this.Item6;
				t6 = default(T6);
				string text6;
				if (t6 == null)
				{
					t6 = this.Item6;
					ptr6 = ref t6;
					if (t6 == null)
					{
						text6 = null;
						goto IL_1A9;
					}
				}
				text6 = ptr6.ToString();
				IL_1A9:
				array[num6] = text6;
				array[12] = ", ";
				int num7 = 13;
				ref T7 ptr7 = ref this.Item7;
				t7 = default(T7);
				string text7;
				if (t7 == null)
				{
					t7 = this.Item7;
					ptr7 = ref t7;
					if (t7 == null)
					{
						text7 = null;
						goto IL_1EE;
					}
				}
				text7 = ptr7.ToString();
				IL_1EE:
				array[num7] = text7;
				array[14] = ", ";
				array[15] = this.Rest.ToString();
				array[16] = ")";
				return string.Concat(array);
			}
			string[] array2 = new string[16];
			array2[0] = "(";
			int num8 = 1;
			ref T1 ptr8 = ref this.Item1;
			t = default(T1);
			string text8;
			if (t == null)
			{
				t = this.Item1;
				ptr8 = ref t;
				if (t == null)
				{
					text8 = null;
					goto IL_262;
				}
			}
			text8 = ptr8.ToString();
			IL_262:
			array2[num8] = text8;
			array2[2] = ", ";
			int num9 = 3;
			ref T2 ptr9 = ref this.Item2;
			t2 = default(T2);
			string text9;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr9 = ref t2;
				if (t2 == null)
				{
					text9 = null;
					goto IL_2A2;
				}
			}
			text9 = ptr9.ToString();
			IL_2A2:
			array2[num9] = text9;
			array2[4] = ", ";
			int num10 = 5;
			ref T3 ptr10 = ref this.Item3;
			t3 = default(T3);
			string text10;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr10 = ref t3;
				if (t3 == null)
				{
					text10 = null;
					goto IL_2E2;
				}
			}
			text10 = ptr10.ToString();
			IL_2E2:
			array2[num10] = text10;
			array2[6] = ", ";
			int num11 = 7;
			ref T4 ptr11 = ref this.Item4;
			t4 = default(T4);
			string text11;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr11 = ref t4;
				if (t4 == null)
				{
					text11 = null;
					goto IL_325;
				}
			}
			text11 = ptr11.ToString();
			IL_325:
			array2[num11] = text11;
			array2[8] = ", ";
			int num12 = 9;
			ref T5 ptr12 = ref this.Item5;
			t5 = default(T5);
			string text12;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr12 = ref t5;
				if (t5 == null)
				{
					text12 = null;
					goto IL_369;
				}
			}
			text12 = ptr12.ToString();
			IL_369:
			array2[num12] = text12;
			array2[10] = ", ";
			int num13 = 11;
			ref T6 ptr13 = ref this.Item6;
			t6 = default(T6);
			string text13;
			if (t6 == null)
			{
				t6 = this.Item6;
				ptr13 = ref t6;
				if (t6 == null)
				{
					text13 = null;
					goto IL_3AE;
				}
			}
			text13 = ptr13.ToString();
			IL_3AE:
			array2[num13] = text13;
			array2[12] = ", ";
			int num14 = 13;
			ref T7 ptr14 = ref this.Item7;
			t7 = default(T7);
			string text14;
			if (t7 == null)
			{
				t7 = this.Item7;
				ptr14 = ref t7;
				if (t7 == null)
				{
					text14 = null;
					goto IL_3F3;
				}
			}
			text14 = ptr14.ToString();
			IL_3F3:
			array2[num14] = text14;
			array2[14] = ", ";
			array2[15] = tupleInternal.ToStringEnd();
			return string.Concat(array2);
		}

		string ITupleInternal.ToStringEnd()
		{
			ITupleInternal tupleInternal = this.Rest as ITupleInternal;
			T1 t;
			T2 t2;
			T3 t3;
			T4 t4;
			T5 t5;
			T6 t6;
			T7 t7;
			if (tupleInternal == null)
			{
				string[] array = new string[16];
				int num = 0;
				ref T1 ptr = ref this.Item1;
				t = default(T1);
				string text;
				if (t == null)
				{
					t = this.Item1;
					ptr = ref t;
					if (t == null)
					{
						text = null;
						goto IL_55;
					}
				}
				text = ptr.ToString();
				IL_55:
				array[num] = text;
				array[1] = ", ";
				int num2 = 2;
				ref T2 ptr2 = ref this.Item2;
				t2 = default(T2);
				string text2;
				if (t2 == null)
				{
					t2 = this.Item2;
					ptr2 = ref t2;
					if (t2 == null)
					{
						text2 = null;
						goto IL_95;
					}
				}
				text2 = ptr2.ToString();
				IL_95:
				array[num2] = text2;
				array[3] = ", ";
				int num3 = 4;
				ref T3 ptr3 = ref this.Item3;
				t3 = default(T3);
				string text3;
				if (t3 == null)
				{
					t3 = this.Item3;
					ptr3 = ref t3;
					if (t3 == null)
					{
						text3 = null;
						goto IL_D5;
					}
				}
				text3 = ptr3.ToString();
				IL_D5:
				array[num3] = text3;
				array[5] = ", ";
				int num4 = 6;
				ref T4 ptr4 = ref this.Item4;
				t4 = default(T4);
				string text4;
				if (t4 == null)
				{
					t4 = this.Item4;
					ptr4 = ref t4;
					if (t4 == null)
					{
						text4 = null;
						goto IL_118;
					}
				}
				text4 = ptr4.ToString();
				IL_118:
				array[num4] = text4;
				array[7] = ", ";
				int num5 = 8;
				ref T5 ptr5 = ref this.Item5;
				t5 = default(T5);
				string text5;
				if (t5 == null)
				{
					t5 = this.Item5;
					ptr5 = ref t5;
					if (t5 == null)
					{
						text5 = null;
						goto IL_15B;
					}
				}
				text5 = ptr5.ToString();
				IL_15B:
				array[num5] = text5;
				array[9] = ", ";
				int num6 = 10;
				ref T6 ptr6 = ref this.Item6;
				t6 = default(T6);
				string text6;
				if (t6 == null)
				{
					t6 = this.Item6;
					ptr6 = ref t6;
					if (t6 == null)
					{
						text6 = null;
						goto IL_1A0;
					}
				}
				text6 = ptr6.ToString();
				IL_1A0:
				array[num6] = text6;
				array[11] = ", ";
				int num7 = 12;
				ref T7 ptr7 = ref this.Item7;
				t7 = default(T7);
				string text7;
				if (t7 == null)
				{
					t7 = this.Item7;
					ptr7 = ref t7;
					if (t7 == null)
					{
						text7 = null;
						goto IL_1E5;
					}
				}
				text7 = ptr7.ToString();
				IL_1E5:
				array[num7] = text7;
				array[13] = ", ";
				array[14] = this.Rest.ToString();
				array[15] = ")";
				return string.Concat(array);
			}
			string[] array2 = new string[15];
			int num8 = 0;
			ref T1 ptr8 = ref this.Item1;
			t = default(T1);
			string text8;
			if (t == null)
			{
				t = this.Item1;
				ptr8 = ref t;
				if (t == null)
				{
					text8 = null;
					goto IL_251;
				}
			}
			text8 = ptr8.ToString();
			IL_251:
			array2[num8] = text8;
			array2[1] = ", ";
			int num9 = 2;
			ref T2 ptr9 = ref this.Item2;
			t2 = default(T2);
			string text9;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr9 = ref t2;
				if (t2 == null)
				{
					text9 = null;
					goto IL_291;
				}
			}
			text9 = ptr9.ToString();
			IL_291:
			array2[num9] = text9;
			array2[3] = ", ";
			int num10 = 4;
			ref T3 ptr10 = ref this.Item3;
			t3 = default(T3);
			string text10;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr10 = ref t3;
				if (t3 == null)
				{
					text10 = null;
					goto IL_2D1;
				}
			}
			text10 = ptr10.ToString();
			IL_2D1:
			array2[num10] = text10;
			array2[5] = ", ";
			int num11 = 6;
			ref T4 ptr11 = ref this.Item4;
			t4 = default(T4);
			string text11;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr11 = ref t4;
				if (t4 == null)
				{
					text11 = null;
					goto IL_314;
				}
			}
			text11 = ptr11.ToString();
			IL_314:
			array2[num11] = text11;
			array2[7] = ", ";
			int num12 = 8;
			ref T5 ptr12 = ref this.Item5;
			t5 = default(T5);
			string text12;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr12 = ref t5;
				if (t5 == null)
				{
					text12 = null;
					goto IL_357;
				}
			}
			text12 = ptr12.ToString();
			IL_357:
			array2[num12] = text12;
			array2[9] = ", ";
			int num13 = 10;
			ref T6 ptr13 = ref this.Item6;
			t6 = default(T6);
			string text13;
			if (t6 == null)
			{
				t6 = this.Item6;
				ptr13 = ref t6;
				if (t6 == null)
				{
					text13 = null;
					goto IL_39C;
				}
			}
			text13 = ptr13.ToString();
			IL_39C:
			array2[num13] = text13;
			array2[11] = ", ";
			int num14 = 12;
			ref T7 ptr14 = ref this.Item7;
			t7 = default(T7);
			string text14;
			if (t7 == null)
			{
				t7 = this.Item7;
				ptr14 = ref t7;
				if (t7 == null)
				{
					text14 = null;
					goto IL_3E1;
				}
			}
			text14 = ptr14.ToString();
			IL_3E1:
			array2[num14] = text14;
			array2[13] = ", ";
			array2[14] = tupleInternal.ToStringEnd();
			return string.Concat(array2);
		}

		int ITupleInternal.Size
		{
			get
			{
				ITupleInternal tupleInternal = this.Rest as ITupleInternal;
				if (tupleInternal != null)
				{
					return 7 + tupleInternal.Size;
				}
				return 8;
			}
		}

		private static readonly EqualityComparer<T1> s_t1Comparer = EqualityComparer<T1>.Default;

		private static readonly EqualityComparer<T2> s_t2Comparer = EqualityComparer<T2>.Default;

		private static readonly EqualityComparer<T3> s_t3Comparer = EqualityComparer<T3>.Default;

		private static readonly EqualityComparer<T4> s_t4Comparer = EqualityComparer<T4>.Default;

		private static readonly EqualityComparer<T5> s_t5Comparer = EqualityComparer<T5>.Default;

		private static readonly EqualityComparer<T6> s_t6Comparer = EqualityComparer<T6>.Default;

		private static readonly EqualityComparer<T7> s_t7Comparer = EqualityComparer<T7>.Default;

		private static readonly EqualityComparer<TRest> s_tRestComparer = EqualityComparer<TRest>.Default;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's first component.
		/// </summary>
		public T1 Item1;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's second component.
		/// </summary>
		public T2 Item2;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's third component.
		/// </summary>
		public T3 Item3;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's fourth component.
		/// </summary>
		public T4 Item4;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's fifth component.
		/// </summary>
		public T5 Item5;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's sixth component.
		/// </summary>
		public T6 Item6;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's seventh component.
		/// </summary>
		public T7 Item7;

		/// <summary>
		/// The current <see cref="T:System.ValueTuple`8" /> instance's eighth component.
		/// </summary>
		public TRest Rest;
	}
}
