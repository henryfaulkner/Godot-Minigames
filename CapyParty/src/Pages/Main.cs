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

	private float CubeScale = 30.0f; 
	private double CubeCapyUnitRatio = 0.175; 

	private double LightColorIndex { get; set; }
	private float time; // Track elapsed time to animate capycube
	private bool MoveXFlag { get; set; }
	private bool MoveYFlag { get; set; }
	private bool MoveZFlag { get; set; }

	private CapyCubeBusiness CapyCubeBusiness { get; set; }
	private PartyColorBusiness PartyColorBusiness { get; set; }

	public Main()
	{
		CapybaraList = new List<Capybara>();
		PartyLightList = new List<PartyLight>();

		CapyCubeBusiness = new CapyCubeBusiness(CubeCapyUnitRatio);
		PartyColorBusiness = new PartyColorBusiness();
	}

	public override void _Ready()
	{
		foreach (var coord in CapyCubeBusiness.Coords)
		{
			CapybaraList.Add(SpawnCapybara(coord * CubeScale));
			PartyLightList.Add(SpawnPartyLight(coord * CubeScale));
		}
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact")) 
		{
			TimingFunctions.SetTimeout(GetSwitchAction(), 50);	
		}

		if (@event.IsActionPressed("q")) 
		{
			MoveXFlag = true;
			MoveYFlag = false;
			MoveZFlag = false;
		}

		if (@event.IsActionPressed("w")) 
		{
			MoveXFlag = false;
			MoveYFlag = true;
			MoveZFlag = false;
		}

		if (@event.IsActionPressed("e")) 
		{
			MoveXFlag = false;
			MoveYFlag = false;
			MoveZFlag = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		ApplyPartyLightColors(delta);
		if (MoveXFlag) ApplySineWaveToXAxis(delta);
		if (MoveYFlag) ApplySineWaveToYAxis(delta);
		if (MoveZFlag) ApplySineWaveToZAxis(delta);
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

	private void ApplyPartyLightColors(double delta)
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
	
	private void ApplySineWaveToXAxis(double delta)
	{
		time += (float)delta;
		// Update positions with a sine wave on the y-axis
		for (int i = 0; i < CapyCubeBusiness.Coords.Count; i++)
		{
			// Get the original coordinate
			var coord = CapyCubeBusiness.Coords[i];

			// Apply a sine wave to the x position, using time for dynamic movement
			float sineWaveXOffset = (float)Math.Sin(coord.X * 3.0f + time) * 5.0f;  // Frequency: 3.0, Amplitude: 15.0

			// Update the capybara position (correctly applying transform)
			CapybaraList[i].GlobalTransform = new Transform3D(
				CapybaraList[i].GlobalTransform.Basis, // Keep the current Basis (orientation/scale)
				new Vector3(coord.X * CubeScale + sineWaveXOffset, coord.Y * CubeScale, coord.Z * CubeScale) // Update the position
			);

			// Update the party light position (ensure correct Basis usage)
			PartyLightList[i].GlobalTransform = new Transform3D(
				PartyLightList[i].GlobalTransform.Basis, // Use the PartyLight's current Basis, not Capybara's
				new Vector3(coord.X * CubeScale + sineWaveXOffset, coord.Y * CubeScale, coord.Z * CubeScale) // Update the position
			);
		}
	}

	private void ApplySineWaveToYAxis(double delta)
	{
		time += (float)delta;
		// Update positions with a sine wave on the y-axis
		for (int i = 0; i < CapyCubeBusiness.Coords.Count; i++)
		{
			// Get the original coordinate
			var coord = CapyCubeBusiness.Coords[i];

			// Apply a sine wave to the y position, using time for dynamic movement
			float sineWaveYOffset = (float)Math.Sin(coord.Y * 3.0f + time) * 15.0f;  // Frequency: 3.0, Amplitude: 15.0

			// Update the capybara position (correctly applying transform)
			CapybaraList[i].GlobalTransform = new Transform3D(
				CapybaraList[i].GlobalTransform.Basis, // Keep the current Basis (orientation/scale)
				new Vector3(coord.X * CubeScale, coord.Y * CubeScale + sineWaveYOffset, coord.Z * CubeScale) // Update the position
			);

			// Update the party light position (ensure correct Basis usage)
			PartyLightList[i].GlobalTransform = new Transform3D(
				PartyLightList[i].GlobalTransform.Basis, // Use the PartyLight's current Basis, not Capybara's
				new Vector3(coord.X * CubeScale, coord.Y * CubeScale + sineWaveYOffset, coord.Z * CubeScale) // Update the position
			);
		}
	}

	private void ApplySineWaveToZAxis(double delta)
	{
		time += (float)delta;
		// Update positions with a sine wave on the y-axis
		for (int i = 0; i < CapyCubeBusiness.Coords.Count; i++)
		{
			// Get the original coordinate
			var coord = CapyCubeBusiness.Coords[i];

			// Apply a sine wave to the z position, using time for dynamic movement
			float sineWaveZOffset = (float)Math.Sin(coord.Z * 3.0f + time) * 15.0f;  // Frequency: 3.0, Amplitude: 15.0

			// Update the capybara position (correctly applying transform)
			CapybaraList[i].GlobalTransform = new Transform3D(
				CapybaraList[i].GlobalTransform.Basis, // Keep the current Basis (orientation/scale)
				new Vector3(coord.X * CubeScale, coord.Y * CubeScale, coord.Z * CubeScale + sineWaveZOffset) // Update the position
			);

			// Update the party light position (ensure correct Basis usage)
			PartyLightList[i].GlobalTransform = new Transform3D(
				PartyLightList[i].GlobalTransform.Basis, // Use the PartyLight's current Basis, not Capybara's
				new Vector3(coord.X * CubeScale, coord.Y * CubeScale, coord.Z * CubeScale + sineWaveZOffset) // Update the position
			);
		}
	}
}
