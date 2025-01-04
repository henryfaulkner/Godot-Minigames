using Godot;
using System;

public interface ITileFactory
{
	FloorTile CreateFloorTile(TileMapLayer tileMapLayer, TileData tileData);
	WallTile CreateWallTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent);
	AgentTile CreateStaffAgentTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent); 
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

	public FloorTile CreateFloorTile(TileMapLayer tileMapLayer, TileData tileData)
	{
		return new FloorTile();
	}

	public WallTile CreateWallTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		var result = new WallTile();
		var wall = _environmentFactory.SpawnWall(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
		result.SetNode(wall);
		return result;
	}

	public AgentTile CreateStaffAgentTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		var result = new AgentTile();
		var staffAgent = _agentFactory.SpawnStaffAgent(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
		result.SetAgent(staffAgent);
		return result;
	}
}
