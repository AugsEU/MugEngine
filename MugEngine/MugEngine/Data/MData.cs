using LDtk;

namespace MugEngine.Data;

/// <summary>
/// Manages all the data. Use this to load.
/// </summary>
public class MData : MSingleton<MData>
{
	#region rMembers

	ContentManager mContentManager;
	List<MDataTheme> mActiveThemes;

	Dictionary<string, MAnimationData> mAnimationDataCache;
	Dictionary<string, SpriteFont> mIDToSpriteFont = new();

	#endregion rMembers





	#region rInit

	/// <summary>
	/// Initialise the data manager.
	/// </summary>
	public void Init(ContentManager content)
	{
		mContentManager = content;

		mAnimationDataCache = new Dictionary<string, MAnimationData>();
		mActiveThemes = new List<MDataTheme>();

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
		return mContentManager.Load<T>(realPath);
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
	/// Generates a new animator from an XML file.
	/// </summary>
	public MAnimation TryLoadAnimation(string path)
	{
		if(path is null)
		{
			return null;
		}

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
			animData = new MAnimationData(realPath);

			// Add to the cache
			mAnimationDataCache.Add(realPath, animData);
		}

		return animData;
	}



	/// <summary>
	/// Load LDtk file
	/// </summary>
	public LDtkFile LoadLDtkFile(string alias)
	{
		string realPath = GetRemappedPath(alias);

		return LDtkFile.FromFile(GetRemappedPath(alias));
	}


	/// <summary>
	/// Load a font in initialisation
	/// </summary>
	public void InitFont(string id, string filePath)
	{
		SpriteFont sf = mContentManager.Load<SpriteFont>(filePath);
		mIDToSpriteFont.Add(id, sf);
	}


	/// <summary>
	/// Get a font from the bank.
	/// </summary>
	public SpriteFont GetFont(string id)
	{
		if(mIDToSpriteFont.TryGetValue(id, out SpriteFont font))
		{
			return font;
		}

		return null;
	}

	#endregion rLoadUnload





	#region rUtil

	/// <summary>
	/// Apply a theme.
	/// </summary>
	public void ApplyTheme(MDataTheme theme)
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
	public void RemoveTheme(string id)
	{
		mActiveThemes.RemoveAll(t => t.GetID() == id);
	}



	/// <summary>
	/// Remove a theme of a certain ID.
	/// </summary>
	public void RemoveTheme(MDataTheme theme)
	{
		mActiveThemes.RemoveAll(t => ReferenceEquals(t, theme));
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



	/// <summary>
	/// Find all files wtihin content folder that match pattern
	/// </summary>
	public List<string> SearchRecursively(string pattern, string subFolder = "")
	{
		List<string> foundFiles = new List<string>();

		string contentPath = Path.Join(mContentManager.RootDirectory, subFolder);

		if (Directory.Exists(contentPath))
		{
			foundFiles.AddRange(Directory.GetFiles(contentPath, pattern, SearchOption.AllDirectories));
		}

		return foundFiles;
	}

	#endregion rUtil
}

