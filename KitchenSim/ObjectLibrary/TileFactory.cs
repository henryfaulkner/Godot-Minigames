using Godot;
using System;

public interface ITileFactory
{
	Wall SpawnWallTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent);
	Table SpawnTableTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent);
	CuttingBoard SpawnCuttingBoardTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent);
	Fridge SpawnFridgeTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent);
	OvenAndStove SpawnOvenAndStoveTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent);
	StaffAgent SpawnStaffAgentTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent); 
}

public partial class TileFactory : Node, ITileFactory
{
	IAgentFactory _agentFactory;
	IEnvironmentFactory _environmentFactory;

	public override void _Ready()
	{
		_agentFactory = GetNode<IAgentFactory>(Constants.SingletonNodes.AgentFactory);
		_environmentFactory = GetNode<IEnvironmentFactory>(Constants.SingletonNodes.EnvironmentFactory);
	}

	public Wall SpawnWallTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _environmentFactory.SpawnWall(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}

	public Table SpawnTableTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _environmentFactory.SpawnTable(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}

	public CuttingBoard SpawnCuttingBoardTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _environmentFactory.SpawnCuttingBoard(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}

	public Fridge SpawnFridgeTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _environmentFactory.SpawnSpawnFridgeTile(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}

	public OvenAndStove SpawnOvenAndStoveTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _environmentFactory.SpawnOvenAndStove(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}

	public StaffAgent SpawnStaffAgentTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _agentFactory.SpawnStaffAgent(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}
}
