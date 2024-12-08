using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IAnimalInteractor
{
	Task<CreatureModel> GetAnimal(int id);
	Task<List<CreatureModel>> GetAllAnimals();
	Task<AnimalEventSummary> GetAnimalEventSummary(int id, TimeSpan? timeSpan);
	Task RenameAnimal(int id, string name);
	Task FeedAnimal(int id, int xpIncrease);
	Task NurtureAnimal(int id, int xpIncrease);
}
