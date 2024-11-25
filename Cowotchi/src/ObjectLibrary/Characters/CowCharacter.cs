using Godot;
using System;
using System.Threading.Tasks;

public partial class CowCharacter : CharacterBody3D, ICharacter<CreatureModel>{
	public Subject<CreatureModel> Subject { get; set; }
	public CreatureModel Model { get; set; }

	private ILoggerService _logger { get; set; }
	private IAnimalInteractor _animalInteractor { get; set; } 
	private Observables _observables { get; set; }

	public void ReadyInstance(CreatureModel model)
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_logger.LogDebug("Start CowCharacter ReadyInstance");
		try
		{
			Model = model;

			Subject = new Subject<CreatureModel>(_logger);
			Subject.ReadyInstance(this, model);

			_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		} 
		catch (Exception ex)
		{
			_logger.LogError($"CowCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("End CowCharacter ReadyInstance");
	}
	 
	public async Task ReceiveLove() 
	{
		int increase = 1;

		_animalInteractor.NurtureAnimal(Model.Id);
		Model.LoveLevel += increase;
		_observables.EmitUpdateHeartMeterValue(Model.LoveLevel);
	} 

	public async Task Eat() 
	{
		int increase = 1;

		_animalInteractor.FeedAnimal(Model.Id);
		Model.StomachLevel += increase;
		_observables.EmitUpdateHungerMeterValue(Model.StomachLevel);
	} 
}
