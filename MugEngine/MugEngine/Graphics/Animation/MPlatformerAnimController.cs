namespace MugEngine.Graphics
{
	public class MPlatformerAnimController : IMUpdate
	{
		MAnimation mIdleAnim;
		MAnimation mRiseAnim;
		MAnimation mFallAnim;
		MAnimation mRunAnim;

		public MPlatformerAnimController(MAnimation idleAnim, 
										MAnimation riseAnim, 
										MAnimation fallAnim, 
										MAnimation runAnim)
		{
			mIdleAnim = idleAnim;
			mRiseAnim = riseAnim;
			mFallAnim = fallAnim;
			mRunAnim = runAnim;
		}

		public MPlatformerAnimController(string idleAnim,
											string riseAnim,
											string fallAnim,
											string runAnim)
		{
			mIdleAnim = MData.I.LoadAnimation(idleAnim);
			mRiseAnim = MData.I.LoadAnimation(riseAnim);
			mFallAnim = MData.I.LoadAnimation(fallAnim);
			mRunAnim = MData.I.LoadAnimation(runAnim);
		}

		public MAnimation GetCurrentAnimation(Vector2 velocity, bool onGround, MCardDir gravityDir)
		{
			float xSpeed = Math.Abs(Vector2.Dot(velocity, gravityDir.ToVec().Perpendicular()));
			float yVel = Vector2.Dot(velocity, gravityDir.ToVec());


			if (onGround)
			{
				if (xSpeed > 0.005f)
				{
					return mRunAnim;
				}

				return mIdleAnim;
			}
			else
			{
				if (yVel > 0.0f)
				{
					return mFallAnim;
				}

				return mRiseAnim;
			}
		}

		public void Update(MUpdateInfo info)
		{
			mIdleAnim.Update(info);
			mRiseAnim.Update(info);
			mRunAnim.Update(info);
			mFallAnim.Update(info);
		}
	}
}
