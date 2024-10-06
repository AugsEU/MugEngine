namespace MugEngine.Graphics.Camera
{
	/// <summary>
	/// Represents a camera movement.
	/// E.g. shaking or panning.
	/// </summary>
	public abstract class MCameraMovement
	{
		/// <summary>
		/// Get spec delta this movement incurs.
		/// </summary>
		/// <param name="time">Time from 0 to 1 representing completion.</param>
		public abstract MCameraSpec GetSpecDelta(float time); 
	}
}
