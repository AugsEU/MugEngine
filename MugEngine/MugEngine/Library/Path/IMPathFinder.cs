namespace MugEngine.Library;

interface IMPathFinder<T> where T : class, IMGraphNode<T>
{
	MPathResults<T>? FindPath(T start, T end);
}
