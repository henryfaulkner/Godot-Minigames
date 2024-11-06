using Godot; 
using System;
using System.Threading.Tasks;

public partial class CowController : CharacterBody3D, IAnimalController, IExecuter
{
	public Animal Modal { get; set; } 

	private ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}

	public async Task Hatch() {}
	public async Task ReceiveLove() {}
	public async Task Eat() {}

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
				_logger.LogError($"CowController ExecuteAction failed to map state. Cow name: {Modal.Name}.");
				throw new Exception($"CowInteractor ExecuteAction failed to map state. Cow name: {Modal.Name}.");
				break;
				break;
		}
	}  
}
