using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class AnimalInteractor : Node, IAnimalInteractor 
{
	private ILoggerService _logger { get; set; }

	public override void _Ready() 
	{
		_logger = GetNode<ILoggerService>("/root/LoggerService");
	}

	public async Task<AnimalModel> GetAnimal(int id)
	{
		AnimalModel result;
		try
		{
			_logger.LogDebug("Start AnimalInteractor GetAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var entity = await unitOfWork.AnimalRepository.GetByIdIncludesAsync(id, IncludesHelper.GetAnimalIncludes());
				result = entity.MapToModel();
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

	public async Task<List<AnimalModel>> GetAllAnimals()
	{
		var result = new List<AnimalModel>();
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
					result.Add(entity.MapToModel());
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

	public async Task<AnimalEventData> GetAnimal_RecentEventData(int id, TimeSpan timeSpan)
	{
		AnimalEventData result = new AnimalEventData();
		try
		{
			_logger.LogDebug("Start AnimalInteractor GetAnimal_RecentEventData");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var aeNutureEntityList = 
					await unitOfWork.AnimalEventRepository
						.QueryAsync(
							q => q.Where(x => x.AnimalId == id)
								.Where(x => x.AnimalEventTypeId == (int)Enumerations.AnimalEventTypes.Nurture)
								.Where(x => timeSpan > (DateTime.Now - x.CreatedDate))
						);
				result.NurtureCount = aeNutureEntityList.Count;

				var aeFeedEntityList = 
					await unitOfWork.AnimalEventRepository
						.QueryAsync(
							q => q.Where(x => x.AnimalId == id)
								.Where(x => x.AnimalEventTypeId == (int)Enumerations.AnimalEventTypes.Feed)
								.Where(x => timeSpan > (DateTime.Now - x.CreatedDate))
						);
				result.FeedCount = aeFeedEntityList.Count;		
			}
			_logger.LogDebug("End AnimalInteractor GetAnimal_RecentEventData");
		}
		catch (Exception ex)
		{
			_logger.LogError($"AnimalInteractor GetAnimal_RecentEventData Error: {ex.Message}", ex);
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

	public async Task FeedAnimal(int id)
	{
		try
		{
			_logger.LogDebug("Start AnimalInteractor FeedAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
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

	public async Task NurtureAnimal(int id)
	{
		try
		{
			_logger.LogDebug("Start AnimalInteractor LoveAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
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
