using Godot;
using System;

public partial class MeterObservable : Node
{
    [Signal]
    public delegate void UpdateHeartMeterValueEventHandler(float value);
    public void EmitUpdateHeartMeterValue(float value)
    {
        EmitSignal(SignalName.UpdateHeartMeterValue, value);
    }

    [Signal]
    public delegate void UpdateHeartMeterMaxEventHandler(float max);
    public void EmitUpdateHeartMeterMax(float max)
    {
        EmitSignal(SignalName.UpdateHeartMeterMax, max);
    }

    [Signal]
    public delegate void UpdateHungerMeterValueEventHandler(float value);
    public void EmitUpdateHungerMeterValue(float value)
    {
        EmitSignal(SignalName.UpdateHungerMeterValue, value);
    }

    [Signal]
    public delegate void UpdateHungerMeterMaxEventHandler(float max);
    public void EmitUpdateHungerMeterMax(float max)
    {
        EmitSignal(SignalName.UpdateHungerMeterMax, max);
    }
}