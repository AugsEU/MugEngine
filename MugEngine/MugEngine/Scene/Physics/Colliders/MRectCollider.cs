namespace MugEngine.Scene
{
	public struct MRectCollider : IMCollider
	{
		// MugPhysics works on a pixel level.
		public Rectangle mRectangle;
		public MColliderMask mMask;

		public MRectCollider(Point position, Point size, MColliderMask mask)
		{
			mRectangle = new Rectangle(position, size);
			mMask = mask;
		}

		public MRectCollider(Rectangle rect, MColliderMask mask)
		{
			mRectangle = rect;
			mMask = mask;
		}

		public MRectCollider MovedBy(Point point)
		{
			return new MRectCollider(new Rectangle(mRectangle.X + point.X, mRectangle.Y + point.Y, mRectangle.Size.X, mRectangle.Y), mMask);
		}

		public void MoveX(int x)
		{
			mRectangle.X += x;
		}

		public void MoveY(int y)
		{
			mRectangle.Y += y;
		}

		public bool CollidesWith(MRectCollider rect)
		{
			return mRectangle.Intersects(rect.mRectangle);
		}

		public MColliderMask GetMask()
		{
			return mMask;
		}
	}
}
