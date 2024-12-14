using Godot;
using System;

public partial class CommandFactory : Node
{
	private readonly StringName FEED_COMMAND_SCENE_PATH = "res://src/Core/Commands/Commands/FeedCommand.tscn";
	private readonly StringName NURTURE_COMMAND_SCENE_PATH = "res://src/Core/Commands/Commands/NurtureCommand.tscn";
	private readonly StringName STATS_COMMAND_SCENE_PATH = "res://src/Core/Commands/Commands/StatsCommand.tscn";
	private readonly StringName SWAP_COMMAND_SCENE_PATH = "res://src/Core/Commands/Commands/SwapCommand.tscn";
	private readonly StringName HATCH_COMMAND_SCENE_PATH = "res://src/Core/Commands/Commands/HatchCommand.tscn";

	private readonly PackedScene _feedCommandScene;
	private readonly PackedScene _nurtureCommandScene;
	private readonly PackedScene _statsCommandScene;
	private readonly PackedScene _swapCommandScene;
	private readonly PackedScene _hatchCommandScene;

	private ILoggerService _logger { get; set; }

	public CommandFactory()
	{
		_feedCommandScene = (PackedScene)ResourceLoader.Load(FEED_COMMAND_SCENE_PATH);
		_nurtureCommandScene = (PackedScene)ResourceLoader.Load(NURTURE_COMMAND_SCENE_PATH);
		_statsCommandScene = (PackedScene)ResourceLoader.Load(STATS_COMMAND_SCENE_PATH);
		_swapCommandScene = (PackedScene)ResourceLoader.Load(SWAP_COMMAND_SCENE_PATH);
		_hatchCommandScene = (PackedScene)ResourceLoader.Load(HATCH_COMMAND_SCENE_PATH);
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public FeedCommand SpawnFeedCommand()
	{
		var result = _feedCommandScene.Instantiate<FeedCommand>();
		AddChild(result);
		return result;
	}

	public NurtureCommand SpawnNurtureCommand()
	{
		var result = _nurtureCommandScene.Instantiate<NurtureCommand>();
		AddChild(result);
		return result;
	}

	public StatsCommand SpawnStatsCommand()
	{
		var result = _statsCommandScene.Instantiate<StatsCommand>();
		AddChild(result);
		return result;
	}

	public SwapCommand SpawnSwapCommand()
	{
		var result = _swapCommandScene.Instantiate<SwapCommand>();
		AddChild(result);
		return result;
	}

	public HatchCommand SpawnHatchCommand()
	{
		var result = _hatchCommandScene.Instantiate<HatchCommand>();
		AddChild(result);
		return result;
	}
}
