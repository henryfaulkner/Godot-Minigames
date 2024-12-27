using Godot;
using System;

public partial class StaffAgent : Area2D, IAgent
{
	ITileMapService _tileMapService;
	ILoggerService _logger;

	#region State Machine
	enum States
	{
		MoveUp,
		MoveRight,
		MoveDown,
		MoveLeft,
	}
	#endregion

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_tileMapService = GetNode<ITileMapService>(Constants.SingletonNodes.TileMapService);
	}
}
