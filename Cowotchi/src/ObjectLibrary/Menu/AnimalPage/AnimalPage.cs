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
	private CommandFactory _commandFactory { get; set; }
	public CommandInvoker _invoker { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		_commandFactory = GetNode<CommandFactory>(Constants.SingletonNodes.CommandFactory);

		_invoker = new CommandInvoker(_logger);
		_invoker.RegisterCommand(Enumerations.Commands.Stats, _commandFactory.SpawnStatsCommand());
		_invoker.RegisterCommand(Enumerations.Commands.Swap, _commandFactory.SpawnSwapCommand());
		_invoker.RegisterCommand(Enumerations.Commands.Nurture, _commandFactory.SpawnNurtureCommand());
		_invoker.RegisterCommand(Enumerations.Commands.Feed, _commandFactory.SpawnFeedCommand());

		Stats.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Stats);
		Swap.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Swap);
		Nurture.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Nurture);
		Feed.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Feed);
	}

	public async Task<bool> ExecuteCommandAsync(Enumerations.Commands command)
	{
		_logger.LogInfo($"AnimalPage ExecuteCommandAsync {command.GetDescription()}");
		return await _invoker.ExecuteCommandAsync(command);
	}
}
