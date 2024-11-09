using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class Main : Node3D
{
	private readonly StringName FOREGROUND_PLACEHOLDER_PATH = new StringName("./Placeholder");

	public List<ForegroundModel> Gallery { get; set; }

	public ForegroundModel ForegroundModel { get; set; }
	public int ForegroundIndex { get; set; }

	private ILoggerService _logger { get; set; }
	private ICommonInteractor _commonInteractor { get; set; } 
	private IEggInteractor _eggInteractor { get; set; } 
	private IAnimalInteractor _animalInteractor { get; set; } 
	private ForegroundActionObservable _foregroundActionObservable { get; set; }

	public override async void _Ready()
	{
		try
		{
			_logger = GetNode<ILoggerService>("/root/LoggerService");
			_commonInteractor = GetNode<ICommonInteractor>("/root/CommonInteractor");
			_eggInteractor = GetNode<IEggInteractor>("/root/EggInteractor");
			_animalInteractor = GetNode<IAnimalInteractor>("/root/AnimalInteractor");

			await _commonInteractor.InitDatabaseIfRequired();

			_foregroundActionObservable = GetNode<ForegroundActionObservable>("/root/ForegroundActionObservable");
			_foregroundActionObservable.StatsPressed += HandleStatsPressed;
			_foregroundActionObservable.SwapPressed += HandleSwapPressed;
			_foregroundActionObservable.NurturePressed += HandleNurturePressed;
			_foregroundActionObservable.FeedPressed += HandleFeedPressed;

			Gallery = await GetGalleryFromDatabase();
			ForegroundIndex = -1;
			RotateForegroundModels();
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
		ForegroundModel.Executer.ExecuteAction(Enumerations.ForegroundActions.Stats);
	}

	private void HandleSwapPressed()
	{
		_logger.LogDebug("Call Menu HandleSwapPressed");
		ForegroundModel.Executer.ExecuteAction(Enumerations.ForegroundActions.Swap);
		RotateForegroundModels(); 
	}

	private void HandleNurturePressed()
	{
		_logger.LogDebug("Call Menu HandleNurturePressed");
		ForegroundModel.Executer.ExecuteAction(Enumerations.ForegroundActions.Nurture);
	}

	private void HandleFeedPressed()
	{
		_logger.LogDebug("Call Menu HandleFeedPressed");
		ForegroundModel.Executer.ExecuteAction(Enumerations.ForegroundActions.Feed);
	}

	private void RotateForegroundModels()
	{
		if (!Gallery.Any())
		{
			_logger.LogInfo("The Gallery is empty.");
			return;
		}
		ForegroundIndex += 1;
		if (ForegroundIndex == Gallery.Count) ForegroundIndex = 0;

		var currPos = ForegroundModel.GlobalPosition;
		ForegroundModel.QueueFree();
		ForegroundModel = Gallery[ForegroundIndex].Instantiate<ForegroundModel>();
		ForegroundModel.GlobalPosition = currPos;
		GetNode(".").AddChild(ForegroundModel);
	}

	private async Task<List<ForegroundModel>> GetGalleryFromDatabase()
	{
		var result = new List<ForegroundModel>();

		try
		{
			result.AddRange(await _animalInteractor.GetAllAnimals());
			result.AddRange(await _eggInteractor.GetAllEggs());
		} 
		catch (Exception ex)
		{
			_logger.LogError($"Main GetGalleryFromDatabase Error: {ex.Message}", ex);
			throw;
		}

		return result;
	}
}
