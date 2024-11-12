using Godot;
using System;
using System.Threading.Tasks;

public partial class CowCharacter : ForegroundSubject
{
	private AnimalModel Model { get; set; }

	public override IExecuter Executer { get; set; }

	public void ReadyInstance(AnimalModel model)
	{
		_logger.LogDebug("Start CowCharacter ReadyInstance");
		try
		{
			Model = model;

			Executer = new AnimalExecuter(
				model,
				_logger,
				nurtureCallback: () => ReceiveLove(),
				feedCallback: () => Eat()
			);
			_logger.LogDebug($"Executer == null {Executer == null}");
		} 
		catch (Exception ex)
		{
			_logger.LogError($"CowCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("End CowCharacter ReadyInstance");
	}

	public async Task Hatch() 
	{
		throw new NotImplementedException();
	}
	 
	public async Task ReceiveLove() 
	{
		throw new NotImplementedException();
	} 

	public async Task Eat() 
	{
		throw new NotImplementedException();
	} 
}
