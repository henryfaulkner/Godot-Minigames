using Godot;
using System;
using System.Threading.Tasks;

public partial class StatsCommand : Command
{    
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
	}

	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		_observables.EmitStats();
		return true;
	}
}
