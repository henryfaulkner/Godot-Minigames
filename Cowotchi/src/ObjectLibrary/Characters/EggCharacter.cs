using Godot;
using System;
using System.Threading.Tasks;

public partial class EggCharacter : ForegroundModel
{
	private Egg Egg { get; set; }
	
	private ILoggerService _logger { get; set; }

	public IExecuter Executer { get; set; } 

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}

	public void InitEgg(Egg egg)
	{
		Egg = egg;

		Executer = new EggExecuter(
			egg,
			_logger
		);
	}

	public async Task Hatch() 
	{
		throw new NotImplementedException();
	} 
}
