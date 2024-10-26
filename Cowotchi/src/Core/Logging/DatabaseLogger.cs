using Godot;
using System;
using System.Threading.Tasks;

public class DatabaseLogger : ILogger
{
	private Enumerations.LogLevels LogLevel { get; set; }

	public DatabaseLogger(Enumerations.LogLevels logLevel)
	{
		LogLevel = logLevel;
	}

	public async Task Log(Enumerations.LogLevels logLevel, string message, Exception? exception = null)
	{
		if ((int)LogLevel <= (int)logLevel)
		{
			PublishLog(logLevel, message, exception);
		}
	}

	private async Task PublishLog(Enumerations.LogLevels logLevel, string message, Exception? exception = null)
	{
		try
		{
			using (var unitOfWork = new UnitOfWork(new AppDbContext()))
			{
				var log = new Log();
				log.Level = EnumHelper.GetEnumDescription(logLevel);
				log.Message = message;
				log.StackTrace = exception?.StackTrace;
				await unitOfWork.LogRepository.AddAsync(log);
				await unitOfWork.SaveChangesAsync();
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"DatabaseLogger PublishLog Failed: {ex.Message}");
		}
	}
}
