using Godot;
using System;

public partial class AnimalPage : Control
{
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
	private ForegroundActionObservable _foregroundActionObservable { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
		_foregroundActionObservable = GetNode<ForegroundActionObservable>("/root/ForegroundActionObservable");

		Stats.Pressed += HandleStatsPressed;
		Swap.Pressed += HandleSwapPressed;
		Nurture.Pressed += HandleNurturePressed;
		Feed.Pressed += HandleFeedPressed;
	}

	private void HandleStatsPressed()
	{
		_foregroundActionObservable.EmitStatsPressed();
	}

	private void HandleSwapPressed()
	{
		_foregroundActionObservable.EmitSwapPressed();
	}

	private void HandleNurturePressed()
	{
		_foregroundActionObservable.EmitNurturePressed();
	}

	private void HandleFeedPressed()
	{
		_foregroundActionObservable.EmitFeedPressed();	
	}
}
