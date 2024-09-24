using System;
using System.Collections.Generic;
using Godot;

public partial class AudioSettingsPanel : BasePanel
{
	[Export]
	private BaseButton FxSoundBtn;

	[Export]
	private BaseButton HomeBtn;

	public List<BaseButton> Buttons { get; set; }

	public override void _Ready()
	{
		FocusIndex = 0;
		Buttons = new List<BaseButton>();
		Buttons.Add(FxSoundBtn);
		Buttons.Add(HomeBtn);
	}
}

