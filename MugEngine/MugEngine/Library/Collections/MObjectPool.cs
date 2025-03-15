using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MugEngine.Library;

internal class MPoolList<T> where T : class, IMObjectPoolItem
{
	int mItemCount = 0;
	T[] mItems;

	public MPoolList()
	{
		mItems = new T[8];
	}

	public T GetFreshInstance<U>() where U : T, new()
	{
		if(mItems.Length <= mItemCount)
		{
			T[] newArr = new T[mItems.Length * 2];
			Array.Copy(mItems, newArr, mItems.Length);
			mItems = newArr;
		}

		T newItem = null;
		if (mItems[mItemCount] is null) // Need a new entity
		{
			newItem = new U();
			mItems[mItemCount] = newItem;
		}
		else
		{
			// Can reuse old entity
			newItem = mItems[mItemCount];
		}

		newItem.OnCreate();
		mItemCount += 1;

		return newItem;
	}

	public void Destroy(int index)
	{
		MugDebug.Assert(0 <= index && index < mItemCount, "Invalid index {0}", index);
		mItems[index].OnDestroy();

		if (index < mItemCount - 1)
		{
			// Swap end and item to delete
			T temp = mItems[mItemCount - 1];
			mItems[mItemCount - 1] = mItems[index];
			mItems[index] = temp;
		}

		mItemCount -= 1;
	}

	public void Destroy(T item)
	{
		for(int i = 0; i < mItemCount; i++)
		{
			if (mItems[i] == item)
			{
				Destroy(item);
			}
		}

		MugDebug.Assert(false, "Failed to find object {0}", item.ToString());
	}

	public IEnumerable<T> Enumerate()
	{
		for(int i = 0; i < mItems.Count(); i++)
		{
			yield return mItems[i];
		}
	}
}


/// <summary>
/// Thing which pools objects. Instead of creating and destroying them it reuses them.
/// </summary>
class MObjectPool<T> where T : class, IMObjectPoolItem
{
	Dictionary<Type, MPoolList<T>> mTypeToPoolLists;

	public MObjectPool()
	{
		mTypeToPoolLists = new();
	}

	public U GetFreshInstance<U>() where U : class, T, new()
	{
		Type uType = typeof(U);
		
		if (!mTypeToPoolLists.TryGetValue(uType, out MPoolList<T> poolList))
		{
			poolList = new MPoolList<T>();
			mTypeToPoolLists.Add(uType, poolList);
		}

		T freshItem = poolList.GetFreshInstance<U>();

		return Unsafe.As<U>(freshItem);
	}

	public void Destroy(T item)
	{
		Type itemType = item.GetType();
		if (mTypeToPoolLists.TryGetValue(itemType, out MPoolList<T> poolList))
		{
			poolList.Destroy(item);
		}
		else
		{
			MugDebug.Assert(false, "Cannot find item {0}", item.ToString());
		}
	}


	public IEnumerable<T> Enumerate()
	{
		foreach(KeyValuePair<Type, MPoolList<T>> kv in mTypeToPoolLists)
		{
			foreach(T item in kv.Value.Enumerate())
			{
				yield return item;
			}
		}
	}
}
