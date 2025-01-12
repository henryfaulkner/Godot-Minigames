using Godot;

public interface ITool
{
	void SetToUsing();
	void StopUsing();
	bool CheckIfInUse();

	Node2D GetNodeSelf();

	Enumerations.ToolTypes GetToolType();
}
