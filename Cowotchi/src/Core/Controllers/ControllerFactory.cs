using Godot;
using Newtonsoft.Json;

public partial class ControllerFactory : Node
{
	#region BgEggControllers
	private readonly StringName BASIC_BG_EGG_CONTROLLER_SCENE_PATH = "res://src/Core/Controllers/BgControllers/BasicBgEggController.tscn";
	private readonly PackedScene _basicBgEggControllerScene;
	#endregion

	#region BgAnimalControllers
	private readonly StringName BASIC_BG_ANIMAL_CONTROLLER_SCENE_PATH = "res://src/Core/Controllers/BgControllers/BasicBgAnimalController.tscn";
	private readonly PackedScene _basicBgAnimalControllerScene;
	#endregion

	#region FgEggControllers
	private readonly StringName BASIC_FG_EGG_CONTROLLER_SCENE_PATH = "res://src/Core/Controllers/FgControllers/BasicFgController.tscn";
	private readonly PackedScene _basicFgEggControllerScene;
	#endregion
	
	#region FgAnimalControllers
	private readonly StringName BASIC_FG_ANIMAL_CONTROLLER_SCENE_PATH = "res://src/Core/Controllers/FgControllers/BasicFgController.tscn";
	private readonly PackedScene _basicFgAnimalControllerScene;
	#endregion

	private ILoggerService _logger { get; set; }

	public ControllerFactory()
	{
		// BgEggControllers
		_basicBgEggControllerScene = (PackedScene)ResourceLoader.Load(BASIC_BG_EGG_CONTROLLER_SCENE_PATH);

		// BgAnimalControllers
		_basicBgAnimalControllerScene = (PackedScene)ResourceLoader.Load(BASIC_BG_ANIMAL_CONTROLLER_SCENE_PATH);

		// FgEggControllers
		_basicFgEggControllerScene = (PackedScene)ResourceLoader.Load(BASIC_FG_EGG_CONTROLLER_SCENE_PATH);

		// FgAnimalControllers
		_basicFgAnimalControllerScene = (PackedScene)ResourceLoader.Load(BASIC_FG_ANIMAL_CONTROLLER_SCENE_PATH);
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public IController SpawnBgEggController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateBgEggController(puppet, model.BgEggController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;
	}

	public IController SpawnBgAnimalController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateBgAnimalController(puppet, model.BgAnimalController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;
	}

	public IController SpawnFgEggController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateFgEggController(puppet, model.FgEggController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;
	}

	public IController SpawnFgAnimalController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateFgAnimalController(puppet, model.FgAnimalController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;	
	}

	private IController? InstantiateBgEggController(Node parent, Enumerations.BgEggControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.BgEggControllers.Basic:
				var node = _basicBgEggControllerScene.Instantiate<BasicBgEggController>();
				parent.AddChild(node);
				result = node;
				break;
			default:
			_logger.LogError($"Unknown BgEggController type: {type}");
				break;
		}
		return result;
	}

	private IController? InstantiateBgAnimalController(Node parent, Enumerations.BgAnimalControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.BgAnimalControllers.Basic:
				var node = _basicBgAnimalControllerScene.Instantiate<BasicBgAnimalController>();
				parent.AddChild(node);
				result = node;
				break;
			default: 
				_logger.LogError($"Unknown BgAnimalController type: {type}");
				break;
		}
		return result;
	}	

	private IController? InstantiateFgEggController(Node parent, Enumerations.FgEggControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.FgEggControllers.Basic:
				var node = _basicFgEggControllerScene.Instantiate<BasicFgController>();
				parent.AddChild(node);
				result = node;
				break;
			default:
				_logger.LogError($"Unknown FgEggController type: {type}");
				break;
		}
		return result;
	}

	private IController? InstantiateFgAnimalController(Node parent, Enumerations.FgAnimalControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.FgAnimalControllers.Basic:
				var node = _basicFgAnimalControllerScene.Instantiate<BasicFgController>();
				parent.AddChild(node);
				result = node;
				break;
			default:
				_logger.LogError($"Unknown FgAnimalController type: {type}");
				break;
		}
		return result;
	}
}
