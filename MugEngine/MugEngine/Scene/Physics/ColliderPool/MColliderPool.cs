using MugEngine.Core.Types;
using MugEngine.Library.Collections;

namespace MugEngine.Scene
{
	/// <summary>
	/// Stores many types of colliders.
	/// </summary>
	internal class MColliderPool
	{
		#region rRegion

		const int INIT_RECT_CAPACITY = 1024;
		const int INIT_QUERY_CAPACITY = 128;

		#endregion rRegion





		#region rMembers

		// Our various types of collider
		MStructArray<MRectCollider> mRectColliders;

		MStructArray<MColliderPoolID> mQueryResultsBuffer;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create a collider pool.
		/// </summary>
		public MColliderPool(int initCapacity)
		{
			mRectColliders = new MStructArray<MRectCollider>(INIT_RECT_CAPACITY);

			mQueryResultsBuffer = new MStructArray<MColliderPoolID>(INIT_QUERY_CAPACITY);
		}


		/// <summary>
		/// Copy from other collider pool
		/// </summary>
		public MColliderPool(MColliderPool other)
		{
			mRectColliders = new MStructArray<MRectCollider>(INIT_RECT_CAPACITY);

			for (int i = 0; i < other.mRectColliders.Count; i++)
			{
				mRectColliders.Add(other.mRectColliders[i]);
			}
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Delete all colliders.
		/// </summary>
		public void Clear()
		{
			mRectColliders.Clear();
		}

		#endregion rUpdate





		#region rAccess

		/// <summary>
		/// Add a collider to the pool
		/// </summary>
		public MColliderPoolID AddCollider(Rectangle rectangle, MColliderMask mask)
		{
			mRectColliders.Add(new MRectCollider(rectangle, mask));
			return new MColliderPoolID(MColliderType.Rectangle, mRectColliders.Count - 1);
		}



		/// <summary>
		/// Add a collider to the pool
		/// </summary>
		public MColliderPoolID AddCollider(MRectCollider rectCollider)
		{
			mRectColliders.Add(rectCollider);
			return new MColliderPoolID(MColliderType.Rectangle, mRectColliders.Count - 1);
		}



		/// <summary>
		/// Add a collider to the pool but do not worry about it's ID
		/// </summary>
		public void AddColliderNoID(MRectCollider rectCollider)
		{
			mRectColliders.Add(rectCollider);
		}



		/// <summary>
		/// Move a collider in X coordinates
		/// </summary>
		public void MoveColliderX(MColliderPoolID id, int x)
		{
			switch (id.mColliderType)
			{
				case MColliderType.Rectangle:
					mRectColliders.GetRef(id.mIndex).MoveX(x);
					break;
				default:
					break;
			}
		}



		/// <summary>
		/// Move a collider in Y coordinates
		/// </summary>
		public void MoveColliderY(MColliderPoolID id, int y)
		{
			switch (id.mColliderType)
			{
				case MColliderType.Rectangle:
					mRectColliders.GetRef(id.mIndex).MoveY(y);
					break;
				default:
					break;
			}
		}



		/// <summary>
		/// Move a collider in X-Y coordinates
		/// </summary>
		public void MoveColliderXY(MColliderPoolID id, int x, int y)
		{
			switch (id.mColliderType)
			{
				case MColliderType.Rectangle:
					mRectColliders.GetRef(id.mIndex).MoveX(x);
					mRectColliders.GetRef(id.mIndex).MoveY(y);
					break;
				default:
					break;
			}
		}



		/// <summary>
		/// Set the mask of a collider
		/// </summary>
		public void SetColliderMask(MColliderPoolID id, MColliderMask mask)
		{
			switch (id.mColliderType)
			{
				case MColliderType.Rectangle:
					mRectColliders.GetRef(id.mIndex).mMask = mask;
					break;
				default:
					break;
			}
		}

		#endregion rAccess






		#region rCollisionChecks

		/// <summary>
		/// Check if collider intersects
		/// </summary>
		public bool Collides(MColliderPoolID id, MColliderMask mask)
		{
			switch (id.mColliderType)
			{
				case MColliderType.Rectangle:
					return RectCollides(id.mIndex, mask);
				default:
					break;
			}

			throw new NotImplementedException();
		}


		/// <summary>
		/// Check if rect at idx collides with anything in the pool.
		/// </summary>
		private bool RectCollides(int idx, MColliderMask mask)
		{
			// To do: Smart lookup instead of brute force?
			for (int i = 0; i < mRectColliders.Count; ++i)
			{
				// Must have the same mask. Cannot collide with itself.
				if (idx == i || ((mask & mRectColliders[i].mMask) == 0b0000_0000))
				{
					continue;
				}

				if (mRectColliders[i].CollidesWith(mRectColliders[idx]))
				{
					return true;
				}
			}

			return false;
		}



		/// <summary>
		/// Check if collider intersects.
		/// Populates query buffer.
		/// </summary>
		public bool Query(MColliderPoolID id, MColliderMask mask)
		{
			mQueryResultsBuffer.Clear();
			switch (id.mColliderType)
			{
				case MColliderType.Rectangle:
					return RectQuery(id.mIndex, mask);
				default:
					break;
			}

			throw new NotImplementedException();
		}



		/// <summary>
		/// Check if rect at idx collides with anything in the pool.
		/// </summary>
		private bool RectQuery(int idx, MColliderMask mask)
		{
			bool result = false;

			// To do: Smart lookup instead of brute force?
			for (int i = 0; i < mRectColliders.Count; ++i)
			{
				// Must have the same mask. Cannot collide with itself.
				if (idx == i || ((mask & mRectColliders[i].mMask) == 0b0000_0000))
				{
					continue;
				}

				if (mRectColliders[i].CollidesWith(mRectColliders[idx]))
				{
					result = true;
					mQueryResultsBuffer.Add(new MColliderPoolID(MColliderType.Rectangle, i));
				}
			}

			return result;
		}



		/// <summary>
		/// Get results of previous query.
		/// </summary>
		public MStructArray<MColliderPoolID> GetQueryResults()
		{
			return mQueryResultsBuffer;
		}

		#endregion rCollisionChecks





		#region rDebug

		/// <summary>
		/// Draw out all the colliders
		/// </summary>
		public void DebugDraw(MDrawInfo info, int layer)
		{
			for (int i = 0; i < mRectColliders.Count; i++)
			{
				Color rectColor = GetColliderColor(mRectColliders[i].mMask);

				info.mCanvas.DrawRect(mRectColliders[i].mRectangle, rectColor, layer);
			}
		}

		/// <summary>
		/// Color for debugging.
		/// </summary>
		private Color GetColliderColor(MColliderMask mask)
		{
			bool isStat = (mask & MColliderMask.Static) != 0;
			bool isKine = (mask & MColliderMask.Kinematic) != 0;
			bool isAct = (mask & MColliderMask.Actor) != 0;
			bool isTrig = (mask & MColliderMask.Trigger) != 0;

			if ((mask & MColliderMask.Static) != 0)
			{
				return Color.DarkSlateGray;
			}
			else if ((mask & MColliderMask.Kinematic) != 0)
			{
				return Color.BlueViolet;
			}
			else if ((mask & MColliderMask.Actor) != 0)
			{
				return Color.GreenYellow;
			}
			else if ((mask & MColliderMask.Trigger) != 0)
			{
				return Color.IndianRed;
			}

			return Color.White;
		}

		#endregion rDebug
	}
}
