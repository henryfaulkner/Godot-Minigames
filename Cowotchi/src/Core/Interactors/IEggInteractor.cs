using System.Threading.Tasks;
using System.Collections.Generic;

public interface IEggInteractor
{
	Task<Egg> CreateEgg();
	Task<List<Egg>> GetAllEggs();
	Task<Egg> GetEgg(int id);
	Task RenameEgg(int id, string name);
	Task<Animal> HatchEgg(int id);
}
