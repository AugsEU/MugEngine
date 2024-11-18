﻿using MugEngine.Types;

namespace MugEngine.Input
{
	public partial class MugInput : MSingleton<MugInput>
	{
		#region rMembers

		MInputHistory mHistory;
		Dictionary<int, MButtonSet> mButtonBindings;

		#endregion rMembers



		#region rInit

		/// <summary>
		/// Initialise input manager
		/// </summary>
		public void Init(int historySize)
		{
			mButtonBindings = new Dictionary<int, MButtonSet>();
			mHistory = new MInputHistory(historySize);
		}



		/// <summary>
		/// Add a new button binding or replace existing one.
		/// </summary>
		public void BindButton<T>(T id, params MButton[] buttons) where T : Enum
		{
			int idx = Convert.ToInt32(id);

			mButtonBindings[idx] = new MButtonSet(buttons);
		}

		#endregion rInit





		#region rUpdate

		/// <summary>
		/// Poll inputs
		/// </summary>
		public void Update(TimeSpan timeStamp)
		{
			mHistory.PollInputs(timeStamp);
		}

		#endregion rUpdate





		#region rPadDetect

		/// <summary>
		/// Gets the "main" controller for singleplayer stuff
		/// </summary>
		public int GetMainGamepadIdx()
		{
			// To do: do something smarter than just saying P1
			return 0;
		}

		#endregion rPadDetect





		#region rUtil

		/// <summary>
		/// Get a button set from an Enum id
		/// </summary>
		private MButtonSet GetButtonSet<T>(T id) where T : Enum
		{
			int idx = Convert.ToInt32(id);

			MButtonSet retVal = null;

			mButtonBindings.TryGetValue(idx, out retVal);

			return retVal;
		}

		#endregion rUtil
	}
}