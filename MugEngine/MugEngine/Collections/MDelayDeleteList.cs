using MugEngine.Core;
using System.Collections;

namespace MugEngine.Collections
{
	public class MDelayDeleteList<T> : IList<T> where T : class
	{
		IList<T> mList;
		HashSet<T> mDeletePool;

		public MDelayDeleteList(IList<T> list)
		{
			mList = list;
			mDeletePool = new HashSet<T>();
		}

		public void ProcessDeletes()
		{
			for (int i = 0; i < mList.Count && mList.Count > 0; i++)
			{
				T item = mList[i];
				if (mDeletePool.Contains(item))
				{
					mDeletePool.Remove(item);
					mList.RemoveAt(i);

					i--;
				}
			}

			MugDebug.Assert(mDeletePool.Count == 0, "Queued items to delete that aren't in the list.");
			mDeletePool.Clear();
		}

		public T this[int index] { get => mList[index]; set => mList[index] = value; }

		public int Count => mList.Count;

		public bool IsReadOnly => mList.IsReadOnly;

		public void Add(T item)
		{
			mList.Add(item);
		}

		public void Clear()
		{
			mList.Clear();
			mDeletePool.Clear();
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
			mList.Insert(index, item);
		}

		public bool Remove(T item)
		{
			return mDeletePool.Add(item);
		}

		public void RemoveAt(int index)
		{
			mDeletePool.Add(mList[index]);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)mList).GetEnumerator();
		}
	}
}
