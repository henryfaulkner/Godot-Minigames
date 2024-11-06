using Godot;
using System;

public partial class EggPage : Control
{
	[ExportGroup("Action Buttons")]
	[Export]
	public ActionButton Stats { get; set; }
	[Export]
	public ActionButton Swap { get; set; }
	
	private ILoggerService _logger { get; set; }
	private ForegroundActionObservable _foregroundActionObservable { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
		_foregroundActionObservable = GetNode<ForegroundActionObservable>("/root/ForegroundActionObservable");
		
		Stats.Pressed += HandleStatsPressed;
		Swap.Pressed += HandleSwapPressed;
	}

	private void HandleStatsPressed()
	{
		_foregroundActionObservable.EmitStatsPressed();
	}

	private void HandleSwapPressed()
	{
		_foregroundActionObservable.EmitSwapPressed();
	}
}
