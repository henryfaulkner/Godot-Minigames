using Godot;
using System;
using System.ComponentModel;

public partial class BgAnimalController : CharacterBody3D
{
	#region Exports

	[ExportGroup("Nodes")]
	[Export]
	private CollisionShape3D Collider { get; set; }

	[Export]
	private Timer Timer { get; set; }

	[ExportGroup("Variables")]
	[Export]
	private float WanderSpeed { get; set; } = 10.0f;

	[Export]
	private float JumpVelocity { get; set; } = 6.0f;

	[Export]
	private int MaxJumpNum { get; set; } = 1;
	
	[Export]
	private float RotationSpeed { get; set; } = 1.0f;

	[Export]
	private float RotationCloseEnough { get; set; } = 0.2f;

	#endregion

	#region Instance Variables

	private ILoggerService _logger { get; set; }

	private bool IsGrounded { get; set; } // If entity is grounded this frame
	private bool WasGrounded { get; set; } // If entity was grounded last frame

	private Vector3 WishDir { get; set; } // Player input direction (wasd)

	private static readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	private int CurrJumpNum { get; set; } = 0;
	
	#endregion

	#region State Machine

	private enum States
	{
		[Description("Standing")]
		Standing = 0,
		[Description("Turning")]
		Turning = 1, 
		[Description("Walking")]
		Walking = 2,
	}

	private States CurrentState { get; set; } = States.Walking; 

	#endregion

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);

		Timer.Timeout += HandleTimerTimeout;
	}

	public override void _PhysicsProcess(double delta)
	{
		try
		{
			// Lock player collider rotation
			Collider.GlobalRotation = Vector3.Zero;

			// Update player state
			WasGrounded = IsGrounded;
			IsGrounded = IsOnFloor();

			// Handle gravity
			if (!IsOnFloor()) 
			{
				Velocity = new Vector3(
								Velocity.X,
								Velocity.Y - (Gravity * (float)delta),
								Velocity.Z
							);
				MoveAndSlide();
			} 
			else CurrJumpNum = 0;
			
			HandleCurrentState();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error in FarmWandererController _PhysicsProcess {ex.Message}", ex);
		}
	}

	private void HandleCurrentState()
	{
		switch(CurrentState)
		{
			case States.Standing:
				HandleStandingState();
				break;
			case States.Turning:
				HandleTurningState();
				break;
			case States.Walking:
				HandleWalkingState();
				break;
			default:
				_logger.LogError($"FarmWandererController HandleCurrentState failed to map state. Node name: {Name}.");
				throw new Exception($"FarmWandererController HandleCurrentState failed to map state. Node name: {Name}.");
				break;
		}
	}

	private void HandleStandingState()
	{

	}

	private void HandleTurningState()
	{
		Timer.Stop();
		
		// Calculate the direction vector from the current position to the target
		Vector3 direction = (WishDir - Vector3.Zero).Normalized();

		// Create the target rotation based on the direction
		// LookAt creates a rotation that points the negative Z axis towards the target direction
		Basis targetBasis = Basis.LookingAt(direction, Vector3.Up);
		Vector3 targetRotation = targetBasis.GetEuler();

		// Smoothly interpolate between the current rotation and the target rotation
		GlobalRotation = GlobalRotation.Lerp(targetRotation, RotationSpeed);

		// check is rotation is "close enough"
		if (GlobalRotation.AngleTo(targetRotation) < RotationCloseEnough)
		{
			_logger.LogDebug("Change state to walking.");
			CurrentState = States.Walking;
			Timer.Start();
		}
	}

	private void HandleWalkingState()
	{
		// Handle move as a vector jump
		if (CurrJumpNum < MaxJumpNum && WishDir != Vector3.Zero)
		{
			Velocity = new Vector3(
							Velocity.X,
							JumpVelocity,
							Velocity.Z
						);
			CurrJumpNum += 1;
		}

		// Handle WASD movement
		Velocity = new Vector3(
						WishDir.X * WanderSpeed,
						Velocity.Y,
						WishDir.Z * WanderSpeed
					);

		// Move 
		MoveAndSlide();
	}

	private Vector2 GetInputDir()
	{
		return Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
	}

	private Vector2 GetRandomDir()
	{
		// Generate a random angle between 0 and 2π (TAU)
		float angle = (float)(GD.Randf() * Math.PI * 2);
		
		// Calculate the normalized vector based on the angle
		return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).Normalized();
	}

	private void HandleTimerTimeout()
	{
		var rand = new Random();

		// change timer interval to 3 - 6 seconds
		Timer.WaitTime = rand.Next(3, 7); 

		// Get new direction
		//var dirVector = GetInputDir();
		var dirVector = GetRandomDir();
		WishDir = (Transform.Basis * new Vector3(dirVector.X, 0, dirVector.Y)).Normalized();
		_logger.LogDebug($"WishDir {WishDir.ToString()}");
		Vector3 direction = (WishDir - Vector3.Zero).Normalized();
		_logger.LogDebug($"direction {direction.ToString()}");

		// Get new state (standing or turning)
		CurrentState = (States)rand.Next(2);
		_logger.LogDebug($"Change state to {CurrentState.GetDescription()}.");
	}
}
