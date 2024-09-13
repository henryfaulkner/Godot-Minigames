using Godot;

public partial class CommandService : Node
{
	public Enumerations.BowlStates BowlState { get; set; }
	public AbstractCommander? BowlCarrier { get; set; }

	public void Init()
	{
		BowlState = Enumerations.BowlStates.Resting;
		BowlCarrier = null;
	}

	[Signal]
	public delegate void AttachBowlToHandEventHandler(int commanderId);
	public void EmitAttachBowlToHandSignal(int commanderId)
	{
		GD.Print("Call EmitAttachBowlToHandSignal");
		EmitSignal(SignalName.AttachBowlToHand, commanderId);
	}

	[Signal]
	public delegate void DetachBowlFromHandEventHandler(int commanderId);
	public void EmitDetachBowlFromHandSignal(int commanderId)
	{
		GD.Print("Call EmitDetachBowlFromHandSignal");
		EmitSignal(SignalName.DetachBowlFromHand, commanderId);
	}

	public bool AreBowlCarrierAndCommanderEqual(AbstractCommander commander)
	{
		if (BowlCarrier == null) return false;
		return BowlCarrier.Id == commander.Id;
	}

	[Signal]
	public delegate void PlayerLostEventHandler(int commanderId);
	public void EmitPlayerLostSignal(int commanderId)
	{
		GD.Print("Call EmitPlayerLostSignal");
		EmitSignal(SignalName.PlayerLost, commanderId);
	}
}
