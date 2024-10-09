using MugEngine.Collections;

namespace MugEngine.Graphics
{


	public class MDrawStrings : MDrawCollection
	{
		MStructArray<MStringDrawData> mDatas;

		public MDrawStrings(int reserveSize = 4096)
		{
			mDatas = new MStructArray<MStringDrawData>(reserveSize);
		}

		public void AddStringDraw(ref MStringDrawData data)
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
			for (int i = 0; i < mDatas.Count; i++)
			{
				MStringDrawData data = mDatas[i];
				sb.DrawString(data.mFont,
					data.mText,
					data.mPosition,
					data.mColor,
					data.mRotation,
					data.mOrigin,
					data.mScale,
					data.mEffects,
					0.0f);
			}
		}

		protected override int GetCount()
		{
			return mDatas.Count;
		}
	}
}
