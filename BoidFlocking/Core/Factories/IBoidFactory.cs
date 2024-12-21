using Godot;

public interface IBoidFactory
{
	Boid SpawnBoid(Node parent, Vector2 position);
}
