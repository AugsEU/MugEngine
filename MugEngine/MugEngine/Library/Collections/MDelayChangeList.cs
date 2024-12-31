using System.Collections;

namespace MugEngine.Library
{
	/// <summary>
	/// A list that only adds or deletes when update is called.
	/// </summary>
	public class MDelayChangeList<T> : IList<T> where T : class
	{
		IList<T> mList;
		List<int> mDeletePool;
		List<T> mAddPool;

		public MDelayChangeList(IList<T> list)
		{
			mList = list;
			mDeletePool = new List<int>();
			mAddPool = new List<T>();
		}


		/// <summary>
		/// Process all adds and deletes
		/// </summary>
		public void ProcessAddsDeletes()
		{
			// Handle deletes
			for (int i = 0; i < mDeletePool.Count; i++)
			{
				int idx = mDeletePool[i];
				mList.RemoveAt(idx);
			}

			// Now add new elements
			for (int i = 0; i < mAddPool.Count; i++)
			{
				mList.Add(mAddPool[i]);
			}

			mDeletePool.Clear();
			mAddPool.Clear();
		}

		public T this[int index] { get => mList[index]; set => mList[index] = value; }

		public int Count => mList.Count;

		public bool IsReadOnly => mList.IsReadOnly;

		public void Add(T item)
		{
			mAddPool.Add(item);
		}

		public void Clear()
		{
			mDeletePool.Clear();
			for (int i = 0; i < mList.Count; i++)
			{
				mDeletePool.Add(i);
			}
		}

		public void ForceClearAll()
		{
			mDeletePool.Clear();
			mList.Clear();
			mAddPool.Clear();
		}

		public bool Contains(T item)
		{
			return mList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			mList.CopyTo(array, arrayIndex);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return mList.GetEnumerator();
		}

		public int IndexOf(T item)
		{
			return mList.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			int index = IndexOf(item);
			if (index == -1)
			{
				return false;
			}

			mDeletePool.Add(index);
			return true;
		}

		public void RemoveAt(int index)
		{
			mDeletePool.Add(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)mList).GetEnumerator();
		}
	}
}
