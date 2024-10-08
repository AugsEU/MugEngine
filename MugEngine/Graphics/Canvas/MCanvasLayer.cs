namespace MugEngine.Graphics
{
	class MCanvasLayer
	{
		MDrawTextures mDrawTextures;
		MDrawStrings mDrawStrings;

		public MCanvasLayer()
		{
			mDrawStrings = new MDrawStrings();
			mDrawTextures = new MDrawTextures();
		}

		public void DrawLayer(SpriteBatch sb, Matrix viewport)
		{
			mDrawTextures.DrawAll(sb, viewport);
			mDrawStrings.DrawAll(sb, viewport);

			mDrawTextures.Clear();
			mDrawStrings.Clear();
		}

		public void DrawTexture(ref MTextureDrawData data)
		{
			mDrawTextures.AddTextureDraw(ref data);
		}

		public void DrawString(ref MStringDrawData data)
		{
			mDrawStrings.AddStringDraw(ref data);
		}
	}
}
