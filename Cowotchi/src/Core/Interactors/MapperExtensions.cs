public static class MapperExtensions
{
	public static EggModel MapToModel(this Egg egg)
	{
		var result = new EggModel();
		result.Id = egg.Id;
		result.Name = egg.Name;
		result.BirthDate = egg.CreatedDate;
		return result;
	}

	public static AnimalModel MapToModel(this Animal animal, AnimalEventSummary eventSummary)
	{
		var result = new AnimalModel();
		result.Id = animal.Id;
		result.Name = animal.Name;
		result.BirthDate = animal.CreatedDate;
		result.SetCreatureType(animal.AnimalType);
		result.StomachLevel = eventSummary.FeedCount;
		result.StomachMax = animal.StomachMax;
		result.LoveLevel = eventSummary.NurtureCount;
		result.LoveMax = animal.LoveMax;
		return result;
	}
}
