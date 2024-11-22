using Godot;

public partial class BackgroundSubject : CharacterBody3D, IBackgroundSubject
{
	public virtual CreatureModel Model { get; set; }
	protected ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}
}
