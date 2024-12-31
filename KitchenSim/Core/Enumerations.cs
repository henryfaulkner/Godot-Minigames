using System;
using System.ComponentModel;

public static class Enumerations
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

	public enum TileTypes
	{
		Floor = 1,
		Wall = 2,
		StaffAgent = 3,
	}
}
