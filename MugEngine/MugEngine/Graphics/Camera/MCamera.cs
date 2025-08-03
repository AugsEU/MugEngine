namespace MugEngine.Graphics
{
	/// <summary>
	/// Camera class. Handles all camera positioning.
	/// </summary>
	public class MCamera : IMUpdate
	{
		#region rMembers

		MCameraSpec mCurrentSpec;
		Vector2 mViewPortSize;
		MAnchorType mCameraAnchor;

		MCameraFocus mFocus;
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
			mCameraAnchor = MAnchorType.Centre;

			mMovements = new List<MCameraMovementPlayer>();
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
			if (mFocus is not null)
			{
				mCurrentSpec = mFocus.UpdateFocusPoint(info, mCurrentSpec);
			}

			for (int i = 0; i < mMovements.Count; i++)
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


		/// <summary>
		/// Stop all camera movement follow movement.
		/// </summary>
		public void ClearMovements(bool goToEnd)
		{
			if(goToEnd)
			{
				for (int i = 0; i < mMovements.Count; i++)
				{
					MCameraMovementPlayer movement = mMovements[i];
					mCurrentSpec += movement.GetFinalDelta();
				}
			}

			mMovements.Clear();
		}



		/// <summary>
		/// Focus the camera on something.
		/// </summary>
		public void SetFocus(MCameraFocus focus)
		{
			focus.StartFocus(mCurrentSpec);
			mFocus = focus;
		}


		/// <summary>
		/// Focus the camera on something.
		/// </summary>
		public MCameraFocus GetFocus()
		{
			return mFocus;
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Caulate perspective matrix.
		/// </summary>
		public Matrix CalculateMatrix()
		{
			MCameraSpec ourSpec = mCurrentSpec;

			foreach (MCameraMovementPlayer movement in mMovements)
			{
				ourSpec += movement.GetSpecDelta();
			}

			Vector3 centrePoint3 = new Vector3(mViewPortSize / 2.0f, 0.0f);

			MAnchorVector2 ancVec = new MAnchorVector2(ourSpec.mPosition, mCameraAnchor);

			Vector2 cameraTopLeft = ancVec.ToVec(mViewPortSize, MAnchorType.TopLeft);

			Vector3 initDelta = new Vector3(-(int)cameraTopLeft.X, -(int)cameraTopLeft.Y, 0) - centrePoint3;

			Matrix result = Matrix.CreateTranslation(initDelta) *
				   Matrix.CreateRotationZ(ourSpec.mRotation) *
				   Matrix.CreateScale(new Vector3(ourSpec.mZoom, ourSpec.mZoom, 1)) *
				   Matrix.CreateTranslation(centrePoint3);

			return result;
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



		/// <summary>
		/// Get the viewport(extended a bit because of culling)
		/// </summary>
		public Rectangle GetViewportForCull()
		{
			const int CULL_PADDING = 32;

			MAnchorVector2 ancVec = new MAnchorVector2(mCurrentSpec.mPosition, mCameraAnchor);
			Vector2 topLeft = ancVec.ToVec(mViewPortSize);

			Vector2 middle = topLeft + 0.5f * mViewPortSize;

			float zoomScale = 1.0f / mCurrentSpec.mZoom;

			topLeft = (topLeft - middle) * zoomScale + middle;

			return new Rectangle((int)topLeft.X - CULL_PADDING,
								(int)topLeft.Y - CULL_PADDING,
								(int)(mViewPortSize.X * zoomScale) + 2 * CULL_PADDING,
								(int)(mViewPortSize.Y * zoomScale) + 2 * CULL_PADDING);
		}

		#endregion rUtil
	}
}
