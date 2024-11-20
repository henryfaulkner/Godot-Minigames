using Godot;
using System;

public partial class BgEggCharacter : BgEggController
{
	public EggModel Model { get; set; }

	public void ReadyInstance(EggModel model)
	{
		_logger.LogDebug("Start BgEggController ReadyInstance");
		try
		{
			Model = model;
		}
		catch (Exception ex)
		{
			_logger.LogError($"BgEggCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("Start BgEggController ReadyInstance");
	}
}
