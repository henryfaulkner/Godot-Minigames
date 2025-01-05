using Godot;
using System;

public partial class AgentController : CharacterBody2D
{
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
		HandlePathFinding();
		HandleCollision();
		HandleMovement();
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
		_logger.LogInfo($"PathFinding Vector: {dir.ToString()}");
		
		bool useX = Mathf.Abs(dir.X) >= Mathf.Abs(dir.Y);
		bool useY = Mathf.Abs(dir.X) < Mathf.Abs(dir.Y);
		if (useY && dir.Y < 0)
		{
			CurrentState = States.MoveUp;
		}
		else if (useX && dir.X >= 0)
		{
			CurrentState = States.MoveRight;
		}
		else if (useY && dir.Y >= 0)
		{
			CurrentState = States.MoveDown;
		}
		else if (useX && dir.X < 0)
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
}
