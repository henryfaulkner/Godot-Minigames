using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class CommonInteractor : ICommonInteractor
{
    private readonly ILoggerService _loggerService;

    public CommonInteractor(ILoggerService loggerService)
    {
        _loggerService = loggerService;
    }

    public async Task InitDatabaseIfRequired()
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(new AppDbContext()))
            {
                await InitAnimalEventTypes(unitOfWork.AnimalEventTypeRepository);
                await InitAnimalTypes(unitOfWork.AnimalTypeRepository);
                await InitHatchRequirementTypes(unitOfWork.HatchRequirementTypeRepository);
                _ = await unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"CommonInteractor InitDatabaseIfRequired Error: {ex.Message}", ex);
            throw;
        }
    }

    public async Task<List<AnimalEventType>> GetAnimalEventTypes()
    {
        try
        {
            using (var animalEventTypeRepository = new Repository<AnimalEventType>(new AppDbContext()))
            {
                return (await animalEventTypeRepository.GetAllAsync()).ToList();
            }
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"CommonInteractor GetAnimalEventTypes Error: {ex.Message}");
            throw;
        }
    }

    public async Task<List<AnimalType>> GetAnimalTypes()
    {
        try
        {
            using (var animalTypeRepository = new Repository<AnimalType>(new AppDbContext()))
            {
                return (await animalTypeRepository.GetAllAsync()).ToList();
            }
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"CommonInteractor GetAnimalTypes Error: {ex.Message}");
            throw;
        }
    }

    public async Task<List<HatchRequirementType>> GetHatchRequirementTypes()
    {
        try
        {
            using (var hatchRequirementTypeRepository = new Repository<HatchRequirementType>(new AppDbContext()))
            {
                return (await hatchRequirementTypeRepository.GetAllAsync()).ToList();
            }
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"CommonInteractor GetHatchRequirementTypes Error: {ex.Message}");
            throw;
        }
    }

    public async Task InitAnimalEventTypes(IRepository<AnimalEventType> animalEventTypeRepository)
    {
        try
        {
            var aetList = new List<AnimalEventType>();
            aetList.Add(
                new AnimalEventType()
                {
                    Name = "Nuture",
                }
            );
            aetList.Add(
                new AnimalEventType()
                {
                    Name = "Feed",
                }
            );
            animalEventTypeRepository.AddRangeAsync(aetList);
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"CommonInteractor InitAnimalEventTypes Error: {ex.Message}");
            throw;
        }
    }

    public async Task InitAnimalTypes(IRepository<AnimalType> animalTypeRepository)
    {
        try
        {
            var atList = new List<AnimalType>();
            atList.Add(
                new AnimalType()
                {
                    Name = "Cow",
                }
            );
            atList.Add(
                new AnimalType()
                {
                    Name = "Chicken",
                }
            );
            animalTypeRepository.AddRangeAsync(atList);
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"CommonInteractor InitAnimalTypes Error: {ex.Message}");
            throw;
        }
    }

    public async Task InitHatchRequirementTypes(IRepository<HatchRequirementType> hatchRequirementRepository)
    {
        try
        {
            var hrtList = new List<HatchRequirementType>();
            hrtList.Add(
                new HatchRequirementType()
                {
                    Name = "Time",
                }
            );
            hatchRequirementRepository.AddRangeAsync(hrtList);
        }
        catch (Exception ex)
        {
            _loggerService.LogError($"CommonInteractor InitHatchRequirementTypes Error: {ex.Message}");
            throw;
        }
    }
}
