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
	private Observables _observables { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		
		Stats.Pressed += HandleStatsPressed;
		Swap.Pressed += HandleSwapPressed;
	}

	private void HandleStatsPressed()
	{
		_observables.EmitStatsPressed();
	}

	private void HandleSwapPressed()
	{
		_observables.EmitSwapPressed();
	}
}
