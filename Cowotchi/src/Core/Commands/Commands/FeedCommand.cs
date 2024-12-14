using Godot;
using System;
using System.Threading.Tasks;

public partial class FeedCommand : Command
{
	private ILoggerService _logger { get; set; }
	private IAnimalInteractor _animalInteractor { get; set; } 
	private Observables _observables { get; set; }
	private IGameStateInteractor _gameStateInteractor { get; set; }
	private IEggInteractor _eggInteractor { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_gameStateInteractor = GetNode<IGameStateInteractor>(Constants.SingletonNodes.GameStateInteractor);
		_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
		_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
	}

	int foodMeterIncrease = 1;
	int xpIncrease = 25;
	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		var fgCharacter = _gameStateInteractor.GetForegroundCharacter();

		if (fgCharacter.Model.StomachLevel < fgCharacter.Model.StomachMax)
		{
			await UpdateDatabase(fgCharacter);
			
			UpdateModel(fgCharacter);

			await UpdateGame(fgCharacter);
		
			return true;
		}
		return false;
	}

	private async Task UpdateDatabase(ICharacter<CreatureModel> fgCharacter)
	{
		_animalInteractor.FeedAnimal(fgCharacter.Model.Id, xpIncrease);
		
		// Get and store eggs ready to hatch 
		//_gameStateInteractor.ReadyEggList = await _eggInteractor.GetReadyEggs();
	}

	private void UpdateModel(ICharacter<CreatureModel> fgCharacter)
	{
		
		fgCharacter.Model.StomachLevel += foodMeterIncrease;
		fgCharacter.Model.XpOffset = fgCharacter.Model.XpOffset + xpIncrease; 
		if (fgCharacter.Model.XpOffset > XpTableHelper.GetLevelsXpGoal(fgCharacter.Model.CreatureLevel))
		{
			fgCharacter.Model.XpOffset = fgCharacter.Model.XpOffset - XpTableHelper.GetLevelsXpGoal(fgCharacter.Model.CreatureLevel);
			fgCharacter.Model.CreatureLevel = fgCharacter.Model.CreatureLevel + 1;
		}
	}

	private async Task UpdateGame(ICharacter<CreatureModel> fgCharacter)
	{
		UpdateMeters(fgCharacter.Model);
		_observables.EmitUpdateCurrentCreatureInfo();
		await fgCharacter.ExecuteActionAsync(Enumerations.CharacterActions.Eat);
	}

	private void UpdateMeters(CreatureModel model)
	{
		_observables.EmitUpdateHungerMeterMax(model.StomachMax);
		_observables.EmitUpdateHungerMeterValue(model.StomachLevel);
	}
}
