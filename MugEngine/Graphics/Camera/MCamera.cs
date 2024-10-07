using AridArnold;
using Microsoft.Xna.Framework.Graphics;
using MugEngine.Types;

namespace MugEngine.Graphics.Camera
{
	/// <summary>
	/// Camera class. Handles all camera positioning.
	/// </summary>
	public class MCamera : IMUpdate
	{
		#region rMembers

		MCameraSpec mCurrentSpec;
		Vector2 mViewPortSize;

		Matrix? mMatrixCache;

		List<MCameraMovementPlayer> mMovements;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create camera
		/// </summary>
		public MCamera(Vector2 viewportSize)
		{
			mCurrentSpec = new MCameraSpec();
			mViewPortSize = viewportSize;
		}



		/// <summary>
		/// Create camera with special spritebatch settings.
		/// </summary>
		public MCamera(Vector2 viewportSize, MSpriteBatchOptions sbSettings)
		{
			mCurrentSpec = new MCameraSpec();
			mViewPortSize = viewportSize;
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update camera.
		/// </summary>
		public void Update(MUpdateInfo info)
		{
			mMatrixCache = CalculateMatrix();

			for(int i = 0; i < mMovements.Count; i++)
			{
				MCameraMovementPlayer movement = mMovements[i];
				movement.Update(info);

				if (movement.IsFinished())
				{
					mCurrentSpec += movement.GetFinalDelta();
					mMovements.RemoveAt(i--);
				}
			}
		}



		/// <summary>
		/// Make the camera follow movement.
		/// </summary>
		public void StartMovement(MCameraMovement movement, float time)
		{
			mMovements.Add(new MCameraMovementPlayer(movement, time));
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Caulate perspective matrix.
		/// </summary>
		Matrix CalculateMatrix()
		{
			MCameraSpec ourSpec = mCurrentSpec;

			foreach(MCameraMovementPlayer movement in mMovements)
			{
				ourSpec += movement.GetSpecDelta();
			}

			Vector3 centrePoint3 = new Vector3(mViewPortSize / 2.0f, 0.0f);

			Vector3 initDelta = new Vector3(-(int)ourSpec.mPosition.X, -(int)ourSpec.mPosition.Y, 0) - centrePoint3;

			return Matrix.CreateTranslation(initDelta) *
				   Matrix.CreateRotationZ(ourSpec.mRotation) *
				   Matrix.CreateScale(new Vector3(ourSpec.mZoom, ourSpec.mZoom, 1)) *
				   Matrix.CreateTranslation(centrePoint3);
		}

		#endregion rDraw





		#region rUtil

		/// <summary>
		/// Get the current spec
		/// </summary>
		public MCameraSpec GetCurrentSpec()
		{
			return mCurrentSpec;
		}



		/// <summary>
		/// Set camera specifications
		/// </summary>
		public void ForceNewSpec(MCameraSpec spec)
		{
			mCurrentSpec = spec;
		}



		/// <summary>
		/// Force camera to move to position
		/// </summary>
		public void ForcePosition(Vector2 position)
		{
			mCurrentSpec.mPosition = position;
		}

		#endregion rUtil
	}
}
