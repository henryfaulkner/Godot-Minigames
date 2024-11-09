using System.Threading.Tasks;
using System.Collections.Generic;

public interface IEggInteractor
{
	Task<EggModel> CreateEgg();
	Task<List<EggModel>> GetAllEggs();
	Task<EggModel> GetEgg(int id);
	Task RenameEgg(int id, string name);
	Task<AnimalModel> HatchEgg(int id);
}
