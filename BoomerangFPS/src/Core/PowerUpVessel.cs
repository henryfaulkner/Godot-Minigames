using Godot;
using System;

public partial class PowerUpVessel : StaticBody3D
{
	[Export]
	private Area3D HitBox { get; set; }
	
	[Export]
	private float RotationSpeed { get; set; }
	
	public override void _Ready()
	{	
		SetAxisLock(PhysicsServer3D.BodyAxis.AngularX, true);
		SetAxisLock(PhysicsServer3D.BodyAxis.AngularY, false); // Spin the Y
		SetAxisLock(PhysicsServer3D.BodyAxis.AngularZ, true);
		SetAxisLock(PhysicsServer3D.BodyAxis.LinearX, true);
		SetAxisLock(PhysicsServer3D.BodyAxis.LinearY, true);
		SetAxisLock(PhysicsServer3D.BodyAxis.LinearZ, true);
		
		HitBox.BodyEntered += Hit;
	}
	
	public override void _PhysicsProcess(double _delta)
	{
		RotateY(RotationSpeed);
	}
	
	private void Hit(Node3D receiver)
	{
		if (receiver.IsInGroup("Player"))
		{
			GD.Print("Power up received!");
			QueueFree();
		}
	} 
}
