using Godot;
using System;

public partial class Table : Area2D, IEnvironment, ITile
{
	public Order? Order { get; set; }

    bool _isUsing = false;

	public void SetToUsing() { _isUsing = true; }
	public void StopUsing() { _isUsing = false; }
	public bool CheckIfInUse() { return _isUsing; }

	public Node2D GetNodeSelf() { return this; }
}
