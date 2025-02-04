using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace MugEngine.Library
{
	/// <summary>
	/// A collection of classes with unique types that we "select" one of.
	/// </summary>
	public class MSelector<T> where T : class
	{
		T mCurrSelected;
		MHandle<T> mCurrHandle;
		Dictionary<MHandle<T>, T> mHandleToClasses;

		public MSelector()
		{
			mHandleToClasses = new();
		}

		public void Add<S>(S item) where S : T
		{
			mHandleToClasses.Add(MHandle<T>.From<S>(), item);
		}

		public T GetCurr()
		{
			return mCurrSelected;
		}

		public MHandle<T> GetCurrHandle()
		{
			return mCurrHandle;
		}

		public void SetCurr(MHandle<T> newType)
		{
			mCurrHandle = newType;
		
			if (mHandleToClasses.TryGetValue(newType, out T value))
			{
				mCurrSelected = value;
			}
			else
			{
				MugDebug.Break("New state is not valid.");
			}
		}
	}
}
