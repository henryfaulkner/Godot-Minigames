using System;

public class CreatureModel
{
	public CreatureModel(Enumerations.CreatureTypes creatureType)
	{
		CreatureType = creatureType;
	}

	public int Id { get; set; }
	public ulong InstanceId { get; set; }
	public string Name { get; set; }
	public Enumerations.CreatureTypes CreatureType { get; protected set; }
	public DateTime BirthDate { get; set; }

	// This levels are calculated by number of event record 
    // created within a designated time span.
    public int StomachLevel { get; set; } 
	public int StomachMax { get; set; }
	public int LoveLevel { get; set; }
	public int LoveMax { get; set; }

	public string Mesh { get; set; }
	public Enumerations.BgEggControllers BgEggController { get; set;}
	public Enumerations.BgAnimalControllers BgAnimalController { get; set;}
	public Enumerations.FgEggControllers FgEggController { get; set; }
	public Enumerations.FgAnimalControllers FgAnimalController { get; set; }

	public int CreatureLevel { get; set; }
	public int XpOffset { get; set; }
}
