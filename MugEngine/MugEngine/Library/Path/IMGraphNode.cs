namespace MugEngine.Library;

public interface IMGraphNode<T>
{
	float PathNeighbourDistance(T other);

	float PathHeuristic(T other);

	IEnumerable<T> PathNeighbours();

	bool IsSameNodeAs(T other);
}
