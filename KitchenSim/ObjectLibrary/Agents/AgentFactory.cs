using Godot;

public interface IAgentFactory 
{
	StaffAgent SpawnStaffAgent(Node parent, Vector2 position);
	CustomerAgent SpawnCustomerAgent(Node parent, Vector2 position);
}

public partial class AgentFactory : Node, IAgentFactory
{
	#region Staff
	private readonly StringName STAFF_SCENE_PATH = "res://ObjectLibrary/Agents/StaffAgent/StaffAgent.tscn";
	private PackedScene _staffScene;
	#endregion

	#region Customer
	private readonly StringName CUSTOMER_SCENE_PATH = "res://ObjectLibrary/Agents/CustomerAgent/CustomerAgent.tscn";
	private PackedScene _customerScene;
	#endregion

	ILoggerService _logger;

	public override void _Ready()
	{
		_staffScene = (PackedScene)ResourceLoader.Load(STAFF_SCENE_PATH);
		_customerScene = (PackedScene)ResourceLoader.Load(CUSTOMER_SCENE_PATH);

		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public StaffAgent SpawnStaffAgent(Node parent, Vector2 position)
	{
		var result = _staffScene.Instantiate<StaffAgent>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}

	public CustomerAgent SpawnCustomerAgent(Node parent, Vector2 position)
	{
		var result = _customerScene.Instantiate<CustomerAgent>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}
}
