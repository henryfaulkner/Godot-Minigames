using Godot;

public interface IEnvironmentFactory
{
	Wall SpawnWall(Node parent, Vector2 position);
	Table SpawnTable(Node parent, Vector2 position);
}

public partial class EnvironmentFactory : Node, IEnvironmentFactory
{
	#region Wall
	private readonly StringName WALL_SCENE_PATH = "res://ObjectLibrary/Environment/Wall/Wall.tscn";
	private readonly PackedScene _wallScene;
	#endregion

	#region Table
	private readonly StringName TABLE_SCENE_PATH = "res://ObjectLibrary/Environment/Table/Table.tscn";
	private readonly PackedScene _tableScene;
	#endregion

	ILoggerService _logger;

	public override void _Ready()
	{
		_wallScene = (PackedScene)ResourceLoader.Load(WALL_SCENE_PATH);
		_tableScene = (PackedScene)ResourceLoader.Load(TABLE_SCENE_PATH);
		
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public Wall SpawnWall(Node parent, Vector2 position)
	{
		var result = _wallScene.Instantiate<Wall>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}

	public Table SpawnTable(Node parent, Vector2 position)
	{
		var result = _tableScene.Instantiate<Table>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}
}
