using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICommonInteractor
{
    public Task InitDatabaseIfRequired();
    public Task<List<AnimalEventType>> GetAnimalEventTypes();
    public Task<List<AnimalType>> GetAnimalTypes();
    public Task<List<HatchRequirementType>> GetHatchRequirementTypes();
}