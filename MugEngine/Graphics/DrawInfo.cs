using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace MugEngine.Graphics
{
	/// <summary>
	/// Info needed to draw
	/// </summary>
	public struct DrawInfo
	{
		public GameTime mGameTime;
		public SpriteBatch mSpriteBatch;
		public GraphicsDeviceManager mGraphics;
		public GraphicsDevice mDevice;

		public DrawInfo()
		{
			mGameTime = null;
			mSpriteBatch = null;
			mGraphics = null;
			mDevice = null;
		}
	}
}
