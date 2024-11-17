using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class Main : Node3D
{
	[Export]
	private RichTextLabel SubjectNameLabel { get; set; }
	
	private readonly string FOREGROUND_PLACEHOLDER_PATH = "./Placeholder";

	public List<CreatureModel> Gallery { get; set; }

	public ForegroundSubject ForegroundSubject { get; set; }
	public int ForegroundIndex { get; set; }

	private ILoggerService _logger { get; set; }
	private ICommonInteractor _commonInteractor { get; set; } 
	private IEggInteractor _eggInteractor { get; set; } 
	private IAnimalInteractor _animalInteractor { get; set; } 
	private ForegroundActionObservable _foregroundActionObservable { get; set; }
	private ForegroundSubjectFactory _fgFactory { get; set; }
	private MeterObservable _meterObservable { get; set; }

	public override async void _Ready()
	{
		try
		{
			_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
			_commonInteractor = GetNode<ICommonInteractor>(Constants.SingletonNodes.CommonInteractor);
			_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
			_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
			_foregroundActionObservable = GetNode<ForegroundActionObservable>("/root/ForegroundActionObservable");
			_fgFactory = GetNode<ForegroundSubjectFactory>(Constants.SingletonNodes.ForegroundSubjectFactory);
			_meterObservable = GetNode<MeterObservable>(Constants.SingletonNodes.MeterObservable);

			await _commonInteractor.InitDatabaseIfRequired();

			_foregroundActionObservable.StatsPressed += HandleStatsPressed;
			_foregroundActionObservable.SwapPressed += HandleSwapPressed;
			_foregroundActionObservable.NurturePressed += HandleNurturePressed;
			_foregroundActionObservable.FeedPressed += HandleFeedPressed;

			Gallery = await GetGalleryFromDatabase();
			ForegroundIndex = -1;
			ForegroundSubject = GetNode<ForegroundSubject>(FOREGROUND_PLACEHOLDER_PATH);
			RotateForegroundSubjects();
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
		ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Stats);
	}

	private void HandleSwapPressed()
	{
		_logger.LogDebug("Call Menu HandleSwapPressed");
		ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Swap);
		RotateForegroundSubjects(); 
	}

	private void HandleNurturePressed()
	{
		_logger.LogDebug("Call Menu HandleNurturePressed");
		ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Nurture);
	}

	private void HandleFeedPressed()
	{
		_logger.LogDebug("Call Menu HandleFeedPressed");
		ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Feed);
	}

	private void RotateForegroundSubjects()
	{
		try
		{
			if (!Gallery.Any())
			{
				_logger.LogInfo("The Gallery is empty.");
				return;
			}
			_logger.LogInfo("The Gallery is NOT empty.");
			ForegroundIndex += 1;
			if (ForegroundIndex == Gallery.Count) ForegroundIndex = 0;

			var fgPos = ForegroundSubject.GlobalPosition;
			ForegroundSubject.QueueFree();

			var nextModel = Gallery[ForegroundIndex];
			switch (nextModel.CreatureType)
			{
				case Enumerations.CreatureTypes.Egg:
					ForegroundSubject = _fgFactory.SpawnEgg(GetNode("."), (EggModel)nextModel, fgPos);
					_logger.LogDebug($"ForegroundSubject == null {ForegroundSubject == null}");
					_logger.LogDebug($"ForegroundSubject.Executer == null {ForegroundSubject.Executer == null}");
					break;
				case Enumerations.CreatureTypes.Cow:
					ForegroundSubject = _fgFactory.SpawnCow(GetNode("."), (AnimalModel)nextModel, fgPos);
					UpdateMetersForAnimal((AnimalModel)nextModel);
					_logger.LogDebug($"ForegroundSubject == null {ForegroundSubject == null}");
					_logger.LogDebug($"ForegroundSubject.Executer == null {ForegroundSubject.Executer == null}");
					break;
				default:
					_logger.LogInfo("Main RotateForegroundSubjects: Next model was not mapped to a creature type");
					break;
			}
			
			SubjectNameLabel.Text = nextModel.Name;
		} 
		catch (Exception ex)
		{
			_logger.LogError($"Main RotateForegroundSubjects Error: {ex.Message}", ex);
			throw;
		}
	}

	private void UpdateMetersForEgg(EggModel model)
	{
		_meterObservable.EmitUpdateHeartMeterMax(-1);
		_meterObservable.EmitUpdateHeartMeterValue(-1);

		_meterObservable.EmitUpdateHungerMeterMax(-1);
		_meterObservable.EmitUpdateHungerMeterValue(-1);
	}

	private void UpdateMetersForAnimal(AnimalModel model)
	{
		
		_meterObservable.EmitUpdateHeartMeterMax(model.LoveMax);
		_meterObservable.EmitUpdateHeartMeterValue(model.LoveLevel);

		_meterObservable.EmitUpdateHungerMeterMax(model.StomachMax);
		_meterObservable.EmitUpdateHungerMeterValue(model.StomachLevel);
	}

	private async Task<List<CreatureModel>> GetGalleryFromDatabase()
	{
		var result = new List<CreatureModel>();

		try
		{
			var animals = await _animalInteractor.GetAllAnimals();
			var eggs = await _eggInteractor.GetAllEggs();

			result = animals.Cast<CreatureModel>()
						.Concat(eggs.Cast<CreatureModel>())
						.OrderBy(x => x.BirthDate)
						.ToList();
		} 
		catch (Exception ex)
		{
			_logger.LogError($"Main GetGalleryFromDatabase Error: {ex.Message}", ex);
			throw;
		}

		return result;
	}
}
