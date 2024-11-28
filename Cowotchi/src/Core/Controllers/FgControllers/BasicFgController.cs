using Godot;
using System;

public partial class BasicFgController : Node, IController
{
	private CharacterBody3D Puppet { get; set; }
	private CollisionShape3D Collider { get; set; }
	private MeshInstance3D Mesh { get; set; }

	protected ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public void SetPuppet(CharacterBody3D puppet)
	{
		Puppet = puppet;
	}

	public void ReadyInstance(CollisionShape3D collider, MeshInstance3D mesh)
	{
		Collider = collider;
		Mesh = mesh;
	}

	public void PhysicsProcess(double delta)
	{
	}
}
