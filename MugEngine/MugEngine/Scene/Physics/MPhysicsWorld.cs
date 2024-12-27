#define DRAW_PHYSICS_OFF

using MugEngine.Core.Types;
using MugEngine.Library.Collections;
using MugEngine.Library.Maths;
using MugEngine.Types;
using TracyWrapper;

namespace MugEngine.Scene
{
	public class MPhysicsWorld : MComponent
	{
		#region rRegion

		const int INIT_KINEMATIC_ARRAY_CAPACITY = 128;
		const int INIT_ACTOR_ARRAY_CAPACITY = 128;
		const int INIT_TRIGGER_ARRAY_CAPACITY = 128;

		#endregion rRegion





		#region rMembers

		MColliderPool mColliderPool;
		MColliderPool mPrevFrameColliderPool;
		Dictionary<MColliderPoolID, int> mColliderIDToActorIdx;
		MStructArray<MActorSubmission> mActorSubmissions;
		MStructArray<MKinematicSubmission> mKinematicSubmissions;
		MStructArray<MTriggerSubmission> mTriggerSubmissions;

		int mDebugDrawLayer;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a new physics world
		/// </summary>
		public MPhysicsWorld(int debugDrawLayer)
		{
			mColliderPool = new MColliderPool(1000);
			mPrevFrameColliderPool = mColliderPool;

			mActorSubmissions = new MStructArray<MActorSubmission>(INIT_ACTOR_ARRAY_CAPACITY);
			mKinematicSubmissions = new MStructArray<MKinematicSubmission>(INIT_ACTOR_ARRAY_CAPACITY);
			mTriggerSubmissions = new MStructArray<MTriggerSubmission>(INIT_ACTOR_ARRAY_CAPACITY);

			mColliderIDToActorIdx = new Dictionary<MColliderPoolID, int>();

			mDebugDrawLayer = debugDrawLayer;
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Run one step in the physics simulation.
		/// </summary>
		public override void Update(MScene scene, MUpdateInfo info)
		{
			Profiler.PushProfileZone("Physics update", ZoneC.BLUE_VIOLET);

			// The three phases of physics.
			// First we move actors such as the player, making sure to not intersect kinematic and static colliders.
			MoveActors(info);

			// Then move kinematic colliders, making sure to move actors in our path.
			MoveKinematics(info);

			// Finally we send back triggers for any actor or kinematic that moved into a trigger.
			//TriggerTriggers(info);

			mPrevFrameColliderPool = new MColliderPool(mColliderPool);

			mColliderPool.Clear();
			mActorSubmissions.Clear();
			mKinematicSubmissions.Clear();
			mTriggerSubmissions.Clear();
			mColliderIDToActorIdx.Clear();

			Profiler.PopProfileZone();
		}

		public override int UpdateOrder()
		{
			return MComponentUpdateOrder.PHYSICS_WORLD;
		}

		#endregion rUpdate





		#region rResolveActors

		/// <summary>
		/// Move all of our actors. Making sure to not intersect kinematic and static colliders.
		/// </summary>
		private void MoveActors(MUpdateInfo info)
		{
			for (int i = 0; i < mActorSubmissions.Count; i++)
			{
				ref MActorSubmission actor = ref mActorSubmissions.GetRef(i);

				MoveActorXY(ref actor, actor.mVelocity * info.mDelta, false);
			}
		}



		/// <summary>
		/// Move an actor by a displacement.
		/// </summary>
		private void MoveActorXY(ref MActorSubmission actor, Vector2 delta, bool isPush)
		{
			Vector2 currPos = actor.mSenderObject.GetPos();
			Vector2 destPos = currPos + delta;

			Point pixelDelta = MugMath.VecToPoint(destPos) - MugMath.VecToPoint(currPos);
			Point pixelsToMove = pixelDelta;
			Point signs = new Point(MathF.Sign(pixelsToMove.X), MathF.Sign(pixelsToMove.Y));

			// Move X and Y separately. This can cause slight inaccuracies but at a high framerate it is negligible.

			// Resolve X
			while (Math.Abs(pixelsToMove.X) >= 1)
			{
				if (TryMoveActorX(actor.mColliderID, signs.X))
				{
					// Actor can move. Keep going...
					pixelsToMove.X -= signs.X;
				}
				else
				{
					// Hit something. Stop moving.
					delta.X = pixelDelta.X - pixelsToMove.X;
					pixelsToMove.X = 0;

					MCardDir collisionNormal = signs.X > 0 ? MCardDir.Left : MCardDir.Right;
					if (isPush)
					{
						actor.mSenderObject.ReactToSquish(collisionNormal);
					}
					else
					{
						actor.mSenderObject.ReactToCollision(collisionNormal);
					}
				}
			}
			currPos.X += delta.X;


			// Resolve Y
			while (Math.Abs(pixelsToMove.Y) >= 1.0f)
			{
				if (TryMoveActorY(actor.mColliderID, signs.Y))
				{
					// Actor can move. Keep going...
					pixelsToMove.Y -= signs.Y;
				}
				else
				{
					// Hit something. Stop moving.
					delta.Y = pixelDelta.Y - pixelsToMove.Y;
					pixelsToMove.Y = 0;

					MCardDir collisionNormal = signs.Y > 0 ? MCardDir.Up : MCardDir.Down;
					if (isPush)
					{
						actor.mSenderObject.ReactToSquish(collisionNormal);
					}
					else
					{
						actor.mSenderObject.ReactToCollision(collisionNormal);
					}
				}
			}
			currPos.Y += delta.Y;

			actor.mSenderObject.SetPos(currPos);
		}



		/// <summary>
		/// Try to move the actor by an X amount.
		/// Returns true if we can, and moves collider.
		/// Returns false if we can't, and collider doesn't move.
		/// </summary>
		private bool TryMoveActorX(MColliderPoolID colliderID, int x)
		{
			mColliderPool.MoveColliderX(colliderID, x);

			if (mColliderPool.Collides(colliderID, MColliderMask.Kinematic | MColliderMask.Static))
			{
				// It collides, no good. Move it back.
				mColliderPool.MoveColliderX(colliderID, -x);
				return false;
			}

			// No collision, all good!
			return true;
		}



		/// <summary>
		/// Try to move the actor by an X amount.
		/// Returns true if we can, and moves collider.
		/// Returns false if we can't, and collider doesn't move.
		/// </summary>
		private bool TryMoveActorY(MColliderPoolID colliderID, int y)
		{
			mColliderPool.MoveColliderY(colliderID, y);

			if (mColliderPool.Collides(colliderID, MColliderMask.Kinematic | MColliderMask.Static))
			{
				// It collides, no good. Move it back.
				mColliderPool.MoveColliderY(colliderID, -y);
				return false;
			}

			// No collision, all good!
			return true;
		}

		#endregion rResolveActors





		#region rResolveKinematic

		/// <summary>
		/// Fill in kinematic riders of kinematic sub
		/// </summary>
		private void FindKinematicRiders(ref MKinematicSubmission kine, ref List<int> riderIdxList)
		{
			riderIdxList.Clear();

			for (int g = 0; g < mActorSubmissions.Count; g++)
			{
				if (mActorSubmissions[g].mSenderObject.IsRiding(kine.mSenderObject))
				{
					riderIdxList.Add(g);
				}
			}
		}



		/// <summary>
		/// Move all of our actors. Making sure to not intersect kinematic and static colliders.
		/// </summary>
		private void MoveKinematics(MUpdateInfo info)
		{
			List<int> riderIdxList = new List<int>();
			for (int k = 0; k < mKinematicSubmissions.Count; k++)
			{
				ref MKinematicSubmission kine = ref mKinematicSubmissions.GetRef(k);

				// Find all actors riding this.
				FindKinematicRiders(ref kine, ref riderIdxList);

				Vector2 currPos = kine.mSenderObject.GetPos();
				Vector2 destPos = currPos + kine.mVelocity * info.mDelta;

				Vector2 delta = destPos - currPos;
				Point pixelDelta = MugMath.VecToPoint(destPos) - MugMath.VecToPoint(currPos);
				Point signs = new Point(MathF.Sign(pixelDelta.X), MathF.Sign(pixelDelta.Y));

				// Turn off collision to avoid actor problems.
				mColliderPool.SetColliderMask(kine.mColliderID, MColliderMask.None);

				// Move collider & push actors.

				// Resolve X
				while (Math.Abs(pixelDelta.X) >= 1)
				{
					// Move along one.
					mColliderPool.MoveColliderX(kine.mColliderID, signs.X);

					mColliderPool.Query(kine.mColliderID, MColliderMask.Actor);
					MStructArray<MColliderPoolID> collidedActors = mColliderPool.GetQueryResults();

					for (int a = 0; a < collidedActors.Count; a++)
					{
						int submissionIdx;
						mColliderIDToActorIdx.TryGetValue(collidedActors[a], out submissionIdx);

						MoveActorXY(ref mActorSubmissions.GetRef(collidedActors[a].mIndex), new Vector2(signs.X, 0.0f), true);

						riderIdxList.Remove(submissionIdx);
					}

					pixelDelta.X -= signs.X;
				}

				// Riders come along too in X dir.
				for (int r = 0; r < riderIdxList.Count; r++)
				{
					MoveActorXY(ref mActorSubmissions.GetRef(r), new Vector2(delta.X, 0.0f), false);
				}

				// Resolve Y
				while (Math.Abs(pixelDelta.Y) >= 1)
				{
					// Move along one.
					mColliderPool.MoveColliderY(kine.mColliderID, signs.Y);

					mColliderPool.Query(kine.mColliderID, MColliderMask.Actor);
					MStructArray<MColliderPoolID> collidedActors = mColliderPool.GetQueryResults();

					for (int a = 0; a < collidedActors.Count; a++)
					{
						int submissionIdx;
						mColliderIDToActorIdx.TryGetValue(collidedActors[a], out submissionIdx);

						MoveActorXY(ref mActorSubmissions.GetRef(submissionIdx), new Vector2(0.0f, signs.Y), true);

						riderIdxList.Remove(submissionIdx);
					}

					pixelDelta.Y -= signs.Y;
				}

				// Riders come along too in Y dir.
				for (int r = 0; r < riderIdxList.Count; r++)
				{
					MoveActorXY(ref mActorSubmissions.GetRef(r), new Vector2(0.0f, delta.Y), false);
				}

				// Turn back on for other colliders.
				mColliderPool.SetColliderMask(kine.mColliderID, MColliderMask.Kinematic);

				// Apply position to GO
				kine.mSenderObject.SetPos(destPos);
			}
		}

		#endregion rResolveKinematic





		#region rDraw

		/// <summary>
		/// Draw physics simulation.
		/// This is used for debugging.
		/// </summary>
		public override void Draw(MScene scene, MDrawInfo info)
		{
#if DRAW_PHYSICS_ON
			mPrevFrameColliderPool.DebugDraw(info, mDebugDrawLayer);
#endif
		}

		#endregion rDraw





		#region rSubmitColliders

		/// <summary>
		/// Add static rectangle collider
		/// </summary>
		public void AddStatic(Rectangle rectangle)
		{
			MColliderMask mask = MColliderMask.Static;

			// Static colliders are just thrown in but we never move them. So we can just forget about their IDs.
			mColliderPool.AddColliderNoID(new MRectCollider(rectangle, mask));
		}



		/// <summary>
		/// Add a kinematic collider
		/// </summary>
		public void AddKinematic(MGameObject sender, Rectangle localBounds, Vector2 velocity)
		{
			// Transform local bounds into world
			localBounds.Location += MugMath.VecToPoint(sender.GetPos());
			MColliderPoolID colliderID = mColliderPool.AddCollider(localBounds, MColliderMask.Kinematic);

			mKinematicSubmissions.Add(new MKinematicSubmission(colliderID, velocity, sender));
		}



		/// <summary>
		/// Add an actor's collider
		/// </summary>
		public void AddActor(MGameObject sender, Rectangle localBounds, Vector2 velocity)
		{
			// Transform local bounds into world
			localBounds.Location += MugMath.VecToPoint(sender.GetPos());
			MColliderPoolID colliderID = mColliderPool.AddCollider(localBounds, MColliderMask.Actor);

			mColliderIDToActorIdx.Add(colliderID, mActorSubmissions.Count);

			mActorSubmissions.Add(new MActorSubmission(colliderID, velocity, sender));
		}

		#endregion rSubmitColliders
	}
}
