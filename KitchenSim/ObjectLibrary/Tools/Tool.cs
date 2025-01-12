using Godot; 
using System;

public abstract partial class Tool : Area2D, ITool
{
	bool _isUsing = false;

	public void SetToUsing() { _isUsing = true; }
	public void StopUsing() { _isUsing = false; }
	public bool CheckIfInUse() { return _isUsing; }

	public Node2D GetNodeSelf() { return this; }
}
