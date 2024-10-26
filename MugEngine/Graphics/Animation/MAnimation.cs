using MugEngine.Types;

namespace MugEngine.Graphics
{
	public class MAnimation : IMUpdate
	{
		#region rTypes

		/// <summary>
		/// Frame of animation
		/// </summary>
		struct AnimationFrame
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
			OneShot, // Play the animation once then stop
			Repeat // Play the animation on repeat
		};

		#endregion rTypes





		#region rMembers

		List<AnimationFrame> mFrames = new List<AnimationFrame>();
		PlayType mPlayType;
		float mTotalDuration;
		float mPlayHead;
		bool mPlaying;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Animation constructor
		/// </summary>
		/// <param name="playType">Play mode.(See enum for details)</param>
		public MAnimation(PlayType playType, params (MTexturePart, float)[] frameData)
		{
			mPlaying = false;
			mTotalDuration = 0.0f;
			mPlayHead = 0.0f;
			mPlayType = playType;

			for (int i = 0; i < frameData.Length; i++)
			{
				mTotalDuration += frameData[i].Item2;
				mFrames.Add(new AnimationFrame(frameData[i].Item1, frameData[i].Item2));
			}

			mPlaying = true;
		}



		/// <summary>
		/// Animation constructor
		/// </summary>
		/// <param name="playType">Play mode.(See enum for details)</param>
		public MAnimation(MTexturePart tex)
		{
			mPlaying = false;
			mTotalDuration = 0.0f;
			mPlayHead = 0.0f;
			mPlayType = PlayType.OneShot;
			mTotalDuration = 1.0f;

			mFrames.Add(new AnimationFrame(tex, 1.0f));

			mPlaying = false;
		}



		/// <summary>
		/// Animation constructor
		/// </summary>
		/// <param name="playType">Play mode.(See enum for details)</param>
		public MAnimation(Texture2D tex)
		{
			mPlaying = false;
			mTotalDuration = 0.0f;
			mPlayHead = 0.0f;
			mPlayType = PlayType.OneShot;
			mTotalDuration = 1.0f;

			mFrames.Add(new AnimationFrame(new MTexturePart(tex), 1.0f));

			mPlaying = false;
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
				mPlayHead += info.mDelta;

				switch (mPlayType)
				{
					case PlayType.OneShot:
						if (mPlayHead > mTotalDuration)
						{
							Stop();
						}
						break;
					case PlayType.Repeat:
						while (mPlayHead > mTotalDuration)
						{
							mPlayHead -= mTotalDuration;
						}
						break;
					default:
						break;
				}

			}
		}



		/// <summary>
		/// Begin playing
		/// </summary>
		public void Play()
		{
			mPlaying = true;
			mPlayHead = 0.0f;
		}



		/// <summary>
		/// Begin playing at a percent marker
		/// </summary>
		public void Play(float percentPlayed)
		{
			mPlaying = true;
			mPlayHead = percentPlayed * mTotalDuration;
		}



		/// <summary>
		/// Stop playing
		/// </summary>
		public void Stop()
		{
			mPlaying = false;
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
			for (; i < mFrames.Count; i++)
			{
				timeLeft -= mFrames[i].mDuration;

				if (timeLeft < 0.0f)
				{
					break;
				}
			}

			i = Math.Min(i, mFrames.Count - 1);

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
