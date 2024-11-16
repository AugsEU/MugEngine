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

		protected Vector2 mPosition;

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





		#region rCollision

		/// <summary>
		/// Get collider for this entity.
		/// </summary>
		/// <returns></returns>
		public virtual Rectangle ColliderBounds()
		{
			return new Rectangle(MugMath.VecToPoint(mPosition), new Point(1, 1));
		}


		/// <summary>
		/// React to a collision.
		/// </summary>
		public virtual void ReactToCollision(MCardDir normal)
		{

		}

		#endregion rCollision





		#region rUtil

		/// <summary>
		/// Get a position
		/// </summary>
		public Vector2 GetPos()
		{
			return mPosition;
		}



		/// <summary>
		/// Set the position of this game object
		/// </summary>
		public void SetPos(Vector2 pos)
		{
			mPosition = pos;
		}

		#endregion rUtil
	}
}
