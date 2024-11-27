using Godot;
using System;
using System.Threading.Tasks;

public partial class CowCharacter : CharacterBody3D, ICharacter<CreatureModel> 
{
	[Export]
	private Node3D HeadNode { get; set; }

	public Subject<CreatureModel> Subject { get; set; }
	public CreatureModel Model { get; set; }

	private ILoggerService _logger { get; set; }
	private IAnimalInteractor _animalInteractor { get; set; } 
	private EffectsFactory _effectsFactory { get; set; }
	private Observables _observables { get; set; }

	private LoveEffect _loveEffect { get; set; }
	private LoveEffect _fedEffect { get; set; }

	public void ReadyInstance(CreatureModel model)
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_logger.LogDebug("Start CowCharacter ReadyInstance");
		try
		{
			Model = model;

			Subject = new Subject<CreatureModel>(_logger);
			Subject.ReadyInstance(this, model);

			_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
			_effectsFactory = GetNode<EffectsFactory>(Constants.SingletonNodes.EffectsFactory);
			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

			_loveEffect = _effectsFactory.SpawnLoveEffect(HeadNode, HeadNode.Position);
			_fedEffect = _effectsFactory.SpawnLoveEffect(HeadNode, HeadNode.Position);
		} 
		catch (Exception ex)
		{
			_logger.LogError($"CowCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("End CowCharacter ReadyInstance");
	}

	public async Task ExecuteActionAsync(Enumerations.CharacterActions action)
	{
		switch (action)
		{
			case Enumerations.CharacterActions.ReceiveLove:
				ReceiveLove();
				break;
			case Enumerations.CharacterActions.Eat:
				Eat();
				break;
			default:
				break;
		}
	}
	 
	public async Task ReceiveLove() 
	{
		int increase = 1;

		_animalInteractor.NurtureAnimal(Model.Id);
		Model.LoveLevel += increase;
		_observables.EmitUpdateHeartMeterValue(Model.LoveLevel);
		_loveEffect.EmitEffect();
	} 

	public async Task Eat() 
	{
		int increase = 1;

		_animalInteractor.FeedAnimal(Model.Id);
		Model.StomachLevel += increase;
		_observables.EmitUpdateHungerMeterValue(Model.StomachLevel);
		_fedEffect.EmitEffect();
	} 
}
