using Godot;
using System;

public partial class StaffAgent : CharacterBody2D, IAgent
{
	ITileMapService _tileMapService;
	ILoggerService _logger;

	#region State Machine
	States CurrentState { get; set; } = States.Idle;
	enum States
	{
		MoveUp,
		MoveRight,
		MoveDown,
		MoveLeft,
		Idle,
	}
	#endregion

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_tileMapService = GetNode<ITileMapService>(Constants.SingletonNodes.TileMapService);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("up"))
		{
			CurrentState = States.MoveUp;
		}
		else if (@event.IsActionPressed("right"))
		{
			CurrentState = States.MoveRight;
		}
		else if (@event.IsActionPressed("down"))
		{
			CurrentState = States.MoveDown;
		}
		else if (@event.IsActionPressed("left"))
		{
			CurrentState = States.MoveLeft;
		}
		else if (@event.IsActionReleased("up") || @event.IsActionReleased("right") ||
				 @event.IsActionReleased("down") || @event.IsActionReleased("left"))
		{
			CurrentState = States.Idle;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		int tileSize = _tileMapService.GetTileSize();
		Vector2 movement = Vector2.Zero;

		if (CurrentState == States.MoveUp)
		{
			movement = new Vector2(0, -tileSize);
		}
		else if (CurrentState == States.MoveRight)
		{
			movement = new Vector2(tileSize, 0);
		}
		else if (CurrentState == States.MoveDown)
		{
			movement = new Vector2(0, tileSize);
		}
		else if (CurrentState == States.MoveLeft)
		{
			movement = new Vector2(-tileSize, 0);
		}

		if (movement != Vector2.Zero)
		{
			// Divide the movement into smaller steps for collision detection
			int stepCount = 10; // Number of steps to divide the movement
			Vector2 step = movement / stepCount;

			for (int i = 0; i < stepCount; i++)
			{
				KinematicCollision2D collision = MoveAndCollide(step);
				if (collision != null)
				{
					// Collision detected, stop further movement
					_logger.LogInfo($"Collision detected with: {collision.GetCollider()}");
					break;
				}
			}
		}
	}

	
	// public override void _PhysicsProcess(double delta)
	// {
	// 	int tileSize = _tileMapService.GetTileSize();
	// 	var movement = Vector2.Zero;

	// 	if (CurrentState == States.MoveUp)
	// 	{
	// 		movement = new Vector2(0, -tileSize);
	// 	}
	// 	else if (CurrentState == States.MoveRight)
	// 	{
	// 		movement = new Vector2(tileSize, 0);
	// 	}
	// 	else if (CurrentState == States.MoveDown)
	// 	{
	// 		movement = new Vector2(0, tileSize);
	// 	}
	// 	else if (CurrentState == States.MoveLeft)
	// 	{
	// 		movement = new Vector2(-tileSize, 0);
	// 	}

	// 	// Update the position instantly
	// 	if (movement != Vector2.Zero)
	// 	{
	// 		Position += movement;
	// 	}
	// }
}
