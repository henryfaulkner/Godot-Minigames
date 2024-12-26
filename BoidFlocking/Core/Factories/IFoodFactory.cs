using Godot;

public interface IFoodFactory
{
	Food SpawnFood(Node parent, Vector2 position);
}
