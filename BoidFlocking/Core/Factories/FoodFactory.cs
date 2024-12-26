using Godot;
using System;

public partial class FoodFactory : Node, IFoodFactory
{
	readonly StringName FOOD_SCENE_PATH = new StringName("res://ObjectLibrary/Food/Food.tscn");

	readonly PackedScene _foodScene;

	public FoodFactory()
	{
		_foodScene = (PackedScene)ResourceLoader.Load(FOOD_SCENE_PATH);
	}

	public Food SpawnFood(Node parent, Vector2 position)
	{
		var result = _foodScene.Instantiate<Food>();
		parent.AddChild(result);
		result.GlobalPosition = position;
		return result;
	}    
}
