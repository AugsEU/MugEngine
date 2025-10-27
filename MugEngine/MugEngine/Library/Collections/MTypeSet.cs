
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MugEngine.Library;


/// <summary>
/// Contains objects but only one per type.
/// </summary>
public class MTypeSet<T>
{
	public Dictionary<Type, T> mObjects;

	public MTypeSet()
	{
		mObjects = new Dictionary<Type, T>();
	}

	public int Count => mObjects.Count;
	
	/// <summary>
	/// Try adding an item to the set. Might fail.
	/// </summary>
	public bool Add(T item)
	{
		Type newItemType = item.GetType();
		if (mObjects.ContainsKey(newItemType))
		{
			return false;
		}

		mObjects.Add(newItemType, item);
		return true;
	}

	public void Clear()
	{
		mObjects.Clear();
	}

	public bool Contains(T obj)
	{
		return mObjects.ContainsKey(obj.GetType());
	}

	public bool ContainsType(Type t)
	{
		return mObjects.ContainsKey(t);
	}

	public bool ContainsType<U>() where U : T
	{
		return mObjects.ContainsKey(typeof(U));
	}

	public IEnumerator<T> GetEnumerator()
	{
		foreach(var keyItem in mObjects)
		{
			yield return keyItem.Value;
		}
	}

	public bool Remove(T item)
	{
		return mObjects.Remove(item.GetType());
	}

	public bool Remove<U>() where U : T
	{
		return mObjects.Remove(typeof(U));
	}
}
