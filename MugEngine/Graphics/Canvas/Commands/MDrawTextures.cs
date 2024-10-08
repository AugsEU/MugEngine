using MugEngine.Collections;

namespace MugEngine.Graphics
{
	public class MDrawTextures : MDrawCollection
	{
		MStructArray<MTextureDrawData> mDatas;

		public MDrawTextures(int reserveSize = 4096)
		{
			mDatas = new MStructArray<MTextureDrawData>(reserveSize);
		}

		public void AddTextureDraw(ref MTextureDrawData data)
		{
			mDatas.Add(data);
		}

		public override void Clear()
		{
			mDatas.Clear();
			base.Clear();
		}

		protected override void DrawIndex(SpriteBatch sb, int index)
		{
			MTextureDrawData data = mDatas[index];
			sb.Draw(data.mTexture,
				data.mPosition,
				data.mSourceRectangle,
				data.mColor,
				data.mRotation,
				data.mOrigin,
				data.mScale,
				data.mEffects,
				0.0f);
		}

		protected override int GetCount()
		{
			return mDatas.Count;
		}
	}
}
