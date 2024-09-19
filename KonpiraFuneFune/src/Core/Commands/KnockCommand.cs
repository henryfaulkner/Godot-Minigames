using Godot;

public partial class KnockCommand : Node, ICommand
{
	private readonly AbstractCommander _commander;
	private readonly CommandService _serviceCommand;

	public KnockCommand(AbstractCommander commander, CommandService serviceCommand)
	{
		_commander = commander;
		_serviceCommand = serviceCommand;
	}

	public void Execute()
	{
		if (_serviceCommand == null)
		{
			GD.Print("KnockCommand: Service Command is null");
			return;
		}

		if (_serviceCommand.BowlState == Enumerations.BowlStates.Carrying
			&& _serviceCommand.AreBowlCarrierAndCommanderEqual(_commander))
		{
			_serviceCommand.EmitDetachBowlFromHandSignal(_commander.Id);
			_serviceCommand.BowlState = Enumerations.BowlStates.Resting;
			_serviceCommand.BowlCarrier = _commander;
		}
		else
		{
			_serviceCommand.EmitPlayerLostSignal(_commander.Id);
		}
	}
}
