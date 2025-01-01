namespace MugEngine.Core
{
	/// <summary>
	/// Wraps a type class such that the type must inherit from
	/// a the given type.
	/// </summary>
	/// <typeparam name="P">Parent class type</typeparam>
	/// <typeparam name="T">The thing of which the type must inherit from</typeparam>
	public class MTypeHandle<T> : IEquatable<MTypeHandle<T>>
		where T : class
	{
		readonly Type mType;

		public MTypeHandle(Type type)
		{
			MugDebug.Assert(type != null && type.IsSubclassOf(typeof(T)));
			mType = type;
		}

		Type ToType()
		{
			return mType;
		}

		public override int GetHashCode()
		{
			return mType.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MTypeHandle<T>);
		}

		public bool Equals(MTypeHandle<T> other)
		{
			return other.mType.Equals(mType);
		}

		public override string ToString()
		{
			return string.Format("Handle of {0}", mType.ToString());
		}
	}
}
