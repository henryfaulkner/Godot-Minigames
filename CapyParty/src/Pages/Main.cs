using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node3D
{
	private static readonly StringName CAPYBARA_SCENE_PATH = new StringName("res://src/ObjectLibrary/Capybara/Capybara.tscn");
	private static readonly StringName PARTY_LIGHT_SCENE_PATH = new StringName("res://src/ObjectLibrary/PartyLight/PartyLight.tscn");

	private List<Capybara> CapybaraList { get; set; }
	private List<PartyLight> PartyLightList { get; set; }

	private double LightColorIndex { get; set; }

	private CapyCubeBusiness CapyCubeBusiness { get; set; }
	private PartyColorBusiness PartyColorBusiness { get; set; }

	public Main()
	{
		CapybaraList = new List<Capybara>();
		PartyLightList = new List<PartyLight>();

		CapyCubeBusiness = new CapyCubeBusiness(0.25);
		PartyColorBusiness = new PartyColorBusiness();
	}

	public override void _Ready()
	{
		foreach (var coord in CapyCubeBusiness.Coords)
		{
			CapybaraList.Add(SpawnCapybara(coord * 20));
			PartyLightList.Add(SpawnPartyLight(coord * 20));
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// this is for testing and is very crude
		LightColorIndex += delta * 40;
		int pos = (int)Math.Floor(LightColorIndex);
		PartyLightList.ForEach(x => x.SetColor(PartyColorBusiness.GetWheelColor(pos)));
	}

	private Capybara SpawnCapybara(Vector3 coord)
	{
		var scene = GD.Load<PackedScene>(CAPYBARA_SCENE_PATH);
		var result = scene.Instantiate<Capybara>();
		result.Position = coord;
		AddChild(result);
		return result;
	}

	private PartyLight SpawnPartyLight(Vector3 coord)
	{
		var scene = GD.Load<PackedScene>(PARTY_LIGHT_SCENE_PATH);
		var result = scene.Instantiate<PartyLight>();
		result.Position = coord;
		AddChild(result);
		return result;
	}
}
