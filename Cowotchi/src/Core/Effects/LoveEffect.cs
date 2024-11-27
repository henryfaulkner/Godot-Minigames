using Godot;
using System;

public partial class LoveEffect : RigidBody3D
{
	[ExportGroup("Particle Effects")]
	[Export]
	private GpuParticles3D Hearts { get; set; }
	private ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		Hearts.OneShot = true;
		Hearts.Emitting = false;
		
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}
	
	public override void _Input(InputEvent @event)
	{
		// Handle ESC input
		if (@event.IsActionPressed("space")) 
		{
			_logger.LogInfo("Handle Space Pressed");
			TriggerEffect();
		}
	}

	public void ReadyInstance(Vector3 position)
	{
		Position = position;
	}

	public void TriggerEffect()
	{
		_logger.LogInfo("Call TriggerEffect");
		_logger.LogInfo($"Effect Position: {Position.ToString()}");
		Hearts.Emitting = true;
	}
}
