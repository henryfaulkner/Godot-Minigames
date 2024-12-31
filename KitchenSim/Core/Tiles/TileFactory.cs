using Godot;
using System;

public interface ITileFactory
{
	FloorTile CreateFloorTile(TileMap tileMap, TileData tileData);
	WallTile CreateWallTile(TileMap tileMap, TileData tileData);
	AgentTile CreateStaffAgentTile(TileMap tileMap, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent); 
}

public partial class TileFactory : Node, ITileFactory
{
	IAgentFactory _agentFactory;

	public override void _Ready()
	{
		_agentFactory = GetNode<IAgentFactory>(Constants.SingletonNodes.TileFactory);
	}

	public FloorTile CreateFloorTile(TileMap tileMap, TileData tileData)
	{
		return new FloorTile();
	}

	public WallTile CreateWallTile(TileMap tileMap, TileData tileData)
	{
		return new WallTile();
	}

	public AgentTile CreateStaffAgentTile(TileMap tileMap, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		var result = new AgentTile();
		var staffAgent = _agentFactory.SpawnStaffAgent(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
		result.SetAgent(staffAgent);
		return result;
	}
}
