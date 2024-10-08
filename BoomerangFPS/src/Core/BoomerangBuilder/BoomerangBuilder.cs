using Godot;
using System;
using System.Collections.Generic;

public class BoomerangBuilder : IBoomerangBuilder
{
	private static readonly StringName BOOMERANG_SCENE_PATH = new StringName("res://src/ObjectLibrary/Boomerang/Boomerang.tscn");
	private static readonly StringName EXPLOSION_SCENE_PATH = new StringName("res://src/ObjectLibrary/Explosion/Explosion.tscn");

	private PackedScene BoomerangScene { get; set; }
	private PackedScene ExplosionScene { get; set; }

	private Boomerang Result { get; set; }

	public BoomerangBuilder() 
	{
		BoomerangScene = GD.Load<PackedScene>(BOOMERANG_SCENE_PATH);
		ExplosionScene = GD.Load<PackedScene>(EXPLOSION_SCENE_PATH);
	}

	public void Reset() 
	{
		Result = BoomerangScene.Instantiate<Boomerang>();
	}

	public void BuildExplosive() 
	{
		var throwAction = new ThrowAction();
		throwAction.Action = () => {
			var explosionNode = ExplosionScene.Instantiate<Explosion>();
			Result.AddChild(explosionNode);
		};
		throwAction.Delay = 0;
		throwAction.Duration = null;
		Result.AddThrowAction(throwAction);
	}

	public void BuildMulti() {}

	public Boomerang GetResult()
	{
		return Result;
	}

	public Boomerang GetReadiedBoomerang(Queue<Enumerations.PowerUps> powerUps)
	{
		Reset();
		foreach (var powerUp in powerUps)
		{
			switch (powerUp)
			{
				case Enumerations.PowerUps.Explosion:
					BuildExplosive(); 
					break;
				case Enumerations.PowerUps.Multi: 
					BuildMulti();
					break;
				default:
					break;
			}
		}
		return Result;
	}
}
