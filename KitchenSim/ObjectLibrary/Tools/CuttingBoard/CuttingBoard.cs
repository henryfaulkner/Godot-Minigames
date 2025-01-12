using Godot;
using System;

public partial class CuttingBoard : Tool, ITile
{
	public override Enumerations.ToolTypes ToolType { get; set; } = Enumerations.ToolTypes.CuttingBoard;
}
