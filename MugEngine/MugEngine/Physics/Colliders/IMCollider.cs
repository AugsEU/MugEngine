namespace MugEngine.Physics
{
	/// <summary>
	/// Represents a thing that can collide.
	/// </summary>
	public interface IMCollider
	{
		public bool CollidesWith(MRectCollider rect);

		public void MoveX(int x);

		public void MoveY(int y);

		public MColliderMask GetMask();
	}
}
