using Godot;
using System;

public partial class Menu : CanvasLayer
{
	[ExportGroup("Meters")]
	[Export]
	public Meter HeartMeter { get; set; }
	[Export]
	public Meter HealthMeter { get; set; }
}
