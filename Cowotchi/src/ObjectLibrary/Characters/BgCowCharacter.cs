using Godot;
using System;

public partial class BgCowCharacter : BgAnimalController, ICharacterWithBackgroundSubject<CreatureModel>
{
	public BackgroundSubject<CreatureModel> BackgroundSubject { get; set; }
	public CreatureModel Model { get; set; }
	
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }

	public void ReadyInstance(CreatureModel model)
	{
		_logger.LogDebug("Start BgCowController ReadyInstance");
		try
		{
			BackgroundSubject = new BackgroundSubject<CreatureModel>(_logger);
			BackgroundSubject.ReadyInstance(this, model);

			Model = model;
			
			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		}
		catch (Exception ex)
		{
			_logger.LogError($"BgCowCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("Start BgCowController ReadyInstance");
	}
}
