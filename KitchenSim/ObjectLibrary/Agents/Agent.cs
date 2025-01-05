using Godot;
using System;

public partial class Agent : Node2D, IAgent
{
	[Export]
	protected AgentController Controller;

	ILoggerService _logger;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}
	
	public void SetNavTarget(Node2D navTarget)
	{
		Controller.SetNavTarget(navTarget);
	}
}
