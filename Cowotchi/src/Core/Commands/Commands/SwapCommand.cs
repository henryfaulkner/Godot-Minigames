using Godot;
using System;
using System.Threading.Tasks;

public partial class SwapCommand : Command
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
		_gameStateInteractor.RotateForegroundSubjects();

		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();

		_observables.EmitUpdateSubjectNameLabel(fgCharacter.Model.Name);
		_observables.EmitUpdateCurrentCreatureInfo();
		UpdateMeters(fgCharacter.Model);

		return true;
	}

	private void UpdateMeters(CreatureModel model)
	{
		_observables.EmitUpdateHeartMeterMax(model.LoveMax);
		_observables.EmitUpdateHeartMeterValue(model.LoveLevel);
		_observables.EmitUpdateHungerMeterMax(model.StomachMax);
		_observables.EmitUpdateHungerMeterValue(model.StomachLevel);
	}
}
