using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public partial class Main : Node2D
{
	const int TileSet_MainCellSourceId = 1;
	static Vector2I TileSet_AtlaCoords_Floor = new Vector2I(0, 0);
	static Vector2I TileSet_AtlaCoords_StaffAgent = new Vector2I(1, 0);
	static Vector2I TileSet_AtlaCoords_Wall = new Vector2I(0, 1);

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
				ITile tileNode = CreateTileNodes(tileData, new Tuple<int, int>(x, y));
				innerList.Add(tileNode);
			}
			result.Add(innerList);
		}
		return result;
	}

	private ITile? CreateTileNodes(TileData tileData, Tuple<int, int> coordinateXY)
	{
		ITile result = null;
 		switch ((Enumerations.TileTypes)(int)tileData.GetCustomData("TileTypeEnum"))
  		{
			// These tile will be handled automatically by the TileMap + TileSet's Physics and Navigation Layers
			case Enumerations.TileTypes.Floor:
 			case Enumerations.TileTypes.Wall:
 				break;
 			case Enumerations.TileTypes.StaffAgent:
 				var staffAgent = _tileFactory.SpawnStaffAgentTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize(), this);
				var tempTable = GetNode<Table>("./Table");
				_logger.LogInfo($"TempTable is null: {tempTable == null}"); 
				staffAgent.SetNavTarget(tempTable);
				result = staffAgent;
				SetTileMapCellAsFloor(coordinateXY);
 				break;
			case Enumerations.TileTypes.Table:
 				result = _tileFactory.SpawnTableTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize(), this);
				SetTileMapCellAsFloor(coordinateXY);
 				break;
 			default: 
 				_logger.LogInfo ("Main GetTileTypeFromCustomData did not map to a TileType!");
 				break;
 		}	
		return result;
	}
	
	// After creating Tile nodes,
	// Set the TileMapLayer as all floor atlases.
	private void SetTileMapCellAsFloor(Tuple<int, int> coordinateXY)
	{
		TileMapLayer.SetCell(new Vector2I(coordinateXY.Item1, coordinateXY.Item2), TileSet_MainCellSourceId, TileSet_AtlaCoords_Floor);
	}
}
