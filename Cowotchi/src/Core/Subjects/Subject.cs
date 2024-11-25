using Godot;

public partial class Subject<TModel> : ISubject<TModel> where TModel : CreatureModel
{
	public CharacterBody3D CharacterBody3D { get; set; }
	public TModel Model { get; set; }
	protected readonly ILoggerService _logger;

	public Subject(ILoggerService logger)
	{
		_logger = logger;
	}

	public virtual void ReadyInstance(CharacterBody3D characterBody3D, TModel model)
	{
		_logger.LogDebug("Start ReadyInstance");
		_logger.LogDebug($"Initializing Subject with model of type {typeof(TModel).Name}");
		CharacterBody3D = characterBody3D;
		Model = model;
		_logger.LogDebug("End ReadyInstance");
	}
}
