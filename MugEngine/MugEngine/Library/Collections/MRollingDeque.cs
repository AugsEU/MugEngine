using Nito.Collections;

namespace MugEngine.Library;

/// <summary>
/// Like a deque but it with a max size where it pops elements
/// </summary>
public class MRollingDeque<T> : IList<T>, ICollection<T>, IEnumerable<T>, IReadOnlyList<T>, IReadOnlyCollection<T>
{
	Deque<T> mQueue;
	int mMaxSize;

	public MRollingDeque(int maxSize)
	{
		mMaxSize = maxSize;
		mQueue = new Deque<T>(mMaxSize);
	}

	public T this[int index] { get => mQueue[index]; set => mQueue[index] = value; }

	public int Count => mQueue.Count;

	public bool IsReadOnly => ((ICollection<T>)mQueue).IsReadOnly;

	public void Add(T item)
	{
		mQueue.AddToBack(item);
		if(mQueue.Count > mMaxSize)
		{
			mQueue.RemoveFromFront();
		}
	}

	public void Clear()
	{
		mQueue.Clear();
	}

	public bool Contains(T item)
	{
		return ((ICollection<T>)mQueue).Contains(item);
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		((ICollection<T>)mQueue).CopyTo(array, arrayIndex);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return mQueue.GetEnumerator();
	}

	public int IndexOf(T item)
	{
		return mQueue.IndexOf(item);
	}

	public void Insert(int index, T item)
	{
		mQueue.Insert(index, item);
	}

	public bool Remove(T item)
	{
		return mQueue.Remove(item);
	}

	public void RemoveAt(int index)
	{
		mQueue.RemoveAt(index);
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return ((System.Collections.IEnumerable)mQueue).GetEnumerator();
	}
}

