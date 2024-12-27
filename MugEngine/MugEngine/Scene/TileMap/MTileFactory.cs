namespace MugEngine.Scene
{
	public abstract class MTileFactory
	{
		public abstract MTile GenerateTile(Point size, int tileId, int rotation, int param);

		public abstract MTile GenerateDummyTile(Point size);
	}
}
