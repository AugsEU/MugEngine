namespace MugEngine.Scene
{
	public abstract class MTileFactory
	{
		public abstract MTile GenerateTile(int tileId, int rotation, int param);

		public abstract MTile GenerateDummyTile();
	}
}
