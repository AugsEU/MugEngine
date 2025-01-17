namespace MugEngine.Scene
{
	public interface IMTileFactory
	{
		public (MTile, string) GenerateTile(int tileId, int rotation, int param);

		public MTile GenerateDummyTile();
	}
}
