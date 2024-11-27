using Godot;
using System;
using System.Threading.Tasks;

public partial class NurtureCommand : Command
{    
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	private IGameStateInteractor _gameStateInteractor { get; set;}
	private IAnimalInteractor _animalInteractor { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_gameStateInteractor = GetNode<IGameStateInteractor>(Constants.SingletonNodes.GameStateInteractor);
		_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
	}

	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		int increase = 1;
		
		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();
		await fgCharacter.ExecuteActionAsync(Enumerations.CharacterActions.ReceiveLove);
		fgCharacter.Model.LoveLevel += increase;
		UpdateMeters(fgCharacter.Model);
		return true;
	}

	private void UpdateMeters(CreatureModel model)
	{
		_observables.EmitUpdateHeartMeterMax(model.LoveMax);
		_observables.EmitUpdateHeartMeterValue(model.LoveLevel);
	}
}
