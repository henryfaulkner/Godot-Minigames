using Godot;
using System;

public partial class OvenAndStove : Tool, ITile
{
	public override Enumerations.ToolTypes ToolType { get; set; } = Enumerations.ToolTypes.OvenAndStove;
}
