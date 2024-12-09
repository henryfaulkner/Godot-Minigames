using Godot;
using System;
using System.Threading.Tasks;

public partial class BgEggCharacter : CharacterBody3D, ICharacter<CreatureModel>
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
	private SparkleEffect? SparkleEffect { get; set; }

	private ILoggerService _logger { get; set; }
	private IEggInteractor _eggInteractor { get; set; } 
	private ControllerFactory _controllerFactory { get; set; }	
	private Observables _observables { get; set; }
	private EffectsFactory _effectsFactory { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_controllerFactory = GetNode<ControllerFactory>(Constants.SingletonNodes.ControllerFactory);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_effectsFactory = GetNode<EffectsFactory>(Constants.SingletonNodes.EffectsFactory);
	} 

	public override void _PhysicsProcess(double delta)
	{
		if (Controller != null) Controller.PhysicsProcess(delta);
	}

	public void ReadyInstance(CreatureModel model)
	{
		_logger.LogDebug("Start BgEggController ReadyInstance");
		try
		{
			Model = model;

			Subject = new Subject<CreatureModel>(_logger);
			Subject.ReadyInstance(this, model);

			Controller = _controllerFactory.SpawnBgEggController(this, model, Collider, Mesh);
			
			if (!model.IsInGallery) SparkleEffect = _effectsFactory.SpawnSparkleEffect(this, HeadNode.Position);
		}
		catch (Exception ex)
		{
			_logger.LogError($"BgEggCharacter ReadyInstance Error: {ex.Message}", ex);
			throw;
		}
		_logger.LogDebug("Start BgEggController ReadyInstance");
	}

	public async Task ExecuteActionAsync(Enumerations.CharacterActions action)
	{
		switch (action)
		{
			case Enumerations.CharacterActions.Hatch:
				Hatch();
				break;
			default:
				break;
		}
	}

	public async Task Hatch() 
	{
		throw new NotImplementedException();
	} 
}
