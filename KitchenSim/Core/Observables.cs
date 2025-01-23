using Godot;
using System;

public partial class Observables : Node
{
	[Signal]
	public delegate void OpenAdminPanelEventHandler();
	public void EmitOpenAdminPanel()
	{
		EmitSignal(SignalName.OpenAdminPanel);
	}

	[Signal]
	public delegate void SetDebugTargetMarkerEventHandler(Node2D node);
	public void EmitSetDebugTargetMarker(Node2D node)
	{
		EmitSignal(SignalName.SetDebugTargetMarker, node);
	}
}
