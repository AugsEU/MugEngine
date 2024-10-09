namespace MugEngine.Graphics
{
	/// <summary>
	/// Options to start a spritebatch.
	/// </summary>
	public struct MSpriteBatchOptions
	{
		public SpriteSortMode mSortMode;
		public BlendState mBlend;
		public SamplerState mSamplerState;
		public DepthStencilState mDepthStencilState;
		public RasterizerState mRasterizerState;
		public Effect mEffect;

		public MSpriteBatchOptions()
		{
			mSortMode = SpriteSortMode.Texture;
			mBlend = BlendState.AlphaBlend;
			mSamplerState = SamplerState.PointClamp;
			mDepthStencilState = DepthStencilState.Default;
			mRasterizerState = RasterizerState.CullNone;
			mEffect = null;
		}

		public bool Equals(ref MSpriteBatchOptions other)
		{
			return mSortMode == other.mSortMode &&
			   ReferenceEquals(mBlend, other.mBlend) &&
			   ReferenceEquals(mSamplerState, other.mSamplerState) &&
			   ReferenceEquals(mDepthStencilState, other.mDepthStencilState) &&
			   ReferenceEquals(mRasterizerState, other.mRasterizerState) &&
			   ReferenceEquals(mEffect, other.mEffect);
		}
	}
}
