using Godot;
using System;

public partial class Main : Node2D
{
	[Export]
	public Puck Puck { get; set; }

	public override void _Ready()
	{
		Puck.IsPuckClicked = false;
		Puck.MouseEntered += HandlePuckMouseEnter;
		Puck.MouseExited += HandlePuckMouseExit;
		Puck.InputEvent += (Node viewport, InputEvent @event, long shape_idx) => HandlePuckMouseInput(viewport, @event, shape_idx);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			if (Puck.IsPuckClicked)
			{
				Puck.MoveToward(eventMouseMotion.Position);
			}
		}
	}

	private void HandlePuckMouseEnter()
	{
		GD.Print("Mouse entered puck region");
	}

	private void HandlePuckMouseInput(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.Pressed)
			{
				GD.Print("Mouse clicked down puck region");
				Puck.IsPuckClicked = true;
			}
			else
			{
				GD.Print("Mouse clicked up puck region");
				Puck.IsPuckClicked = false;
			}
		}
	}

	private void HandlePuckMouseExit()
	{
		GD.Print("Mouse exited puck region");
		Puck.IsPuckClicked = false;
	}
}
