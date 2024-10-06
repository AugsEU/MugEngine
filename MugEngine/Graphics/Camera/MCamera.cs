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
		MSpriteBatchOptions mSpriteBatchOptions;
		Vector2 mViewPortSize;

		Matrix? mMatrixCache;

		MCameraMovementPlayer mCurrentMovement;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Create camera
		/// </summary>
		public MCamera(Vector2 viewportSize)
		{
			mCurrentSpec = new MCameraSpec();
			mSpriteBatchOptions = new MSpriteBatchOptions();
			mViewPortSize = viewportSize;
		}



		/// <summary>
		/// Create camera with special spritebatch settings.
		/// </summary>
		public MCamera(Vector2 viewportSize, MSpriteBatchOptions sbSettings)
		{
			mCurrentSpec = new MCameraSpec();
			mSpriteBatchOptions = sbSettings;
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
			
			if(mCurrentMovement is not null)
			{
				mCurrentMovement.Update(info);
				if(mCurrentMovement.IsFinished())
				{
					EndCurrentMovement();
				}
			}
		}



		/// <summary>
		/// Make the camera follow movement.
		/// </summary>
		public void StartMovement(MCameraMovement movement, float time)
		{
			if(mCurrentMovement is not null)
			{
				EndCurrentMovement();
			}

			mCurrentMovement = new MCameraMovementPlayer(movement, time);
		}



		/// <summary>
		/// Immediately end the current movement, jumping to the end.
		/// </summary>
		public void EndCurrentMovement()
		{
			if (mCurrentMovement is null)
			{
				return;
			}

			mCurrentSpec += mCurrentMovement.GetFinalDelta();

			mCurrentMovement = null;
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Start the sprite batch
		/// </summary>
		public void StartSpriteBatch(SpriteBatch spriteBatch)
		{
			if(!mMatrixCache.HasValue)
			{
				mMatrixCache = CalculateMatrix();
			}

			spriteBatch.Begin(mSpriteBatchOptions.mSortMode,
									mSpriteBatchOptions.mBlend,
									mSpriteBatchOptions.mSamplerState,
									mSpriteBatchOptions.mDepthStencilState,
									mSpriteBatchOptions.mRasterizerState,
									null,
									mMatrixCache.Value);
		}



		/// <summary>
		/// End the sprite batch
		/// </summary>
		/// <param name="info"></param>
		public void EndSpriteBatch(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
		}



		/// <summary>
		/// Caulate perspective matrix.
		/// </summary>
		Matrix CalculateMatrix()
		{
			MCameraSpec ourSpec = mCurrentSpec;

			if(mCurrentMovement is not null)
			{
				ourSpec += mCurrentMovement.GetSpecDelta();
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
		/// Set sprite batch options
		/// </summary>
		public void SetOptions(MSpriteBatchOptions options)
		{
			mSpriteBatchOptions = options;
		}



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
