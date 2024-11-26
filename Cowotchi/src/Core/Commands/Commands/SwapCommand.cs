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
		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();
		_gameStateInteractor.RotateForegroundSubjects(fgCharacter);
		_observables.EmitUpdateSubjectNameLabel(fgCharacter.Model.Name);
		return true;
	}
}
