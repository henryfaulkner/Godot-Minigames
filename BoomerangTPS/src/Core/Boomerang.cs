// This tutorial about projectiles was helpful when making this
// https://www.youtube.com/watch?v=IDsoEAj5xG0

using Godot;
using System;

public partial class Boomerang : RigidBody3D
{
	[Export]
	private Area3D HitBox { get; set; }
	
	[Export]
	private float MaxSpeed { get; set; } = 10.0f;

	[Export]
	private float RotationSpeed { get; set; } = 10.0f;

	public override void _Ready()
	{
		TopLevel = true;
		
		HitBox.BodyEntered += Hit;
	}

	public void Throw(Vector3 throwDirection, float throwSpeedPercentage)
	{
		// directional (position) impulse w/o rotation
		ApplyCentralImpulse(throwDirection * (throwSpeedPercentage * MaxSpeed));

		// rotation impulse w/o position
		ApplyTorqueImpulse(new Vector3(0, 1 * RotationSpeed, 0));
	}
	
	public void Hit(Node3D victim)
	{
		if (victim.IsInGroup("Enemy"))
		{
			GD.Print("Enemy was hit!");
		}
	}
}
