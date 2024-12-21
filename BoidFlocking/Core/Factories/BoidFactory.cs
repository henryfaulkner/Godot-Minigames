using Godot;
using System;

public partial class BoidFactory : Node, IBoidFactory
{
	readonly StringName BOID_SCENE_PATH = new StringName("res://ObjectLibrary/Boid.tscn");

	readonly PackedScene _boidScene;

	public BoidFactory()
	{
		_boidScene = (PackedScene)ResourceLoader.Load(BOID_SCENE_PATH);
	}

	public Boid SpawnBoid(Node parent, Vector2 position)
	{
		var result = _boidScene.Instantiate<Boid>();
		parent.AddChild(result);
		result.GlobalPosition = position;
		return result;
	}    
}
