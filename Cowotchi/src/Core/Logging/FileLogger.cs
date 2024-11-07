using Godot;
using System;
using System.Threading.Tasks;

public class FileLogger : ILogger
{
	public FileLogger(string filePath, Enumerations.LogLevels logLevel)
	{
		FilePath = filePath;
		LogLevel = logLevel;
	}

	private string FilePath { get; set; }
	private Enumerations.LogLevels LogLevel { get; set; }

	public async Task Log(Enumerations.LogLevels logLevel, string message, Exception? exception = null)
	{
		if ((int)LogLevel <= (int)logLevel)
		{
			PublishLog(message, exception);
		}
	}

	private async Task PublishLog(string message, Exception? exception = null)
	{
		try
		{
			string str;
			if (exception != null) str = $"{message}. Exception: {exception.Message} {exception.InnerException.Message}";
			else str = message;

			using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
			file.StoreString(str);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"FileLogger PublishLog Failed: {ex.Message}");
		}
	}
}
