using Godot;
using System;

public interface 

public partial class ToolFactory : Node
{
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

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		
		_cuttingBoardScene = (PackedScene)ResourceLoader.Load(CUTTING_BOARD_SCENE_PATH);
		_fridgeScene = (PackedScene)ResourceLoader.Load(FRIDGE_SCENE_PATH);
		_ovenAndStoveScene = (PackedScene)ResourceLoader.Load(OVEN_AND_STOVE_SCENE_PATH);
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
