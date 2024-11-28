using Godot;

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
		_basicBgEggControllerScene = (PackedScene)ResourceLoader.Load(BASIC_BG_EGG_CONTROLLER_SCENE_PATH);

		// FgAnimalControllers
		_basicBgAnimalControllerScene = (PackedScene)ResourceLoader.Load(BASIC_BG_ANIMAL_CONTROLLER_SCENE_PATH);
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public IController SpawnBgEggController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateBgEggController(model.BgEggController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;
	}

	public IController SpawnBgAnimalController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateBgAnimalController(model.BgAnimalController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;
	}

	public IController SpawnFgEggController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateFgEggController(model.FgEggController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;
	}

	public IController SpawnFgAnimalController(CharacterBody3D puppet, CreatureModel model, CollisionShape3D collider, MeshInstance3D mesh)
	{
		var result = InstantiateFgAnimalController(model.FgAnimalController);
		result.SetPuppet(puppet);
		result.ReadyInstance(collider, mesh);
		return result;	
	}

	private IController? InstantiateBgEggController(Enumerations.BgEggControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.BgEggControllers.Basic:
				result = _basicBgEggControllerScene.Instantiate<BasicBgEggController>();
				break;
			default:
				break;
		}
		return result;
	}

	private IController? InstantiateBgAnimalController(Enumerations.BgAnimalControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.BgAnimalControllers.Basic:
				result = _basicBgAnimalControllerScene.Instantiate<BasicBgAnimalController>();
				break;
			default: 
				break;
		}
		return result;
	}	

	private IController? InstantiateFgEggController(Enumerations.FgEggControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.FgEggControllers.Basic:
				result = _basicFgEggControllerScene.Instantiate<BasicFgController>();
				break;
			default:
				break;
		}
		return result;
	}

	private IController? InstantiateFgAnimalController(Enumerations.FgAnimalControllers type)
	{
		IController? result = null;
		switch (type)
		{
			case Enumerations.FgAnimalControllers.Basic:
				result = _basicFgAnimalControllerScene.Instantiate<BasicFgController>();
				break;
			default:
				break;
		}
		return result;
	}
}
