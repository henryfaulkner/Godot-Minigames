using Godot;
using System;
using System.Collections.Generic;

public partial class PauseMenu : Node
{
	private static readonly StringName _SCENE_MAIN_MENU = new StringName("res://Main.tscn");

	#region Input Constants

	private static readonly StringName _PAUSE_INPUT = new StringName("escape");
	private static readonly StringName _TAB_INPUT = new StringName("tab");
	private static readonly StringName _UP_INPUT = new StringName("up");
	private static readonly StringName _DOWN_INPUT = new StringName("down");

	#endregion

	#region Panels

	[Export]
	private MainPanel MainPanel;

	[Export]
	private AudioSettingsPanel AudioSettingsPanel;

	[Export]
	private GameplaySettingsPanel GameplaySettingsPanel;

	[Export]
	private PlayerControlsPanel PlayerControlsPanel;

	#endregion

	#region Audio Exports

	[Export]
	private AudioStreamPlayer SelectAudio;

	[Export]
	private AudioStreamPlayer SwitchAudio;

	#endregion

	public List<BasePanel> BasePanelList { get; set; }

	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.WhenPaused;
		BasePanelList = GetPauseBasePanels();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(_UP_INPUT))
		{
			BasePanelList.ForEach(BasePanel =>
			{
				BasePanel.MoveFocusBackward(SwitchAudio);
			});
		}

		if (Input.IsActionJustPressed(_TAB_INPUT)
		|| Input.IsActionJustPressed(_DOWN_INPUT))
		{
			BasePanelList.ForEach(BasePanel =>
			{
				BasePanel.MoveFocusForward(SwitchAudio);
			});
		}
	}

	private List<BasePanel> GetPauseBasePanels()
	{
		var result = new List<BasePanel>();
		result.Add(MainPanel);
		result.Add(AudioSettingsPanel);
		result.Add(GameplaySettingsPanel);
		result.Add(PlayerControlsPanel);
		return result;
	}
}
