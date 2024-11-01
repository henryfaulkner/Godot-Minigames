using System;
using System.Threading.Tasks;

public class UnitOfWork : IUnitOfWork
{
	private readonly AppDbContext _context;

	public IRepository<Animal> AnimalRepository { get; }
	public IRepository<AnimalEvent> AnimalEventRepository { get; }
	public IRepository<AnimalEventType> AnimalEventTypeRepository { get; }
	public IRepository<AnimalType> AnimalTypeRepository { get; }

	public IRepository<Egg> EggRepository { get; }
	public IRepository<HatchRequirement> HatchRequirementRepository { get; }
	public IRepository<HatchRequirementType> HatchRequirementTypeRepository { get; }
	public IRepository<NameOption> NameOptionRepository { get; }

	public IRepository<Log> LogRepository { get; }

	public UnitOfWork(AppDbContext context)
	{
		_context = context;

		AnimalEventTypeRepository = new Repository<AnimalEventType>(_context);
		AnimalTypeRepository = new Repository<AnimalType>(_context);
		HatchRequirementTypeRepository = new Repository<HatchRequirementType>(_context);
		LogRepository = new Repository<Log>(_context);
	}

	public async Task<bool> SaveChangesAsync()
	{
		var numberOfEntries = await _context.SaveChangesAsync();
		return numberOfEntries > 0;
	}

	public void Dispose()
	{
		_context.Dispose();
	}
}
