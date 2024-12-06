using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class AnimalInteractor : Node, IAnimalInteractor 
{
	private TimeSpan DEFAULT_EVENT_CAPTURE_SPAN = new TimeSpan(1, 0, 0, 0);

	private ILoggerService _logger { get; set; }

	public override void _Ready() 
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public async Task<CreatureModel> GetAnimal(int id)
	{
		CreatureModel result;
		try
		{
			_logger.LogDebug("Start AnimalInteractor GetAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var entity = await unitOfWork.AnimalRepository.GetByIdIncludesAsync(id, IncludesHelper.GetAnimalIncludes());
				var eventSummary = await GetAnimalEventSummary(entity.Id);
				result = entity.MapToModel(eventSummary);
			}
			_logger.LogDebug("End AnimalInteractor GetAnimal");
		}
		catch (Exception ex)
		{
			_logger.LogError($"AnimalInteractor GetAnimal Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task<List<CreatureModel>> GetAllAnimals()
	{
		var result = new List<CreatureModel>();
		try
		{
			_logger.LogDebug("Start AnimalInteractor GetAllAnimals");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var entityList = 
					(await unitOfWork.AnimalRepository.GetAllIncludesAsync(IncludesHelper.GetAnimalIncludes()))
						.Where(x => !x.IsDeleted)
						.ToList();
				
				foreach (var entity in entityList)
				{
					var eventSummary = await GetAnimalEventSummary(entity.Id);
					result.Add(entity.MapToModel(eventSummary));
				}
			}
			_logger.LogDebug("End AnimalInteractor GetAllAnimals");
		}
		catch (Exception ex)
		{
			_logger.LogError($"AnimalInteractor GetAllAnimals Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task<AnimalEventSummary> GetAnimalEventSummary(int id, TimeSpan? timeSpan = null)
	{
		if (timeSpan == null) timeSpan = DEFAULT_EVENT_CAPTURE_SPAN;
		
		AnimalEventSummary result = new AnimalEventSummary();
		try
		{
			_logger.LogDebug("Start AnimalInteractor GetAnimalEventSummary");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				DateTime thresholdDate = DateTime.Now - timeSpan.Value;

				var aeNutureEntityList = 
					(await unitOfWork.AnimalEventRepository
						.QueryAsync(
							q => q.Where(x => x.AnimalId == id)
								.Where(x => x.AnimalEventTypeId == (int)Enumerations.AnimalEventTypes.Nurture)
								.Where(x => x.CreatedDate >= thresholdDate)
						)).ToList();
				result.NurtureCount = aeNutureEntityList.Count;

				var aeFeedEntityList = 
					(await unitOfWork.AnimalEventRepository
						.QueryAsync(
							q => q.Where(x => x.AnimalId == id)
								.Where(x => x.AnimalEventTypeId == (int)Enumerations.AnimalEventTypes.Feed)
								.Where(x => x.CreatedDate >= thresholdDate)
						)).ToList();
				result.FeedCount = aeFeedEntityList.Count;		
			}
			_logger.LogDebug("End AnimalInteractor GetAnimalEventSummary");
		}
		catch (Exception ex)
		{
			_logger.LogError($"AnimalInteractor GetAnimalEventSummary Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task RenameAnimal(int id, string name)
	{
		try
		{
			_logger.LogDebug("Start AnimalInteractor RenameAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var aEntity = await unitOfWork.AnimalRepository.GetByIdIncludesAsync(id, IncludesHelper.GetAnimalIncludes());
				aEntity.Name = name;
				_ = await unitOfWork.SaveChangesAsync();
			}
			_logger.LogDebug("End AnimalInteractor RenameAnimal");
		}
		catch (Exception ex)
		{
			_logger.LogError($"AnimalInteractor RenameAnimal Error: {ex.Message}", ex);
			throw;
		}
	}

	public async Task FeedAnimal(int id, int xp)
	{
		try
		{
			_logger.LogDebug("Start AnimalInteractor FeedAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var aEntity = await unitOfWork.AnimalRepository.GetByIdIncludesAsync(id, IncludesHelper.GetAnimalIncludes());
				aEntity.XpOffset = aEntity.XpOffset + xp; 
				if (aEntity.XpOffset > XpTableHelper.GetLevelsXpGoal(aEntity.Level))
				{
					aEntity.XpOffset = aEntity.XpOffset - XpTableHelper.GetLevelsXpGoal(aEntity.Level);
					aEntity.Level = aEntity.Level + 1;
				}

				var aeEntity = new AnimalEvent();
				aeEntity.AnimalId = id;
				aeEntity.AnimalEventTypeId = (int)Enumerations.AnimalEventTypes.Feed;
				await unitOfWork.AnimalEventRepository.AddAsync(aeEntity);
				_ = await unitOfWork.SaveChangesAsync();
			}
			_logger.LogDebug("End AnimalInteractor FeedAnimal");
		}
		catch (Exception ex)
		{
			_logger.LogError($"AnimalInteractor FeedAnimal Error: {ex.Message}", ex);
			throw;
		}
	}

	public async Task NurtureAnimal(int id, int xp)
	{
		try
		{
			_logger.LogDebug("Start AnimalInteractor LoveAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var aEntity = await unitOfWork.AnimalRepository.GetByIdIncludesAsync(id, IncludesHelper.GetAnimalIncludes());
				aEntity.XpOffset = aEntity.XpOffset + xp; 
				if (aEntity.XpOffset > XpTableHelper.GetLevelsXpGoal(aEntity.Level))
				{
					aEntity.XpOffset = aEntity.XpOffset - XpTableHelper.GetLevelsXpGoal(aEntity.Level);
					aEntity.Level = aEntity.Level + 1;
				}

				var aeEntity = new AnimalEvent();
				aeEntity.AnimalId = id;
				aeEntity.AnimalEventTypeId = (int)Enumerations.AnimalEventTypes.Nurture;
				await unitOfWork.AnimalEventRepository.AddAsync(aeEntity);

				_ = await unitOfWork.SaveChangesAsync();
			}
			_logger.LogDebug("End AnimalInteractor LoveAnimal");
		}
		catch (Exception ex)
		{
			_logger.LogError($"AnimalInteractor LoveAnimal Error: {ex.Message}", ex);
			throw;
		}
	}
}
