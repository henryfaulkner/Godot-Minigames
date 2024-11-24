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
	public List<BackgroundSubject<CreatureModel>> Gallery { get; set; }

	public ICharacterWithForegroundSubject<CreatureModel> ForegroundCharacter { get; set; }
	public int ForegroundCreatureIndex { get; set; } = -1;

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
			_logger.LogInfo("Log 1");

			await _commonInteractor.InitDatabaseIfRequired();

			_observables.StatsPressed += HandleStatsPressed;
			_observables.SwapPressed += HandleSwapPressed;
			_observables.NurturePressed += HandleNurturePressed;
			_observables.FeedPressed += HandleFeedPressed;

			_logger.LogInfo("Log 2");
			CreatureList = await GetCreatureListFromDatabase();
			Gallery = new List<BackgroundSubject<CreatureModel>>();
			foreach (var creature in CreatureList)
			{
				AddBackgroundSubject(creature);
			}
			_logger.LogInfo("Log 3");
			var placeholder = GetNode<CharacterBody3D>(FOREGROUND_PLACEHOLDER_PATH);
			ForegroundCharacter = _fgFactory.SpawnEgg(GetNode("."), new CreatureModel(Enumerations.CreatureTypes.Egg), placeholder.Position);
			_logger.LogInfo("Log 4");
			RotateForegroundSubjects(ForegroundCharacter);
			_logger.LogInfo("Log Last");

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
		ForegroundCharacter.ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Stats);
	}

	private void HandleSwapPressed()
	{
		_logger.LogDebug("Call Menu HandleSwapPressed");
		_logger.LogInfo($"ForegroundCharacter == null {ForegroundCharacter == null}");
		_logger.LogInfo($"ForegroundCharacter.ForegroundSubject == null {ForegroundCharacter.ForegroundSubject == null}");
		_logger.LogInfo($"ForegroundCharacter.ForegroundSubject.Executer == null {ForegroundCharacter.ForegroundSubject.Executer == null}");
		ForegroundCharacter.ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Swap);
		RotateForegroundSubjects(ForegroundCharacter); 
	}

	private void HandleNurturePressed()
	{
		_logger.LogDebug("Call Menu HandleNurturePressed");
		ForegroundCharacter.ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Nurture);
	}

	private void HandleFeedPressed()
	{
		_logger.LogDebug("Call Menu HandleFeedPressed");
		ForegroundCharacter.ForegroundSubject.Executer.ExecuteAction(Enumerations.ForegroundActions.Feed);
	}

	private void RotateForegroundSubjects(ICharacterWithForegroundSubject<CreatureModel> fgCharacter)
	{
		try
		{
			_logger.LogInfo("Log 5");
			if (!CreatureList.Any())
			{
				_logger.LogInfo("The CreatureList is empty.");
				return;
			}
			_logger.LogDebug("The CreatureList is NOT empty.");
			ForegroundCreatureIndex += 1;
			if (ForegroundCreatureIndex == CreatureList.Count) ForegroundCreatureIndex = 0;

			_logger.LogInfo("Log 5.5");
			var fgPos = fgCharacter.ForegroundSubject.CharacterBody3D.GlobalPosition;
			_logger.LogInfo("Log 5.6");
			var oldFgModel = fgCharacter.ForegroundSubject.Model;
			fgCharacter.ForegroundSubject.CharacterBody3D.QueueFree();
			_logger.LogInfo("Log 6");

			var nextModel = CreatureList[ForegroundCreatureIndex];
			switch (nextModel.CreatureType)
			{
				case Enumerations.CreatureTypes.Egg:
					fgCharacter.ForegroundSubject = _fgFactory.SpawnEgg(GetNode("."), (CreatureModel)nextModel, fgPos).ForegroundSubject;
					UpdateMetersForEgg((CreatureModel)nextModel);
					break;
				case Enumerations.CreatureTypes.Cow:
					fgCharacter.ForegroundSubject = _fgFactory.SpawnCow(GetNode("."), (CreatureModel)nextModel, fgPos).ForegroundSubject;
					UpdateMetersForAnimal((CreatureModel)nextModel);
					break;
				default:
					_logger.LogError("Main RotateForegroundSubjects: Next model was not mapped to a creature type");
					break;
			}
			
			SubjectNameLabel.Text = nextModel.Name;

			_logger.LogInfo($"nextModel == null {nextModel == null}");
			var newFgInstanceId = fgCharacter.ForegroundSubject.Model.InstanceId;
			_logger.LogInfo("Log 7");
			BackgroundSubject<CreatureModel> newFgBg = GetBackgroundSubject(newFgInstanceId);
			_logger.LogInfo("Log 8");
			RemoveBackgroundSubject(newFgBg);
			_logger.LogInfo("Log 9");
			if (oldFgModel != null) AddBackgroundSubject(oldFgModel);
			_logger.LogInfo("Log 10");
		} 
		catch (Exception ex)
		{
			_logger.LogError($"Main RotateForegroundSubjects Error: {ex.Message}", ex);
			throw;
		}
	}

	
	private void RemoveBackgroundSubject(BackgroundSubject<CreatureModel> bgSubject)
	{
		Gallery.Remove(bgSubject);
		bgSubject.CharacterBody3D.QueueFree();
	}

	private void AddBackgroundSubject(CreatureModel model)
	{
		_logger.LogInfo("Log 7.1");
		var defaultSpawnPointNode = GetNode<Node3D>(Constants.KeyNodePaths.BgSpawnPoint);
		_logger.LogInfo("Log 7.2");
		var spawnPoint = AlterSpawnPoint(defaultSpawnPointNode.Position);
		_logger.LogInfo("Log 7.3");
		switch (model.CreatureType)
		{
			case Enumerations.CreatureTypes.Egg:
				{
					var bgSubject = _bgFactory.SpawnEgg(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
					_logger.LogInfo("Log 7.4");
					Gallery.Add(bgSubject.BackgroundSubject);
					break;
				}
			case Enumerations.CreatureTypes.Cow:
				{
					var bgSubject = _bgFactory.SpawnCow(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
					_logger.LogInfo("Log 7.4");
					Gallery.Add(bgSubject.BackgroundSubject);
					break;
				}
			default:
				_logger.LogError("Main AddBackgroundSubject: Next model was not mapped to a creature type");
				break;
		}
		_logger.LogInfo("Log 7.5");
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

	private void UpdateMetersForEgg(CreatureModel model)
	{
		_logger.LogInfo("Start UpdateMetersForEgg");
		_observables.EmitUpdateHeartMeterMax(1);
		_observables.EmitUpdateHeartMeterValue(0);

		_observables.EmitUpdateHungerMeterMax(1);
		_observables.EmitUpdateHungerMeterValue(0);
		_logger.LogInfo("End UpdateMetersForEgg");
	}

	private void UpdateMetersForAnimal(CreatureModel model)
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
			_logger.LogInfo($"GetCreatureInstance model.instanceId {model.InstanceId}");
			_logger.LogInfo($"GetCreatureInstance instanceId {instanceId}");
			
			if (model.InstanceId == instanceId) return model;
		}
		_logger.LogError("Main GetCreatureInstance: Creature not found.");
		throw new Exception("Creature not found.");
	}

	private BackgroundSubject<CreatureModel> GetBackgroundSubject(ulong instanceId)
	{
		_logger.LogInfo("Log 7.01");
		foreach (var bgSubject in Gallery)
		{
			_logger.LogInfo($"bgSubject.Model == null {bgSubject.Model == null}");
			if (bgSubject.Model.InstanceId == instanceId) 
			{
				_logger.LogInfo("Log 7.02");
				return bgSubject;
			}
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
