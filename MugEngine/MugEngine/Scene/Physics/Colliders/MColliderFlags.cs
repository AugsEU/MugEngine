namespace MugEngine.Scene
{
	enum MColliderType : byte
	{
		Rectangle
	}


	[Flags]
	public enum MColliderMask : byte
	{
		None = 0b0000,              // Collision is disabled.
		Static = 0b0001,            // Contains colliders that do not move
		Kinematic = 0b0010,         // Contains colliders that move
		Actor = 0b0100,             // Contains colliders that move but do not affect other colliders
		Trigger = 0b1000            // Contains "trigger" colliders that can trigger things for gameobjects.
	}
}
