using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MugEngine.Library;

public interface IMGraphNode<T> where T : class
{
	float PathNeighbourDistance(T other);

	float PathHeuristic(T other);

	IEnumerable<T> PathNeighbours();
}
