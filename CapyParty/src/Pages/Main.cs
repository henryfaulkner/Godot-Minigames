using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class Main : Node3D
{
	public static StringName RARE_ITEM_REVEAL_SCENE_PATH = new StringName("res://src/Pages/RareItemReveal.tscn");
	
	private static readonly StringName CAPYBARA_SCENE_PATH = new StringName("res://src/ObjectLibrary/Capybara/LowPolyCapybara.tscn");
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

		CapyCubeBusiness = new CapyCubeBusiness(0.175);
		PartyColorBusiness = new PartyColorBusiness();
	}

	public override void _Ready()
	{
		foreach (var coord in CapyCubeBusiness.Coords)
		{
			CapybaraList.Add(SpawnCapybara(coord * 30));
			PartyLightList.Add(SpawnPartyLight(coord * 30));
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact")) 
		{
			TimingFunctions.SetTimeout(GetSwitchAction(), 50);	
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Increment color index
		LightColorIndex = (LightColorIndex + delta * 40) % 256; // Keep it within bounds of 0-255
		int pos = (int)Math.Floor(LightColorIndex);
		
		// Use a normal for-loop for better performance than ForEach
		for (int i = 0; i < PartyLightList.Count; i++)
		{
			PartyLightList[i].SetColor(PartyColorBusiness.GetWheelColor(pos));
		}

		// Use Parallel.ForEach to update lights in parallel
		// Parallel.ForEach(PartyLightList, light =>
		// {
		// 	light.SetColor(PartyColorBusiness.GetWheelColor(pos));
		// });
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
	
	private Action GetSwitchAction()
	{
		return () =>
		{
			try
			{
				var scene = GD.Load<PackedScene>(RARE_ITEM_REVEAL_SCENE_PATH);
				GetTree().ChangeSceneToPacked(scene);
			}
			catch (Exception e)
			{
				GD.PrintErr($"Switch error: {e.Message}");
			}
		};
	}
}
