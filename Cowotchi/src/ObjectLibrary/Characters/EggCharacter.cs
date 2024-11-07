using Godot;
using System;
using System.Threading.Tasks;

public partial class EggCharacter : CharacterBody3D, IExecuter
{
	public Egg Egg { get; set; }
	
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
				_logger.LogError($"EggController ExecuteAction failed to map state. Egg name: {Egg.Name}.");
				throw new Exception($"EggInteractor ExecuteAction failed to map state. Egg name: {Egg.Name}.");
				break;
				break;
		}
	}  

	public async Task Hatch() 
	{
		throw new NotImplementedException();
	} 
}
