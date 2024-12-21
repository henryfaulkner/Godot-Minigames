using Godot;
using System;
using System.Collections.Generic;

// Boid's flocking pattern follows 3 local rules:
// 1. Separation: Avoid getting too close to other agents.
// 2. Alignment: Move in the same general direction as nearby agents.
// 3. Cohesion: Move towards the average position of nearby agents.  
public partial class Boid : CharacterBody2D
{
	[ExportGroup("Variables")]
	[Export]
	float Speed = 5f;
	[Export]
	float NeighborRadius = 2f;
	[Export]
	float SepatationDistance = 1f;

	[ExportGroup("Nodes")]
	[Export]
	Area2D Area2D { get; set; }
	[Export]
	CollisionShape2D CollisionShape2D { get; set; }
	[Export]
	MeshInstance2D MeshInstance2D { get; set; }

	public override void _Ready()
	{
		// Initialize random movement direction
		Velocity = Random_InsideUnitCircle();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 alignment = Align();
		Vector2 cohesion = Cohere();
		Vector2 separation = Separate();

		// Combine all forces
		Velocity += alignment + cohesion + separation;
		Velocity = Velocity.Normalized() * Speed;		

		// Move the agent
		MoveAndSlide();

		// Rotate to face movement direction
		float angleInRads = Mathf.Atan2(Velocity.Y, Velocity.X);
		GlobalRotation = angleInRads;
	}

	// Steer toward the average heading of nearby agents
	private Vector2 Align()
	{
		Vector2 result = Vector2.Zero;

		var boidNeighborList = GetBoidNeighbors();
		foreach (var boidNeighbor in boidNeighborList)
		{
			result += boidNeighbor.Velocity;
		}

		var count = boidNeighborList.Count; 
		if (count > 0)
		{
			result /= count;
			result = result.Normalized();
		}
		
		return result;
	}
	
	// Move towards the center of nearby agents
	private Vector2 Cohere()
	{
		Vector2 result = Vector2.Zero;

		var boidNeighborList = GetBoidNeighbors();
		foreach (var boidNeighbor in boidNeighborList)
		{
			result += boidNeighbor.GlobalPosition;
		}

		var count = boidNeighborList.Count; 
		if (count > 0)
		{
			result /= count;
			result = (result - GlobalPosition).Normalized();
		}
		
		return result;
	}
	
	// Avoid crowding nearby agents
	private Vector2 Separate()
	{
		Vector2 result = Vector2.Zero;

		var boidNeighborList = GetBoidNeighbors();
		foreach (var boidNeighbor in boidNeighborList)
		{
			float distance = GlobalPosition.DistanceTo(boidNeighbor.GlobalPosition);
			if (distance < SepatationDistance)
			{
				result += GlobalPosition - boidNeighbor.GlobalPosition;
			}
		}

		var count = boidNeighborList.Count; 
		if (count > 0)
		{
			result /= count;
			result = result.Normalized();
		}
		
		return result;
	}

	private List<Boid> GetBoidNeighbors()
	{
		var result = new List<Boid>();
		
		var overlappingBodies = Area2D.GetOverlappingBodies();
		foreach (var body in overlappingBodies)
		{
			if (body is Boid boid && boid != this)
			{
				result.Add(boid);
			}
		}

		return result;
	}

	private Vector2 Random_InsideUnitCircle()
	{
		Vector2 vec = Vector2.Right * GD.RandRange(0, 100);
		return vec.Rotated((float)GD.RandRange(0, Mathf.Pi));
	}
}
