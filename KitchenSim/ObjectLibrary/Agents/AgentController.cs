using Godot;
using System;

public partial class AgentController : CharacterBody2D
{
	[Signal]
	public delegate void WithinOneCardinalBlockFromNavTargetEventHandler();

	[ExportGroup("RayCasts")]
	[Export]
	protected RayCast2D RayCastUp;
	[Export]
	protected RayCast2D RayCastRight;
	[Export]
	protected RayCast2D RayCastDown;
	[Export]
	protected RayCast2D RayCastLeft;

	// Good NavigationAgent2D tutorial
	// https://www.youtube.com/watch?v=Lt9YdQ6Ztm4&t=13s
	[ExportGroup("Movement")]
	[Export]
	protected NavigationAgent2D NavAgent;
	[Export]
	protected Timer MovementTimer;

	Node2D _navTarget;

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

		MovementTimer.Timeout += HandleTimerTimeout;
		NavAgent.VelocityComputed += HandleNavAgentVelocityComputed; // I am trying to get the agents to avoid each other

		Velocity = new Vector2(1, 1);
	}

	public override void _Input(InputEvent @event)
	{
		//HandleManualInputs(@event);
	}
	
	public void SetNavTarget(Node2D navTarget)
	{
		_navTarget = navTarget;
	}
	
	private void HandleTimerTimeout()
	{
		if (_navTarget == null) return;
		HandlePathFinding();
		//HandleCollision();
		HandleMovement();
		HandleArrival();
	}

	private void HandleMovement()
	{
		int tileSize = _tileMapService.GetTileSize();
		var movement = Vector2.Zero;

		switch (CurrentState)
		{
			case States.MoveUp:
				movement = new Vector2(0, -tileSize);
				break;
			case States.MoveRight:
				movement = new Vector2(tileSize, 0);
				break;
			case States.MoveDown:
				movement = new Vector2(0, tileSize);
				break;
			case States.MoveLeft:
				movement = new Vector2(-tileSize, 0);
				break;
			default:
				break;
		}

		// Update the position instantly
		if (movement != Vector2.Zero)
		{
			Position += movement;
		}
	}

	private void HandleCollision()
	{
		switch (CurrentState)
		{
			case States.MoveUp: 
				if (RayCastUp.IsColliding()) CurrentState = States.Idle;
				break;
			case States.MoveRight:
				if (RayCastRight.IsColliding()) CurrentState = States.Idle;
				break;
			case States.MoveDown:
				if (RayCastDown.IsColliding()) CurrentState = States.Idle;
				break;
			case States.MoveLeft:
				if (RayCastLeft.IsColliding()) CurrentState = States.Idle;
				break;
		}
	}

	private void HandlePathFinding()
	{
		NavAgent.TargetPosition = _navTarget.GlobalPosition;
		var dir = ToLocal(NavAgent.GetNextPathPosition()).Normalized();

		NavAgent.Velocity // I am trying to get the agents to avoid each other
			= GlobalPosition.DirectionTo(NavAgent.TargetPosition) * _tileMapService.GetTileSize();
		
		bool useX = Mathf.Abs(dir.X) >= Mathf.Abs(dir.Y);
		bool useY = Mathf.Abs(dir.X) < Mathf.Abs(dir.Y);
		if (useY && dir.Y < 0 && Mathf.Floor(NavAgent.Velocity.Y) != 0)
		{
			CurrentState = States.MoveUp;
		}
		else if (useX && dir.X >= 0 && Mathf.Floor(NavAgent.Velocity.X) != 0)
		{
			CurrentState = States.MoveRight;
		}
		else if (useY && dir.Y >= 0 && Mathf.Floor(NavAgent.Velocity.Y) != 0)
		{
			CurrentState = States.MoveDown;
		}
		else if (useX && dir.X < 0 && Mathf.Floor(NavAgent.Velocity.Y) != 0)
		{
			CurrentState = States.MoveLeft;
		}
	}

	private void HandleManualInputs(InputEvent @event)
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

	// Signal should only be emitted once per target
	Vector2 _globalPositionAtArrival = Vector2.Zero;
	private void HandleArrival()
	{
		int tileSize = _tileMapService.GetTileSize();
		bool shouldEmitSignal = GlobalPosition != _globalPositionAtArrival;

		if (shouldEmitSignal
			&& GlobalPosition.Y == _navTarget.GlobalPosition.Y + tileSize
			&& GlobalPosition.X == _navTarget.GlobalPosition.X)
		{
			// if target is immediately north 
			_logger.LogDebug($"target is immediately north");
			EmitSignal(SignalName.WithinOneCardinalBlockFromNavTarget);
		}
		else if (shouldEmitSignal
			&& GlobalPosition.X == _navTarget.GlobalPosition.X - tileSize
			&& GlobalPosition.Y == _navTarget.GlobalPosition.Y)
		{
			// if target is immediately east 
			_logger.LogDebug($"target is immediately east");
			EmitSignal(SignalName.WithinOneCardinalBlockFromNavTarget);
		} 
		else if (shouldEmitSignal
			&& GlobalPosition.Y == _navTarget.GlobalPosition.Y - tileSize
			&& GlobalPosition.X == _navTarget.GlobalPosition.X)
		{
			// if target is immediately south 
			_logger.LogDebug($"target is immediately south");
			EmitSignal(SignalName.WithinOneCardinalBlockFromNavTarget);
		} 
		else if (shouldEmitSignal
			&& GlobalPosition.X == _navTarget.GlobalPosition.X + tileSize
			&& GlobalPosition.Y == _navTarget.GlobalPosition.Y)
		{
			// if target is immediately west 
			_logger.LogDebug($"target is immediately west");
			EmitSignal(SignalName.WithinOneCardinalBlockFromNavTarget);
		}
	}

	// I am trying to get the agents to avoid each other
	private void HandleNavAgentVelocityComputed(Vector2 safeVelocity)
	{
		_logger.LogDebug($"Safe Velocity {safeVelocity.ToString()}");
		Velocity = safeVelocity;
	}
}
