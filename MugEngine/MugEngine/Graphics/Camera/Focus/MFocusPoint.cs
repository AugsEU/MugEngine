
namespace MugEngine.Graphics
{
	/// <summary>
	/// Focus a point while lerping over to it.
	/// </summary>
	public class MFocusPoint : MCameraFocus
	{
		IMEase mEasing;
		MPercentTimer mPercentTimer;
		Vector2 mTarget;

		public MFocusPoint(Vector2 target, float lerpTime, IMEase easing) : base()
		{
			mTarget = target;
			mPercentTimer = new MPercentTimer(lerpTime);
			mEasing = easing;
			mPercentTimer.Start();
		}

		public override MCameraSpec UpdateFocusPoint(MUpdateInfo info, MCameraSpec curr)
		{
			mPercentTimer.Update(info);

			float t = mEasing.Eval(mPercentTimer.GetPercentage());

			curr.mPosition = MugMath.Lerp(GetStartSpec().mPosition, mTarget, t);

			return curr;
		}
	}
}
