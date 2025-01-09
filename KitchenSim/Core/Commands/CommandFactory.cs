using Godot;
using System;

public partial class CommandFactory : Node
{
	private readonly StringName PLACEORDER_COMMAND_SCENE_PATH = "res://src/Core/Commands/Commands/PlaceOrderCommand.tscn";

	private readonly PackedScene _placeOrderCommandScene;

	private ILoggerService _logger { get; set; }

	public CommandFactory()
	{
		_placeOrderCommandScene = (PackedScene)ResourceLoader.Load(PLACE_ORDER_COMMAND_SCENE_PATH);
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public FeedCommand SpawnFeedCommand()
	{
		var result = _placeOrderCommandScene.Instantiate<PlaceOrderCommand>();
		AddChild(result);
		return result;
	}
}
