using Godot;
using System;
using System.Threading.Tasks;

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
			TriggerEffectAsync();
		}
	}

	public void ReadyInstance(Vector3 position)
	{
		Position = position;
	}

	private bool _isDebounced = false;
	public async void TriggerEffectAsync()
	{
		if (_isDebounced) return;
		_isDebounced = true;

		Hearts.Emitting = true;
		Hearts.Restart();

		TimingFunctions.SetTimeout(() => {
			_isDebounced = false;
		}, (int)Hearts.Lifetime * 1000);
	}
}
