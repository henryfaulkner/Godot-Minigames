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

	public void ReadyInstance(List<CreatureModel> CreatureList, CharacterBody3D placeholder)
	{
		BgGallery = new List<Subject<CreatureModel>>();
		foreach (var creature in CreatureList)
		{
			AddBackgroundSubject(creature);
		}
		_logger.LogInfo("Log 3");
		ForegroundCharacter = _fgFactory.SpawnEgg(GetNode("."), new CreatureModel(Enumerations.CreatureTypes.Egg), placeholder.Position);
		_logger.LogInfo("Log 4");
		RotateForegroundSubjects(ForegroundCharacter);
		_logger.LogInfo("Log Last");
	}

	public ICharacter<CreatureModel> GetForegroundCharacter()
	{
		return ForegroundCharacter;
	}

	public List<CreatureModel> GetCreatureList()
	{
		return CreatureList;
	}

	public void RotateForegroundSubjects(ICharacter<CreatureModel> fgCharacter)
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
			var fgPos = fgCharacter.Subject.CharacterBody3D.GlobalPosition;
			_logger.LogInfo("Log 5.6");
			var oldFgModel = fgCharacter.Subject.Model;
			fgCharacter.Subject.CharacterBody3D.QueueFree();
			_logger.LogInfo("Log 6");

			var nextModel = CreatureList[ForegroundCreatureIndex];
			switch (nextModel.CreatureType)
			{
				case Enumerations.CreatureTypes.Egg:
					fgCharacter.Subject = _fgFactory.SpawnEgg(GetNode("."), (CreatureModel)nextModel, fgPos).Subject;
					break;
				case Enumerations.CreatureTypes.Cow:
					fgCharacter.Subject = _fgFactory.SpawnCow(GetNode("."), (CreatureModel)nextModel, fgPos).Subject;
					break;
				default:
					_logger.LogError("Main RotateForegroundSubjects: Next model was not mapped to a creature type");
					break;
			}

			_logger.LogInfo($"nextModel == null {nextModel == null}");
			var newFgInstanceId = fgCharacter.Subject.Model.InstanceId;
			_logger.LogInfo("Log 7");
			Subject<CreatureModel> newFgBg = GetBackgroundSubject(newFgInstanceId);
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
	
	public void RemoveBackgroundSubject(Subject<CreatureModel> bgSubject)
	{
		BgGallery.Remove(bgSubject);
		bgSubject.CharacterBody3D.QueueFree();
	}

	public void AddBackgroundSubject(CreatureModel model)
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
					BgGallery.Add(bgSubject.Subject);
					break;
				}
			case Enumerations.CreatureTypes.Cow:
				{
					var bgSubject = _bgFactory.SpawnCow(GetNode(Constants.KeyNodePaths.FarmWanderers), (CreatureModel)model, spawnPoint);
					_logger.LogInfo("Log 7.4");
					BgGallery.Add(bgSubject.Subject);
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
		_logger.LogInfo("Log 7.01");
		foreach (var bgSubject in BgGallery)
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
}
