using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class GameStateInteractor : Node, IGameStateInteractor 
{
	public List<CreatureModel> ReadyEggList { get; set; }

	private List<CreatureModel> CreatureList { get; set; }
	private Menu Menu { get; set; }
	private List<ICharacter<CreatureModel>> BgGallery { get; set; }
	private List<ICharacter<CreatureModel>> BgNewEggsList { get; set; }
	private ICharacter<CreatureModel> ForegroundCharacter { get; set; }
	private int ForegroundCreatureIndex { get; set; } = 0;

	private ILoggerService _logger { get; set; } 
	private CharacterFactory _characterFactory { get; set; }
	private Observables _observables { get; set; }
	private IEggInteractor _eggInteractor { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_characterFactory = GetNode<CharacterFactory>(Constants.SingletonNodes.CharacterFactory);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
			
		_observables.GrabEgg += HandleGrabEgg;
	}

	public void ReadyInstance(List<CreatureModel> creatureList, Vector3 initialPosition, Menu menu)
	{
		CreatureList = creatureList;
		Menu = menu;
		BgGallery = new List<ICharacter<CreatureModel>>();
		BgNewEggsList = new List<ICharacter<CreatureModel>>();
		foreach (var creature in CreatureList)
		{
			AddBackgroundSubject(creature);
		}
		
		// Set initial foreground creature
		var model = CreatureList[ForegroundCreatureIndex];
		ForegroundCharacter = SetCreatureInForeground(model, initialPosition);
		
		// Remove foreground creature from background
		var newFgInstanceId = ForegroundCharacter.Subject.Model.InstanceId;
		RemoveBackgroundSubject(GetBackgroundSubject(newFgInstanceId));
		
		_observables.EmitUpdateSubjectNameLabel(ForegroundCharacter.Model.Name);
		_observables.EmitUpdateCurrentCreatureInfo();
		_observables.EmitUpdateHeartMeterMax(model.LoveMax);
		_observables.EmitUpdateHeartMeterValue(model.LoveLevel);
		_observables.EmitUpdateHungerMeterMax(model.StomachMax);
		_observables.EmitUpdateHungerMeterValue(model.StomachLevel);
	}

	public ICharacter<CreatureModel> GetForegroundCharacter()
	{
		return ForegroundCharacter;
	}

	public List<CreatureModel> GetCreatureList()
	{
		return CreatureList;
	}

	private ICharacter<CreatureModel>? SetCreatureInForeground(CreatureModel model, Vector3 position)
	{
		ICharacter<CreatureModel>? result = null;
		if (model.CreatureType == Enumerations.CreatureTypes.Egg) 
		{
			result = _characterFactory.SpawnFgEgg(GetNode("."), (CreatureModel)model, position);
			Menu.SwapPage(model, Enumerations.MenuPageType.Egg);
		}
		else
		{ 
			result = _characterFactory.SpawnFgAnimal(GetNode("."), (CreatureModel)model, position);
			Menu.SwapPage(model, Enumerations.MenuPageType.Animal);
		}
		return result;	
	}

	public void RotateForegroundSubjects()
	{
		try
		{
			if (!CreatureList.Any())
			{
				_logger.LogError("The CreatureList is empty.");
				return;
			}
			_logger.LogDebug("The CreatureList is NOT empty.");

			var fgPos = ForegroundCharacter.Subject.CharacterBody3D.GlobalPosition;
			var oldFgModel = ForegroundCharacter.Subject.Model;
			ForegroundCharacter.Subject.CharacterBody3D.QueueFree();

			CreatureModel nextModel = null;
			do 
			{
				ForegroundCreatureIndex += 1;
				if (ForegroundCreatureIndex == CreatureList.Count) ForegroundCreatureIndex = 0;
				nextModel = CreatureList[ForegroundCreatureIndex];
			} while (nextModel == null || !nextModel.IsInGallery);
			ForegroundCharacter = SetCreatureInForeground(nextModel, fgPos);

			// Remove foreground creature from background
			var newFgInstanceId = ForegroundCharacter.Subject.Model.InstanceId;
			RemoveBackgroundSubject(GetBackgroundSubject(newFgInstanceId));

			if (oldFgModel != null) AddBackgroundSubject(oldFgModel);
		} 
		catch (Exception ex)
		{
			_logger.LogError($"Main RotateForegroundSubjects Error: {ex.Message}", ex);
			throw;
		}
	}
	
	public void RemoveBackgroundSubject(ICharacter<CreatureModel> bgCharacter)
	{
		BgGallery.Remove(bgCharacter);
		bgCharacter.Subject.CharacterBody3D.QueueFree();
	}

	public void AddBackgroundSubject(CreatureModel model)
	{
		var defaultSpawnPointNode = GetNode<Node3D>(Constants.KeyNodePaths.BgSpawnPoint);
		var spawnPoint = AlterSpawnPoint(defaultSpawnPointNode.Position);
		if (model.CreatureType == Enumerations.CreatureTypes.Egg && model.IsInGallery)
		{
			var bgCharacter = _characterFactory.SpawnBgEgg(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
			BgGallery.Add(bgCharacter);
		}
		else if (model.CreatureType == Enumerations.CreatureTypes.Egg && !model.IsInGallery)
		{
			var bgCharacter = _characterFactory.SpawnBgEgg(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
			BgNewEggsList.Add(bgCharacter);
		}
		else
		{
			var bgCharacter = _characterFactory.SpawnBgAnimal(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
			BgGallery.Add(bgCharacter);
		}
	}

	private Vector3 AlterSpawnPoint(Vector3 spawnPoint)
	{
		var r = new Random();
		var xDelta = 5;
		var zDelta = 5;
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

	public void ToggleInfoContainer()
	{
		var toggleValue = !Menu.IsOn_InfoContainer;
		Menu.ToggleInfoContainer(toggleValue);
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

	private ICharacter<CreatureModel> GetBackgroundSubject(ulong instanceId)
	{
		foreach (var bgCharacter in BgGallery)
		{
			if (bgCharacter.Model.InstanceId == instanceId) 
			{
				return bgCharacter;
			}
		}
		_logger.LogError("Main GetBackgroundSubject: BackgroundSubject not found.");
		throw new Exception("Creature not found.");
	}

	private void HandleGrabEgg(ulong instanceId)
	{
		_logger.LogError("Call HandleGrabEgg");
		_logger.LogError($"instanceId {instanceId}");

		ICharacter<CreatureModel>? newEggCharacter = null;
		foreach (var character in BgNewEggsList)
		{
			if (character.Model.InstanceId == instanceId)
			{
				newEggCharacter = character;
			}
		}

		if (newEggCharacter != null) 
		{
			BgNewEggsList.Remove(newEggCharacter);
			BgGallery.Add(newEggCharacter);
			newEggCharacter.Model.IsInGallery = true;
			_eggInteractor.AddEggToGallery(newEggCharacter.Model.Id);
		}
	}
}
