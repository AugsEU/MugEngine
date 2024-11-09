using MugEngine.Types;

namespace MugEngine.Graphics
{
	public class MAnimation : IMUpdate
	{
		#region rTypes

		/// <summary>
		/// Frame of animation
		/// </summary>
		public struct AnimationFrame
		{
			public AnimationFrame(MTexturePart tex, float t)
			{
				mTexture = tex;
				mDuration = t;
			}

			public MTexturePart mTexture;
			public float mDuration;
		}



		/// <summary>
		/// How the animation is played.
		/// </summary>
		public enum PlayType
		{
			Forward, // Play the animation forwards
			Backwards, // Play the animation backwards
			PingPong, // Play the animation forwards then backwards
			PingPongBackwards // Play the animation backwards then forwards
		};

		#endregion rTypes





		#region rMembers

		AnimationFrame[] mFrames = null;

		PlayType mPlayType = PlayType.Forward;

		int mNumRepeats = 0;
		int mRepeatsRemaining = 0;

		float mTotalDuration = 0.0f;
		float mPlayHead = 0.0f;
		float mPlayDirection = 1.0f;
		
		bool mPlaying = true;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Animation constructor
		/// </summary>
		/// <param name="playType">Play mode.(See enum for details)</param>
		public MAnimation(PlayType playType, int numRepeats, AnimationFrame[] frameData)
		{
			mPlayType = playType;

			mNumRepeats = numRepeats;
			mRepeatsRemaining = numRepeats;

			mFrames = frameData;
			CalcTotalDuration();
		}



		/// <summary>
		/// Animation constructor
		/// </summary>
		/// <param name="playType">Play mode.(See enum for details)</param>
		public MAnimation(MTexturePart tex)
		{
			AnimationFrame frame = new AnimationFrame(tex, 1.0f);
			mFrames = new AnimationFrame[] { frame };

			CalcTotalDuration();
		}



		/// <summary>
		/// Animation constructor
		/// </summary>
		/// <param name="playType">Play mode.(See enum for details)</param>
		public MAnimation(Texture2D tex)
		{
			AnimationFrame frame = new AnimationFrame(new MTexturePart(tex), 1.0f);
			mFrames = new AnimationFrame[] { frame };
			CalcTotalDuration();
		}



		/// <summary>
		/// Calculate the animation's total duration and cache it.
		/// </summary>
		private void CalcTotalDuration()
		{
			for (int i = 0; i < mFrames.Length; i++)
			{
				mTotalDuration += mFrames[i].mDuration;
			}
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Update animation
		/// </summary>
		public void Update(MUpdateInfo info)
		{
			if (mPlaying)
			{
				UpdatePlayhead(info);

				if(mNumRepeats != 0 && mRepeatsRemaining == 0)
				{
					Stop();
				}
			}
		}


		/// <summary>
		/// Updates the play head position based on delta time and play mode.
		/// Handles loop boundaries and direction changes.
		/// </summary>
		private void UpdatePlayhead(MUpdateInfo info)
		{
			// Update position
			mPlayHead += info.mDelta * mPlayDirection;

			// Check boundaries
			bool reachedEnd = mPlayHead >= mTotalDuration;
			bool reachedStart = mPlayHead <= 0.0f;

			// Handle boundary cases
			if (reachedEnd || reachedStart)
			{
				// Calculate overflow/underflow amount
				float overflow = reachedEnd ? mPlayHead - mTotalDuration : -mPlayHead;

				// Update direction and position based on play type
				switch (mPlayType)
				{
					case PlayType.Forward:
						mPlayHead = overflow;
						mPlayDirection = 1.0f;
						break;

					case PlayType.Backwards:
						mPlayHead = mTotalDuration - overflow;
						mPlayDirection = -1.0f;
						break;

					case PlayType.PingPong:
					case PlayType.PingPongBackwards:
						mPlayHead = reachedEnd ? mTotalDuration - overflow : overflow;
						mPlayDirection *= -1.0f;
						break;
				}

				mRepeatsRemaining--;
			}
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Get texture that is currently showing
		/// </summary>
		/// <returns>Texture that is currently showing</returns>
		public MTexturePart GetCurrentTexture()
		{
			float timeLeft = mPlayHead;
			int i = 0;
			for (; i < mFrames.Length; i++)
			{
				timeLeft -= mFrames[i].mDuration;

				if (timeLeft < 0.0f)
				{
					break;
				}
			}

			i = Math.Min(i, mFrames.Length - 1);

			return mFrames[i].mTexture;
		}



		/// <summary>
		/// Get texture at index
		/// </summary>
		/// <param name="index">Frame index you want to access.</param>
		/// <returns>Texture at specified index</returns>
		public MTexturePart GetTexture(int index)
		{
			return mFrames[index].mTexture;
		}

		#endregion rDraw





		#region rControl

		/// <summary>
		/// Begin playing
		/// </summary>
		public void Play()
		{
			mPlaying = true;
			mRepeatsRemaining = mNumRepeats;
			switch (mPlayType)
			{
				case PlayType.PingPong:
				case PlayType.Forward:
					mPlayDirection = 1.0f;
					mPlayHead = 0.0f;
					break;
				case PlayType.Backwards:
				case PlayType.PingPongBackwards:
					mPlayDirection = -1.0f;
					mPlayHead = mTotalDuration;
					break;
			}
		}



		/// <summary>
		/// Stop playing
		/// </summary>
		public void Stop()
		{
			mPlaying = false;
		}



		/// <summary>
		/// Seek to point in animation.
		/// </summary>
		public void Seek(float percent)
		{
			switch (mPlayType)
			{
				case PlayType.PingPong:
				case PlayType.Forward:
					mPlayHead = percent * mTotalDuration;
					break;
				case PlayType.Backwards:
				case PlayType.PingPongBackwards:
					mPlayHead = (1.0f - percent) * mTotalDuration;
					break;
			}
		}

		#endregion rControl





		#region rUtil

		/// <summary>
		/// Check if we are playing
		/// </summary>
		/// <returns>True if we are playing</returns>
		public bool IsPlaying()
		{
			return mPlaying;
		}



		/// <summary>
		/// Set type of animation play
		/// </summary>
		public void SetType(PlayType type)
		{
			mPlayType = type;
		}



		/// <summary>
		/// Get current decseconds
		/// </summary>
		public float GetPlayHead()
		{
			return mPlayHead;
		}


		/// <summary>
		/// Set anim play position
		/// </summary>
		public void SetPlayHead(float head)
		{
			mPlayHead = head;
		}

		#endregion rUtil
	}
}
