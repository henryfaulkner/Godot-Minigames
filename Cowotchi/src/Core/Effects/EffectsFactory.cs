using Godot;
using System;
using System.Threading.Tasks;

public partial class EffectsFactory : Node
{
	private readonly StringName LOVE_EFFECT_PATH = new StringName("res://src/Core/Effects/LoveEffect.tscn");
	private readonly StringName FEED_EFFECT_PATH = new StringName("res://src/Core/Effects/FeedEffect.tscn");

	private readonly PackedScene _loveEffectPath;
	private readonly PackedScene _feedEffectPath;

	public EffectsFactory()
	{
		_loveEffectPath = (PackedScene)ResourceLoader.Load(LOVE_EFFECT_PATH);
		_feedEffectPath = (PackedScene)ResourceLoader.Load(FEED_EFFECT_PATH);
	}

	public LoveEffect SpawnLoveEffect(Node parent, Vector3 position)
	{
		var result = _loveEffectPath.Instantiate<LoveEffect>();
		result.ReadyInstance(position);
		parent.AddChild(result);
		return result;
	}

	public LoveEffect SpawnFeedEffect(Node parent, Vector3 position)
	{
		var result = _feedEffectPath.Instantiate<LoveEffect>();
		result.ReadyInstance(position);
		parent.AddChild(result);
		return result;
	}
}
