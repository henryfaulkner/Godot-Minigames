using Godot;

public partial class ForegroundSubject : CharacterBody3D, IForegroundSubject
{
	public IExecuter Executer { get; set; }

	protected ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}
}
