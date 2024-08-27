using Godot;
using System;

public partial class Puck : RigidBody2D
{
	[Export]
	public float Speed { get; set; }
	public Vector2 Velocity { get; set; }
	public bool IsPuckClicked { get; set; }

	public override void _Ready()
	{
		// Initialize Velocity or other setup tasks
		Velocity = Vector2.Zero;

		// Ensure the Puck can receive mouse events
		SetProcessInput(true);
	}

	public override void _PhysicsProcess(double _delta)
	{
		// For RigidBody2D, you should use AddForce, ApplyCentralImpulse, or similar methods
		LinearVelocity = Velocity;
	}

	public void MoveToward(Vector2 target)
	{
		var direction = (target - Position).Normalized();
		Velocity = direction * Speed;
	}
}
