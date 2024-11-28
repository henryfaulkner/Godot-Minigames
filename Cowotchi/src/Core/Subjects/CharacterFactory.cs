using Godot;
using System;

public partial class CharacterFactory : Node
{
	private readonly StringName BG_EGG_SCENE_PATH = "res://src/ObjectLibrary/Characters/BgEggCharacter.tscn";
	private readonly StringName BG_ANIMAL_SCENE_PATH = "res://src/ObjectLibrary/Characters/BgAnimalCharacter.tscn";
	private readonly StringName FG_EGG_SCENE_PATH = new StringName("res://src/ObjectLibrary/Characters/FgEggCharacter.tscn");
	private readonly StringName FG_ANIMAL_SCENE_PATH = new StringName("res://src/ObjectLibrary/Characters/FgAnimalCharacter.tscn");

	private readonly PackedScene _bgEggScene;
	private readonly PackedScene _bgAnimalScene;
	private readonly PackedScene _fgEggScene;
	private readonly PackedScene _fgAnimalScene;

	private ILoggerService _logger { get; set; }

	public CharacterFactory()
	{
		_bgEggScene = (PackedScene)ResourceLoader.Load(BG_EGG_SCENE_PATH);
		_bgAnimalScene = (PackedScene)ResourceLoader.Load(BG_ANIMAL_SCENE_PATH);
		_fgEggScene = (PackedScene)ResourceLoader.Load(FG_EGG_SCENE_PATH);
		_fgAnimalScene = (PackedScene)ResourceLoader.Load(FG_ANIMAL_SCENE_PATH);
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public BgEggCharacter SpawnBgEgg(Node parent, CreatureModel model, Vector3 position)
	{
		var result = _bgEggScene.Instantiate<BgEggCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}

	public BgAnimalCharacter SpawnBgAnimal(Node parent, CreatureModel model, Vector3 position)
	{
		var result = _bgAnimalScene.Instantiate<BgAnimalCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}

	public FgEggCharacter SpawnFgEgg(Node parent, CreatureModel model, Vector3 position)
	{
		var result = _fgEggScene.Instantiate<FgEggCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}

	public FgAnimalCharacter SpawnFgAnimal(Node parent, CreatureModel model, Vector3 position)
	{
		var result = _fgAnimalScene.Instantiate<FgAnimalCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}
}
