// This tutorial about projectiles was helpful
// https://www.youtube.com/watch?v=IDsoEAj5xG0

using Godot;
using System;
using System.Collections.Generic;

public partial class Boomerang : RigidBody3D
{
	[Export]
	private Area3D HitBox { get; set; }
	
	[Export]
	private float MaxSpeed { get; set; } = 10.0f;

	[Export]
	private float RotationSpeed { get; set; } = 10.0f;
	
	private bool ThrowCollided { get; set; } = false;

	private List<ThrowAction> ThrowActionList { get; set; }

	public Boomerang()
	{
		ThrowActionList =  new List<ThrowAction>();
	}

	public override void _Ready()
	{
		TopLevel = true;
		
		HitBox.BodyEntered += OnBodyEntered;
		HitBox.AreaEntered += OnAreaEntered;
	}

	public void Throw(Vector3 throwDirection, float throwSpeedPercentage)
	{
		ThrowCollided = false;
		
		// directional (position) impulse w/o rotation
		ApplyCentralImpulse(throwDirection * (throwSpeedPercentage * MaxSpeed));

		// rotation impulse w/o position
		ApplyTorqueImpulse(new Vector3(0, 1 * RotationSpeed * throwSpeedPercentage, 0));

		ThrowActionList.ForEach(throwAction => throwAction.Execute());
		
		TimingFunctions.SetTimeout(() => {
			if (ThrowCollided) return;
			ApplyCentralImpulse(throwDirection * (-1.333f * throwSpeedPercentage * MaxSpeed));
		}, 1000);
	}

	public void AddThrowAction(ThrowAction throwAction) 
	{
		ThrowActionList.Add(throwAction);
	}
	
	private void OnBodyEntered(Node3D victim)
	{
		GD.Print($"Body Victim is {victim.Name}");
		if (victim.IsInGroup("Enemy"))
		{
			GD.Print("Enemy was hit!");
		} 
	}
	
	private void OnAreaEntered(Node3D victim)
	{
		GD.Print($"Area Victim is {victim.Name}");
		if (victim.IsInGroup("Structure")) 
		{
			GD.Print("Throw collided!");
			ThrowCollided = true;
		}
	}
}
