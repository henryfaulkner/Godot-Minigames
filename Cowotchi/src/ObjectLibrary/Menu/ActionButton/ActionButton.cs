using Godot;
using System;

public partial class ActionButton : Control
{	
	[Signal]
	public delegate void PressedEventHandler();
	
	private static readonly StringName PANEL_STYLEBOX_NAME = new StringName("panel");
	private static readonly StringName NORMAL_PANEL_STYLE = new StringName("res://src/ObjectLibrary/Menu/PageStyles/Disabled_PagePanelOption.tres");
	private static readonly StringName HOVER_PANEL_STYLE = new StringName("res://src/ObjectLibrary/Menu/PageStyles/Active_PagePanelOption.tres");

	[ExportGroup("Nodes")]
	[Export]
	private Panel Panel { get; set; }
	[Export]
	private TextureButton TextureButton { get; set; }

	[ExportGroup("Textures")]
	[Export]
	private Texture2D NormalTexture { get; set; }
	[Export]
	private Texture2D HoverTexture { get; set; }
	
	private StyleBoxFlat ActivePagePanelOptionStyle { get; set; }
	private StyleBoxFlat InactivePagePanelOptionStyle { get; set; }
	
	private LoggerService _logger { get; set; }

	public ActionButton() 
	{
		ActivePagePanelOptionStyle = GD.Load<StyleBoxFlat>(HOVER_PANEL_STYLE);
		InactivePagePanelOptionStyle = GD.Load<StyleBoxFlat>(NORMAL_PANEL_STYLE);
	}

	public override void _Ready()
	{
		if (NormalTexture != null) TextureButton.TextureNormal = NormalTexture;
		if (HoverTexture != null) TextureButton.TextureHover = HoverTexture;
		
		TextureButton.MouseEntered += HandleMouseEntered;
		TextureButton.MouseExited += HandleMouseExited;
		TextureButton.Pressed += HandlePressed;
		
		_logger = GetNode<LoggerService>(Constants.SingletonNodes.LoggerService);
	}


	public void HandleMouseEntered()
	{
		_logger.LogDebug("Call HandleMouseEntered");
		ApplyActivePagePanelOption(Panel);
	}

	public void HandleMouseExited()
	{
		_logger.LogDebug("Call HandleMouseExited");
		ApplyInactivePagePanelOption(Panel);
	}
	
	public void HandlePressed()
	{
		_logger.LogDebug("Call HandlePressed");
		EmitSignal(SignalName.Pressed);
	}	

	public void ApplyActivePagePanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(PANEL_STYLEBOX_NAME, ActivePagePanelOptionStyle);
	}

	public void ApplyInactivePagePanelOption(Panel panel)
	{
		panel.AddThemeStyleboxOverride(PANEL_STYLEBOX_NAME, InactivePagePanelOptionStyle);
	}
}
