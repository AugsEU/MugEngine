using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MugEngine.Library;

/// <summary>
/// A list that only adds or deletes when update is called.
/// </summary>
public class MDelayChangeList<T> : IList<T> where T : class
{
	List<T> mList;
	HashSet<T> mDeletePool;
	List<T> mAddPool;

	public MDelayChangeList()
	{
		mList = new();
		mDeletePool = new HashSet<T>();
		mAddPool = new List<T>();
	}


	/// <summary>
	/// Process all adds and deletes
	/// </summary>
	public void ProcessAddsDeletes()
	{
		// Handle deletes
		if (mDeletePool.Count > 0)
		{
			int freeIndex = 0;

			// Find the first item which needs to be removed.
			while (!mDeletePool.Contains(mList[freeIndex]))
			{
				freeIndex++;
			}

			int current = freeIndex + 1;
			while (current < mList.Count)
			{
				// Find the first item which needs to be kept.
				while (mDeletePool.Contains(mList[current]))
				{
					current++;
					if (current >= mList.Count)
					{
						goto lEndSwaps;
					}
				}

				// Copy item to the free slot.
				mList[freeIndex++] = mList[current++];
			}
		lEndSwaps:

			mList.RemoveRange(freeIndex, mList.Count - freeIndex);
		}


		// Now add new elements
		mList.AddRange(mAddPool);

		mDeletePool.Clear();
		mAddPool.Clear();
	}

	public T this[int index] { get => mList[index]; set => mList[index] = value; }

	public int Count => mList.Count;

	public bool IsReadOnly => false;

	public void Add(T item)
	{
		mAddPool.Add(item);
	}

	public void Clear()
	{
		for (int i = 0; i < mList.Count; i++)
		{
			mDeletePool.Add(mList[i]);
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
#if DEBUG // Perf hack: do not allow this case. But it should normally return false
		if (!mList.Contains(item))
		{
			throw new Exception("Can't delete element no in list.");
		}
#endif // DEBUG

		mDeletePool.Add(item);
		return true;
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

