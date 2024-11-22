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

	public List<CreatureModel> CreatureList { get; set; }
	public List<BackgroundSubject> Gallery { get; set; }

	public ForegroundSubject ForegroundSubject { get; set; }
	public int ForegroundIndex { get; set; }

	private ILoggerService _logger { get; set; }
	private ICommonInteractor _commonInteractor { get; set; } 
	private IEggInteractor _eggInteractor { get; set; } 
	private IAnimalInteractor _animalInteractor { get; set; } 
	private ForegroundSubjectFactory _fgFactory { get; set; }
	private BackgroundSubjectFactory _bgFactory { get; set; }
	private Observables _observables { get; set; }

	public override async void _Ready()
	{
		try
		{
			_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
			_commonInteractor = GetNode<ICommonInteractor>(Constants.SingletonNodes.CommonInteractor);
			_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
			_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
			_fgFactory = GetNode<ForegroundSubjectFactory>(Constants.SingletonNodes.ForegroundSubjectFactory);
			_bgFactory = GetNode<BackgroundSubjectFactory>(Constants.SingletonNodes.BackgroundSubjectFactory);
			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

			await _commonInteractor.InitDatabaseIfRequired();

			_observables.StatsPressed += HandleStatsPressed;
			_observables.SwapPressed += HandleSwapPressed;
			_observables.NurturePressed += HandleNurturePressed;
			_observables.FeedPressed += HandleFeedPressed;

			CreatureList = await GetCreatureListFromDatabase();
			Gallery = new List<BackgroundSubject>();
			foreach (var creature in CreatureList)
			{
				AddBackgroundSubject(creature);
			}
			ForegroundIndex = -1;
			ForegroundSubject = GetNode<ForegroundSubject>(FOREGROUND_PLACEHOLDER_PATH);
			RotateForegroundSubjects();

			_observables.GrabEgg += HandleGrabEgg;
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
			if (!CreatureList.Any())
			{
				_logger.LogInfo("The CreatureList is empty.");
				return;
			}
			_logger.LogDebug("The CreatureList is NOT empty.");
			ForegroundIndex += 1;
			if (ForegroundIndex == CreatureList.Count) ForegroundIndex = 0;

			var fgPos = ForegroundSubject.GlobalPosition;
			var oldFgInstanceId = ForegroundSubject.Model.InstanceId;
			ForegroundSubject.QueueFree();

			var nextModel = CreatureList[ForegroundIndex];
			var newFgInstanceId = ForegroundSubject.Model.InstanceId;
			switch (nextModel.CreatureType)
			{
				case Enumerations.CreatureTypes.Egg:
					ForegroundSubject = _fgFactory.SpawnEgg(GetNode("."), (EggModel)nextModel, fgPos);
					UpdateMetersForEgg((EggModel)nextModel);
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
					_logger.LogError("Main RotateForegroundSubjects: Next model was not mapped to a creature type");
					break;
			}
			
			SubjectNameLabel.Text = nextModel.Name;

			BackgroundSubject newFgBg = GetBackgroundSubject(oldFgInstanceId);
			RemoveBackgroundSubject(newFgBg);
			AddBackgroundSubject(GetCreatureInstance(oldFgInstanceId));
		} 
		catch (Exception ex)
		{
			_logger.LogError($"Main RotateForegroundSubjects Error: {ex.Message}", ex);
			throw;
		}
	}

	
	private void RemoveBackgroundSubject(BackgroundSubject bgSubject)
	{
		Gallery.Remove(bgSubject);
		bgSubject.QueueFree();
	}

	private void AddBackgroundSubject(CreatureModel model)
	{
		var defaultSpawnPointNode = GetNode<Node3D>(Constants.KeyNodePaths.BgSpawnPoint);
		var spawnPoint = AlterSpawnPoint(defaultSpawnPointNode.Position);
		
		switch (model.CreatureType)
		{
			case Enumerations.CreatureTypes.Egg:
				{
					var bgSubject = _bgFactory.SpawnEgg(GetNode(Constants.KeyNodePaths.FarmWanderers), (EggModel)model, spawnPoint);
					Gallery.Add((BackgroundSubject)bgSubject);
					break;
				}
			case Enumerations.CreatureTypes.Cow:
				{
					var bgSubject = _bgFactory.SpawnCow(GetNode(Constants.KeyNodePaths.FarmWanderers), (AnimalModel)model, spawnPoint);
					Gallery.Add((BackgroundSubject)bgSubject);
					break;
				}
			default:
				_logger.LogError("Main AddBackgroundSubject: Next model was not mapped to a creature type");
				break;
		}
	}

	private Vector3 AlterSpawnPoint(Vector3 spawnPoint)
	{
		var r = new Random();
		var xDelta = 1;
		var zDelta = 1;
		var xRoll = r.NextDouble();
		var zRoll = r.NextDouble();
		var xPos = (xRoll * (xDelta * 2)) - xDelta;
		var zPos = (zRoll * (zDelta * 2)) - zDelta;

		return new Vector3(
			spawnPoint.X + (float)xPos,
			spawnPoint.Y,
			spawnPoint.Z + (float)zPos
		);
	}

	private void UpdateMetersForEgg(EggModel model)
	{
		_logger.LogInfo("Start UpdateMetersForEgg");
		_observables.EmitUpdateHeartMeterMax(1);
		_observables.EmitUpdateHeartMeterValue(0);

		_observables.EmitUpdateHungerMeterMax(1);
		_observables.EmitUpdateHungerMeterValue(0);
		_logger.LogInfo("End UpdateMetersForEgg");
	}

	private void UpdateMetersForAnimal(AnimalModel model)
	{
		_logger.LogInfo("Start UpdateMetersForAnimal");
		_observables.EmitUpdateHeartMeterMax(model.LoveMax);
		_observables.EmitUpdateHeartMeterValue(model.LoveLevel);

		_observables.EmitUpdateHungerMeterMax(model.StomachMax);
		_observables.EmitUpdateHungerMeterValue(model.StomachLevel);
		_logger.LogInfo("End UpdateMetersForAnimal");
	}

	private async Task<List<CreatureModel>> GetCreatureListFromDatabase()
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
			_logger.LogError($"Main GetCreatureListFromDatabase Error: {ex.Message}", ex);
			throw;
		}

		return result;
	}

	private CreatureModel GetCreatureInstance(ulong instanceId)
	{
		foreach (var model in CreatureList)
		{
			if (model.InstanceId == instanceId) return model;
		}
		_logger.LogError("Main GetCreatureInstance: Creature not found.");
		throw new Exception("Creature not found.");
	}

	private BackgroundSubject GetBackgroundSubject(ulong instanceId)
	{
		foreach (var bgSubject in Gallery)
		{
			if (bgSubject.Model.InstanceId == instanceId) return bgSubject;
		}
		_logger.LogError("Main GetBackgroundSubject: BackgroundSubject not found.");
		throw new Exception("Creature not found.");
	}

	private void HandleGrabEgg(ulong instanceId)
	{
		_logger.LogError("Call HandleGrabEgg");
		_logger.LogError($"instanceId {instanceId}");

		foreach (var model in CreatureList)
		{
			_logger.LogError($"model.InstanceId {model.InstanceId}");
			if (model.InstanceId == instanceId)
			{
				_logger.LogError("Grabbed egg was found in gallery");
			}
		}
	}
}
