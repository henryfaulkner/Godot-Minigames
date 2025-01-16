using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

public partial class Main : Node2D
{
	[ExportGroup("TileMap")]
	[Export]
	TileMapLayer TileMapLayer { get; set; }
	[Export]
	int GridDimension_X { get; set; }
	[Export]
	int GridDimension_Y { get; set; }

	[ExportGroup("UI")]
	[Export]
	DebugPopup DebugPopup { get; set; } 

	[ExportGroup("Customer")]
	[Export]
	Marker2D CustomerSpawnPoint { get; set; }

	const int TileSet_MainCellSourceId = 1;
	static Vector2I TileSet_AtlaCoords_Floor = new Vector2I(0, 0);
	static Vector2I TileSet_AtlaCoords_StaffAgent = new Vector2I(1, 0);
	static Vector2I TileSet_AtlaCoords_Wall = new Vector2I(0, 1);

	ILoggerService _logger;
	ITileMapService _tileMapService;
	IAgentFactory _agentFactory;
	ITileFactory _tileFactory;
	IOrderQueueSingleton _orderQueueSingleton;
	IToolsSingleton _toolsSingleton;
	ITablesSingleton _tablesSingleton;

	#region Agents
	List<StaffAgent> _staffList = new List<StaffAgent>();
	List<CustomerAgent> _customerList = new List<CustomerAgent>();
	#endregion

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_tileMapService = GetNode<ITileMapService>(Constants.SingletonNodes.TileMapService);
		_agentFactory = GetNode<IAgentFactory>(Constants.SingletonNodes.AgentFactory);
		_tileFactory = GetNode<ITileFactory>(Constants.SingletonNodes.TileFactory);
		_orderQueueSingleton = GetNode<IOrderQueueSingleton>(Constants.SingletonNodes.OrderQueueSingleton);
		_toolsSingleton = GetNode<IToolsSingleton>(Constants.SingletonNodes.ToolsSingleton);
		_tablesSingleton = GetNode<ITablesSingleton>(Constants.SingletonNodes.TablesSingleton);

		_tileMapService.SetTileSize(GetTileSize());

		ScanTileGrid();

		// have DebugMenu open as default
		DebugPopup.Visible = true;
	}

	public override void _Process(double delta)
	{
		HandleDebugMenu();
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("debug"))
		{
			// toggle menu on
			DebugPopup.Visible = !DebugPopup.Visible;
		}

		if (@event.IsActionReleased("spawn_customer"))
		{
			_customerList.Add(_agentFactory.SpawnCustomerAgent(GetNode<Node>("."), CustomerSpawnPoint.Position));
		}
	}

	private int GetTileSize()
	{
		return _tileMapService.GetTileSize();
	}

	private void ScanTileGrid()
	{
		for (int x = 0; x < GridDimension_X; x += 1)
		{
			for (int y = 0; y < GridDimension_Y; y += 1)
			{
				TileData tileData = TileMapLayer.GetCellTileData(new Vector2I(x, y));
				CreateTileNodes(tileData, new Tuple<int, int>(x, y));
			}
		}
	}

	private void CreateTileNodes(TileData tileData, Tuple<int, int> coordinateXY)
	{
 		switch ((Enumerations.TileTypes)(int)tileData.GetCustomData("TileTypeEnum"))
  		{
			// These tile will be handled automatically by the TileMap + TileSet's Physics and Navigation Layers
			case Enumerations.TileTypes.Floor:
 			case Enumerations.TileTypes.Wall:
 			case Enumerations.TileTypes.CounterTop:
 				break;
 			case Enumerations.TileTypes.StaffAgent:
 				var staffAgent = _tileFactory.SpawnStaffAgentTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize(), this);
				_staffList.Add(staffAgent);
				SetTileMapCellAsFloor(coordinateXY);
 				break;
			case Enumerations.TileTypes.Table:
				_logger.LogInfo("Spawn table");
 				var table = _tileFactory.SpawnTableTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize(), this);
				_tablesSingleton.AddTable(table);
				SetTileMapCellAsFloor(coordinateXY);
 				break;
			case Enumerations.TileTypes.Fridge:
 				var fridge = _tileFactory.SpawnFridgeTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize(), this);
				_toolsSingleton.AddFridge(fridge);
				SetTileMapCellAsFloor(coordinateXY);
 				break;
			case Enumerations.TileTypes.CuttingBoard:
 				var cuttingBoard = _tileFactory.SpawnCuttingBoardTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize(), this);
				_toolsSingleton.AddCuttingBoard(cuttingBoard);
				SetTileMapCellAsFloor(coordinateXY);
 				break;
			case Enumerations.TileTypes.OvenAndStove:
				_logger.LogInfo("Try create OvenAndStoveTile");
 				var ovenAndStove = _tileFactory.SpawnOvenAndStoveTile(TileMapLayer, tileData, coordinateXY, _tileMapService.GetTileSize(), this);
				_toolsSingleton.AddOvenAndStove(ovenAndStove);
				SetTileMapCellAsFloor(coordinateXY);
 				break;
 			default: 
 				_logger.LogError("Main GetTileTypeFromCustomData did not map to a TileType!");
 				break;
 		}	
	}
	
	// After creating Tile nodes,
	// Set the TileMapLayer as all floor atlases.
	private void SetTileMapCellAsFloor(Tuple<int, int> coordinateXY)
	{
		TileMapLayer.SetCell(new Vector2I(coordinateXY.Item1, coordinateXY.Item2), TileSet_MainCellSourceId, TileSet_AtlaCoords_Floor);
	}
	
	private void HandleDebugMenu()
	{
		var debugTextBuilder = new StringBuilder(string.Empty);
		debugTextBuilder.Append(GetStaffDebugText());
		DebugPopup.SetText(debugTextBuilder.ToString());
	}
	
	private string GetStaffDebugText()
	{
		var staffDebugTextBuilder = new StringBuilder(string.Empty);
		foreach (var staff in _staffList)
		{
			staffDebugTextBuilder.Append($"{staff.GetActivity()}\n");
		}
		return staffDebugTextBuilder.ToString();
	}
}
