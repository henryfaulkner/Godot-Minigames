using Godot;
using System;

public interface ITileFactory
{
	Wall CreateWallTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent);
	StaffAgent CreateStaffAgentTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent); 
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

	public Wall CreateWallTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _environmentFactory.SpawnWall(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}

	public StaffAgent CreateStaffAgentTile(TileMapLayer tileMapLayer, TileData tileData, Tuple<int, int> coordinateXY, int tileSize, Node parent)
	{
		return _agentFactory.SpawnStaffAgent(parent, 
			new Vector2((coordinateXY.Item1*tileSize) + (tileSize/2), (coordinateXY.Item2*tileSize) + (tileSize/2))
		);
	}
}
