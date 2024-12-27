using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	ILoggerService _logger;
	ITileMapService _tileMapService;
	IAgentFactory _agentFactory;

	List<IAgent> _agentList = new List<IAgent>();

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_tileMapService = GetNode<ITileMapService>(Constants.SingletonNodes.TileMapService);
		_agentFactory = GetNode<IAgentFactory>(Constants.SingletonNodes.AgentFactory);

		_tileMapService.SetTileSize(GetTileSize());
		_tileMapService.SetTileGrid(ScanTileGrid());

		// It would be better for the simulation to place initial agents and environment 	
		// from the scene editor. 
		// Maybe read editor textures and replace them with nodes.
		// Obviously, customers will be spawn programatically.
	}

	public override void _Process(double delta)
	{
	}

	private int GetTileSize()
	{
		throw NotImplementedException();
	}

	private List<List<ITileGrid>> ScanTileGrid()
	{
		throw NotImplementedException();
	}
}
