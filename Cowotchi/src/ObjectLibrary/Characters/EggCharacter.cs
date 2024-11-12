using Godot;
using System;
using System.Threading.Tasks;

public partial class EggCharacter : ForegroundSubject
{
	private EggModel Model { get; set; }
	
	public override IExecuter Executer { get; set; } 

	public void ReadyInstance(EggModel model)
	{
		_logger.LogDebug("Start EggCharacter ReadyInstance");
		try
		{
			Model = model;

			Executer = new EggExecuter(
				model,
				_logger
			);
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
