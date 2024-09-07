using Godot;
using System;

public partial class GameScene : Node3D
{
	[Export]
	private float Speed { get; set; }

	[Export]
	private PathFollow3D PathFollow_Player1 { get; set; }

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		ProcessPathFollow(PathFollow_Player1, delta);
	}

	private void ProcessPathFollow(PathFollow3D pf, double delta)
	{
		pf.ProgressRatio += Speed * (float)delta;
	}
}
