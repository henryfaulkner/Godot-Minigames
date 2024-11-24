using Godot;
using System;
using System.Threading.Tasks;

public partial class EggCharacter : CharacterBody3D, ICharacterWithForegroundSubject<CreatureModel>
{
	public ForegroundSubject<CreatureModel> ForegroundSubject { get; set; }
	public CreatureModel Model { get; set; }
	public IExecuter Executer { get; set; }
	
	private ILoggerService _logger { get; set; }
	private IEggInteractor _eggInteractor { get; set; } 
	private Observables _observables { get; set; }

	public void ReadyInstance(CreatureModel model)
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_logger.LogDebug("Start EggCharacter ReadyInstance");
		try
		{
			Model = model;

			Executer = new EggExecuter(
				model,
				_logger
			);

			ForegroundSubject = new ForegroundSubject<CreatureModel>(_logger);
			ForegroundSubject.ReadyInstance(this, model, Executer);

			_eggInteractor = GetNode<IEggInteractor>(Constants.SingletonNodes.EggInteractor);
			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		} 
		catch (Exception ex)
		{
			_logger.LogError($"EggCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("End EggCharacter ReadyInstance");
	}

	public async Task Hatch() 
	{
		throw new NotImplementedException();
	} 
}
