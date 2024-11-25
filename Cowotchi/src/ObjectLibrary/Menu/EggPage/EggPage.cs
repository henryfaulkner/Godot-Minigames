using Godot;
using System;
using System.Threading.Tasks;

public partial class EggPage : Control
{
	[ExportGroup("Action Buttons")]
	[Export]
	public ActionButton Stats { get; set; }
	[Export]
	public ActionButton Swap { get; set; }
	
	private ILoggerService _logger { get; set; }
	public CommandInvoker _invoker { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		
		_invoker = new CommandInvoker(_logger);
		_invoker.RegisterCommand(Enumerations.Commands.Stats);
		_invoker.RegisterCommand(Enumerations.Commands.Swap);

		Stats.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Stats);
		Swap.Pressed += () => ExecuteCommandAsync(Enumerations.Commands.Swap);
	}

	public async Task<bool> ExecuteCommandAsync(Enumerations.Commands command)
	{
		return await _invoker.ExecuteCommandAsync(command);
	}

	private void HandleStatsPressed()
	{
		_invoker.ExecuteCommandAsync(Enumerations.Commands.Stats);
	}

	private void HandleSwapPressed()
	{
		_invoker.ExecuteCommandAsync(Enumerations.Commands.Swap);
	}
}
