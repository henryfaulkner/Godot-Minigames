using Godot;
using System;

public partial class SpinToWin : Control
{
	[Export]
	private Line2D WheelHand { get; set; }

	[Export]
	private Control Wheel { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		WheelHand.Rotation += 2.5f * (float)delta;
	}
}
