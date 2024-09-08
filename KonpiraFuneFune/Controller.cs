using Godot;
using System;

public partial class Controller : Node3D
{
	[Export]
	private Character Character { get; set; }

	private StringName TapInput { get; set; }
	private StringName GrabInput { get; set; }
	private StringName KnockInput { get; set; }

	public override void _PhysicsProcess(double _delta)
	{
		if (TapInput == null || TapInput == string.Empty
			|| GrabInput == null || GrabInput == string.Empty
			|| KnockInput == null || KnockInput == string.Empty)
		{
			GD.Print("A Controller input is null or empty.");
			return;
		}

		GD.Print(TapInput);
		GD.Print(GrabInput);
		GD.Print(KnockInput);

		if (Input.IsActionJustPressed(TapInput))
		{
			Tap();
		}
		else if (Input.IsActionJustPressed(GrabInput))
		{
			Grab();
		}
		else if (Input.IsActionJustPressed(KnockInput))
		{
			Knock();
		}
	}

	[Signal]
	public delegate void CommandEventHandler(int commandType);

	public void SetTapInput(string inputKey)
	{
		TapInput = new StringName(inputKey);
	}

	public void SetGrabInput(string inputKey)
	{
		GrabInput = new StringName(inputKey);
	}

	public void SetKnockInput(string inputKey)
	{
		KnockInput = new StringName(inputKey);
	}

	private void Tap()
	{
		Character.Tap();
		EmitSignal(SignalName.Command, (int)Enumerations.Commands.Tap);
	}

	private void Grab()
	{
		Character.Grab();
		EmitSignal(SignalName.Command, (int)Enumerations.Commands.Grab);
	}

	private void Knock()
	{
		Character.Knock();
		EmitSignal(SignalName.Command, (int)Enumerations.Commands.Knock);
	}
}
