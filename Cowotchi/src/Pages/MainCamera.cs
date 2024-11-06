// This video tutorial was helpful for the ray-casting
// https://www.youtube.com/watch?v=mJRDyXsxT9g&t=2s

using Godot;
using System;

public partial class MainCamera : Camera3D
{
	private ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}
	
	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("click"))
		{
			ShootRay();
		}
	}
	
	public void ShootRay()
	{
		var mousePos = GetViewport().GetMousePosition();
		var rayLen = 1000;
		var from = ProjectRayOrigin(mousePos);
		var to = from + ProjectRayNormal(mousePos) * rayLen;
		var space = GetWorld3D().DirectSpaceState;
		var rayQuery = new PhysicsRayQueryParameters3D();
		rayQuery.From = from;
		rayQuery.To = to;
		var result = space.IntersectRay(rayQuery);
		_logger.LogError($"Raycast result: {result.ToString()}");
	}
}
