using Godot;
using System;

public partial class Main : Node3D
{
	public IExecuter ForegroundExecuter { get; set; }

	private ILoggerService _logger { get; set; }
	private ICommonInteractor _commonInteractor { get; set; } 
	private ForegroundActionObservable _foregroundActionObservable { get; set; }

	public override void _Ready()
	{
		try
		{
			_logger = GetNode<ILoggerService>("/root/LoggerService");
			
			_commonInteractor = GetNode<ICommonInteractor>("/root/CommonInteractor");
			_commonInteractor.InitDatabaseIfRequired();

			_foregroundActionObservable = GetNode<ForegroundActionObservable>("/root/ForegroundActionObservable");
			_foregroundActionObservable.StatsPressed += HandleStatsPressed;
			_foregroundActionObservable.SwapPressed += HandleSwapPressed;
			_foregroundActionObservable.NurturePressed += HandleNurturePressed;
			_foregroundActionObservable.FeedPressed += HandleFeedPressed;
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Main _Ready Error: {ex.Message}");
			_logger.LogError($"Main _Ready Error: {ex.Message}", ex);
		}
	}

	private void HandleStatsPressed()
	{
		_logger.LogDebug("Call Menu HandleStatsPressed");
		ForegroundExecuter.ExecuteAction(Enumerations.ForegroundActions.Stats);
	}

	private void HandleSwapPressed()
	{
		_logger.LogDebug("Call Menu HandleSwapPressed");
		ForegroundExecuter.ExecuteAction(Enumerations.ForegroundActions.Swap);
	}

	private void HandleNurturePressed()
	{
		_logger.LogDebug("Call Menu HandleNurturePressed");
		ForegroundExecuter.ExecuteAction(Enumerations.ForegroundActions.Nurture);
	}

	private void HandleFeedPressed()
	{
		_logger.LogDebug("Call Menu HandleFeedPressed");
		ForegroundExecuter.ExecuteAction(Enumerations.ForegroundActions.Feed);
	}
}
