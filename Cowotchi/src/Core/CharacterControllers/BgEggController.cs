using Godot;
using System;

public partial class BgEggController : CharacterBody3D
{
	[ExportGroup("Nodes")]
	[Export]
	private CollisionShape3D Collider { get; set; }

	private ILoggerService _logger { get; set; }

	private bool IsGrounded { get; set; } // If entity is grounded this frame
	private bool WasGrounded { get; set; } // If entity was grounded last frame

	private static readonly float Gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
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
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error in BackgroundEggController _PhysicsProcess {ex.Message}", ex);
		}
	}
}
