using Godot;
using System;

public partial class Projectile : RigidBody3D
{
	[Export]
	private float Speed { get; set; } = 10.0f;

	private bool Thrown { get; set; } = false;

	public override void _Ready()
	{
		// inherit parent transformation until thrown
		TopLevel = false;
	}

	public override void _Process(double delta)
	{
		if (Thrown)
		{
			TopLevel = true;
			ApplyImpulse(Transform.Basis.Z, -Transform.Basis.Z);
		}
	}
}
