using Godot;
using System;

public partial class BackgroundSubjectFactory : Node
{
	private readonly StringName BG_EGG_SCENE_PATH = "res://src/ObjectLibrary/Characters/BgEggCharacter.tscn";
	private readonly StringName BG_COW_SCENE_PATH = "res://src/ObjectLibrary/Characters/BgCowCharacter.tscn";

	private readonly PackedScene _bgEggScene;
	private readonly PackedScene _bgCowScene;

	public BackgroundSubjectFactory()
	{
		_bgEggScene = (PackedScene)ResourceLoader.Load(BG_EGG_SCENE_PATH);
		_bgCowScene = (PackedScene)ResourceLoader.Load(BG_COW_SCENE_PATH);
	}

	public BgEggCharacter SpawnEgg(Node parent, CreatureModel model, Vector3 position)
	{
		var result = _bgEggScene.Instantiate<BgEggCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	}

	public BgCowCharacter SpawnCow(Node parent, CreatureModel model, Vector3 position)
	{
		var result = _bgCowScene.Instantiate<BgCowCharacter>();
		model.InstanceId = result.GetInstanceId();
		parent.AddChild(result);
		result.GlobalPosition = position;
		result.ReadyInstance(model);
		return result;
	} 
}
