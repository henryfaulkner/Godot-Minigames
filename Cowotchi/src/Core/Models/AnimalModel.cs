using System.Collections.Generic;

public class AnimalModel : CreatureModel
{
    // This levels are calculated by number of event record 
    // created within a designated time span.
    public int StomachLevel { get; set; } 
	public int StomachMax { get; set; }
	public int LoveLevel { get; set; }
	public int LoveMax { get; set; }

    public void SetCreatureType(AnimalType animalType)
    {
        switch (animalType.Id)
        {
            case (int)Enumerations.AnimalTypes.Cow:
                CreatureType = Enumerations.CreatureTypes.Cow;
                break;
            case (int)Enumerations.AnimalTypes.Chicken:
                CreatureType = Enumerations.CreatureTypes.Chicken;
                break;
            default:
                break;
        }
    }
}