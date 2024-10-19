using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node3D
{
	private static readonly StringName CAPYBARA_SCENE_PATH = new StringName("res://src/ObjectLibrary/Capybara.tscn");
	private static readonly StringName PARTY_LIGHT_SCENE_PATH = new StringName("res://src/ObjectLibrary/PartyLight.tscn");
	
	private Queue<Capybara> CapybaraQueue { get; set; } 
	private List<PartyLight> PartyLightList { get; set; }

	[Export]
	private PartyLight TestLight { get; set; }
	[Export]
	private PartyLight TestLight2 { get; set; }
	[Export]
	private PartyLight TestLight3 { get; set; }
	
	private double LightColorIndex { get; set; }
	
	private CapyCubeBusiness CapyCubeBusiness { get; set; }
	private PartyColorBusiness PartyColorBusiness { get; set; }

	public Main() 
	{
		CapybaraQueue = new Queue<Capybara>();
		PartyLightList = new List<PartyLight>();
		
		CapyCubeBusiness = new CapyCubeBusiness(0.25);
		PartyColorBusiness = new PartyColorBusiness();
	}

	public override void _Ready()
	{
		foreach (var coord in CapyCubeBusiness.Coords)
		{
			CapybaraQueue.Enqueue(SpawnCapybara(coord * 20));
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// this is for testing and is very crude
		LightColorIndex += delta * 40;
		int pos = (int)Math.Floor(LightColorIndex);
		GD.Print($"Color Position: {pos}");
		TestLight.SetColor(PartyColorBusiness.GetWheelColor(pos));
		TestLight2.SetColor(PartyColorBusiness.GetWheelColor(pos));
		TestLight3.SetColor(PartyColorBusiness.GetWheelColor(pos));
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
