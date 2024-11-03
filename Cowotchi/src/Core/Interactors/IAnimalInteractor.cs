using System;
using System.Threading.Tasks;

public interface IAnimalInteractor
{
    Task<Animal> GetAnimal(int id);
    Task<AnimalEventData> GetAnimal_RecentEventData(int id, TimeSpan timeSpan);
    Task RenameAnimal(int id, string name);
    Task FeedAnimal(int id);
    Task NurtureAnimal(int id);
}