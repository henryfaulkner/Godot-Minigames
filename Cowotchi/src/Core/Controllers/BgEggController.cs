using Godot;
using System;
using System.ComponentModel;

public partial class BgEggController : CharacterBody3D
{
	[ExportGroup("Nodes")]
	[Export]
	private CollisionShape3D Collider { get; set; }
	[Export]
	private MeshInstance3D Mesh { get; set; }

	protected ILoggerService _logger { get; set; }
	private AnimationPathFactory _animationPathFactory { get; set; }

	private BouncePath BouncePath { get; set; }

	private bool IsGrounded { get; set; } // If entity is grounded this frame
	private bool WasGrounded { get; set; } // If entity was grounded last frame

	private static readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	#region State Machine

	private enum States
	{
		[Description("Standing")]
		Standing = 0,
		[Description("Bouncing")]
		Bouncing = 1,
	}

	private States CurrentState { get; set; } = States.Bouncing;

	#endregion

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_animationPathFactory = GetNode<AnimationPathFactory>(Constants.SingletonNodes.AnimationPathFactory);
	}

	private bool IsReady = false;
	public override void _PhysicsProcess(double delta)
	{
		if (!IsReady)
		{
			BouncePath = _animationPathFactory.SpawnBouncePath(GetNode(".."), this, Mesh);
			IsReady = true;
		}
		
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

			HandleCurrentState();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error in BgEggController _PhysicsProcess {ex.Message}", ex);
		}
	}

	private bool InBounceAnimation = false;
	private void HandleCurrentState()
	{
		switch(CurrentState)
		{
			case States.Standing:
				HandleStandingState();
				break;
			case States.Bouncing:
				HandleBouncingState();
				break;
			default:
				_logger.LogError($"BgEggController HandleCurrentState failed to map state. Node name: {Name}.");
				throw new Exception($"BgEggController HandleCurrentState failed to map state. Node name: {Name}.");
				break;
		}
	}

	private void HandleStandingState()
	{

	}

	public void HandleBouncingState()
	{
		BouncePath.StartAnimation();
	}
}
