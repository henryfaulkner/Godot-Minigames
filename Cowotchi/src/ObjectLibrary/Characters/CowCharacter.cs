using Godot;
using System;
using System.Threading.Tasks;

public partial class CowCharacter : CharacterBody3D, IExecuter
{
	public Animal Animal { get; set; }
	
	private ILoggerService _logger { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}

	public async Task ExecuteAction(Enumerations.ForegroundActions menuAction)
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
				_logger.LogError($"CowController ExecuteAction failed to map state. Cow name: {Animal.Name}.");
				throw new Exception($"CowInteractor ExecuteAction failed to map state. Cow name: {Animal.Name}.");
				break;
				break;
		}
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
