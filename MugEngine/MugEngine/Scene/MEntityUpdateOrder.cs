namespace MugEngine.Scene
{
	/// <summary>
	/// Comparer for sorting by update order
	/// </summary>
	public class MEntityUpdateOrderComparer : IComparer<MEntity>
	{
		public int Compare(MEntity x, MEntity y)
		{
			return x.UpdateOrder().CompareTo(y.UpdateOrder());
		}
	}



	/// <summary>
	/// Update orders
	/// </summary>
	static public class MEntityUpdateOrder
	{
		public const int DEFAULT_ORDER = 0;

		// We want this to update at the end.
		public const int PHYSICS_WORLD = 32;
	}
}
