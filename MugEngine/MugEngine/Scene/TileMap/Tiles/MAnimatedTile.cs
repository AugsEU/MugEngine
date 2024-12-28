namespace MugEngine.Scene
{
	public class MAnimatedTile : MTile
	{
		MAnimation mAnim;

		public MAnimatedTile(string animPath)
		{
			mAnim = MData.I.LoadAnimation(animPath);
		}

		public override void Update(MScene scene, MUpdateInfo info)
		{
			mAnim.Update(info);
			base.Update(scene, info);
		}

		public override MTexturePart GetTexture()
		{
			return mAnim.GetCurrentTexture();
		}
	}
}
