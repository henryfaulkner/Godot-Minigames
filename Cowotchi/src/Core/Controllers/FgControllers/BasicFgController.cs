using Godot;
using System;

public partial class BasicFgController : Node, IController
{
	private CharacterBody3D Puppet { get; set; }
	private CollisionShape3D Collider { get; set; }
	private MeshInstance3D Mesh { get; set; }

	protected ILoggerService _logger { get; set; }

	private bool IsGrounded { get; set; } // If entity is grounded this frame
	private bool WasGrounded { get; set; } // If entity was grounded last frame

	private static readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
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

	public void PhysicsProcess(double delta)
	{
		try
		{
			HandleGravity(delta);	
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error in BasicFgController _PhysicsProcess {ex.Message}", ex);
			throw;
		}
	}
	
	public void HandleGravity(double delta)
	{
		// Apply gravity always
		Puppet.Velocity += new Vector3(0, -Gravity * (float)delta, 0);
		
		// Move the puppet and handle collisions
		Puppet.MoveAndSlide();
		
		// Optional: Zero-out velocity when hitting the floor
		if (Puppet.IsOnFloor())
		{
			Puppet.Velocity = new Vector3(Puppet.Velocity.X, 0, Puppet.Velocity.Z);
		}
	}
}
