using Godot;
using System.Threading.Tasks;

public partial class EggController : CharacterBody3D, IEggController, IExecuter
{
	[Export]
	public AnimalController InnerAnimal { get; set; }

	public Egg Modal { get; set; } 

	private ILoggerService _logger { get; set; }

    public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}

	public async Task Bounce() {}

	public async Task Hatch() 
	{
		InnerAnimal.Hatch();
	}  

	Task ExecuteAction(Enumerations.ForegroundActions menuAction)
	{
		switch(menuAction)
		{
			case Enumeration.ForegroundActions.Stats:
				break;
			case Enumeration.ForegroundActions.Swap:
				break;
			case Enumeration.ForegroundActions.Nurture:
				break;
			case Enumeration.ForegroundActions.Feed:
				break;
			default:
				_logger.LogError($"EggController ExecuteAction failed to map state. Egg name: {Modal.Name}.");
				throw new Exception($"EggInteractor ExecuteAction failed to map state. Egg name: {Modal.Name}.");
				break;
				break;
		}
	}  
}
