using Godot; 
using System;

public abstract partial class Tool : Area2D, ITool
{
	public abstract Enumerations.ToolTypes ToolType { get; set; }
	bool _isUsing = false;

	public void SetToUsing() { _isUsing = true; }
	public void StopUsing() { _isUsing = false; }
	public bool CheckIfInUse() { return _isUsing; }

	public Node2D GetNodeSelf() { return this; }

	public Enumerations.ToolTypes GetToolType() { return ToolType; }
}
