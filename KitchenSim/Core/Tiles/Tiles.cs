public class FloorTile : ITile
{
}

public class WallTile : ITile
{
	Wall _node { get; set; }
	
	public void SetNode(Wall node)
	{
		_node = node;
	}
}

public class AgentTile : ITile
{
	IAgent _agent { get; set; }

	public AgentTile()
	{
	}

	public void SetAgent(IAgent agent)
	{
		_agent = agent;
	} 
}
