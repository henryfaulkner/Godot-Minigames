using Godot;
using System;

public partial class BgEggCharacter : BgEggController, ICharacterWithBackgroundSubject<CreatureModel>
{
	public BackgroundSubject<CreatureModel> BackgroundSubject { get; set; }
	public CreatureModel Model { get; set; }
	
	private Observables _observables { get; set; }

	public void ReadyInstance(CreatureModel model)
	{
		_logger.LogDebug("Start BgEggController ReadyInstance");
		try
		{
			_logger.LogInfo("7.35");
			BackgroundSubject = new BackgroundSubject<CreatureModel>(_logger);
			BackgroundSubject.ReadyInstance(this, model);

			Model = model;
			
			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		}
		catch (Exception ex)
		{
			_logger.LogError($"BgEggCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("Start BgEggController ReadyInstance");
	}
}
