using Godot;
using System;
using System.Threading.Tasks;

public partial class BgEggCharacter : BgEggController, ICharacter<CreatureModel>
{
	public Subject<CreatureModel> Subject { get; set; }
	public CreatureModel Model { get; set; }
	
	private Observables _observables { get; set; }

	public void ReadyInstance(CreatureModel model)
	{
		_logger.LogDebug("Start BgEggController ReadyInstance");
		try
		{
			Model = model;

			Subject = new Subject<CreatureModel>(_logger);
			Subject.ReadyInstance(this, model);

			_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		}
		catch (Exception ex)
		{
			_logger.LogError($"BgEggCharacter ReadyInstance Error: {ex.Message}", ex);
		}
		_logger.LogDebug("Start BgEggController ReadyInstance");
	}

	public async Task ExecuteActionAsync(Enumerations.CharacterActions action)
	{
		switch (action)
		{
			case Enumerations.CharacterActions.Hatch:
				Hatch();
				break;
			default:
				break;
		}
	}

	public async Task Hatch() 
	{
		throw new NotImplementedException();
	} 
}
