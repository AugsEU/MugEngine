﻿using System;

namespace MugEngine.Library;

/// <summary>
/// Thing which can be added to an object pool.
/// </summary>
interface IMObjectPoolItem
{
	/// <summary>
	/// Called when "created in the pool"
	/// </summary>
	public void OnCreate();



	/// <summary>
	/// Called when "destroyed" in the object pool
	/// </summary>
	public void OnDestroy();
}
