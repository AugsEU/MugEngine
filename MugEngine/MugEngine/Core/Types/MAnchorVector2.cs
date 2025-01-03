namespace MugEngine.Core
{
	public struct MAnchorVector2
	{
		public static MAnchorVector2 Zero { get { return new MAnchorVector2(Vector2.Zero, MAnchorType.Centre); } }

		Vector2 mPostion;
		MAnchorType mAnchor;

		public MAnchorVector2(Vector2 pos, MAnchorType anchor)
		{
			mPostion = pos;
			mAnchor = anchor;
		}

		public Vector2 ToVec(Point size, MAnchorType baseAnchor = MAnchorType.TopLeft)
		{
			Vector2 sizeVec = size.ToVec();
			return ToVec(sizeVec, baseAnchor);
		}

		public Vector2 ToVec(Vector2 size, MAnchorType baseAnchor = MAnchorType.TopLeft)
		{
			MRect2f rect = new MRect2f(mPostion, mPostion + size);

			Vector2 projectedPos = mAnchor.ToPosOnRect(rect);
			Vector2 diff = projectedPos - mPostion;

			Vector2 topLeft = mPostion - diff;
			rect = new MRect2f(topLeft, topLeft + size);

			return baseAnchor.ToPosOnRect(rect);
		}
	}
}
