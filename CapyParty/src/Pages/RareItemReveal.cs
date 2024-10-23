using Godot;
using System;

public partial class RareItemReveal : StaticBody3D
{
	public static StringName MAIN_SCENE_PATH = new StringName("res://src/Pages/Main.tscn");
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact")) 
		{
			TimingFunctions.SetTimeout(GetSwitchAction(), 50);	
		}
	}
	
	private Action GetSwitchAction()
	{
		return () =>
		{
			try
			{
				var scene = GD.Load<PackedScene>(MAIN_SCENE_PATH);
				GetTree().ChangeSceneToPacked(scene);
			}
			catch (Exception e)
			{
				GD.PrintErr($"Switch error: {e.Message}");
			}
		};
	}
}
