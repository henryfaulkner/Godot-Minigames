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

	public static AnimalModel MapToModel(this Animal animal)
	{
		var result = new AnimalModel();
		result.Id = animal.Id;
		result.Name = animal.Name;
		result.SetCreatureType(animal.AnimalType);
		result.BirthDate = animal.CreatedDate;
		return result;
	}
}
