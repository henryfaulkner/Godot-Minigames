using System;
using System.Threading.Tasks;

public class EggExecuter : IExecuter
{
	private CreatureModel Model { get; set; }
	private ILoggerService _logger { get; set; }

	private Action? StatsCallback { get; set; }
	private Action? SwapCallback { get; set; }
	private Action? NurtureCallback { get; set; }
	private Action? FeedCallback { get; set; }

	public EggExecuter(
		CreatureModel model, 
		ILoggerService logger,
		Action? statsCallback = null, 
		Action? swapCallback = null,
		Action? nurtureCallback = null,
		Action? feedCallback = null)
	{
		Model = model;
		_logger = logger;
		StatsCallback = statsCallback;
		SwapCallback = swapCallback;
		NurtureCallback = nurtureCallback;
		FeedCallback = feedCallback;
	}

	public async Task ExecuteAction(Enumerations.ForegroundActions menuAction)
	{
		switch(menuAction)
		{
			case Enumerations.ForegroundActions.Stats:
				if (StatsCallback != null) StatsCallback();	
				break;
			case Enumerations.ForegroundActions.Swap:
				if (SwapCallback != null) SwapCallback();
				break;
			case Enumerations.ForegroundActions.Nurture:
				if (NurtureCallback != null) NurtureCallback();
				break;
			case Enumerations.ForegroundActions.Feed:
				if (FeedCallback != null) FeedCallback();
				break;
			default:
				_logger.LogError($"EggController ExecuteAction failed to map state. Egg name: {Model.Name}.");
				throw new Exception($"EggInteractor ExecuteAction failed to map state. Egg name: {Model.Name}.");
				break;
				break;
		}
	}  
}
