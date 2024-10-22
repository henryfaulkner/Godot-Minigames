using Godot;
using System;
using System.Collections.Generic;

public partial class LoggingService : Node
{
	private List<ILogger> Loggers { get; set; }

	public LoggingService()
	{
		Loggers = new List<ILogger>();
		Loggers.Add(new FileLogger("res://Core/LoggerService/logs/log.txt", Enumerations.LogLevels.Error));
        Loggers.Add(new DatabaseLogger(Enumerations.Debug));
	}

	public void LogDebug(string message)
	{
		////GD.Print("LogDebug");
		Loggers.ForEach((logger) =>
			{
				logger.Log(Enumerations.LogLevels.Debug, message);
			}
		);
	}

	public void LogInfo(string message)
	{
		////GD.Print("LogInfo");
		Loggers.ForEach((logger) =>
			{
				logger.Log(Enumerations.LogLevels.Info, message);
			}
		);
	}

	public void LogError(string message)
	{
		////GD.Print("LogError");
		Loggers.ForEach((logger) =>
			{
				logger.Log(Enumerations.LogLevels.Error, message);
			}
		);
	}
}