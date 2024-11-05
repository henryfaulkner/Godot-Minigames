using Godot;
using System;

public partial class Menu : CanvasLayer
{
    [Signal]
	private delegate void StatsPressedEventHandler();
	[Signal]
	private delegate void SwapPressedEventHandler();
	[Signal]
	private delegate void NurturePressedEventHandler();
	[Signal]
	private delegate void FeedPressedEventHandler();

    [ExportGroup("Pages")]
    [Export]
    private AnimalPage AnimalPage { get; set; }
    [Export]
    private EggPage EggPage { get; set; }

    private ILoggerService _logger { get; set; }

    public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");

		AnimalPage.StatsPressed += HandleStatsPressed;
		AnimalPage.SwapPressed += HandleSwapPressed;
		AnimalPage.NurturePressed += HandleNurturePressed;
		AnimalPage.FeedPressed += HandleFeedPressed;

        EggPage.StatsPressed += HandleStatsPressed;
		EggPage.SwapPressed += HandleSwapPressed;
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