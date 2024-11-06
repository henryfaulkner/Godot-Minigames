using Godot;
using System;

public partial class ForegroundActionObservable : Node
{
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

}
