namespace MugEngine.Input
{
	/// <summary>
	/// Button press methods for MugInput
	/// </summary>
	public partial class MugInput : MSingleton<MugInput>
	{
		/// <summary>
		/// Is this button currently down?
		/// </summary>
		public bool ButtonDown<T>(T id) where T : Enum
		{
			MButtonSet set = GetButtonSet(id);
			MInputSnapshot snapShot = mHistory.SnapshotFromFrames(0);
			return set.IsPressed(ref snapShot);
		}



		/// <summary>
		/// Is this button currently down but wasn't on the previous frame?
		/// </summary>
		public bool ButtonPressed<T>(T id, int buffer = 1) where T : Enum
		{
			MButtonSet set = GetButtonSet(id);

			MInputSnapshot snapShotNow = mHistory.SnapshotFromFrames(0);

			if (set.IsPressed(ref snapShotNow))
			{
				for (int b = 0; b < buffer; b++)
				{
					MInputSnapshot snapShotBefore = mHistory.SnapshotFromFrames(b + 1);

					if (!set.IsPressed(ref snapShotBefore))
					{
						return true;
					}
				}
			}

			return false;
		}



		/// <summary>
		/// Creates non-normalised vector based on 4 directional buttons
		/// </summary>
		public Vector2 DPadToVector<T>(T up, T down, T left, T right) where T : Enum
		{
			Vector2 result = Vector2.Zero;
			if (ButtonDown(up))
			{
				result.Y -= 1.0f;
			}

			if (ButtonDown(down))
			{
				result.Y += 1.0f;
			}

			if (ButtonDown(left))
			{
				result.X -= 1.0f;
			}

			if (ButtonDown(right))
			{
				result.X += 1.0f;
			}

			return result;
		}


		/// <summary>
		/// Gets a walk direction from left-right
		/// </summary>
		public MWalkDir WalkDirFromButtons<T>(T left, T right) where T : Enum
		{
			bool isLeft = ButtonDown(left);
			bool isRight = ButtonDown(right);

			if (isLeft == isRight)
			{
				return MWalkDir.None;
			}

			if (isLeft) return MWalkDir.Left;
			if (isRight) return MWalkDir.Right;

			throw new NotImplementedException();
		}


		/// <summary>
		/// For debugging presses. Will always return false in release.
		/// </summary>
		public bool DebugButtonPressed(Keys key, int buffer = 1)
		{
#if DEBUG
			MInputSnapshot snapShotNow = mHistory.SnapshotFromFrames(0);

			if (snapShotNow.mKeyboardState.IsKeyDown(key))
			{
				for (int b = 0; b < buffer; b++)
				{
					MInputSnapshot snapShotBefore = mHistory.SnapshotFromFrames(b + 1);

					if (!snapShotBefore.mKeyboardState.IsKeyDown(key))
					{
						return true;
					}
				}
			}

			return false;
#else // DEBUG
			return false;
#endif // DEBUG

		}
	}
}
