

namespace MugEngine.Scene
{
	abstract public class MSPlatformingActor : MSPhysicalActor
	{
		#region rConst

		const float DEFAULT_GRAVITY = 140.8f;

		#endregion rConst




		#region rMembers

		MWalkDir mFacingDir;
		bool mOnGround;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create platfomer at position.
		/// </summary>
		public MSPlatformingActor(Vector2 position) : base(position)
		{
			mGravityStrength = DEFAULT_GRAVITY;
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update platforming entity
		/// </summary>
		public override void Update(MUpdateInfo info)
		{
			mOnGround = false;
			base.Update(info);
		}


		/// <summary>
		/// Are we riding this solid?
		/// </summary>
		public override bool IsRiding(MSSolid solid)
		{
			Rectangle myShiftedBounds = BoundsRect();
			myShiftedBounds.Location += mGravityDir.ToPoint();
			myShiftedBounds.Location += mGravityDir.ToPoint();
			myShiftedBounds.Location += mGravityDir.ToPoint();
			myShiftedBounds.Location += mGravityDir.ToPoint();

			if (solid.BoundsRect().Intersects(myShiftedBounds))
			{
				return true;
			}

			return false;
		}



		/// <summary>
		/// Make a jump.
		/// </summary>
		public void Jump(float speed)
		{
			mVelocity.Y = -speed;
		}



		/// <summary>
		/// Walk in a direction.
		/// </summary>
		public void WalkIn(MWalkDir dir, float speed)
		{
			Vector2 walkVec = dir.ToVec(mGravityDir);

			Vector2 componentInWalkVec = Vector2.Dot(mVelocity, walkVec) * walkVec;

			mVelocity = mVelocity - componentInWalkVec + walkVec * speed;

			mFacingDir = dir;
		}

		#endregion rUpdate





		#region rCollision

		/// <summary>
		/// React to solid.
		/// </summary>
		/// <param name="normal"></param>
		public override void OnHitSolid(MCardDir normal)
		{
			if (normal == MCardDir.Up)
			{
				mOnGround = true;
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

		#endregion rCollision





		#region rUtil

		#endregion rUtil






	}
}
