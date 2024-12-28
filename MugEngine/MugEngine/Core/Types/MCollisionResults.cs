namespace MugEngine.Core
{
	struct MCollisionResults
	{
		public Vector2 mNormal;
		public float mT;

		public MCollisionResults(Vector2 normal, float t)
		{
			mNormal = normal;
			mT = t;
		}
	}
}
