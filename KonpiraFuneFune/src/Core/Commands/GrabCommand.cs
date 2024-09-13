using Godot;

public partial class GrabCommand : Node, ICommand
{
	private readonly AbstractCommander _commander;
	private readonly CommandService _serviceCommand;

	public GrabCommand(AbstractCommander commander, CommandService serviceCommand)
	{
		_commander = commander;
		_serviceCommand = serviceCommand;
	}

	public void Execute()
	{
		if (_serviceCommand == null)
		{
			GD.Print("GrabCommand: Service Command is null");
			return;
		}

		if (_serviceCommand.BowlState == Enumerations.BowlStates.Resting)
		{
			_serviceCommand.EmitAttachBowlToHandSignal(_commander.Id);
			_serviceCommand.BowlState = Enumerations.BowlStates.Carrying;
			_serviceCommand.BowlCarrier = _commander;
		}
		else if (_serviceCommand.BowlState == Enumerations.BowlStates.Carrying
			&& !_serviceCommand.AreBowlCarrierAndCommanderEqual(_commander))
		{
			_serviceCommand.EmitPlayerLostSignal(_commander.Id);
		}
	}
}
