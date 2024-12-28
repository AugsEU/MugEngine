namespace MugEngine.Scene
{
	public interface IMCollisionQueryable
	{
		public bool QueryCollides(Rectangle bounds, MCardDir travelDir);
	}
}
