#define DRAW_PHYSICS_ON

using MugEngine.Collections;
using MugEngine.Maths;
using MugEngine.Physics.Submission;
using MugEngine.Scene;
using MugEngine.Types;

namespace MugEngine.Physics
{
	public class MPhysicsWorld : MEntity
	{
		#region rRegion

		const int INIT_KINEMATIC_ARRAY_CAPACITY = 128;
		const int INIT_ACTOR_ARRAY_CAPACITY = 128;
		const int INIT_TRIGGER_ARRAY_CAPACITY = 128;

		#endregion rRegion





		#region rMembers

		MColliderPool mColliderPool;
		MColliderPool mPrevFrameColliderPool;
		MStructArray<MGameObjectSubmission> mActorSubmissions;
		MStructArray<MGameObjectSubmission> mKinematicSubmissions;
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

			mActorSubmissions = new MStructArray<MGameObjectSubmission>(INIT_ACTOR_ARRAY_CAPACITY);
			mKinematicSubmissions = new MStructArray<MGameObjectSubmission>(INIT_ACTOR_ARRAY_CAPACITY);
			mTriggerSubmissions = new MStructArray<MTriggerSubmission>(INIT_ACTOR_ARRAY_CAPACITY);

			mDebugDrawLayer = debugDrawLayer;
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Run one step in the physics simulation.
		/// </summary>
		public override void Update(MScene scene, MUpdateInfo info)
		{
			// The three phases of physics.
			// First we move actors such as the player, making sure to not intersect kinematic and static colliders.
			MoveActors(info);

			// Then move kinematic colliders, making sure to move actors in our path.
			//MoveKinematic(info);

			// Finally we send back triggers for any actor or kinematic that moved into a trigger.
			//TriggerTriggers(info);

			mPrevFrameColliderPool = new MColliderPool(mColliderPool);

			mColliderPool.Clear();
			mActorSubmissions.Clear();
			mKinematicSubmissions.Clear();
			mTriggerSubmissions.Clear();
		}

		public override int UpdateOrder()
		{
			return MEntityUpdateOrder.PHYSICS_WORLD;
		}

		#endregion rUpdate



		#region rResolveActors

		/// <summary>
		/// Move all of our actors. Making sure to not intersect kinematic and static colliders.
		/// </summary>
		private void MoveActors(MUpdateInfo info)
		{
			for(int i = 0; i < mActorSubmissions.Count; i++)
			{
				ref MGameObjectSubmission sub = ref mActorSubmissions.GetRef(i);
				Vector2 currPos = sub.mSenderObject.GetPos();
				Vector2 destPos = currPos + sub.mVelocity * info.mDelta;

				Vector2 delta = destPos - currPos;
				Point coordDelta = MugMath.VecToPoint(destPos) - MugMath.VecToPoint(currPos);
				Point signs = new Point(MathF.Sign(delta.X), MathF.Sign(delta.Y));
				
				// Move X and Y separately. This can cause slight inaccuracies but at a high framerate it is negligible.

				// Resolve X
				while(Math.Abs(coordDelta.X) >= 1)
				{
					if(TryMoveActorX(sub.mColliderID, signs.X))
					{
						// Actor can move. Keep going...
						coordDelta.X -= signs.X;
						delta.X -= signs.X;
						currPos.X += signs.X;
					}
					else
					{
						// Hit something.
						coordDelta.X = 0;
						delta.X = 0.0f;
						sub.mSenderObject.ReactToCollision(signs.X > 0 ? MCardDir.Left : MCardDir.Right);
					}
				}
				currPos.X += delta.X;


				// Resolve Y
				while (Math.Abs(coordDelta.Y) >= 1.0f)
				{
					if (TryMoveActorY(sub.mColliderID, signs.Y))
					{
						// Actor can move. Keep going...
						coordDelta.Y -= signs.Y;
						delta.Y -= signs.Y;
						currPos.Y += signs.Y;
					}
					else
					{
						// Hit something. Stop moving.
						coordDelta.Y = 0;
						delta.Y = 0.0f;
						sub.mSenderObject.ReactToCollision(signs.Y > 0 ? MCardDir.Up : MCardDir.Down);
					}
				}
				currPos.Y += delta.Y;

				sub.mSenderObject.SetPos(currPos);
			}
		}



		/// <summary>
		/// Try to move the actor by an X amount.
		/// Returns true if we can, and moves collider.
		/// Returns false if we can't, and collider doesn't move.
		/// </summary>
		private bool TryMoveActorX(MColliderPoolID colliderID, int x)
		{
			mColliderPool.MoveColliderX(colliderID, x);

			if(mColliderPool.Collides(colliderID, MColliderMask.Kinematic | MColliderMask.Static))
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
		public void AddKinematic(MGameObject sender, Rectangle rectangle, Vector2 velocity)
		{
			MColliderPoolID colliderID = mColliderPool.AddCollider(rectangle, MColliderMask.Kinematic);

			mKinematicSubmissions.Add(new MGameObjectSubmission(colliderID, velocity, sender));
		}



		/// <summary>
		/// Add an actor's collider
		/// </summary>
		public void AddActor(MGameObject sender, Rectangle rectangle, Vector2 velocity)
		{
			MColliderPoolID colliderID = mColliderPool.AddCollider(rectangle, MColliderMask.Actor);

			mActorSubmissions.Add(new MGameObjectSubmission(colliderID, velocity, sender));
		}

		#endregion rSubmitColliders
	}
}
