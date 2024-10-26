using System;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
	IRepository<AnimalEventType> AnimalEventTypeRepository { get; }
	IRepository<AnimalType> AnimalTypeRepository { get; }
	IRepository<HatchRequirementType> HatchRequirementTypeRepository { get; }
	IRepository<Log> LogRepository { get; }

	Task<bool> SaveChangesAsync();
}
