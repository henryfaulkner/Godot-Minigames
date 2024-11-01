using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICommonInteractor
{
    Task InitDatabaseIfRequired();
    Task<List<AnimalEventType>> GetAnimalEventTypes();
    Task<List<AnimalType>> GetAnimalTypes();
    Task<List<HatchRequirementType>> GetHatchRequirementTypes();
}