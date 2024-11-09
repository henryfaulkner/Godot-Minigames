using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IAnimalInteractor
{
    Task<Animal> GetAnimal(int id);
    Task<List<Animal>> GetAllAnimals();
    Task<AnimalEventData> GetAnimal_RecentEventData(int id, TimeSpan timeSpan);
    Task RenameAnimal(int id, string name);
    Task FeedAnimal(int id);
    Task NurtureAnimal(int id);
}