using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class GameStateInteractor : Node, IGameStateInteractor 
{
	private List<CreatureModel> CreatureList { get; set; }
	private List<Subject<CreatureModel>> BgGallery { get; set; }
	private ICharacter<CreatureModel> ForegroundCharacter { get; set; }
	private int ForegroundCreatureIndex { get; set; } = -1;

	private ILoggerService _logger { get; set; } 
	private ForegroundSubjectFactory _fgFactory { get; set; }
	private BackgroundSubjectFactory _bgFactory { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_fgFactory = GetNode<ForegroundSubjectFactory>(Constants.SingletonNodes.ForegroundSubjectFactory);
		_bgFactory = GetNode<BackgroundSubjectFactory>(Constants.SingletonNodes.BackgroundSubjectFactory);
	}

	public void ReadyInstance(List<CreatureModel> creatureList, CharacterBody3D placeholder)
	{
		CreatureList = creatureList;
		BgGallery = new List<Subject<CreatureModel>>();
		foreach (var creature in CreatureList)
		{
			AddBackgroundSubject(creature);
		}
		ForegroundCharacter = _fgFactory.SpawnEgg(GetNode("."), new CreatureModel(Enumerations.CreatureTypes.Egg), placeholder.Position);
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
			switch (nextModel.CreatureType)
			{
				case Enumerations.CreatureTypes.Egg:
					ForegroundCharacter = _fgFactory.SpawnEgg(GetNode("."), (CreatureModel)nextModel, fgPos);
					break;
				case Enumerations.CreatureTypes.Cow:
					ForegroundCharacter = _fgFactory.SpawnCow(GetNode("."), (CreatureModel)nextModel, fgPos);
					break;
				default:
					_logger.LogError("Main RotateForegroundSubjects: Next model was not mapped to a creature type");
					break;
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
		switch (model.CreatureType)
		{
			case Enumerations.CreatureTypes.Egg:
				{
					var bgSubject = _bgFactory.SpawnEgg(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
					BgGallery.Add(bgSubject.Subject);
					break;
				}
			case Enumerations.CreatureTypes.Cow:
				{
					var bgSubject = _bgFactory.SpawnCow(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
					BgGallery.Add(bgSubject.Subject);
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
