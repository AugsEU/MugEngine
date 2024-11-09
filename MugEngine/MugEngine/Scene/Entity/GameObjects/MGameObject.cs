using MugEngine.Maths;
using MugEngine.Types;

namespace MugEngine.Scene
{
	/// <summary>
	/// A game object is something that exists within the game.
	/// </summary>
	public abstract class MGameObject : IMSceneUpdate, IMSceneDraw
	{
		#region rMembers

		Vector2 mPosition;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create game object at position.
		/// </summary>
		public MGameObject(Vector2 position)
		{
			mPosition = position;
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update the game object
		/// </summary>
		public abstract void Update(MScene scene, MUpdateInfo info);

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Update the draw object
		/// </summary>
		public abstract void Draw(MScene scene, MDrawInfo info);

		#endregion rDraw





		#region rUtil

		/// <summary>
		/// Get collider for this entity.
		/// </summary>
		/// <returns></returns>
		public virtual MRect2f ColliderBounds()
		{
			return new MRect2f(mPosition, 1.0f, 1.0f);
		}

		#endregion rUtil





		#region rAccess

		#endregion rAccess
	}
}
