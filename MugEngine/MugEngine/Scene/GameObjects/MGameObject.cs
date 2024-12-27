using MugEngine.Core;
using MugEngine.Library;

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
		/// Get bounding box for this entity.
		/// </summary>
		public virtual Rectangle LocalBounds()
		{
			return new Rectangle(0, 0, 1, 1);
		}



		/// <summary>
		/// Get collider in world coordinates
		/// </summary>
		public Rectangle AbsoluteBounds()
		{
			Rectangle ret = LocalBounds();
			ret.Location += MugMath.VecToPoint(mPosition);
			return ret;
		}



		/// <summary>
		/// React to a collision.
		/// </summary>
		public virtual void ReactToCollision(MCardDir normal)
		{

		}



		/// <summary>
		/// React to a collision.
		/// </summary>
		public virtual void ReactToSquish(MCardDir normal)
		{

		}



		/// <summary>
		/// Are we riding atop another game object?
		/// </summary>
		public virtual bool IsRiding(MGameObject other)
		{
			return false;
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
