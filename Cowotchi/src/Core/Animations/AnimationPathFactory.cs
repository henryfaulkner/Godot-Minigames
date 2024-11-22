using Godot;
using System;
using System.Threading.Tasks;

public partial class AnimationPathFactory : Node
{
	private readonly StringName BOUNCE_PATH_PATH = new StringName("res://src/Core/Animations/BouncePath.tscn");

	private readonly PackedScene _bouncePathPath;

	public AnimationPathFactory()
	{
		_bouncePathPath = (PackedScene)ResourceLoader.Load(BOUNCE_PATH_PATH);
	}

	public BouncePath SpawnBouncePath(Node parent, CharacterBody3D character, MeshInstance3D mesh)
	{
		var result = _bouncePathPath.Instantiate<BouncePath>();
		parent.AddChild(result);
		result.ReadyInstance(character, mesh);
		return result;
	}
}
