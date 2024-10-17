using Godot;
using System;

public partial class OpenClosePauseMenuListener : Node2D
{
	private static readonly StringName _PAUSE_INPUT = new StringName("escape");

	[Export]
	private Panel BaseMenuPanels;

	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.Always;
	}

	public override void _Process(double _delta)
	{
		if (Input.IsActionJustPressed(_PAUSE_INPUT))
		{
			if (GetTree().Paused)
			{
				HideAll();
				EmitSignal(SignalName.CloseMenu);
				GetTree().Paused = false;
			}
			else
			{
				//BaseMenuPanels[0].Show(); // show base panel
				EmitSignal(SignalName.OpenMenu);
				GetTree().Paused = true;
			}
		}
	}

	[Signal]
	public delegate void OpenMenuEventHandler();

	[Signal]
	public delegate void CloseMenuEventHandler();

	public void HideAll()
	{
		//foreach (var panel in BaseMenuPanels)
		//{
			//panel.Hide();
		//}
	}
}
