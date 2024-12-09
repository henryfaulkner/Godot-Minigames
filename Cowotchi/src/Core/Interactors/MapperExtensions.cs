using System;

public static class MapperExtensions
{
	public static CreatureModel MapToModel(this Egg egg)
	{
		var result = new CreatureModel(Enumerations.CreatureTypes.Egg);

		result.Id = egg.Id;
		result.Name = egg.Name;
		result.BirthDate = egg.CreatedDate;
		result.IsInGallery = egg.IsInGallery;

		result.StomachLevel = 0;
		result.StomachMax = 0;
		result.LoveLevel = 0;
		result.LoveMax = 0;

		result.Mesh = egg.AnimalType.Mesh;
		result.BgEggController = (Enumerations.BgEggControllers)egg.AnimalType.BgEggController;
		result.BgAnimalController = (Enumerations.BgAnimalControllers)egg.AnimalType.BgAnimalController;
		result.FgEggController = (Enumerations.FgEggControllers)egg.AnimalType.FgEggController;
		result.FgAnimalController = (Enumerations.FgAnimalControllers)egg.AnimalType.FgAnimalController;

		result.CreatureLevel = 0;
		result.XpOffset = 0;

		return result;
	}

	public static CreatureModel MapToModel(this Animal animal, AnimalEventSummary eventSummary)
	{
		var creatureType = ConvertAnimalTypeToCreatureType(animal.AnimalType);
		var result = new CreatureModel(creatureType);

		result.Id = animal.Id;
		result.Name = animal.Name;
		result.BirthDate = animal.CreatedDate;
		result.IsInGallery = true;
		
		result.StomachLevel = eventSummary.FeedCount;
		result.StomachMax = animal.StomachMax;
		result.LoveLevel = eventSummary.NurtureCount;
		result.LoveMax = animal.LoveMax;

		result.Mesh = animal.AnimalType.Mesh;
		result.BgEggController = (Enumerations.BgEggControllers)animal.AnimalType.BgEggController;
		result.BgAnimalController = (Enumerations.BgAnimalControllers)animal.AnimalType.BgAnimalController;
		result.FgEggController = (Enumerations.FgEggControllers)animal.AnimalType.FgEggController;
		result.FgAnimalController = (Enumerations.FgAnimalControllers)animal.AnimalType.FgAnimalController;

		result.CreatureLevel = animal.Level;
		result.XpOffset = animal.XpOffset;

		return result;
	}
	
	private static Enumerations.CreatureTypes ConvertAnimalTypeToCreatureType(AnimalType animalType)
	{
		switch (animalType.Id)
		{
			case (int)Enumerations.AnimalTypes.Cow:
				return Enumerations.CreatureTypes.Cow;
				break;
			case (int)Enumerations.AnimalTypes.Pig:
				return Enumerations.CreatureTypes.Pig;
				break;
			default:
				throw new Exception("MapperExtensions ConvertAnimalTypeToCreatureType: AnimalType not mapped.");
				break;
		}
	}
}
