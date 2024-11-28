using Godot;
using System;
using System.ComponentModel;

public partial class BasicBgEggController : Node, IController
{
	private CharacterBody3D Puppet { get; set; }
	private CollisionShape3D Collider { get; set; }
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

	public void SetPuppet(CharacterBody3D puppet)
	{
		Puppet = puppet;
	}

	public void ReadyInstance(CollisionShape3D collider, MeshInstance3D mesh)
	{
		Collider = collider;
		Mesh = mesh;
	}

	private bool IsReady = false;
	public void PhysicsProcess(double delta)
	{
		if (!IsReady)
		{
			BouncePath = _animationPathFactory.SpawnBouncePath(GetNode(".."), Puppet, Mesh);
			IsReady = true;
		}
		
		try
		{
			// Lock player collider rotation
			Collider.GlobalRotation = Vector3.Zero;

			// Update player state
			WasGrounded = IsGrounded;
			IsGrounded = Puppet.IsOnFloor();

			// Handle gravity
			if (!Puppet.IsOnFloor()) 
			{
				Puppet.Velocity = new Vector3(
								Puppet.Velocity.X,
								Puppet.Velocity.Y - (Gravity * (float)delta),
								Puppet.Velocity.Z
							);
				Puppet.MoveAndSlide();
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
