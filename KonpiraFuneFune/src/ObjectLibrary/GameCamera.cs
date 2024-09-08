using Godot;
using System;

public partial class GameCamera : Camera3D
{
	[Export]
	private Node3D Target { get; set; }

	public override void _Ready()
	{
		LookAt(Target.Position);
	}
}
