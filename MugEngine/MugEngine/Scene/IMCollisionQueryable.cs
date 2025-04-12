namespace MugEngine.Scene
{
	[Flags]
	public enum CollisionFlags : UInt64
	{
		None =					0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000,
		FallthroughPlatforms =	0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001
	}

	public interface IMCollisionQueryable
	{
		public bool QueryCollides(Rectangle bounds, MCardDir travelDir, CollisionFlags flags);
	}
}
