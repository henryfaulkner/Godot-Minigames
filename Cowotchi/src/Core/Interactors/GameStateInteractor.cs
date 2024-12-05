using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class GameStateInteractor : Node, IGameStateInteractor 
{
	private List<CreatureModel> CreatureList { get; set; }
	private Menu Menu { get; set; }
	private List<Subject<CreatureModel>> BgGallery { get; set; }
	private ICharacter<CreatureModel> ForegroundCharacter { get; set; }
	private int ForegroundCreatureIndex { get; set; } = -1;

	private ILoggerService _logger { get; set; } 
	private CharacterFactory _characterFactory { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_characterFactory = GetNode<CharacterFactory>(Constants.SingletonNodes.CharacterFactory);
	}

	public void ReadyInstance(List<CreatureModel> creatureList, Vector3 initialPosition, Menu menu)
	{
		CreatureList = creatureList;
		Menu = menu;
		BgGallery = new List<Subject<CreatureModel>>();
		foreach (var creature in CreatureList)
		{
			AddBackgroundSubject(creature);
		}
		ForegroundCharacter = _characterFactory.SpawnFgEgg(GetNode("."), new CreatureModel(Enumerations.CreatureTypes.Egg), initialPosition);
		RotateForegroundSubjects();
	}

	public ICharacter<CreatureModel> GetForegroundCharacter()
	{
		return ForegroundCharacter;
	}

	public List<CreatureModel> GetCreatureList()
	{
		return CreatureList;
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

			ForegroundCreatureIndex += 1;
			if (ForegroundCreatureIndex == CreatureList.Count) ForegroundCreatureIndex = 0;

			var fgPos = ForegroundCharacter.Subject.CharacterBody3D.GlobalPosition;
			var oldFgModel = ForegroundCharacter.Subject.Model;
			ForegroundCharacter.Subject.CharacterBody3D.QueueFree();

			var nextModel = CreatureList[ForegroundCreatureIndex];
			Menu.SetCreatureInfo(nextModel);
			if (nextModel.CreatureType == Enumerations.CreatureTypes.Egg) 
			{
				ForegroundCharacter = _characterFactory.SpawnFgEgg(GetNode("."), (CreatureModel)nextModel, fgPos);
				Menu.SwapPage(Enumerations.MenuPageType.Egg);
			}
			else
			{ 
				ForegroundCharacter = _characterFactory.SpawnFgAnimal(GetNode("."), (CreatureModel)nextModel, fgPos);
				Menu.SwapPage(Enumerations.MenuPageType.Animal);
			}

			_logger.LogInfo($"nextModel.Name {nextModel.Name}");
			var newFgInstanceId = ForegroundCharacter.Subject.Model.InstanceId;
			Subject<CreatureModel> newFgBg = GetBackgroundSubject(newFgInstanceId);
			RemoveBackgroundSubject(newFgBg);
			if (oldFgModel != null) AddBackgroundSubject(oldFgModel);
		} 
		catch (Exception ex)
		{
			_logger.LogError($"Main RotateForegroundSubjects Error: {ex.Message}", ex);
			throw;
		}
	}
	
	public void RemoveBackgroundSubject(Subject<CreatureModel> bgSubject)
	{
		BgGallery.Remove(bgSubject);
		bgSubject.CharacterBody3D.QueueFree();
	}

	public void AddBackgroundSubject(CreatureModel model)
	{
		var defaultSpawnPointNode = GetNode<Node3D>(Constants.KeyNodePaths.BgSpawnPoint);
		var spawnPoint = AlterSpawnPoint(defaultSpawnPointNode.Position);
		if (model.CreatureType == Enumerations.CreatureTypes.Egg)
		{
			var bgChar = _characterFactory.SpawnBgEgg(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
			BgGallery.Add(bgChar.Subject);
		}
		else
		{
			var bgChar = _characterFactory.SpawnBgAnimal(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
			BgGallery.Add(bgChar.Subject);
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

	private Subject<CreatureModel> GetBackgroundSubject(ulong instanceId)
	{
		foreach (var bgSubject in BgGallery)
		{
			_logger.LogInfo($"bgSubject.Model == null {bgSubject.Model == null}");
			if (bgSubject.Model.InstanceId == instanceId) 
			{
				return bgSubject;
			}
		}
		_logger.LogError("Main GetBackgroundSubject: BackgroundSubject not found.");
		throw new Exception("Creature not found.");
	}
}
