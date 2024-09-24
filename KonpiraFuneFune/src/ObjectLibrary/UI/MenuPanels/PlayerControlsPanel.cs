using System;
using System.Collections.Generic;
using Godot;

public partial class PlayerControlsPanel : BasePanel
{
	[Export]
	private BaseButton HomeBtn;

	public override void _Ready()
	{
		FocusIndex = 0;
		Buttons = new List<BaseButton>();
		Buttons.Add(HomeBtn);
	}
}

