using Godot;
using System;

public partial class BgCowCharacter : BgAnimalController
{
	public AnimalModel Model { get; set; }

	public void ReadyInstance(AnimalModel model)
	{
		_logger.LogDebug("Start BgCowController ReadyInstance");
		try
		{
			Model = model;
		}
		catch (Exception ex)
		{
			_logger.LogError($"BgCowCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("Start BgCowController ReadyInstance");
	}
}
