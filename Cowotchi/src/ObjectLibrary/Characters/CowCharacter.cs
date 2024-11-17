using Godot;
using System;
using System.Threading.Tasks;

public partial class CowCharacter : ForegroundSubject
{
	private AnimalModel Model { get; set; }

	public override IExecuter Executer { get; set; }
	
	private IAnimalInteractor _animalInteractor { get; set; } 
	private MeterObservable _meterObservable { get; set; }

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

			_animalInteractor = GetNode<IAnimalInteractor>(Constants.SingletonNodes.AnimalInteractor);
			_meterObservable = GetNode<MeterObservable>(Constants.SingletonNodes.MeterObservable);
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
		int increase = 1;

		_animalInteractor.NurtureAnimal(Model.Id);
		Model.LoveLevel += increase;
		_meterObservable.EmitUpdateHeartMeterValue(Model.LoveLevel);
	} 

	public async Task Eat() 
	{
		int increase = 1;

		_animalInteractor.FeedAnimal(Model.Id);
		Model.StomachLevel += increase;
		_meterObservable.EmitUpdateHungerMeterValue(Model.StomachLevel);
	} 
}
