using Godot;
using System;

public partial class RevealCapybara : StaticBody3D
{
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
	}
	
	public override void _PhysicsProcess(double _delta)
	{
		RotateY(RotationSpeed);
	}
}
