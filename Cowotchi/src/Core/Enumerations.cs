using System;
using System.ComponentModel;

public partial class Enumerations
{
	public enum LogLevels
	{
		[Description("Debug")]
		Debug = 0,
		[Description("Info")]
		Info = 1,
		[Description("Error")]
		Error = 2,
	}

	public enum HatchRequirementTypes
	{
		[Description("Time")]
		Time = 1,
	}

	public enum AnimalTypes
	{
		[Description("Cow")]
		Cow = 1,
		[Description("Chicken")]
		Chicken = 2, 
	}

	public enum AnimalEventTypes
	{
		[Description("Nurture")]
		Nurture = 1,
		[Description("Feed")]
		Feed = 2, 
	}

	public enum CreatureTypes
	{
		[Description("Egg")]
		Egg = 0,
		[Description("Cow")]
		Cow = 1,
		[Description("Chicken")]
		Chicken = 2, 
	}



	public enum Commands
	{
		[Description("Stats")]
		Stats,
		[Description("Swap")]
		Swap,
		[Description("Nurture")]
		Nurture,
		[Description("Feed")]
		Feed,
	}
}
