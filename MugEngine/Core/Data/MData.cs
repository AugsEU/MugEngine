using Microsoft.Xna.Framework.Content;
using MugEngine.Graphics;
using MugEngine.Types;
using System.IO;

namespace MugEngine.Core
{
	/// <summary>
	/// Manages all the data. Use this to load.
	/// </summary>
	public class MData : MSingleton<MData>
	{
		#region rMembers

		ContentManager mContentManager;
		List<MDataTheme> mActiveThemes;

		Dictionary<string, MAnimationData> mAnimationDataCache;

		#endregion rMembers





		#region rInit

		/// <summary>
		/// Init data
		/// </summary>
		public MData()
		{
			mAnimationDataCache = new Dictionary<string, MAnimationData>();
		}


		/// <summary>
		/// Initialise the data manager.
		/// </summary>
		public void Init(ContentManager content)
		{
			mContentManager = content;

			// To do: try to populate animation cache.
		}

		#endregion rInit





		#region rLoadUnload

		/// <summary>
		/// Load basic monogame type.
		/// If this type has been loaded before, this won't incur a disc read.
		/// </summary>
		public T Load<T>(string alias)
		{
			string realPath = GetRemappedPath(alias);
			return mContentManager.Load<T>(GetRemappedPath(alias));
		}



		/// <summary>
		/// Generates a new animator from an XML file.
		/// </summary>
		public MAnimation LoadAnimation(string path)
		{
			MAnimationData animData = LoadAnimData(path);

			return animData.GenerateAnimation();
		}



		/// <summary>
		/// Load animator data
		/// </summary>
		private MAnimationData LoadAnimData(string alias)
		{
			string realPath = GetRemappedPath(alias);

			MAnimationData animData = null;

			if (mAnimationDataCache.TryGetValue(realPath, out animData))
			{
				// Loaded anim data from cache.
			}
			else
			{
				animData = new MAnimationData(alias);

				// Add to the cache
				mAnimationDataCache.Add(realPath, animData);
			}

			return animData;
		}

		#endregion rLoadUnload





		#region rUtil

		/// <summary>
		/// Apply a theme.
		/// </summary>
		void ApplyTheme(MDataTheme theme)
		{
#if DEBUG
			// Check for conflicts.
			foreach (MDataTheme otherTheme in mActiveThemes)
			{
				MugDebug.Assert(!otherTheme.DoesConflict(theme), "Conflicting themes detected.");
			}
#endif // DEBUG

			mActiveThemes.Add(theme);
		}


		/// <summary>
		/// Remove a theme of a certain ID.
		/// </summary>
		void RemoveTheme(string id)
		{
			for (int i = 0; i < mActiveThemes.Count; i++)
			{
				if (mActiveThemes[i].GetID() == id)
				{
					mActiveThemes.RemoveAt(i);
					break;
				}
			}
		}



		/// <summary>
		/// Gets remapped path on account of all the themes applied.
		/// </summary>
		string GetRemappedPath(string alias)
		{
			foreach (MDataTheme theme in mActiveThemes)
			{
				alias = theme.GetPath(alias);
			}

			return alias;
		}

		#endregion rUtil
	}
}
