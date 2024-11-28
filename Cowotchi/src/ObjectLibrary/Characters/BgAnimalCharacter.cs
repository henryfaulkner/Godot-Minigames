using Godot;
using System;
using System.Threading.Tasks;

public partial class BgAnimalCharacter : CharacterBody3D, ICharacter<CreatureModel>
{
	[ExportGroup("Nodes")]
	[Export]
	private Node3D HeadNode { get; set; }

	[Export]
	private MeshInstance3D Mesh { get; set; }
	
	[Export]
	private CollisionShape3D Collider { get; set; }

	public Subject<CreatureModel> Subject { get; set; }
	public CreatureModel Model { get; set; }
	public IController Controller { get; set; }
	
	private ILoggerService _logger { get; set; }
	private IAnimalInteractor _animalInteractor { get; set; } 
	private EffectsFactory _effectsFactory { get; set; }
	private ControllerFactory _controllerFactory { get; set; }	
	private Observables _observables { get; set; }

	private LoveEffect _loveEffect { get; set; }
	private LoveEffect _fedEffect { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
		_effectsFactory = GetNode<EffectsFactory>(Constants.SingletonNodes.EffectsFactory);
		_controllerFactory = GetNode<ControllerFactory>(Constants.SingletonNodes.ControllerFactory);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

		_loveEffect = _effectsFactory.SpawnLoveEffect(HeadNode, HeadNode.Position);
		_fedEffect = _effectsFactory.SpawnLoveEffect(HeadNode, HeadNode.Position);
	} 

	public override void _PhysicsProcess(double delta)
	{
		if (Controller != null) Controller.PhysicsProcess(delta);
	}

	public void ReadyInstance(CreatureModel model)
	{
		_logger.LogDebug("Start BgCowController ReadyInstance");
		try
		{
			Model = model;

			Subject = new Subject<CreatureModel>(_logger);
			Subject.ReadyInstance(this, model);

			Controller = _controllerFactory.SpawnBgAnimalController(this, model, Collider, Mesh);
		}
		catch (Exception ex)
		{
			_logger.LogError($"BgCowCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("Start BgCowController ReadyInstance");
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
		_loveEffect.TriggerEffect();
	} 

	public async Task Eat() 
	{
		int increase = 1;

		_animalInteractor.FeedAnimal(Model.Id);
		Model.StomachLevel += increase;
		_observables.EmitUpdateHungerMeterValue(Model.StomachLevel);
		_fedEffect.TriggerEffect();
	} 
}
