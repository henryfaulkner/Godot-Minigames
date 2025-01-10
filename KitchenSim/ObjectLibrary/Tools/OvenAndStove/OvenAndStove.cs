using Godot;
using System;

public partial class OvenAndStove : Area2D, ITool, ITile
{
    bool _isUsing = false;

    public void SetToUsing() { _isUsing = true; }
	public void StopUsing() { _isUsing = false; }
	public bool CheckIfInUse() { return _isUsing; }
}
