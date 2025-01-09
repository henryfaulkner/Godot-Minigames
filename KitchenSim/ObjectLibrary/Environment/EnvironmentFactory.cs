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

	#region 
	private readonly StringName CUTTING_BOARD_SCENE_PATH = "res://ObjectLibrary/Environment/CuttingBoard/CuttingBoard.tscn";
	private readonly PackedScene _cuttingBoardScene;
	#endregion

	#region 
	private readonly StringName FRIDGE_SCENE_PATH = "res://ObjectLibrary/Environment/Fridge/Fridge.tscn";
	private readonly PackedScene _fridgeScene;
	#endregion

	#region 
	private readonly StringName OVEN_AND_STOVE_SCENE_PATH = "res://ObjectLibrary/Environment/OvenAndStove/OvenAndStove.tscn";
	private readonly PackedScene _ovenAndStoveScene;
	#endregion

	ILoggerService _logger;

	public EnvironmentFactory()
	{
		_wallScene = (PackedScene)ResourceLoader.Load(WALL_SCENE_PATH);
		_tableScene = (PackedScene)ResourceLoader.Load(TABLE_SCENE_PATH);
		_cuttingBoardScene = (PackedScene)ResourceLoader.Load(CUTTING_BOARD_SCENE_PATH);
		_fridgeScene = (PackedScene)ResourceLoader.Load(FRIDGE_SCENE_PATH);
		_ovenAndStoveScene = (PackedScene)ResourceLoader.Load(OVEN_AND_STOVE_SCENE_PATH);
	}

	public override void _Ready()
	{
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

	public CuttingBoard SpawnCuttingBoard(Node parent, Vector2 position)
	{
		var result = _cuttingBoardScene.Instantiate<CuttingBoard>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}

	public Fridge SpawnFridge(Node parent, Vector2 position)
	{
		var result = _fridgeScene.Instantiate<Fridge>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}

	public OvenAndStove SpawnOvenAndStove(Node parent, Vector2 position)
	{
		var result = _ovenAndStoveScene.Instantiate<OvenAndStove>();
		parent.AddChild(result);
		result.Position = position;
		return result;
	}
}
