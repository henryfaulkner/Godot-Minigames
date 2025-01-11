using Godot;
using System;

public partial class CommandFactory : Node
{
	private readonly PackedScene _placeOrderCommandScene;

	private ILoggerService _logger { get; set; }

	public CommandFactory()
	{
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}
}
