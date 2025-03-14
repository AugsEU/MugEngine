
using MugEngine.Core;
using TracyWrapper;

namespace MugEngine.Scene;

/// <summary>
/// A simple actor.
/// Uses it's bounding box as a collider and collision physics.
/// Based off Maddy Makes Games
/// </summary>
public abstract class MSActor : MGameObject
{
	// Since we move per-pixel, the cost of moving is O(n). We want to limit this.
	const float MAX_MOVE = 10.0f;

	/// <summary>
	/// Create simple actor at position.
	/// </summary>
	public MSActor(Vector2 position) : base(position)
	{
	}



	/// <summary>
	/// Move by an X amount with collision checks.
	/// </summary>
	public void MoveX(float amount, bool isPush)
	{
		amount = MathF.Min(MAX_MOVE, amount);
		Vector2 destPos = mPosition + Vector2.UnitX * amount;

		int move = (int)destPos.X - (int)mPosition.X;

		if (move != 0)
		{
			int sign = Math.Sign(move);
			MCardDir dir = sign < 0 ? MCardDir.Left : MCardDir.Right;

			while (move != 0)
			{
				// Move by one.
				move -= sign;
				mPosition.X += sign;

				if (CollidesWithAnySolid(dir))
				{
					// Hit something, move back and do callback.
					mPosition.X -= sign;
					ResolveHitSolid(dir.Inverted(), isPush);
					return;
				}
			}
		}

		// Move to dest position exactly to account for sub-pixel movement.
		mPosition = destPos;
	}



	/// <summary>
	/// Move by a Y amount with collisions checks.
	/// </summary>
	public void MoveY(float amount, bool isPush)
	{
		amount = MathF.Min(MAX_MOVE, amount);
		Vector2 destPos = mPosition + Vector2.UnitY * amount;

		int move = (int)destPos.Y - (int)mPosition.Y;

		if (move != 0)
		{
			int sign = Math.Sign(move);
			MCardDir dir = sign < 0 ? MCardDir.Up : MCardDir.Down;

			while (move != 0)
			{
				// Move by one.
				move -= sign;
				mPosition.Y += sign;

				if (CollidesWithAnySolid(dir))
				{
					// Hit something, move back and do callback.
					mPosition.Y -= sign;
					ResolveHitSolid(dir.Inverted(), isPush);
					return;
				}
			}
		}

		// Move to dest position exactly to account for sub-pixel movement.
		mPosition = destPos;
	}



	/// <summary>
	/// Simple collision check.
	/// </summary>
	public bool CollidesWithAnySolid(MCardDir dir)
	{
		return CollidesWithAnySolid(BoundsRect(), dir);
	}



	/// <summary>
	/// Simple collision check.
	/// </summary>
	public bool CollidesWithAnySolid(Rectangle bounds, MCardDir dir)
	{
		Profiler.PushProfileZone("SA Collide");

		foreach (MGameObject other in GO().GetInRect(bounds, GetLayerMask()))
		{
			if (ReferenceEquals(other, this))
			{
				continue;
			}

			if (other is MSSolid solid)
			{
				bool collides = solid.QueryCollides(bounds, dir);
				if (collides)
				{
					Profiler.PopProfileZone();
					return true;
				}
			}
		}

		bool? levelCollides = GO().GetLevel()?.QueryCollides(bounds, dir);

		Profiler.PopProfileZone();
		return levelCollides.HasValue && levelCollides.Value;
	}



	/// <summary>
	/// Dispatch hit solid event to relevent function
	/// </summary>
	void ResolveHitSolid(MCardDir normal, bool isPush)
	{
		if (isPush)
		{
			Squish(normal);
		}
		else
		{
			OnHitSolid(normal);
		}
	}



	/// <summary>
	/// Called every time a solid stops us from moving.
	/// </summary>
	public virtual void OnHitSolid(MCardDir normal)
	{
	}



	/// <summary>
	/// Called every time something pushes us into a another solid.
	/// Usually we should just die.
	/// </summary>
	public virtual void Squish(MCardDir normal)
	{
		Kill();
	}



	/// <summary>
	/// Are we riding this solid?
	/// </summary>
	public abstract bool IsRiding(MSSolid solid);



	/// <summary>
	/// Try and leave collisions
	/// </summary>
	protected bool TryPushOutOfCollision(MCardDir dir, int maxIterations = 50)
	{
		Vector2 originalPos = mPosition;
		int i = 0;
		while(CollidesWithAnySolid(dir))
		{
			if (i > maxIterations)
			{
				mPosition = originalPos;
				return false;
			}

			mPosition += dir.ToVec();
			i++;
		}

		return true;
	}



	/// <summary>
	/// Go in direction until we hit something.
	/// </summary>
	protected bool TryPushIntoCollision(MCardDir dir, int maxIterations = 50)
	{
		Vector2 originalPos = mPosition;
		int i = 0;
		while (true)
		{
			if (i > maxIterations)
			{
				mPosition = originalPos;
				return false;
			}

			mPosition += dir.ToVec();
			i++;

			if (CollidesWithAnySolid(dir))
			{
				mPosition -= dir.ToVec();
				break;
			}
		}

		return true;
	}
}

