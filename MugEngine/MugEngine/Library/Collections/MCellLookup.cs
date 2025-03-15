namespace MugEngine.Library;

public class MCellLookup<T> where T : class, IMBounds
{
	#region rMembers

	Dictionary<Point, List<T>> mCellToItems;
	Dictionary<T, List<Point>> mItemToCells;
	Point mCellSize;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create cell look up.
	/// </summary>
	/// <param name="cellSize">How big each cell is. Should be about 4x the size of your average entity.</param>
	public MCellLookup(Point cellSize)
	{
		mCellToItems = new Dictionary<Point, List<T>>();
		mItemToCells = new Dictionary<T, List<Point>>();
		mCellSize = cellSize;
	}

	#endregion rInit





	#region rModify

	/// <summary>
	/// Insert an item into the cell lookup.
	/// </summary>
	public void Insert(T item)
	{
		Rectangle bounds = item.BoundsRect();
		List<Point> cells = GetCellsInRect(bounds);

		mItemToCells.Add(item, cells);

		foreach(Point cell in cells)
		{
			if (mCellToItems.TryGetValue(cell, out List<T> cellItems))
			{
				cellItems.Add(item);
			}
			else
			{
				List<T> newItems = new List<T>();
				newItems.Add(item);
				mCellToItems.Add(cell, newItems);
			}
		}
	}



	/// <summary>
	/// Remove an item from the cell lookup.
	/// </summary>
	public void Remove(T item)
	{
		if (mItemToCells.TryGetValue(item, out List<Point> cells))
		{
			foreach (Point cell in cells)
			{
				if (mCellToItems.TryGetValue(cell, out List<T> items))
				{
					items.Remove(item);
					if (items.Count == 0)
					{
						mCellToItems.Remove(cell);
					}
				}
			}

			mItemToCells.Remove(item);
		}
		else
		{
			throw new Exception("Item not part of cell lookup. Cannot be removed.");
		}
	}



	/// <summary>
	/// Update an item to reflect it's new position.
	/// </summary>
	public void UpdateItem(T item)
	{
		Remove(item);
		Insert(item);
	}

	#endregion rModify





	#region rQuery

	/// <summary>
	/// Get list of cell points in a rectangle.
	/// </summary>
	public List<Point> GetCellsInRect(Rectangle rect)
	{
		// Top left in cell space:
		Point startCellPt = rect.Location / mCellSize;
		Point endCellPt = (rect.Location + new Point(rect.Size.X - 1, rect.Size.Y - 1)) / mCellSize;

		int reserveCapacity = (endCellPt.X - startCellPt.X + 1) * (endCellPt.Y - startCellPt.Y + 1);
		List<Point> cells = new List<Point>(reserveCapacity);

		for (int x = startCellPt.X; x <= endCellPt.X; ++x)
		{
			for (int y = startCellPt.Y; y <= endCellPt.Y; ++y)
			{
				cells.Add(new Point(x, y));
			}
		}

		return cells;
	}



	/// <summary>
	/// Find all cells intersecting this bounding box.
	/// </summary>
	public HashSet<T> Query(Rectangle bounds)
	{
		HashSet<T> result = new HashSet<T>();
		List<Point> cellsToCheck = GetCellsInRect(bounds);

		foreach (Point cell in cellsToCheck)
		{
			if (mCellToItems.TryGetValue(cell, out List<T> items))
			{
				foreach (T item in items)
				{
					if (item.BoundsRect().Intersects(bounds))
					result.Add(item);
				}
			}
		}

		return result;
	}



	/// <summary>
	/// Find if any cell instersects this bounding box
	/// </summary>
	public bool QueryCollides(Rectangle bounds)
	{
		List<Point> cellsToCheck = GetCellsInRect(bounds);

		foreach (Point cell in cellsToCheck)
		{
			if (mCellToItems.TryGetValue(cell, out List<T> items))
			{
				foreach (T item in items)
				{
					if (item.BoundsRect().Intersects(bounds))
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	#endregion rQuery
}
