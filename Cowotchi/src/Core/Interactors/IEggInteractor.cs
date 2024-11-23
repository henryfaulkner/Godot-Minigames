using System.Threading.Tasks;
using System.Collections.Generic;

public interface IEggInteractor
{
	Task<CreatureModel> CreateEgg();
	Task<List<CreatureModel>> GetAllEggs();
	Task<CreatureModel> GetEgg(int id);
	Task RenameEgg(int id, string name);
	Task<CreatureModel> HatchEgg(int id);
}
