using System.Diagnostics.CodeAnalysis;

namespace MugEngine.Core;

/// <summary>
/// Wraps a type class such that the type must inherit from
/// a the given type.
/// </summary>
/// <typeparam name="T">The thing of which the type must inherit from</typeparam>
public struct MHandle<T> : IEquatable<MHandle<T>>
	where T : class
{
	readonly Type mType;

	public MHandle(Type type)
	{
		MugDebug.Assert(type != null && type.IsSubclassOf(typeof(T)));
		mType = type;
	}

	public static implicit operator MHandle<T>(Type type)
	{
		return new MHandle<T>(type);
	}

	public static MHandle<T> From<S>() where S : T
	{
		return new MHandle<T>(typeof(S));
	}

	Type ToType()
	{
		return mType;
	}

	public override int GetHashCode()
	{
		return mType.GetHashCode();
	}

	public bool Equals(MHandle<T> other)
	{
		return other.mType.Equals(mType);
	}

	public override string ToString()
	{
		return string.Format("Handle of {0}", mType.ToString());
	}
}

