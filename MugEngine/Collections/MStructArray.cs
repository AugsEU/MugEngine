using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace MugEngine.Collections
{
	/// <summary>
	/// Array designed to hold a lot of information.
	/// O(1) Insertion
	/// O(n-i) deletion
	/// O(1) Clear
	/// No safety!
	/// </summary>
	internal class MStructArray<T> : IList<T> where T : struct
	{
		#region rMembers

		T[] mData;
		int mHead;

		#endregion rMembers



		#region rInit

		public MStructArray(int initSize)
		{
			mData = new T[initSize];
			mHead = 0;
		}

		#endregion rInit



		#region rInterface

		public T this[int index]
		{
			get
			{
				return mData[index];
			}
			set
			{
				mData[index] = value;
			}
		}

		public int Count => mHead;

		public bool IsReadOnly => false;

		public void Add(T item)
		{
			if(mData.Length <= mHead)
			{
				GrowList();
			}
			mData[mHead++] = item;
		}

		public void Clear()
		{
			mHead = 0;
		}

		public bool Contains(T item)
		{
			for (int i = 0; i < mHead; i++)
			{
				if (EqualityComparer<T>.Default.Equals(mData[i], item))
				{
					return true;
				}
			}
			return false;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(mData, 0, array, arrayIndex, mHead);
		}

		public int IndexOf(T item)
		{
			for (int i = 0; i < mHead; i++)
			{
				if (EqualityComparer<T>.Default.Equals(mData[i], item))
				{
					return i;
				}
			}
			return -1; // Item not found
		}

		public void Insert(int index, T item)
		{
			if (mData.Length <= mHead)
			{
				GrowList();
			}

			// Shift elements to the right
			Array.Copy(mData, index, mData, index + 1, mHead - index);

			mData[index] = item;
			mHead++;
		}

		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index >= 0)
			{
				RemoveAt(index);
				return true;
			}
			return false;
		}

		public void RemoveAt(int index)
		{
			// Shift elements to the left
			for (int i = index; i < mHead - 1; i++)
			{
				mData[i] = mData[i + 1];
			}

			mHead--;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < mHead; i++)
			{
				yield return mData[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion rInterface





		#region rLogic

		private void GrowList()
		{
			int newSize = mData.Length * 2;
			T[] newList = new T[newSize];
			Array.Copy(mData, newList, mHead);
			mData = newList;
		}

		#endregion rLogic
	}
}
