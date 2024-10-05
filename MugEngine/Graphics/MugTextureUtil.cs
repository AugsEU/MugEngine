namespace MugEngine.Graphics
{
	/// <summary>
	/// Utility functions for textures.
	/// Warning: These are slow!
	/// </summary>
	public static class MugTextureUtil
	{
		/// <summary>
		/// Memcpy a texture
		/// </summary>
		public static void MemCopyTexture(GraphicsDevice graphics, Texture2D source, Texture2D dest)
		{
			// Recreate texture if size is different
			if (dest is null || dest.Width != source.Width || dest.Height != source.Height)
			{
				dest = new Texture2D(graphics, source.Width, source.Height);
			}

			int count = source.Width * source.Height;
			Color[] data = new Color[count];
			source.GetData<Color>(data);
			dest.SetData(data);
		}



		/// <summary>
		/// Create texture from memcpy
		/// </summary>
		public static Texture2D MemCopyTexture(GraphicsDevice graphics, Texture2D source)
		{
			// Recreate texture if size is different
			Texture2D dest = new Texture2D(graphics, source.Width, source.Height);

			int count = source.Width * source.Height;
			Color[] data = new Color[count];
			source.GetData<Color>(data);
			dest.SetData(data);

			return dest;
		}



		/// <summary>
		/// Make texture greyscale
		/// </summary>
		public static Texture2D MakeTextureGreyscale(GraphicsDevice graphics, Texture2D source)
		{
			// Recreate texture
			Texture2D dest = new Texture2D(graphics, source.Width, source.Height);
			int count = source.Width * source.Height;
			Color[] data = new Color[count];
			source.GetData<Color>(data);

			// Make greyscale
			for (int i = 0; i < count; i++)
			{
				Color col = data[i];
				float lum = 0.2126f * col.R + 0.7152f * col.G + 0.0722f * col.B;
				lum = Math.Clamp(lum, 0.0f, 255.0f) / 255.0f;
				data[i] = new Color(lum, lum, lum, col.A / 255.0f);
			}

			// Save to dest
			dest.SetData(data);

			return dest;
		}
	}
}
