
using Microsoft.Xna.Framework.Input;

namespace MugEngine.Library;

public class MEnumDictionary<E, T> : IDictionary<E, T> where E : Enum
{
	readonly T[] mValues;
	readonly int mLength;
	readonly int mOffset;
	readonly E[] mEnumValues;

	public MEnumDictionary()
	{
		if (!typeof(E).IsEnum)
		{
			throw new ArgumentException("E must be an enumerated type");
		}

		mEnumValues = (E[])Enum.GetValues(typeof(E));
		mLength = mEnumValues.Length;

		// Check if enum is too large (more than 1024 values)
		if (mLength > 1024)
		{
			throw new ArgumentException($"Enum {typeof(E).Name} has {mLength} values, which exceeds the maximum allowed size of 1024");
		}

		mOffset = Convert.ToInt32(mEnumValues[0]);
		mValues = new T[mLength];

		// Initialize all values with default(T)
		for (int i = 0; i < mLength; i++)
		{
			mValues[i] = default!;
		}
	}

	private int EnumToIndex(E enumVal)
	{
		return Convert.ToInt32(enumVal) - mOffset;
	}

	public T this[E key]
	{
		get
		{
			return mValues[EnumToIndex(key)];
		}
		set
		{
			mValues[EnumToIndex(key)] = value;
		}
	}

	public ICollection<E> Keys => mEnumValues;

	public ICollection<T> Values => mValues;

	public int Count => mLength;

	public bool IsReadOnly => false;

	public void Add(E key, T value)
	{
		mValues[EnumToIndex(key)] = value;
	}

	public void Add(KeyValuePair<E, T> item)
	{
		Add(item.Key, item.Value);
	}

	public void Clear()
	{
		for (int i = 0; i < mLength; i++)
		{
			mValues[i] = default!;
		}
	}

	public bool Contains(KeyValuePair<E, T> item)
	{
		return true;
	}

	public bool ContainsKey(E key)
	{
		return true;
	}

	public void CopyTo(KeyValuePair<E, T>[] array, int arrayIndex)
	{
		for (int i = 0; i < mLength; i++)
		{
			array[arrayIndex + i] = new KeyValuePair<E, T>(mEnumValues[i], mValues[i]);
		}
	}

	public IEnumerator<KeyValuePair<E, T>> GetEnumerator()
	{
		for (int i = 0; i < mLength; i++)
		{
			yield return new KeyValuePair<E, T>(mEnumValues[i], mValues[i]);
		}
	}

	public bool Remove(E key)
	{
		int index = EnumToIndex(key);

		mValues[index] = default!;
		return true;
	}

	public bool Remove(KeyValuePair<E, T> item)
	{
		int index = EnumToIndex(item.Key);

		mValues[index] = default!;
		return true;
	}

	public bool TryGetValue(E key, out T value)
	{
		value = mValues[EnumToIndex(key)];
		return true;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
