using Godot;
using System;

public partial class Menu : CanvasLayer
{
	[ExportGroup("Meters")]
	[Export]
	public Meter LoveMeter { get; set; }
	[Export]
	public Meter StomachMeter { get; set; }

	private Observables _observables;

	public override void _Ready()
	{
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

		_observables.UpdateHeartMeterValue += LoveMeter.UpdateValue;
		_observables.UpdateHeartMeterMax += LoveMeter.UpdateMax;

		_observables.UpdateHungerMeterValue += StomachMeter.UpdateValue;
		_observables.UpdateHungerMeterMax += StomachMeter.UpdateMax;
	}
}
