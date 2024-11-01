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

	private ILoggerService _loggerService { get; set; }
	private IAnimalInteractor _animalInteractor { get; set; }

	public override void _Ready() 
	{
		_loggerService = GetNode<ILoggerService>("/root/LoggerService");
		_animalInteractor = GetNode<IAnimalInteractor>("/root/AnimalInteractor");
	}

	public async Task<Egg> CreateEgg()
	{
		Egg result;
		try
		{
			_loggerService.LogDebug("Start EggInteractor CreateEgg");
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
				result = eEntity;
			}
			_loggerService.LogDebug("End EggInteractor CreateEgg");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"EggInteractor CreateEgg Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task<Egg> GetEgg(int id)
	{
		Egg result;
		try
		{
			_loggerService.LogDebug("Start EggInteractor GetEgg");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				result = await unitOfWork.EggRepository.GetByIdAsync(id);
			}
			_loggerService.LogDebug("End EggInteractor GetEgg");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"EggInteractor GetEgg Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task RenameEgg(int id, string name)
	{
		try
		{
			_loggerService.LogDebug("Start EggInteractor RenameEgg");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var eEntity = await unitOfWork.EggRepository.GetByIdAsync(id);
				eEntity.Name = name;
				await unitOfWork.SaveChangesAsync();
			}
			_loggerService.LogDebug("End EggInteractor RenameEgg");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"EggInteractor RenameEgg Error: {ex.Message}", ex);
			throw;
		}
	}

	public async Task<Animal> HatchEgg(int id)
	{
		Animal result;
		try
		{
			_loggerService.LogDebug("Start EggInteractor HatchEgg");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var eEntity = await unitOfWork.EggRepository.GetByIdAsync(id);
				eEntity.IsHatched = true;

				var aEntity = new Animal();
				aEntity.Name = eEntity.Name;
				aEntity.AnimalType = eEntity.AnimalType;
				aEntity.Egg = eEntity;
				await unitOfWork.AnimalRepository.AddAsync(aEntity);

				_ = await unitOfWork.SaveChangesAsync();
				result = aEntity;
			}
			_loggerService.LogDebug("End EggInteractor HatchEgg");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"EggInteractor HatchEgg Error: {ex.Message}", ex);
			throw;
		}
		return result;
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
				_loggerService.LogError($"EggInteractor GetHatchRequirementTypes failed to map state. Egg name: {egg.Name}.");
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
