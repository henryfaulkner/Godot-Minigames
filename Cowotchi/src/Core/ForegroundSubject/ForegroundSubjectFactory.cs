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

	public EggCharacter SpawnEgg(EggModel model, Vector3 position)
	{
		var result = _eggScene.Instantiate<EggCharacter>();
		result.GlobalPosition = position;
		GetNode(".").AddChild(result);
		result.ReadyInstance(model);
		return result;
	}

	public CowCharacter SpawnCow(AnimalModel model, Vector3 position)
	{
		var result = _cowScene.Instantiate<CowCharacter>();
		result.GlobalPosition = position;
		GetNode(".").AddChild(result);
		result.ReadyInstance(model);
		return result;
	}
}
