using Godot;

public partial class ForegroundSubject<TModel> : IForegroundSubject<TModel> where TModel : CreatureModel
{
	public CharacterBody3D CharacterBody3D { get; set; }
	public TModel Model { get; set; }
	public IExecuter Executer { get; set; }
	protected readonly ILoggerService _logger;

	public ForegroundSubject(ILoggerService logger)
	{
		_logger = logger;
	}

	public virtual void ReadyInstance(CharacterBody3D characterBody3D, TModel model)
	{
		_logger.LogDebug("Start ReadyInstance");
		_logger.LogDebug($"Initializing ForegroundSubject with model of type {typeof(TModel).Name}");
		CharacterBody3D = characterBody3D;
		Model = model;
		_logger.LogDebug("End ReadyInstance");
	}
}
