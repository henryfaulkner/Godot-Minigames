using Godot;
using System;

public partial class Observables: Node
{
	#region Foreground Actions
	[Signal]
	public delegate void StatsEventHandler();
	public void EmitStats()
	{
		EmitSignal(SignalName.Stats);
	}

	[Signal]
	public delegate void SwapEventHandler();
	public void EmitSwap()
	{
		EmitSignal(SignalName.Swap);
	}

	[Signal]
	public delegate void NurtureEventHandler();
	public void EmitNurture()
	{
		EmitSignal(SignalName.Nurture);
	}


	[Signal]
	public delegate void FeedEventHandler();
	public void EmitFeed()
	{
		EmitSignal(SignalName.Feed);	
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

	#region Label Actions
	[Signal]
	public delegate void UpdateSubjectNameLabelEventHandler(string text); 
	public void EmitUpdateSubjectNameLabel(string text)
	{
		EmitSignal(SignalName.UpdateSubjectNameLabel, text);
	}
	#endregion

	#region Info Container Actions
	[Signal]
	public delegate void UpdateCurrentCreatureInfoEventHandler();
	public void EmitUpdateCurrentCreatureInfo()
	{
		EmitSignal(SignalName.UpdateCurrentCreatureInfo);
	}
	#endregion
}
