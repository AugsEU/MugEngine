namespace MugEngine.Graphics
{
	/// <summary>
	/// Options to start a spritebatch.
	/// </summary>
	public struct MSpriteBatchOptions
	{
		public MSpriteBatchOptions()
		{
			mSortMode = SpriteSortMode.FrontToBack;
			mBlend = BlendState.AlphaBlend;
			mSamplerState = SamplerState.PointClamp;
			mDepthStencilState = DepthStencilState.Default;
			mRasterizerState = RasterizerState.CullNone;
		}

		public SpriteSortMode mSortMode;
		public BlendState mBlend;
		public SamplerState mSamplerState;
		public DepthStencilState mDepthStencilState;
		public RasterizerState mRasterizerState;
	}
}
