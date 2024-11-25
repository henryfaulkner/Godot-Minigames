using Godot;
using System;
using System.Threading.Tasks;

public partial class EffectsFactory : Node
{
	private readonly StringName LOVE_EFFECT_PATH = new StringName("res://src/Core/Effects/LoveEffect.tscn");

	private readonly PackedScene _loveEffectPath;

	public EffectsFactory()
	{
		_loveEffectPath = (PackedScene)ResourceLoader.Load(LOVE_EFFECT_PATH);
	}

	public LoveEffect SpawnLoveEffect(Node parent, Vector3 position)
	{
		var result = _loveEffectPath.Instantiate<LoveEffect>();
		result.ReadyInstance(position);
		parent.AddChild(result);
		return result;
	}
}
