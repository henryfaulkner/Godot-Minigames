using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class AnimalInteractor : Node, IAnimalInteractor 
{
	private ILoggerService _loggerService { get; set; }

	public override void _Ready() 
	{
		_loggerService = GetNode<ILoggerService>("/root/LoggerService");
	}

	public async Task<Animal> GetAnimal(int id)
	{
		Animal result;
		try
		{
			_loggerService.LogDebug("Start AnimalInteractor GetAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				result = await unitOfWork.AnimalRepository.GetByIdAsync(id);
			}
			_loggerService.LogDebug("End AnimalInteractor GetAnimal");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"AnimalInteractor GetAnimal Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task<AnimalEventData> GetAnimal_RecentEventData(int id, TimeSpan timeSpan)
	{
		AnimalEventData result = new AnimalEventData();
		try
		{
			_loggerService.LogDebug("Start AnimalInteractor GetAnimal_RecentEventData");
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
			_loggerService.LogDebug("End AnimalInteractor GetAnimal_RecentEventData");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"AnimalInteractor GetAnimal_RecentEventData Error: {ex.Message}", ex);
			throw;
		}
		return result;
	}

	public async Task RenameAnimal(int id, string name)
	{
		try
		{
			_loggerService.LogDebug("Start AnimalInteractor RenameAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var aEntity = await unitOfWork.AnimalRepository.GetByIdAsync(id);
				aEntity.Name = name;
				_ = await unitOfWork.SaveChangesAsync();
			}
			_loggerService.LogDebug("End AnimalInteractor RenameAnimal");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"AnimalInteractor RenameAnimal Error: {ex.Message}", ex);
			throw;
		}
	}

	public async Task FeedAnimal(int id)
	{
		try
		{
			_loggerService.LogDebug("Start AnimalInteractor FeedAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var aeEntity = new AnimalEvent();
				aeEntity.AnimalId = id;
				aeEntity.AnimalEventTypeId = (int)Enumerations.AnimalEventTypes.Feed;
				await unitOfWork.AnimalEventRepository.AddAsync(aeEntity);
				_ = await unitOfWork.SaveChangesAsync();
			}
			_loggerService.LogDebug("End AnimalInteractor FeedAnimal");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"AnimalInteractor FeedAnimal Error: {ex.Message}", ex);
			throw;
		}
	}

	public async Task NurtureAnimal(int id)
	{
		try
		{
			_loggerService.LogDebug("Start AnimalInteractor LoveAnimal");
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var aeEntity = new AnimalEvent();
				aeEntity.AnimalId = id;
				aeEntity.AnimalEventTypeId = (int)Enumerations.AnimalEventTypes.Nurture;
				await unitOfWork.AnimalEventRepository.AddAsync(aeEntity);
				_ = await unitOfWork.SaveChangesAsync();
			}
			_loggerService.LogDebug("End AnimalInteractor LoveAnimal");
		}
		catch (Exception ex)
		{
			_loggerService.LogError($"AnimalInteractor LoveAnimal Error: {ex.Message}", ex);
			throw;
		}
	}
}
