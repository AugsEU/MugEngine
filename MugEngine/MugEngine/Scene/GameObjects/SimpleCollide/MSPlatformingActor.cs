﻿

namespace MugEngine.Scene;

abstract public class MSPlatformingActor : MSPhysicalActor
{
	#region rConst

	const float DEFAULT_GRAVITY = 420.0f;
	const float DEFAULT_FAST_FALL = DEFAULT_GRAVITY * 0.5f;

	#endregion rConst





	#region rMembers

	MWalkDir mFacingDir;
	MSSolid mOnlyStandingSolid = null;
	bool mOnGround;
	MWalkDir? mWallSlideCache;
	protected float mFastFallStr = DEFAULT_FAST_FALL; // Amount added by fast fall.
	bool mInFastFall;

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Create platfomer at position.
	/// </summary>
	public MSPlatformingActor()
	{
		mGravityStrength = DEFAULT_GRAVITY;
		mInFastFall = false;
	}



	/// <summary>
	/// Called after the object is fully constructed.
	/// </summary>
	public override void PostInitSetup()
	{
		PushUntilOnGround();
		base.PostInitSetup();
	}

	#endregion rInit





	#region rUpdate

	/// <summary>
	/// Update platforming entity
	/// </summary>
	public override void Update(MUpdateInfo info)
	{
		mOnlyStandingSolid = null;
		mOnGround = GroundCheck();

		if (mOnGround)
		{
			mInFastFall = false;
		}
		else if (mInFastFall)
		{
			mVelocity += mGravityDir.ToVec() * mFastFallStr * info.mDelta;
		}

		base.Update(info);

		mWallSlideCache = null;
	}


	/// <summary>
	/// Are we riding this solid?
	/// </summary>
	public override bool IsRiding(MSSolid solid)
	{
		if (mOnlyStandingSolid == null)
		{
			Rectangle myShiftedBounds = BoundsRect();
			myShiftedBounds.Location += mGravityDir.ToPoint();

			if (solid.QueryCollides(myShiftedBounds, mGravityDir, 0))
			{
				mOnlyStandingSolid = solid;
				return true;
			}
		}

		return false;
	}



	/// <summary>
	/// Make a jump.
	/// </summary>
	public void Jump(float speed)
	{
		mVelocity.Y = -speed;
		mInFastFall = false;
	}



	/// <summary>
	/// Begin fast falling.
	/// </summary>
	public void BeginFastFall(float strength = DEFAULT_FAST_FALL)
	{
		if (mInFastFall)
		{
			//Already fast falling.
			return;
		}

		mInFastFall = true;

		mFastFallStr = strength;
		Vector2 gravVec = mGravityDir.ToVec();
		mVelocity -= Vector2.Dot(gravVec, mVelocity) * gravVec;

		mVelocity += mGravityDir.ToVec() * mFastFallStr * 1.5f;
	}



	/// <summary>
	/// Walk in a direction.
	/// </summary>
	public void WalkIn(MWalkDir dir, float speed)
	{
		Vector2 walkVec = dir.ToVec(mGravityDir);
		Vector2 componentInWalkVec = Vector2.Dot(mVelocity, walkVec) * walkVec;

		if (dir == MWalkDir.None)
		{
			mVelocity = mVelocity - componentInWalkVec;
		}
		else
		{
			mVelocity = mVelocity - componentInWalkVec + walkVec * speed;
			mFacingDir = dir;
		}
	}



	/// <summary>
	/// Make the actor face in a dir.
	/// </summary>
	public void FaceIn(MWalkDir dir)
	{
		mFacingDir = dir;
	}



	/// <summary>
	/// Walk in a direction.
	/// </summary>
	public void DriftIn(MWalkDir dir, float dv, float maxSpeed)
	{
		if (dir == MWalkDir.None)
		{
			Vector2 walkVec = GetFacingDir().ToVec(mGravityDir);
			float componentInWalkVec = Vector2.Dot(mVelocity, walkVec);

			float reducedCompInWalkVec = MugMath.MoveToZero(componentInWalkVec, dv * 0.7f);

			mVelocity = mVelocity + (reducedCompInWalkVec - componentInWalkVec) * walkVec;
		}
		else
		{
			Vector2 walkVec = dir.ToVec(mGravityDir);

			mVelocity = mVelocity + walkVec * dv;

			float cappedLength = MugMath.ClampAbs(Vector2.Dot(mVelocity, walkVec), maxSpeed);

			mVelocity = mVelocity + (cappedLength - Vector2.Dot(mVelocity, walkVec)) * walkVec;
		}
	}



	/// <summary>
	/// Apply wall slide friction
	/// </summary>
	public void ApplyWallFriction(MUpdateInfo info, float friction)
	{
		float vertSpeed = GetVertSpeed(); // Negative speed is up.

		vertSpeed = MugMath.MoveToZero(vertSpeed, friction * info.mDelta);

		SetVertSpeed(vertSpeed);
	}



	/// <summary>
	/// Set the direction of facing from the velocity.
	/// </summary>
	public void SetFacingDirFromVelocity(float thresh = 0.1f)
	{
		Vector2 faceVec = mFacingDir.ToVec(mGravityDir);
		float compInFacing = Vector2.Dot(faceVec, mVelocity);

		if (compInFacing < -thresh)
		{
			mFacingDir = mFacingDir.Inverted();
		}
	}

	#endregion rUpdate





	#region rDraw


	/// <summary>
	/// Draw a platformer with the correct position with regard to the bounds.
	/// </summary>
	public void DrawPlatformer(MDrawInfo info, MTexturePart texture, int layer)
	{
		Rectangle bounds = BoundsRect();

		Vector2 footPoint = new Vector2((bounds.Left + bounds.Right) / 2, bounds.Bottom);
		Vector2 texTopLeft = footPoint - new Vector2(texture.mUV.Width / 2, texture.mUV.Height);

		SpriteEffects spriteFlip = mFacingDir == MWalkDir.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

		info.mCanvas.DrawTexture(texture, texTopLeft, spriteFlip, layer);
	}

	#endregion rDraw





	#region rCollision

	/// <summary>
	/// React to solid.
	/// </summary>
	/// <param name="normal"></param>
	public override void OnHitSolid(MCardDir normal)
	{
		if (normal == MCardDir.Up)
		{
			mVelocity.Y = 0;
		}

		base.OnHitSolid(normal);
	}



	/// <summary>
	/// Are we on the ground?
	/// </summary>
	public bool OnGround()
	{
		return mOnGround;
	}



	/// <summary>
	/// Makes sure thing is seated on ground.
	/// </summary>
	public void PushUntilOnGround()
	{
		bool success = TryPushOutOfCollision(mGravityDir.Inverted());
		//MugDebug.Assert(success);

		success = TryPushIntoCollision(mGravityDir);
		//MugDebug.Assert(success);
	}

	#endregion rCollision





	#region rUtil

	/// <summary>
	/// Get direction actor is facing.
	/// </summary>
	public MWalkDir GetFacingDir()
	{
		return mFacingDir;
	}






	/// <summary>
	/// Check if we are on the ground.
	/// Run only once per frame then cached.
	/// </summary>
	public bool GroundCheck()
	{
		if (GetVertSpeed() < 0.0f)
		{
			return false;
		}

		Rectangle myShiftedBounds = BoundsRect();
		myShiftedBounds.Location += mGravityDir.ToPoint();

		return CollidesWithAnySolid(myShiftedBounds, mGravityDir);
	}



	/// <summary>
	/// Gets the
	/// </summary>
	/// <returns></returns>
	public MWalkDir WallsCheck()
	{
		if (mWallSlideCache.HasValue)
		{
			return mWallSlideCache.Value;
		}

		MugDebug.Assert(mGravityDir == MCardDir.Down, "This assumes we are facing down");

		Rectangle leftBounds = BoundsRect();
		leftBounds.Location += (MWalkDir.Left).ToPoint(mGravityDir);
		leftBounds.Height /= 4;

		Rectangle rightBounds = BoundsRect();
		rightBounds.Location += (MWalkDir.Right).ToPoint(mGravityDir);
		rightBounds.Height /= 4;

		bool leftCollide = CollidesWithAnySolid(leftBounds, MWalkDir.Left.ToCardDir(mGravityDir));
		bool rightCollide = CollidesWithAnySolid(rightBounds, MWalkDir.Right.ToCardDir(mGravityDir));

		MWalkDir result = MWalkDir.None;

		if (leftCollide)
		{
#if DEBUG
			if (rightCollide)
			{
				MugDebug.Warning("Unresolved wall check. Defaulting to left");
			}
#endif
			result = MWalkDir.Left;
		}
		else if (rightCollide)
		{
			result = MWalkDir.Right;
		}

		mWallSlideCache = result;
		return result;
	}

	#endregion rUtil
}

