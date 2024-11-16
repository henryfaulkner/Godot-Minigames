using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IAnimalInteractor
{
	Task<AnimalModel> GetAnimal(int id);
	Task<List<AnimalModel>> GetAllAnimals();
	Task<AnimalEventSummary> GetAnimal_RecentEventData(int id, TimeSpan? timeSpan);
	Task RenameAnimal(int id, string name);
	Task FeedAnimal(int id);
	Task NurtureAnimal(int id);
}
