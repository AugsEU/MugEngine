namespace MugEngine
{
	public abstract class MScreen : IMUpdate, IMDraw
	{
		#region rMembers

		Point mResolution;
		protected MCanvas2D mCanvas;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create screen
		/// </summary>
		public MScreen(Point resolution)
		{
			mCanvas = new MCanvas2D(resolution);
		}


		/// <summary>
		/// Called once at the start of the game.
		/// </summary>
		public virtual void Initialise()
		{
		}


		/// <summary>
		/// Called each time the screen is activated.
		/// </summary>
		public virtual void OnActivate()
		{

		}



		/// <summary>
		/// Called when the screen is switched out for another.
		/// This might not happen instantly since we could have an exit sequence that would require us to run a few frames.
		/// </summary>
		public virtual void BeginDeactivate()
		{

		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update screen.
		/// </summary>
		public virtual void Update(MUpdateInfo info)
		{
			mCanvas.Update(info);
		}



		/// <summary>
		/// Are we allowed to exit this screen?
		/// </summary>
		public virtual bool AllowQuit()
		{
			return true;
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draw the screen to it's own canvas.
		/// </summary>
		public abstract void Draw(MDrawInfo info);



		/// <summary>
		/// Get screen's own canvas.
		/// </summary>
		public MCanvas2D GetCanvas()
		{
			return mCanvas;
		}

		#endregion rDraw





		#region rUtil

		#endregion rUtil





		#region rGetSet

		#endregion rGetSet
	}
}
