using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node2D
{
	[Export]
	int INIT_NUM_BOIDS = 10;

	#region Variables 
	List<Boid> BoidList = new List<Boid>();
	#endregion 

	#region Singletons
	IBoidFactory _boidFactory;
	#endregion

	public override void _Ready()
	{
		_boidFactory = GetNode<IBoidFactory>(Constants.SingletonNodes.BoidFactory);

		Vector2 centerOfViewport = GetViewportRect().Size / 2;

		for (int i = 0; i < INIT_NUM_BOIDS; i += 1)
		{
			_boidFactory.SpawnBoid(this, GetAlteredSpawnPoint(centerOfViewport));
		}
	}

	Vector2 GetAlteredSpawnPoint(Vector2 spawnPoint)
	{
		var r = new Random();
		var xDelta = 1;
		var yDelta = 1;
		var xRoll = r.NextDouble();
		var yRoll = r.NextDouble();
		var xPos = (xRoll * (xDelta * 2)) - xDelta;
		var yPos = (yRoll * (yDelta * 2)) - yDelta;

		return new Vector2(
			spawnPoint.X + (float)xPos,
			spawnPoint.Y + (float)yPos
		);
	}
}
