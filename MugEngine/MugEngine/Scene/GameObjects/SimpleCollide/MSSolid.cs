﻿
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MugEngine.Scene
{
	/// <summary>
	/// A solid in the simple collisions system.
	/// Does not collide with anything. But it can move
	/// and push other MSActors
	/// </summary>
	public abstract class MSSolid : MGameObject
	{
		/// <summary>
		/// Create solid at position
		/// </summary>
		public MSSolid(Vector2 position) : base(position)
		{
		}



		/// <summary>
		/// Move by an amount, pushing actors along the way.
		/// </summary>
		public void Move(Vector2 delta)
		{
			Vector2 destPos = mPosition + delta;
			
			int moveX = (int)destPos.X - (int)mPosition.X;
			int moveY = (int)destPos.Y - (int)mPosition.Y;

			// Turn ourselves off so riders don't collide with us.
			bool origEnabled = IsEnabled();
			SetEnabled(false);

			if (moveX != 0 || moveY != 0)
			{
				//Loop through every Actor in the Level, add it to 
				//a list if actor.IsRiding(this) is true 
				List<MSActor> riding = GetAllRidingActors();

				if (moveX != 0)
				{
					MCardDir dirX = moveX > 0 ? MCardDir.Right : MCardDir.Left;

					// The rectangle which we will end up at.
					Rectangle xMovedRect = BoundsRect();
					xMovedRect.X += moveX;

					List<MSActor> pushedX = new List<MSActor>();

					foreach (MSActor actor in GO().GetInRect(xMovedRect).OfType<MSActor>())
					{
						float moveDist = (float)GetPushDistance(xMovedRect, actor, dirX);
						actor.MoveX(moveDist, true);

						pushedX.Add(actor);
					}

					// Loop over riders not already pushed.
					foreach (MSActor rider in riding.Where(r => !pushedX.Contains(r)))
					{
						rider.MoveX(moveX, false);
					}

					mPosition.X = destPos.X;
				}

				if (moveY != 0)
				{
					MCardDir dirY = moveY > 0 ? MCardDir.Down : MCardDir.Up;

					// The rectangle which we will end up at.
					Rectangle yMovedRect = BoundsRect();
					yMovedRect.Y += moveY;

					List<MSActor> pushedY = new List<MSActor>();

					foreach (MSActor actor in GO().GetInRect(yMovedRect).OfType<MSActor>())
					{
						float moveDist = (float)GetPushDistance(yMovedRect, actor, dirY);
						actor.MoveY(moveDist, true);

						pushedY.Add(actor);
					}

					// Loop over riders not already pushed.
					foreach (MSActor rider in riding.Where(r => !pushedY.Contains(r)))
					{
						rider.MoveY(moveY, false);
					}

					mPosition.Y = destPos.Y;
				}
			}

			// Reset enabled state.
			SetEnabled(origEnabled);

			mPosition = destPos;
		}



		/// <summary>
		/// Get all riding actors
		/// </summary>
		List<MSActor> GetAllRidingActors()
		{
			return GO().ActiveObjects(GetLayerMask())
						.OfType<MSActor>()
						.Where(a => a.IsRiding(this))
						.ToList();
		}


		/// <summary>
		/// Finds distance to push an actor, given a future rect and a direction of travel.
		/// </summary>
		int GetPushDistance(Rectangle ourRect, MSActor actor, MCardDir dir)
		{
			Rectangle actorRect = actor.BoundsRect();

			switch (dir)
			{
				case MCardDir.Up:
					return ourRect.Top - actorRect.Bottom;
				case MCardDir.Right:
					return ourRect.Right - actorRect.Left;
				case MCardDir.Down:
					return ourRect.Bottom - actorRect.Top;
				case MCardDir.Left:
					return ourRect.Left - actorRect.Right;
				default:
					break;
			}

			throw new NotImplementedException();
		}
	}
}
