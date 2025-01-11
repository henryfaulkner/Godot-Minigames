using Godot;
using System;

public partial class DebugPopup : PopupPanel
{
	[Export]
	Label Label { get; set; }
	
	ILoggerService _logger;
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("debug"))
		{
			// toggle menu off
			Visible = !Visible;
		}
	}
	
	public void SetText(string text)
	{
		Label.Text = text;
	}
}
