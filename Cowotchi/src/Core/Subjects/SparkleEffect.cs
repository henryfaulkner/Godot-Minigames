using Godot;
using System;

public partial class SparkleEffect : RigidBody3D
{
	[ExportGroup("Particle Effects")]
	[Export]
	private GpuParticles3D Sparkles { get; set; }
	private ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		Sparkles.OneShot = false;
		Sparkles.Emitting = true;
		
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}
	
	public void ReadyInstance(Vector3 position)
	{
		Position = position;
	}
}
