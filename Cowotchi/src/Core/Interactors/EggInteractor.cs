using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class EggInteractor : Node, IEggInteractor
{
	public static readonly string[] DefaultFarmAnimalNames = new string[]
	{
		"Bessie", "Daisy", "Moo Moo", "Clucky", "Woolly", 
		"Ginger", "Nibbles", "Spot", "Fluffy", "Buster",
		"Penny", "Coco", "Rusty", "Duke", "Ethel", 
		"Chickadee", "Sally", "Porky", "Clover", "Rascal", 
		"Bambi", "Marigold", "Bubbles", "Whiskers", "Lucky", 
		"Shadow", "Mittens", "Peanut", "Sparky", "Cotton", 
		"Cinnamon", "Hickory", "Bingo", "Toffee", "Pumpkin", 
		"Twinkle", "Snowball", "Jasper", "Trixie", "Waffles", 
		"Dandelion", "Oreo", "Rosie", "Gizmo", "Gus", 
		"Beatrice", "Petunia", "Chester", "Duchess", "Toby"
	};

	private ILoggerService _logger { get; set; }

	public override void _Ready() 
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public async Task<CreatureModel> CreateEgg()
	{
		CreatureModel result;
		try
		{
			_logger.LogDebug("Start EggInteractor CreateEgg");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var eEntity = new Egg();
				eEntity.Name = GetRandomName();
				eEntity.AnimalType = await GetAnimalType(unitOfWork.AnimalTypeRepository);
				eEntity.IsHatched = false;
				await unitOfWork.EggRepository.AddAsync(eEntity);

				var eggHatchRequirementTypes = await GetHatchRequirementTypes(unitOfWork.HatchRequirementTypeRepository);
				foreach (var hatchRequirementType in eggHatchRequirementTypes)
				{
					var hrtEntity = CreateHatchRequirement(eEntity, (Enumerations.HatchRequirementTypes)hatchRequirementType.Id);
					await unitOfWork.HatchRequirementRepository.AddAsync(hrtEntity);
				}

				_ = await unitOfWork.SaveChangesAsync();
				result = eEntity.MapToModel();
			}
			_logger.LogDebug("End EggInteractor CreateEgg");
		}
		catch (Exception ex)
		{
			_logger.LogError($"EggInteractor CreateEgg Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task<CreatureModel> GetEgg(int id)
	{
		CreatureModel result;
		try
		{
			_logger.LogDebug("Start EggInteractor GetEgg");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var entity = await unitOfWork.EggRepository.GetByIdIncludesAsync(id, IncludesHelper.GetEggIncludes());
				result = entity.MapToModel();
			}
			_logger.LogDebug("End EggInteractor GetEgg");
		}
		catch (Exception ex)
		{
			_logger.LogError($"EggInteractor GetEgg Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}
	
	public async Task<List<CreatureModel>> GetAllEggs()
	{
		var result = new List<CreatureModel>();
		try
		{
			_logger.LogDebug("Start EggInteractor GetAllEggs");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var entityList = 
					(await unitOfWork.EggRepository.GetAllIncludesAsync(IncludesHelper.GetEggIncludes()))
						.Where(x => !x.IsHatched)
						.Where(x => !x.IsDeleted)
						.ToList();

				foreach (var entity in entityList)
				{
					result.Add(entity.MapToModel());
				}
			}
			_logger.LogDebug("End EggInteractor GetAllEggs");
		}
		catch (Exception ex)
		{
			_logger.LogError($"EggInteractor GetAllEggs Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task RenameEgg(int id, string name)
	{
		try
		{
			_logger.LogDebug("Start EggInteractor RenameEgg");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var eEntity = await unitOfWork.EggRepository.GetByIdIncludesAsync(id, IncludesHelper.GetEggIncludes());
				eEntity.Name = name;
				await unitOfWork.SaveChangesAsync();
			}
			_logger.LogDebug("End EggInteractor RenameEgg");
		}
		catch (Exception ex)
		{
			_logger.LogError($"EggInteractor RenameEgg Error: {ex.Message}", ex);
			throw;
		}
	}

	public async Task<CreatureModel> HatchEgg(int id)
	{
		CreatureModel result;
		try
		{
			_logger.LogDebug("Start EggInteractor HatchEgg");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var eEntity = await unitOfWork.EggRepository.GetByIdIncludesAsync(id, IncludesHelper.GetEggIncludes());
				eEntity.IsHatched = true;

				var aEntity = new Animal();
				aEntity.Name = eEntity.Name;
				aEntity.AnimalType = eEntity.AnimalType;
				aEntity.Egg = eEntity;
				await unitOfWork.AnimalRepository.AddAsync(aEntity);

				_ = await unitOfWork.SaveChangesAsync();
				
				var eventSummary = new AnimalEventSummary()
				{
					NurtureCount = 0,
					FeedCount = 0,
				}; 
				result = aEntity.MapToModel(eventSummary);
			}
			
			_logger.LogDebug("End EggInteractor HatchEgg");
		}
		catch (Exception ex)
		{
			_logger.LogError($"EggInteractor HatchEgg Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}
	
	public async Task AddEggToGallery(int id)
	{
		try
		{
			_logger.LogDebug("Start EggInteractor AddEggToGallery");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var eEntity = await unitOfWork.EggRepository.GetByIdAsync(id);
				eEntity.IsInGallery = true;
				_ = await unitOfWork.SaveChangesAsync();
			}
			_logger.LogDebug("End EggInteractor AddEggToGallery");
		}
		catch (Exception ex)
		{
			_logger.LogError($"EggInteractor AddEggToGallery Error: {ex.Message}", ex);
			throw;
		}
	}

	private string GetRandomName()
	{
		var rand = new Random();
		var randIndex = rand.Next(DefaultFarmAnimalNames.Length);
		return DefaultFarmAnimalNames[randIndex];
	}

	private async Task<AnimalType> GetAnimalType(IRepository<AnimalType> animalTypeRepository)
	{
		var animalTypes = (await animalTypeRepository.GetAllAsync()).ToList();
		var rand = new Random();
		var randIndex = rand.Next(animalTypes.Count);
		return animalTypes[randIndex];
	}

	private async Task<List<HatchRequirementType>> GetHatchRequirementTypes(IRepository<HatchRequirementType> hatchRequirementTypeRepository)
	{
		var hatchRequirementTypes = (await hatchRequirementTypeRepository.GetAllAsync()).ToList();
		var rand = new Random();
		var result = new List<HatchRequirementType>();
		while (hatchRequirementTypes.Count > 0)
		{
			// Number of requirements based on a scaling 2^n probability
			// ergo, 100% of a first requirement -> 50% of a second requirement -> 25% of a third -> 12.5% of a fouth
			var denominator = (int)Mathf.Floor(Mathf.Pow(2, result.Count));
			 
			var shouldAddIndex = rand.Next(denominator);
			if (shouldAddIndex != 0) break;
			
			var randIndex = rand.Next(hatchRequirementTypes.Count);
			result.Add(hatchRequirementTypes[randIndex]);

			// Remove chosen HatchRequirementType from pool
			hatchRequirementTypes.Remove(hatchRequirementTypes[randIndex]);
		}
		return result;
	}

	private HatchRequirement CreateHatchRequirement(Egg egg, Enumerations.HatchRequirementTypes hatchRequirementType)
	{
		HatchRequirement result; 
		switch (hatchRequirementType)
		{
			case Enumerations.HatchRequirementTypes.Time:
				result = CreateTimeHatchRequirement(egg);
				break;
			default:
				_logger.LogError($"EggInteractor GetHatchRequirementTypes failed to map state. Egg name: {egg.Name}.");
				throw new Exception($"EggInteractor GetHatchRequirementTypes failed to map state. Egg name: {egg.Name}.");
				break;
		}
		return result;
	}

	private HatchRequirement CreateTimeHatchRequirement(Egg egg)
	{
		throw new NotImplementedException();
	}
}
