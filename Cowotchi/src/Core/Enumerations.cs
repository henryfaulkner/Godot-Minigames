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
		Time,
	}

	public enum AnimalTypes
	{
		[Description("Cow")]
		Cow,
		[Description("Chicken")]
		Chicken, 
	}

	public enum AnimalEventTypes
	{
		[Description("Nuture")]
		Nuture,
		[Description("Chicken")]
		Feed, 
	}
}