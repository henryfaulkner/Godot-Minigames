using Godot;
using System;
using System.Threading.Tasks;

public partial class AnimalPage : Control, ICommander
{
	[ExportGroup("Action Buttons")]
	[Export]
	public ActionButton Stats { get; set; }
	[Export]
	public ActionButton Swap { get; set; }
	[Export]
	public ActionButton Nurture { get; set; }
	[Export]
	public ActionButton Feed { get; set; } 
	
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	public CommandInvoker _invoker { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

		_invoker = new CommandInvoker(_logger);
		_invoker.RegisterCommand(Enumerations.Commands.Stats, new StatsCommand());
		_invoker.RegisterCommand(Enumerations.Commands.Swap, new SwapCommand());
		_invoker.RegisterCommand(Enumerations.Commands.Nurture, new NurtureCommand());
		_invoker.RegisterCommand(Enumerations.Commands.Feed, new FeedCommand());

		Stats.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Stats);
		Swap.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Swap);
		Nurture.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Nurture);
		Feed.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Feed);
	}

	public async Task<bool> ExecuteCommandAsync(Enumerations.Commands command)
	{
		return await _invoker.ExecuteCommandAsync(command);
	}

	private void HandleStatsPressed()
	{
		_logger.LogDebug("Call Menu HandleStatsPressed");
		_invoker.ExecuteCommandAsync(Enumerations.Commands.Stats);
	}

	private void HandleSwapPressed()
	{
		_invoker.ExecuteCommandAsync(Enumerations.Commands.Swap);
	}

	private void HandleNurturePressed()
	{
		_logger.LogDebug("Call Menu HandleNurturePressed");
		ForegroundCharacter.ExecuteCommandAsync(Enumerations.Commands.Nurture);
	}

	private void HandleFeedPressed()
	{
		_logger.LogDebug("Call Menu HandleFeedPressed");
		ForegroundCharacter.ExecuteCommandAsync(Enumerations.Commands.Feed);
	}
}
