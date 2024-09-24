using System;
using System.Collections.Generic;
using Godot;

public partial class GameplaySettingsPanel : BasePanel
{
	[Export]
	private BaseButton PlayerControlsBtn;

	[Export]
	private BaseButton HomeBtn;

	public List<BaseButton> Buttons { get; set; }

	public override void _Ready()
	{
		FocusIndex = 0;
		Buttons = new List<BaseButton>();
		Buttons.Add(PlayerControlsBtn);
		Buttons.Add(HomeBtn);
	}
}

