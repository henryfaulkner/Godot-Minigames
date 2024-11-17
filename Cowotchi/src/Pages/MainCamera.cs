// This video tutorial was helpful for the ray-casting
// https://www.youtube.com/watch?v=mJRDyXsxT9g&t=2s

using Godot;
using System;

public partial class MainCamera : Camera3D
{
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
	}
	
	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("click"))
		{
			var hitNode = ShootRay();
			if (hitNode == null) return;
			if (hitNode.Name.ToString().Contains("BgEgg"))
			{
				_logger.LogError($"Grab Egg");
				_observables.EmitGrabEgg(hitNode.GetInstanceId());
			}
		}
	}
	
	public override void _PhysicsProcess(double _delta)
	{
		var hitNode = ShootRay();
		if (hitNode == null) return;
		if (hitNode.Name.ToString().Contains("BgEgg"))
		{
			_logger.LogError($"Show Grab Egg");
		}
	}
	
	public Node? ShootRay()
	{
		Node? result = null;
		
		var mousePos = GetViewport().GetMousePosition();
		var rayLen = 1000;
		var from = ProjectRayOrigin(mousePos);
		var to = from + ProjectRayNormal(mousePos) * rayLen;
		var space = GetWorld3D().DirectSpaceState;
		var rayQuery = new PhysicsRayQueryParameters3D();
		rayQuery.From = from;
		rayQuery.To = to;
		var castResponse = space.IntersectRay(rayQuery);
		
		if (castResponse.ContainsKey("collider"))
		{
			result = (Node3D)castResponse["collider"];
		}
		
		return result;
	}
}
