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
	public delegate void AttachBowlToHandEventHandler();
	public void EmitAttachBowlToHandSignal()
	{
		GD.Print("Call EmitAttachBowlToHandSignal");
		EmitSignal(SignalName.AttachBowlToHand);
	}

	[Signal]
	public delegate void DetachBowlFromHandEventHandler();
	public void EmitDetachBowlFromHandSignal()
	{
		GD.Print("Call EmitDetachBowlFromHandSignal");
		EmitSignal(SignalName.DetachBowlFromHand);
	}

	public bool AreBowlCarrierAndCommanderEqual(AbstractCommander commander)
	{
		if (BowlCarrier == null) return false;
		return BowlCarrier.Id == commander.Id;
	}

	[Signal]
	public delegate void PlayerLostEventHandler(int playerId);
	public void EmitPlayerLostSignal(int playerId)
	{
		GD.Print("Call EmitPlayerLostSignal");
		EmitSignal(SignalName.PlayerLost, playerId);
	}
}
