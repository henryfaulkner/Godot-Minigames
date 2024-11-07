using Godot;
using System;
using System.Threading.Tasks;

public class LocalLogger : ILogger
{
	private Enumerations.LogLevels LogLevel { get; set; }

	public LocalLogger(Enumerations.LogLevels logLevel)
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
			string str;
			if (exception != null) str = $"{message}. Exception: {exception.Message} {exception.InnerException.Message}";
			else str = message;

			switch(logLevel)
			{
				case Enumerations.LogLevels.Error:
					GD.PrintErr(str);
					break;
				default:
					GD.Print(str);
					break;
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"LocalLogger PublishLog Failed: {ex.Message}");
		}
	}
}
