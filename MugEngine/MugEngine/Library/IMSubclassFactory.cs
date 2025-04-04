namespace MugEngine.Library;


/// <summary>
/// A factory that can create any subclass of T
/// </summary>
public interface IMSubclassFactory<T>
{
	public U CreateNew<U>() where U : T, new();
}
