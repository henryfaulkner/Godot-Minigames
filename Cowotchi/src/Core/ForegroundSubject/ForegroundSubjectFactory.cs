using Godot;

public partial class ForegroundSubjectFactory : Node
{
	private readonly StringName EGG_SCENE_PATH = new StringName("res://src/ObjectLibrary/Characters/EggCharacter.tscn");
	private readonly StringName COW_SCENE_PATH = new StringName("res://src/ObjectLibrary/Characters/CowCharacter.tscn");

	private readonly PackedScene _eggScene;
	private readonly PackedScene _cowScene;

	public ForegroundSubjectFactory()
	{
		_eggScene = (PackedScene)ResourceLoader.Load(EGG_SCENE_PATH);
		_cowScene = (PackedScene)ResourceLoader.Load(COW_SCENE_PATH);
	}

	public EggCharacter SpawnEgg(Node parent, EggModel model, Vector3 position)
	{
		var result = _eggScene.Instantiate<EggCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}

	public BgEggCharacter SpawnBgEgg(Node parent, EggModel model, Vector3 position)
	{
		var result = _eggScene.Instantiate<BgEggCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}

	public CowCharacter SpawnCow(Node parent, AnimalModel model, Vector3 position)
	{
		var result = _cowScene.Instantiate<CowCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}

	public BgCowCharacter SpawnBgCow(Node parent, AnimalModel model, Vector3 position)
	{
		var result = _cowScene.Instantiate<BgCowCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	} 
}