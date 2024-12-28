
using MugEngine.Core;

namespace MugEngine.Scene
{
	/// <summary>
	/// A simple actor.
	/// Uses it's bounding box as a collider and collision physics.
	/// Based off Maddy Makes Games
	/// </summary>
	public abstract class MSActor : MGameObject
	{
		/// <summary>
		/// Create simple actor at position.
		/// </summary>
		public MSActor(Vector2 position) : base(position)
		{
		}



		/// <summary>
		/// Create simple actor at position with size.
		/// </summary>
		public MSActor(Vector2 position, Point size) : base(position, size)
		{
		}



		/// <summary>
		/// Move by an X amount with collision checks.
		/// </summary>
		public void MoveX(float amount, bool isPush)
		{
			Vector2 destPos = mPosition + Vector2.UnitX * amount;

			int move = (int)destPos.X - (int)mPosition.X;

			if (move != 0)
			{
				int sign = Math.Sign(move);
				Vector2 normal = Vector2.UnitX * sign;
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
						ResolveHitSolid(normal, isPush);
						return;
					}
					else
					{
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
			Vector2 destPos = mPosition + Vector2.UnitY * amount;

			int move = (int)destPos.Y - (int)mPosition.Y;

			if (move != 0)
			{
				int sign = Math.Sign(move);
				Vector2 normal = Vector2.UnitY * sign;
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
						ResolveHitSolid(normal, isPush);
						return;
					}
					else
					{
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
			Rectangle bounds = BoundsRect();

			foreach (MGameObject other in GO().GetInRect(bounds, GetLayerMask()))
			{
				if (other is not MSSolid || ReferenceEquals(other, this))
				{
					continue;
				}

				if (other.BoundsRect().Intersects(bounds))
				{
					return true;
				}
			}

			bool levelCollides = GO().GetLevel().QueryCollides(bounds, dir);

			return levelCollides;
		}



		/// <summary>
		/// Dispatch hit solid event to relevent function
		/// </summary>
		void ResolveHitSolid(Vector2 normal, bool isPush)
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
		public virtual void OnHitSolid(Vector2 normal)
		{
		}



		/// <summary>
		/// Called every time something pushes us into a another solid.
		/// Usually we should just die.
		/// </summary>
		public virtual void Squish(Vector2 normal)
		{
			Kill();
		}



		/// <summary>
		/// Are we riding this solid?
		/// </summary>
		public abstract bool IsRiding(MSSolid solid);
	}
}
