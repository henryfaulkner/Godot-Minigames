using Godot;
using System;
using System.Threading.Tasks;

public partial class FgAnimalCharacter : CharacterBody3D, ICharacter<CreatureModel> 
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
		_logger.LogDebug("Start CowCharacter ReadyInstance");
		try
		{
			Model = model;

			Subject = new Subject<CreatureModel>(_logger);
			Subject.ReadyInstance(this, model);

			Controller = _controllerFactory.SpawnFgAnimalController(this, model, Collider, Mesh);

			Mesh.Mesh = GD.Load<Mesh>(model.Mesh);
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
		_loveEffect.TriggerEffectAsync();
	} 

	public async Task Eat() 
	{
		_fedEffect.TriggerEffectAsync();
	} 
}
