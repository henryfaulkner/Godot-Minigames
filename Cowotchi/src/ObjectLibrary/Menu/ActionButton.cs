using Godot;
using System;

public partial class ActionButton : Panel
{
	private static readonly StringName PANEL_STYLEBOX_NAME = new StringName("panel");
	private static readonly StringName INACTIVE_PANEL_STYLE = new StringName("res://src/ObjectLibrary/Menu/PageStyles/Disabled_PagePanelOption.tres");
	private static readonly StringName ACTIVE_PANEL_STYLE = new StringName("res://src/ObjectLibrary/Menu/PageStyles/Active_PagePanelOption.tres");

	[Export]
	public TextureButton TextureButton { get; set; }

	public override void _Ready()
	{
		TextureButton.MouseEntered += HandleMouseEntered;
		TextureButton.MouseExited += HandleMouseExited;
	}


	public void HandleMouseEntered()
	{
		ApplyInactivePagePanelOption(this);
	}

	public void HandleMouseExited()
	{
		ApplyActivePagePanelOption(this);
	}

	public void ApplyActivePagePanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(PANEL_STYLEBOX_NAME, ACTIVE_PANEL_STYLE);
	}

	public void ApplyInactivePagePanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(PANEL_STYLEBOX_NAME, INACTIVE_PANEL_STYLE);
	}
}
