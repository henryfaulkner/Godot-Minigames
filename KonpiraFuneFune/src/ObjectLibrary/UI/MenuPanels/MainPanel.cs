using System;
using System.Collections.Generic;
using Godot;

public partial class MainPanel : BasePanel
{
	[Export]
	private BaseButton ResumeBtn;

	[Export]
	private BaseButton AudioSettingsBtn;

	[Export]
	private BaseButton GameplaySettingsBtn;

	[Export]
	private BaseButton MainMenuBtn;

	public override void _Ready()
	{
		FocusIndex = 0;
		Buttons = new List<BaseButton>();
		Buttons.Add(ResumeBtn);
		Buttons.Add(AudioSettingsBtn);
		Buttons.Add(GameplaySettingsBtn);
		Buttons.Add(MainMenuBtn);
	}
}

