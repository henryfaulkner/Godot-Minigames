using Godot;
using System;
using System.Threading.Tasks;
using System.Text;

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
			var stringBuilder = new StringBuilder();
			stringBuilder.Append(message);
			if (exception != null && !string.IsNullOrEmpty(exception.Message)) stringBuilder.Append(" Exception: ").Append(exception.Message);
			if (exception?.InnerException != null && !string.IsNullOrEmpty(exception.InnerException.Message)) stringBuilder.Append(" Inner Exception").Append(exception.InnerException.Message);
			string str = stringBuilder.ToString();

			using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
			file.StoreString(str);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"FileLogger PublishLog Failed: {ex.Message}");
		}
	}
}
