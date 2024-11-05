using Godot;
using System;

public partial class AnimalPage : Control
{
	[Signal]
	private delegate void StatsPressedEventHandler();
	[Signal]
	private delegate void SwapPressedEventHandler();
	[Signal]
	private delegate void NurturePressedEventHandler();
	[Signal]
	private delegate void FeedPressedEventHandler();

	[ExportGroup("Action Buttons")]
	[Export]
	public ActionButton Stats { get; set; }
	[Export]
	public ActionButton Swap { get; set; }
	[Export]
	public ActionButton Nurture { get; set; }
	[Export]
	public ActionButton Feed { get; set; } 
	
	private ILoggerService _logger { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");

		Stats.Pressed += HandleStatsPressed;
		Swap.Pressed += HandleSwapPressed;
		Nurture.Pressed += HandleNurturePressed;
		Feed.Pressed += HandleFeedPressed;
	}

	private void HandleStatsPressed()
	{
		EmitSignal(SignalName.StatsPressed);
	}

	private void HandleSwapPressed()
	{
		EmitSignal(SignalName.SwapPressed);
	}

	private void HandleNurturePressed()
	{
		EmitSignal(SignalName.NurturePressed);
	}

	private void HandleFeedPressed()
	{
		EmitSignal(SignalName.FeedPressed);	
	}
}
