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
	private Observables _observables { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

		Stats.Pressed += HandleStatsPressed;
		Swap.Pressed += HandleSwapPressed;
		Nurture.Pressed += HandleNurturePressed;
		Feed.Pressed += HandleFeedPressed;
	}

	private void HandleStatsPressed()
	{
		_observables.EmitStatsPressed();
	}

	private void HandleSwapPressed()
	{
		_observables.EmitSwapPressed();
	}

	private void HandleNurturePressed()
	{
		_observables.EmitNurturePressed();
	}

	private void HandleFeedPressed()
	{
		_observables.EmitFeedPressed();	
	}
}
