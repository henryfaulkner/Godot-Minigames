using Godot;
using System;

public partial class Main : Node3D
{
	[Export]
	private Menu Menu { get; set; }

	public IExecuter ForegroundAnimal { get; set; }

	private ILoggerService _logger { get; set; }
	private ICommonInteractor _commonInteractor { get; set; } 

	public override void _Ready()
	{
		GD.Print($"Call Main _Ready");
		try
		{
			_commonInteractor = GetNode<ICommonInteractor>("/root/CommonInteractor");
			_commonInteractor.InitDatabaseIfRequired();

			Menu.StatsPressed += HandleStatsPressed;
			Menu.SwapPressed += HandleSwapPressed;
			Menu.NurturePressed += HandleNurturePressed;
			Menu.FeedPressed += HandleFeedPressed;
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Main _Ready Error: {ex.Message}");
			_logger.LogError($"Main _Ready Error: {ex.Message}", ex);
		}
	}

	private void HandleStatsPressed()
	{
		ForegroundAnimal.ExecuteAction(Enumerations.ForegroundActions.Stats);
	}

	private void HandleSwapPressed()
	{
		ForegroundAnimal.ExecuteAction(Enumerations.ForegroundActions.Swap);
	}

	private void HandleNurturePressed()
	{
		ForegroundAnimal.ExecuteAction(Enumerations.ForegroundActions.Nurture);
	}

	private void HandleFeedPressed()
	{
		ForegroundAnimal.ExecuteAction(Enumerations.ForegroundActions.Feed);
	}
}
