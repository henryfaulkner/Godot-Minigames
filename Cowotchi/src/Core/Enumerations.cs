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
		[Description("Action Count Over Time")]
		ActionCount = 1,
	}

	public enum AnimalTypes
	{
		[Description("Cow")]
		Cow = 1,
		[Description("Pig")]
		Pig = 2, 
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
		[Description("Pig")]
		Pig = 2, 
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
		[Description("Hatch")]
		Hatch,
	}

	public enum CharacterActions
	{
		[Description("ReceiveLove")]
		ReceiveLove,
		[Description("Eat")]
		Eat,
		[Description("Hatch")]
		Hatch,
	}

	public enum BgEggControllers
	{
		[Description("Background Basic Egg Controller")]
		Basic = 1,
	}

	public enum BgAnimalControllers
	{
		[Description("Background Basic Animal Controller")]
		Basic = 1,
	}

	public enum FgEggControllers
	{
		[Description("Foreground Basic Egg Controller")]
		Basic = 1,
	}

	public enum FgAnimalControllers
	{
		[Description("Foreground Basic Animal Controller")]
		Basic = 1,
	}

	public enum MenuPageType
	{
		Egg,
		Animal,
	}
}
