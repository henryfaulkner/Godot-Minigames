using Godot;
using System;

public partial class Main : Node3D
{
	private static readonly StringName CAPYBARA_SCENE_PATH = new StringName("res://src/ObjectLibrary/Capybara.tscn");

	public override void _Ready()
	{
		var ccb = new CapyCubeBusiness(0.25);
		foreach (var coord in ccb.Coords)
		{
			SpawnCapybara(coord * 20);
		}
	}

	private void SpawnCapybara(Vector3 coord)
	{
		GD.Print($"Capybara Coordinate: {coord.ToString()}");
		var capyScene = GD.Load<PackedScene>(CAPYBARA_SCENE_PATH);
		var capy = capyScene.Instantiate<Capybara>();
		capy.Position = coord;
		AddChild(capy);
	}
}
