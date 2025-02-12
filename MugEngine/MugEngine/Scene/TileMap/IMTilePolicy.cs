namespace MugEngine.Scene
{
	public interface IMTilePolicy
	{
		public string GetTileAnimPath(MTile tile, int param);

		public bool QueryTileCollision(MTile tile, Rectangle tileRect, Rectangle objectRect);
	}
}
