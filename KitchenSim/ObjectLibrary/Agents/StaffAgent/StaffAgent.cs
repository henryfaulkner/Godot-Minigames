using Godot;
using System;

public partial class StaffAgent : Agent, ITile
{
	ILoggerService _logger;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}
}
