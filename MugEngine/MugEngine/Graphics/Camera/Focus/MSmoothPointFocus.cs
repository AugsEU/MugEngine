﻿
namespace MugEngine.Graphics;

/// <summary>
/// Focus to follow a moving point smoothly
/// </summary>
public abstract class MSmoothPointFocus : MCameraFocus
{
	#region rConstants

	const float OVERSHOOT_DIST = 8.0f;
	const float SNAP_TO_DIST = 1.5f;

	#endregion rConstants




	#region rMembers

	public Vector4 pSpeed { get; set; }

	MRollingVector2 mPositionStack = new MRollingVector2(3);


	#endregion rMembers





	#region rInit

	/// <summary>
	/// Init player focus object
	/// </summary>
	public MSmoothPointFocus()
	{
		pSpeed = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
	}

	#endregion rInit





	#region rUpdate

	/// <summary>
	/// Update focus point
	/// </summary>
	public override MCameraSpec UpdateFocusPoint(MUpdateInfo info, MCameraSpec curr)
	{
		Vector2 target = GetTargetPoint();
		Vector2 toTarget = target - curr.mPosition;
		float toTargetLen = toTarget.Length();

		Vector2 overshootTarget = target + toTarget * (OVERSHOOT_DIST / toTargetLen);

		if (toTargetLen < SNAP_TO_DIST)
		{
			curr.mPosition = target;
		}
		else
		{
			float speedX = toTarget.X > 0.0f ? pSpeed.X : pSpeed.Z;
			float speedY = toTarget.Y > 0.0f ? pSpeed.Y : pSpeed.W;

			Vector2 dtDamp = new Vector2(1.0f - MathF.Exp(-speedX * info.mDelta), 1.0f - MathF.Exp(-speedY * info.mDelta));

			curr.mPosition += (overshootTarget - curr.mPosition) * dtDamp;

			Vector2 newToTarget = target - curr.mPosition;
			if (Vector2.Dot(newToTarget, toTarget) < 0.0f)
			{
				curr.mPosition = target;
			}
		}

		mPositionStack.Add(curr.mPosition);

		curr.mPosition = mPositionStack.GetAverage();
		curr.mPosition.Round();

		return curr;

	}

	/// <summary>
	/// Get a target point
	/// </summary>
	/// <returns></returns>
	abstract protected Vector2 GetTargetPoint();

	#endregion rUpdate
}

