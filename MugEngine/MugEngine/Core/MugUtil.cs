namespace MugEngine.Core
{
	/// <summary>
	/// Misc methods for the mug engine
	/// </summary>
	static internal class MugUtil
	{
		/// <summary>
		/// Convert gameTime object to milliseconds
		/// </summary>
		static public float ToDelta(GameTime gameTime)
		{
			return (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
	}
}
