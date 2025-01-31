

namespace MugEngine.Scene
{
	abstract public class MSPlatformingActor : MSPhysicalActor
	{
		#region rConst

		const float DEFAULT_GRAVITY = 140.8f;

		#endregion rConst




		#region rMembers

		MWalkDir mFacingDir;
		MSSolid mOnlyStandingSolid = null;
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
			base.Update(info);
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

				if (solid.QueryCollides(myShiftedBounds, mGravityDir))
				{
					mOnlyStandingSolid = solid;
					return true;
				}
			}

			return false;
		}



		/// <summary>
		/// Check if we are on the ground.
		/// Run only once per frame then cached.
		/// </summary>
		public bool GroundCheck()
		{
			Rectangle myShiftedBounds = BoundsRect();
			myShiftedBounds.Location += mGravityDir.ToPoint();

			foreach (MGameObject other in GO().GetInRect(myShiftedBounds, GetLayerMask()))
			{
				if (ReferenceEquals(other, this))
				{
					continue;
				}

				if (other is MSSolid solid && solid.QueryCollides(myShiftedBounds, mGravityDir))
				{
					return true;
				}
			}

			bool? levelCollides = GO().GetLevel()?.QueryCollides(myShiftedBounds, mGravityDir);

			return levelCollides.HasValue && levelCollides.Value;
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

		#endregion rUtil






	}
}
