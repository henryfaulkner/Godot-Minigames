using Godot;
using System;
using System.Threading.Tasks;

public partial class EggController : CharacterBody3D, IEggController, IExecuter
{
	public Egg Modal { get; set; } 

	private ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}

	public async Task Bounce() 
	{
		throw new NotImplementedException();
	}

	public async Task Hatch() 
	{
		throw new NotImplementedException();
	}  

	public Task ExecuteAction(Enumerations.ForegroundActions menuAction)
	{
		switch(menuAction)
		{
			case Enumerations.ForegroundActions.Stats:
				throw new NotImplementedException();	
				break;
			case Enumerations.ForegroundActions.Swap:
				throw new NotImplementedException();	
				break;
			case Enumerations.ForegroundActions.Nurture:
				throw new NotImplementedException();	
				break;
			case Enumerations.ForegroundActions.Feed:
				throw new NotImplementedException();
				break;
			default:
				_logger.LogError($"EggController ExecuteAction failed to map state. Egg name: {Modal.Name}.");
				throw new Exception($"EggInteractor ExecuteAction failed to map state. Egg name: {Modal.Name}.");
				break;
				break;
		}
	}  
}
