namespace MugEngine.Scene
{
	/// <summary>
	/// An entity is a thing managed by the scene.
	/// </summary>
	public abstract class MComponent : IMUpdate, IMDraw
	{
		MScene mParent = null;

		/// <summary>
		/// Add this component to the scene.
		/// </summary>
		public void SetScene(MScene parent)
		{
			mParent = parent;
		}



		/// <summary>
		/// Get the parent.
		/// </summary>
		public MScene GetScene()
		{
			MugDebug.Assert(mParent != null);
			return mParent;
		}



		/// <summary>
		/// Called every frame to update the state.
		/// </summary>
		public virtual void Update(MUpdateInfo info)
		{
		}


		/// <summary>
		/// Called every frame draw the entity.
		/// </summary>
		public virtual void Draw(MDrawInfo info)
		{
		}


		/// <summary>
		/// Gets order of updates.
		/// Higher is later.
		/// </summary>
		public virtual int UpdateOrder()
		{
			return MComponentUpdateOrder.DEFAULT_ORDER;
		}
	}
}
