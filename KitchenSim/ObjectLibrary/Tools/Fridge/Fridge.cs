using Godot;
using System;

public partial class Fridge : Tool, ITile
{
	public override Enumerations.ToolTypes ToolType { get; set; } = Enumerations.ToolTypes.Fridge;
}
