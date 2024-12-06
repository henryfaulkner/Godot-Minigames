using Godot;
using System;
using System.Threading.Tasks;

public partial class FeedCommand : Command
{
	private ILoggerService _logger { get; set; }
	private IAnimalInteractor _animalInteractor { get; set; } 
	private Observables _observables { get; set; }
	private IGameStateInteractor _gameStateInteractor { get; set;}
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_gameStateInteractor = GetNode<IGameStateInteractor>(Constants.SingletonNodes.GameStateInteractor);
		_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
	}

	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		int foodMeterIncrease = 1;
		int xpIncrease = 5;
		
		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();
		fgCharacter.Model.StomachLevel += increase;
		UpdateMeters(fgCharacter.Model);
		_animalInteractor.FeedAnimal(fgCharacter.Model.Id, xpIncrease);
		await fgCharacter.ExecuteActionAsync(Enumerations.CharacterActions.Eat);
		return true;
	}

	private void UpdateMeters(CreatureModel model)
	{
		_observables.EmitUpdateHungerMeterMax(model.StomachMax);
		_observables.EmitUpdateHungerMeterValue(model.StomachLevel);
	}
}
