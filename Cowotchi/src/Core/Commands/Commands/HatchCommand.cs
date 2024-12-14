using Godot;
using System;
using System.Threading.Tasks;

public partial class HatchCommand : Command
{
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	private IGameStateInteractor _gameStateInteractor { get; set; }
	private IEggInteractor _eggInteractor { get; set; }
	private IAnimalInteractor _animalInteractor { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_gameStateInteractor = GetNode<IGameStateInteractor>(Constants.SingletonNodes.GameStateInteractor);
		_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
		_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
	}

	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		// egg
		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();

		// animal
		var newAnimalModel = await _eggInteractor.HatchEgg(fgCharacter.Model.Id);

		_observables.EmitSpinTheWheel(newAnimalModel.Id);

		TimingFunctions.SetTimeout(() => {
			_gameStateInteractor.AddBackgroundSubject(newAnimalModel);
			var oldEggFgModel = _gameStateInteractor.SwapBackgroundToForeground(newAnimalModel);
			_gameStateInteractor.RemoveBackgroundSubject(oldEggFgModel);
		}, 1500);

		return true;
	}
}
