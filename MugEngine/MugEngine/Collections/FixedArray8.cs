using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugEngine.Collections
{
	/// <summary>
	/// An array that holds 8 elements.
	/// Advantage over using a normal array is that we don't store this on the heap
	/// Disadvantage is we can only have 8 elements.
	/// </summary>>
	public struct FixedArray8<T> where T : struct
	{
		private T mItem0;
		private T mItem1;
		private T mItem2;
		private T mItem3;
		private T mItem4;
		private T mItem5;
		private T mItem6;
		private T mItem7;
		private int mCount;

		public int Count => mCount;

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= mCount)
					throw new IndexOutOfRangeException();

				return index switch
				{
					0 => mItem0,
					1 => mItem1,
					2 => mItem2,
					3 => mItem3,
					4 => mItem4,
					5 => mItem5,
					6 => mItem6,
					7 => mItem7,
					_ => throw new IndexOutOfRangeException()
				};
			}
		}

		public bool Add(T item)
		{
			if (mCount >= 8)
				return false;

			switch (mCount)
			{
				case 0: mItem0 = item; break;
				case 1: mItem1 = item; break;
				case 2: mItem2 = item; break;
				case 3: mItem3 = item; break;
				case 4: mItem4 = item; break;
				case 5: mItem5 = item; break;
				case 6: mItem6 = item; break;
				case 7: mItem7 = item; break;
			}

			mCount++;
			return true;
		}

		public void Clear()
		{
			mCount = 0;
		}

		public bool Contains(T item)
		{
			return IndexOf(item) != -1;
		}

		public int IndexOf(T item)
		{
			if (mCount > 0 && EqualityComparer<T>.Default.Equals(mItem0, item)) return 0;
			if (mCount > 1 && EqualityComparer<T>.Default.Equals(mItem1, item)) return 1;
			if (mCount > 2 && EqualityComparer<T>.Default.Equals(mItem2, item)) return 2;
			if (mCount > 3 && EqualityComparer<T>.Default.Equals(mItem3, item)) return 3;
			if (mCount > 4 && EqualityComparer<T>.Default.Equals(mItem4, item)) return 4;
			if (mCount > 5 && EqualityComparer<T>.Default.Equals(mItem5, item)) return 5;
			if (mCount > 6 && EqualityComparer<T>.Default.Equals(mItem6, item)) return 6;
			if (mCount > 7 && EqualityComparer<T>.Default.Equals(mItem7, item)) return 7;
			return -1;
		}

		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index == -1)
				return false;

			RemoveAt(index);
			return true;
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= mCount)
				throw new IndexOutOfRangeException();

			mCount--;

			// Shift elements down to fill the gap
			for (int i = index; i < mCount; i++)
			{
				switch (i)
				{
					case 0: mItem0 = mItem1; break;
					case 1: mItem1 = mItem2; break;
					case 2: mItem2 = mItem3; break;
					case 3: mItem3 = mItem4; break;
					case 4: mItem4 = mItem5; break;
					case 5: mItem5 = mItem6; break;
					case 6: mItem6 = mItem7; break;
				}
			}
		}
	}
}
