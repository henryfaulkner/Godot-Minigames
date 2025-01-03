using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public partial class Main : Node2D
{
	[Export]
	TileMapLayer TileMapLayer { get; set; }
	[Export]
	int GridDimension_X { get; set; }
	[Export]
	int GridDimension_Y { get; set; } 

	ILoggerService _logger;
	ITileMapService _tileMapService;
	IAgentFactory _agentFactory;
	ITileFactory _tileFactory;

	List<IAgent> _agentList = new List<IAgent>();

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_tileMapService = GetNode<ITileMapService>(Constants.SingletonNodes.TileMapService);
		_agentFactory = GetNode<IAgentFactory>(Constants.SingletonNodes.AgentFactory);
		_tileFactory = GetNode<ITileFactory>(Constants.SingletonNodes.TileFactory);

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
		return _tileMapService.GetTileSize();
	}

	private List<List<ITile>> ScanTileGrid()
	{
		var result = new List<List<ITile>>();
		for (int x = 0; x < GridDimension_X; x += 1)
		{
			var innerList = new List<ITile>();
			for (int y = 0; y < GridDimension_Y; y += 1)
			{
				TileData tileData = TileMapLayer.GetCellTileData(new Vector2I(x, y));
				_logger.LogInfo(JsonConvert.SerializeObject(tileData));
				ITile tileElement = GetTileTypeFromCustomData(tileData, new Tuple<int, int>(x, y));
				innerList.Add(tileElement);
			}
			result.Add(innerList);
		}
		return result;
	}

	private ITile? GetTileTypeFromCustomData(TileData tileData, Tuple<int, int> coordinateXY)
	{
		ITile result = _tileFactory.CreateFloorTile(TileMapLayer, tileData);
		//switch (tileData["TileTypeEnum"])
		//{
			//case Enumerations.TileTypes.Floor:
				//result = _tileFactory.CreateFloorTile(TileMapLayer, tileData);
				//break;
			//case Enumerations.TileTypes.Wall:
				//result = _tileFactory.CreateWallTile(TileMapLayer, tileData);
				//break;
			//case Enumerations.TileTypes.Staff:
				//result = _tileFactory.CreateStaffAgentTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize, self);
				//break;
			//default: 
				//_logger.LogInfo ("Main GetTileTypeFromCustomData did not map to a TileType!");
				//break;
		//}	
		_logger.LogError(JsonConvert.SerializeObject(tileData));
		return result;
	}
}
