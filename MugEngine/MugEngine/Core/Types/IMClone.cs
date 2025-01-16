namespace MugEngine.Core
{
	public interface IMClone<T> where T : IMClone<T>
	{
		public T Clone();
	}
}
