using Godot;
using System;
using System.Threading.Tasks;

public partial class NurtureCommand : Command
{    
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	private IGameStateInteractor _gameStateInteractor { get; set;}
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_gameStateInteractor = GetNode<IGameStateInteractor>(Constants.SingletonNodes.GameStateInteractor);
	}

	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();
		UpdateMeters(fgCharacter.Model);
		return true;
	}

	private void UpdateMeters(CreatureModel model)
	{
		_logger.LogInfo("Start UpdateMetersForAnimal");
		_observables.EmitUpdateHeartMeterMax(model.LoveMax);
		_observables.EmitUpdateHeartMeterValue(model.LoveLevel);

		_observables.EmitUpdateHungerMeterMax(model.StomachMax);
		_observables.EmitUpdateHungerMeterValue(model.StomachLevel);
		_logger.LogInfo("End UpdateMetersForAnimal");
	}
}
