using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
	IRepository<Animal> AnimalRepository { get; }
	IRepository<AnimalEvent> AnimalEventRepository { get; }
	IRepository<AnimalEventType> AnimalEventTypeRepository { get; }
	IRepository<AnimalType> AnimalTypeRepository { get; }

	IRepository<Egg> EggRepository { get; }
	IRepository<HatchRequirement> HatchRequirementRepository { get; }
	IRepository<HatchRequirementType> HatchRequirementTypeRepository { get; }

	IRepository<Log> LogRepository { get; }

	Task<bool> SaveChangesAsync();
}
