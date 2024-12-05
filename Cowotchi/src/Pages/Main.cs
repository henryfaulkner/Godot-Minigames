using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class Main : Node3D
{
	[Export]
	private RichTextLabel SubjectNameLabel { get; set; }
	[Export]
	private Menu Menu { get; set; }

	private readonly string FOREGROUND_PLACEHOLDER_PATH = "./Placeholder";

	private IGameStateInteractor _gameStateInteractor;

	private ILoggerService _logger { get; set; }
	private ICommonInteractor _commonInteractor { get; set; } 
	private IEggInteractor _eggInteractor { get; set; } 
	private IAnimalInteractor _animalInteractor { get; set; } 
	private CharacterFactory _characterFactory { get; set; }
	private Observables _observables { get; set; }
	public CommandInvoker _invoker { get; set; }

	public 
	override async void _Ready()
	{
		try
		{
			_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
			_commonInteractor = GetNode<ICommonInteractor>(Constants.SingletonNodes.CommonInteractor);
			_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
			_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
			_characterFactory = GetNode<CharacterFactory>(Constants.SingletonNodes.CharacterFactory);
			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
			_gameStateInteractor = GetNode<GameStateInteractor>(Constants.SingletonNodes.GameStateInteractor);

			await _commonInteractor.InitDatabaseIfRequired();

			var placeholder = GetNode<CharacterBody3D>(FOREGROUND_PLACEHOLDER_PATH);
			var position = placeholder.Position;
			placeholder.QueueFree();

			_gameStateInteractor.ReadyInstance(await GetCreatureListFromDatabase(), position, Menu);
			
			_observables.GrabEgg += HandleGrabEgg;
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Main _Ready Error: {ex.Message}");
			_logger.LogError($"Main _Ready Error: {ex.Message}", ex);
			throw;
		}
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

	private void HandleGrabEgg(ulong instanceId)
	{
		_logger.LogError("Call HandleGrabEgg");
		_logger.LogError($"instanceId {instanceId}");

		foreach (var model in _gameStateInteractor.GetCreatureList())
		{
			_logger.LogError($"model.InstanceId {model.InstanceId}");
			if (model.InstanceId == instanceId)
			{
				_logger.LogError("Grabbed egg was found in gallery");
			}
		}
	}
}
