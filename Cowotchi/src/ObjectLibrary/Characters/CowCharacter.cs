using Godot;
using System;
using System.Threading.Tasks;

public partial class CowCharacter : ForegroundModel
{
	private Animal Animal { get; set; }
	
	private ILoggerService _logger { get; set; }

	public IExecuter Executer { get; set; }

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}

	public void InitAnimal(Animal animal)
	{
		Animal = animal;

		Executer = new AnimalExecuter(
			animal,
			_logger,
			nurtureCallback: () => ReceiveLove(),
			feedCallback: () => Eat()
		);
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
