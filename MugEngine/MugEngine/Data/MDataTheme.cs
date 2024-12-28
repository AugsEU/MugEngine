namespace MugEngine.Data
{
	public class MDataTheme
	{
		string mID;
		Dictionary<string, string> mAliasToPathMap;

		public MDataTheme(string id)
		{
			mID = id;
			mAliasToPathMap = new Dictionary<string, string>();
		}

		public void MapAliasToPath(string alias, string value)
		{
			MugDebug.Assert(alias != null && !HasAlias(alias), "Invalid alias {0}", alias);
			mAliasToPathMap[alias] = value;
		}

		public void RemoveAliasMap(string alias)
		{
			MugDebug.Assert(alias != null && HasAlias(alias), "Cannot remove alias {0}", alias);
			mAliasToPathMap.Remove(alias);
		}

		public string GetPath(string alias)
		{
			if (alias.StartsWith("Content"))
			{
				alias = alias.Substring(8);
			}
#if DEBUG
			else if (alias.Contains(":"))
			{
				throw new Exception("Trying to access path |" + alias + "| is not valid. Make relative to the game.");
			}
#endif

			string newPath = alias;
			while (mAliasToPathMap.TryGetValue(alias, out newPath))
			{
				alias = newPath;
			}

			return alias;
		}

		public bool HasAlias(string alias)
		{
			return mAliasToPathMap.ContainsKey(alias);
		}

		public bool DoesConflict(MDataTheme other)
		{
			foreach (var kv in mAliasToPathMap)
			{
				if (other.HasAlias(kv.Key))
				{
					return true;
				}
			}

			return false;
		}

		public string GetID()
		{
			return mID;
		}
	}
}
