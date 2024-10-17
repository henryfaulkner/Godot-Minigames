using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

	private PauseMenuService PauseMenuService;
	public List<BaseMenuPanel> PanelList { get; set; }

	public override void _Ready()
	{
		ProcessMode = Node.ProcessModeEnum.WhenPaused;
		PanelList = GetPauseMenuPanels();
		PauseMenuService = GetNode<PauseMenuService>("/root/PauseMenuService");
		PauseMenuService.SetPanelList(GetPauseMenuPanels());
		SubscribeToPanelEvents();
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(_UP_INPUT))
		{
			PanelList.ForEach(x =>
			{
				x.MoveFocusBackward(SwitchAudio);
			});
		}

		if (Input.IsActionJustPressed(_TAB_INPUT)
		|| Input.IsActionJustPressed(_DOWN_INPUT))
		{
			PanelList.ForEach(x =>
			{
				x.MoveFocusForward(SwitchAudio);
			});
		}
	}

	private List<BaseMenuPanel> GetPauseMenuPanels()
	{
		var result = new List<BaseMenuPanel>();
		result.Add(MainPanel);
		result.Add(AudioSettingsPanel);
		result.Add(GameplaySettingsPanel);
		result.Add(PlayerControlsPanel);
		return result;
	}

	private void SubscribeToPanelEvents()
	{
		SubscribeToMainPanelEvents();
		SubscribeToAudioSettingsPanelEvents();
		SubscribeToGameplaySettingsPanelEvents();
		SubscribeToPlayerControlsPanelEvents();
	}

	private void SubscribeToMainPanelEvents()
	{
		MainPanel.Open += (int openPanelId) => OpenPanel(openPanelId);
	}

	private void SubscribeToAudioSettingsPanelEvents()
	{
		AudioSettingsPanel.Open += (int openPanelId) => OpenPanel(openPanelId);
	}

	private void SubscribeToGameplaySettingsPanelEvents()
	{
		GameplaySettingsPanel.Open += (int openPanelId) => OpenPanel(openPanelId);
	}

	private void SubscribeToPlayerControlsPanelEvents()
	{
		PlayerControlsPanel.Open += (int openPanelId) => OpenPanel(openPanelId);
	}

	public void OpenPanel(int openPanelId)
	{
		HideAllPanels();
		var openPanel = PanelList.Where(x => (int)x.Id == openPanelId).First();
		openPanel.Visible = true;
	}

	public void HideAllPanels()
	{
		PanelList.ForEach(x =>
		{
			x.Visible = false;
		});
	}
}
