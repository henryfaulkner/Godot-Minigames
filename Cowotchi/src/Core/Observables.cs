using Godot;
using System;

public partial class Observables: Node
{
	#region Foreground Actions
	[Signal]
	public delegate void StatsPressedEventHandler();
	public void EmitStatsPressed()
	{
		EmitSignal(SignalName.StatsPressed);
	}

	[Signal]
	public delegate void SwapPressedEventHandler();
	public void EmitSwapPressed()
	{
		GD.PrintErr("Call EmitSwapPressed");
		EmitSignal(SignalName.SwapPressed);
	}

	[Signal]
	public delegate void NurturePressedEventHandler();
	public void EmitNurturePressed()
	{
		EmitSignal(SignalName.NurturePressed);
	}


	[Signal]
	public delegate void FeedPressedEventHandler();
	public void EmitFeedPressed()
	{
		EmitSignal(SignalName.FeedPressed);	
	}
	#endregion

	#region Meter Actions
	[Signal]
	public delegate void UpdateHeartMeterValueEventHandler(int value);
	public void EmitUpdateHeartMeterValue(int value)
	{
		EmitSignal(SignalName.UpdateHeartMeterValue, value);
	}

	[Signal]
	public delegate void UpdateHeartMeterMaxEventHandler(int max);
	public void EmitUpdateHeartMeterMax(int max)
	{
		EmitSignal(SignalName.UpdateHeartMeterMax, max);
	}

	[Signal]
	public delegate void UpdateHungerMeterValueEventHandler(int value);
	public void EmitUpdateHungerMeterValue(int value)
	{
		EmitSignal(SignalName.UpdateHungerMeterValue, value);
	}

	[Signal]
	public delegate void UpdateHungerMeterMaxEventHandler(int max);
	public void EmitUpdateHungerMeterMax(int max)
	{
		EmitSignal(SignalName.UpdateHungerMeterMax, max);
	}
	#endregion

	#region Raycast Actions
	[Signal]
	public delegate void GrabEggEventHandler(ulong instanceId);
	public void EmitGrabEgg(ulong instanceId)
	{
		EmitSignal(SignalName.GrabEgg, instanceId);
	}
	#endregion

	#region Labels
	[Signal]
	public delegate void UpdateSubjectNameLabelEventHandler(string text); 
	public void EmitUpdateSubjectNameLabel(string text)
	{
		EmitSignal(SignalName.UpdateSubjectNameLabel, text);
	}
	#endregion
}
