using Godot;
using System;

public class DatabaseLogger : ILogger 
{
    private readonly AppDbContext _context;
    private readonly Enumerations.LogLevels LogLevel { get; set; }

    public DatabaseLogger(Enumerations.LogLevels logLevel)
    {
        _context = new AppDbContext();
        LogLevel = logLevel;
    }

    public void Log(Enumerations.LogLevels logLevel, string message)
	{
		if ((int)LogLevel >= (int)logLevel)
		{
			PublishLog(message);
		}
    }

    private void PublishLogger(string message, Exception? exception = null)
    {
        try
        {
            var log = new Log();
            log.Message = message;
            log.StackTrace = exception.StackTrace;
            _context.Logs.Add(log);
        }
    }
}