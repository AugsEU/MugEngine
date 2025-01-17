namespace MugEngine.Input
{
	public struct MDirPad<T>(T up, T down, T left, T right) where T : Enum
	{
		public T mUp = up;
		public T mDown = down;
		public T mLeft = left;
		public T mRight = right;

		public Vector2 ToVector()
		{
			return MugInput.I.DPadToVector(mUp, mDown, mLeft, mRight);
		}
	}
}
