using Godot;
using System;
using System.Threading.Tasks;

public class DatabaseLogger : ILogger 
{
	private readonly AppDbContext _context;
	private Enumerations.LogLevels LogLevel { get; set; }

	public DatabaseLogger(Enumerations.LogLevels logLevel)
	{
		_context = new AppDbContext();
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
			GD.Print("Database PublishLog");
			var log = new Log();
			log.Level = EnumHelper.GetEnumDescription(logLevel);
			log.Message = message;
			log.StackTrace = exception?.StackTrace;
			_context.Logs.AddAsync(log);
			_context.SaveChangesAsync();
		} 
		catch (Exception ex)
		{
			GD.PrintErr($"DatabaseLogger PublishLog Failed: {ex.Message}");
		}
	}
}
