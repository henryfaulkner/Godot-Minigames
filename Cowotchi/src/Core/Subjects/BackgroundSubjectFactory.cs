using Godot;
using System;

public partial class BackgroundSubjectFactory : Node
{
	private readonly StringName BG_EGG_SCENE_PATH = "res://src/ObjectLibrary/Characters/BgEggCharacter.tscn";
	private readonly StringName BG_COW_SCENE_PATH = "res://src/ObjectLibrary/Characters/BgCowCharacter.tscn";

	private readonly PackedScene _bgEggScene;
	private readonly PackedScene _bgCowScene;

	private ILoggerService _logger { get; set; }

	public BackgroundSubjectFactory()
	{
		_bgEggScene = (PackedScene)ResourceLoader.Load(BG_EGG_SCENE_PATH);
		_bgCowScene = (PackedScene)ResourceLoader.Load(BG_COW_SCENE_PATH);
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public BgEggCharacter SpawnEgg(Node parent, CreatureModel model, Vector3 position)
	{
		_logger.LogInfo("7.301");
		var result = _bgEggScene.Instantiate<BgEggCharacter>();
		_logger.LogInfo("7.302");
		model.InstanceId = result.GetInstanceId();
		_logger.LogInfo("7.303");
		parent.AddChild(result);
		_logger.LogInfo("7.304");
		result.GlobalPosition = position;
		_logger.LogInfo("7.305");
		result.ReadyInstance(model);
		_logger.LogInfo("7.306");
		return result;
	}

	public BgCowCharacter SpawnCow(Node parent, CreatureModel model, Vector3 position)
	{
		_logger.LogInfo("7.301");
		var result = _bgCowScene.Instantiate<BgCowCharacter>();
		_logger.LogInfo("7.302");
		model.InstanceId = result.GetInstanceId();
		_logger.LogInfo("7.303");
		parent.AddChild(result);
		_logger.LogInfo("7.304");
		result.GlobalPosition = position;
		_logger.LogInfo("7.305");
		result.ReadyInstance(model);
		_logger.LogInfo("7.306");
		return result;
	} 
}
