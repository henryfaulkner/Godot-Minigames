using Godot;
using System;

public partial class Observables : Node
{
	[Signal]
	public delegate void OpenAdminPanelEventHandler();
	public void EmitOpenAdminPanel()
	{
		EmitOpenAdminPanel();
	}
}
