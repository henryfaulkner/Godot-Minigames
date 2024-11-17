using Godot;
using System;

public partial class MeterObservable : Node
{
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
}