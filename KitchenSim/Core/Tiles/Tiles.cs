public class FloorTile : ITile
{
}

public class WallTile : ITile
{
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
