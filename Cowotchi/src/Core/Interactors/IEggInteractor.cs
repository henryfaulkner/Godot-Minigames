using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IEggInteractor
{
	Task<CreatureModel> CreateEgg();
	Task<List<CreatureModel>> GetAllEggs();
	Task<CreatureModel> GetEgg(int id);
	Task RenameEgg(int id, string name); 
	bool IsReadyToHatch(int eggId, TimeSpan? timeSpan = null);
	Task<List<CreatureModel>> GetReadyEggs(TimeSpan? timeSpan = null);
	Task<CreatureModel> HatchEgg(int id);
	Task AddEggToGallery(int id);
	Task<List<CreatureModel>> HatchReadyEggs();
}
