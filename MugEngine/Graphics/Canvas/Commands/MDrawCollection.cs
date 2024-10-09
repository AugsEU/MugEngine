namespace MugEngine.Graphics
{
	struct MSpriteBatchBookmark
	{
		public int mIndex;
		public MSpriteBatchOptions mOptions;

		public MSpriteBatchBookmark(int index, MSpriteBatchOptions options)
		{
			mIndex = index;
			mOptions = options;
		}
	}

	public abstract class MDrawCollection
	{
		List<MSpriteBatchBookmark> mBookmarks;
		MSpriteBatchOptions mStartBatchOptions;

		public MDrawCollection()
		{
			mBookmarks = new List<MSpriteBatchBookmark>();
			mStartBatchOptions = new MSpriteBatchOptions();
		}

		public void DrawAll(SpriteBatch sb, Matrix viewPort)
		{
			int count = GetCount();

			if (count == 0)
			{
				return;
			}

			int bookmarkIdx = mBookmarks.Count > 0 ? 0 : -1;
			MSpriteBatchOptions options = mStartBatchOptions;

			sb.MugStartSpriteBatch(options, viewPort);

			for (int i = 0; i < count; i++)
			{
				if (bookmarkIdx != -1 && i == mBookmarks[bookmarkIdx].mIndex)
				{
					sb.End();
					sb.MugStartSpriteBatch(mBookmarks[bookmarkIdx].mOptions, viewPort);
					bookmarkIdx++;

					if (bookmarkIdx == mBookmarks.Count)
					{
						bookmarkIdx = -1;
					}
				}

				DrawIndex(sb, i);
			}

			sb.End();
		}


		public void SetSpritebatchOptions(MSpriteBatchOptions options)
		{
			int count = GetCount();

			if (count == 0)
			{
				mStartBatchOptions = options;
			}
			else
			{
				mBookmarks.Add(new MSpriteBatchBookmark(GetCount(), options));
			}
		}

		protected abstract void DrawIndex(SpriteBatch sb, int index);

		protected abstract int GetCount();

		public virtual void Clear()
		{
			mBookmarks.Clear();
			mStartBatchOptions = new MSpriteBatchOptions();
		}
	}
}
