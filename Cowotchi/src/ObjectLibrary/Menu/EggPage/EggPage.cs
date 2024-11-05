using Godot;
using System;

public partial class EggPage : Control
{
	[Signal]
	private delegate void StatsPressedEventHandler();
	[Signal]
	private delegate void SwapPressedEventHandler();

	[ExportGroup("Action Buttons")]
	[Export]
	public ActionButton Stats { get; set; }
	[Export]
	public ActionButton Swap { get; set; }
	
	private ILoggerService _logger { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");

		Stats.Pressed += HandleStatsPressed;
		Swap.Pressed += HandleSwapPressed;
	}

	private void HandleStatsPressed()
	{
		EmitSignal(SignalName.StatsPressed);
	}

	private void HandleSwapPressed()
	{
		EmitSignal(SignalName.SwapPressed);
	}
}
