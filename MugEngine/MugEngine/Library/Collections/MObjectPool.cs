using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace MugEngine.Library;

/// <summary>
/// A list containing an array with entities of a single type.
/// We can request a fresh instance which resuses an existing instance.
/// </summary>
internal class MPoolList<T> where T : class, IMObjectPoolItem
{
	#region rMembers

	int mItemCount = 0;
	T[] mItems;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create new pool list.
	/// </summary>
	public MPoolList()
	{
		mItems = new T[8];
	}

	#endregion rInit





	#region rCollection

	/// <summary>
	/// Get a new instance of a the pool type.
	/// </summary>
	public U GetFreshInstance<U>(IMSubclassFactory<T> factory) where U : class, T, new()
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
			newItem = factory.CreateNew<U>();
			mItems[mItemCount] = newItem;
		}
		else
		{
			// Can reuse old entity
			newItem = mItems[mItemCount];
		}

		newItem.OnCreate();
		mItemCount += 1;

		MugDebug.Assert(VerifyItemTypes(), "Mixed types in object pool. They should all be the same types.");
		return Unsafe.As<U>(newItem);
	}



	/// <summary>
	/// Deactivate the instance at index.
	/// Keeps reference so it can be used again later.
	/// </summary>
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



	/// <summary>
	/// Deactivate the instance.
	/// Keeps reference so it can be used again later.
	/// </summary>
	public void Destroy(T item)
	{
		for(int i = 0; i < mItemCount; i++)
		{
			if (mItems[i] == item)
			{
				Destroy(item);
				return;
			}
		}

		MugDebug.Assert(false, "Failed to find object {0}", item.ToString());
	}



	/// <summary>
	/// Deactivate all instances.
	/// </summary>
	public void Clear()
	{
		for (int i = 0; i < mItemCount; i++)
		{
			mItems[i].OnDestroy();
		}

		mItemCount = 0;
	}

	#endregion rCollection





	#region rUtil

	/// <summary>
	/// Get enumerator for this.
	/// </summary>
	public IEnumerable<T> GetEnumerator()
	{
		for(int i = 0; i < mItemCount; i++)
		{
			yield return mItems[i];
		}
	}



	/// <summary>
	/// Verify that all elements are of the same type.
	/// </summary>
	private bool VerifyItemTypes()
	{
#if DEBUG
		if (mItemCount == 0)
			return true;

		Type baseType = mItems[0].GetType();
		for (int i = 1; i < mItemCount; i++)
		{
			if (mItems[i].GetType() != baseType)
			{
				return false;
			}
		}

		return true;
#else
		return true;
#endif
	}

	#endregion rUtil
}


/// <summary>
/// Thing which pools objects. Instead of creating and destroying them it reuses them.
/// </summary>
class MObjectPool<T> where T : class, IMObjectPoolItem
{
	#region rMembers

	Dictionary<Type, MPoolList<T>> mTypeToPoolLists;

	#endregion rMembers




	#region rInit

	/// <summary>
	/// Create object pool that can hold items of a type.
	/// </summary>
	public MObjectPool()
	{
		mTypeToPoolLists = new();
	}

	#endregion rInit





	#region rCollection

	/// <summary>
	/// Get an instance of the type we can use.
	/// </summary>
	public U GetFreshInstance<U>(IMSubclassFactory<T> factory) where U : class, T, new()
	{
		Type uType = typeof(U);

		if (!mTypeToPoolLists.TryGetValue(uType, out MPoolList<T> poolList))
		{
			poolList = new MPoolList<T>();
			mTypeToPoolLists.Add(uType, poolList);
		}

		U freshItem = poolList.GetFreshInstance<U>(factory);

		return freshItem;
	}



	/// <summary>
	/// Deactivate an instance.
	/// </summary>
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



	/// <summary>
	/// Clear all the entities.
	/// </summary>
	public void Clear()
	{
		foreach(var kv in mTypeToPoolLists)
		{
			kv.Value.Clear();
		}
	}

	#endregion rCollection





	#region rUtil

	/// <summary>
	/// Enumerate the pool.
	/// </summary>
	public IEnumerable<T> GetEnumerator()
	{
		foreach (KeyValuePair<Type, MPoolList<T>> kv in mTypeToPoolLists)
		{
			foreach (T item in kv.Value.GetEnumerator())
			{
				yield return item;
			}
		}
	}

	#endregion rUtil
}
