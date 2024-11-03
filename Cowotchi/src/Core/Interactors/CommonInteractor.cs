using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Godot;

public partial class CommonInteractor : Node, ICommonInteractor
{
	private ILoggerService _loggerService { get; set; }

	public override void _Ready() 
	{
		_loggerService = GetNode<ILoggerService>("/root/LoggerService");
	}
	
	public async Task InitDatabaseIfRequired()
	{
		try
		{
			_loggerService.LogDebug("Start CommonInteractor InitDatabaseIfRequired");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				if(!(await unitOfWork.AnimalEventTypeRepository.AnyAsync())) await InitAnimalEventTypes(unitOfWork.AnimalEventTypeRepository);
				if(!(await unitOfWork.AnimalTypeRepository.AnyAsync())) await InitAnimalTypes(unitOfWork.AnimalTypeRepository);
				if(!(await unitOfWork.HatchRequirementTypeRepository.AnyAsync())) await InitHatchRequirementTypes(unitOfWork.HatchRequirementTypeRepository);
				_ = await unitOfWork.SaveChangesAsync();
			}
			_loggerService.LogDebug("End CommonInteractor InitDatabaseIfRequired");
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
			_loggerService.LogDebug("Start CommonInteractor GetAnimalEventTypes");
			using (var animalEventTypeRepository = new Repository<AnimalEventType>(new AppDbContext()))
			{
				return (await animalEventTypeRepository.GetAllAsync()).ToList();
			}
			_loggerService.LogDebug("End CommonInteractor GetAnimalEventTypes");
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
			_loggerService.LogDebug("Start CommonInteractor GetAnimalTypes");
			using (var animalTypeRepository = new Repository<AnimalType>(new AppDbContext()))
			{
				return (await animalTypeRepository.GetAllAsync()).ToList();
			}
			_loggerService.LogDebug("End CommonInteractor GetAnimalTypes");
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
			_loggerService.LogDebug("Start CommonInteractor GetHatchRequirementTypes");
			using (var hatchRequirementTypeRepository = new Repository<HatchRequirementType>(new AppDbContext()))
			{
				return (await hatchRequirementTypeRepository.GetAllAsync()).ToList();
			}
			_loggerService.LogDebug("End CommonInteractor GetHatchRequirementTypes");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"CommonInteractor GetHatchRequirementTypes Error: {ex.Message}");
			throw;
		}
	}

	private async Task InitAnimalEventTypes(IRepository<AnimalEventType> animalEventTypeRepository)
	{
		try
		{
			var aetList = new List<AnimalEventType>();
			aetList.Add(
				new AnimalEventType()
				{
					Id = (int)Enumerations.AnimalEventTypes.Nurture,
					Name = Enumerations.AnimalEventTypes.Nurture.GetDescription(),
				}
			);
			aetList.Add(
				new AnimalEventType()
				{
					Id = (int)Enumerations.AnimalEventTypes.Feed,
					Name = Enumerations.AnimalEventTypes.Feed.GetDescription(),
				}
			);
			await animalEventTypeRepository.AddRangeAsync(aetList);
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"CommonInteractor InitAnimalEventTypes Error: {ex.Message}");
			throw;
		}
	}

	private async Task InitAnimalTypes(IRepository<AnimalType> animalTypeRepository)
	{
		try
		{
			var atList = new List<AnimalType>();
			atList.Add(
				new AnimalType()
				{
					Id = (int)Enumerations.AnimalTypes.Cow,
					Name = Enumerations.AnimalTypes.Cow.GetDescription(),
				}
			);
			atList.Add(
				new AnimalType()
				{
					Id = (int)Enumerations.AnimalTypes.Chicken,
					Name = Enumerations.AnimalTypes.Chicken.GetDescription(),
				}
			);
			await animalTypeRepository.AddRangeAsync(atList);
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"CommonInteractor InitAnimalTypes Error: {ex.Message}");
			throw;
		}
	}

	private async Task InitHatchRequirementTypes(IRepository<HatchRequirementType> hatchRequirementRepository)
	{
		try
		{
			var hrtList = new List<HatchRequirementType>();
			hrtList.Add(
				new HatchRequirementType()
				{
					Id = (int)Enumerations.HatchRequirementTypes.Time,
					Name = Enumerations.HatchRequirementTypes.Time.GetDescription(),
				}
			);
			await hatchRequirementRepository.AddRangeAsync(hrtList);
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"CommonInteractor InitHatchRequirementTypes Error: {ex.Message}");
			throw;
		}
	}
}
