using Godot;

public interface IAgentFactory 
{
	IAgent SpawnStaffAgent(Node parent, Vector2 position);
}

public partial class AgentFactory : Node, IAgentFactory
{
	#region Staff
	private readonly StringName STAFF_SCENE_PATH = "res://ObjectLibrary/Agents/StaffAgent/StaffAgent.tscn";
	private readonly PackedScene _staffScene;
	#endregion

	private ILoggerService _logger { get; set; }

	public AgentFactory()
	{
		_staffScene = (PackedScene)ResourceLoader.Load(STAFF_SCENE_PATH);
	}

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public IAgent SpawnStaffAgent(Node parent, Vector2 position)
	{
		var result = _staffScene.Instantiate<StaffAgent>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}
}
