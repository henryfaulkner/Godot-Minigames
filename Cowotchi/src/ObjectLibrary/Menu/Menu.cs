using Godot;
using System;

public partial class Menu : CanvasLayer
{
	[ExportGroup("Meters")]
	[Export]
	public Meter LoveMeter { get; set; }
	[Export]
	public Meter StomachMeter { get; set; }

	private MeterObservable _meterObservable;

	public override void _Ready()
	{
		_meterObservable = GetNode<MeterObservable>(Constants.SingletonNodes.MeterObservable);

		_meterObservable.UpdateHeartMeterValue += LoveMeter.UpdateValue;
		_meterObservable.UpdateHeartMeterMax += LoveMeter.UpdateMax;

		_meterObservable.UpdateHungerMeterValue += StomachMeter.UpdateValue;
		_meterObservable.UpdateHungerMeterMax += StomachMeter.UpdateMax;
	}
}
